﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PedidoStore.Core.AppSettings;
using PedidoStore.Core.Extensions;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;
using Polly;
using Polly.Retry;
using System.Linq.Expressions;
using System.Reflection;
namespace PedidoStore.Query.Data.Context
{
    public sealed class NoSqlDbContext : IReadDbContext, ISynchronizeDb
    {
        #region Constructor

        private const string DatabaseName = "Pedido";
        private const int RetryCount = 2;

        private static readonly ReplaceOptions DefaultReplaceOptions = new()
        {
            IsUpsert = true
        };

        private static readonly CreateIndexOptions DefaultCreateIndexOptions = new()
        {
            Unique = true,
            Sparse = true
        };

        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly ILogger<NoSqlDbContext> _logger;
        private readonly AsyncRetryPolicy _mongoRetryPolicy;

        public NoSqlDbContext(IOptions<ConnectionOptions> options, ILogger<NoSqlDbContext> logger)
        {
            ConnectionString = options.Value.NoSqlConnection;

            _mongoClient = new MongoClient(options.Value.NoSqlConnection);
            _mongoDatabase = _mongoClient.GetDatabase(DatabaseName);
            _logger = logger;
            _mongoRetryPolicy = CreateRetryPolicy(logger);
        }

        #endregion

        #region IReadDbContext

        public string ConnectionString { get; }

        public IMongoCollection<TQueryModel> GetCollection<TQueryModel>() where TQueryModel : IQueryModel =>
            _mongoDatabase.GetCollection<TQueryModel>(typeof(TQueryModel).Name);

        public async Task CreateCollectionsAsync()
        {
            using var asyncCursor = await _mongoDatabase.ListCollectionNamesAsync();
            var collections = await asyncCursor.ToListAsync();

            foreach (var collectionName in GetCollectionNamesFromAssembly())
            {
                // Check if the collection does not exist in the database
                if (!collections.Exists(db => db.Equals(collectionName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    _logger.LogInformation("----- MongoDB: creating the Collection {Name}", collectionName);

                    await _mongoDatabase.CreateCollectionAsync(collectionName, new CreateCollectionOptions
                    {
                        ValidationLevel = DocumentValidationLevel.Strict
                    });
                }
                else
                {
                    _logger.LogInformation("----- MongoDB: the {Name} collection already exists", collectionName);
                }
            }

            await CreateIndexOrderAsync();
            await CreateIndexProductAsync();
        }

        private async Task CreateIndexOrderAsync()
        {
            //_logger.LogInformation("----- MongoDB: creating indexes...");

            //var indexDefinition = Builders<OrderQueryModel>.IndexKeys.Ascending(model => model.OrderNumber); // example field
            //var indexOptions = new CreateIndexOptions { Unique = true, Sparse = true };
            //var indexModel = new CreateIndexModel<OrderQueryModel>(indexDefinition, indexOptions);

            //var collection = GetCollection<OrderQueryModel>();
            //var indexName = await collection.Indexes.CreateOneAsync(indexModel);

            //_logger.LogInformation("----- MongoDB: indexes successfully created - {indexName}", indexName);
        }
        private async Task CreateIndexProductAsync()
        {
            //_logger.LogInformation("----- MongoDB: creating indexes...");

            //// Define the index key as ascending order of the Email field in the CustomerQueryModel class
            //var indexDefinition = Builders<ProductQueryModel>.IndexKeys.Ascending(model => model.Id);

            //// Create an index model with the defined index key and default index options
            //var indexModel = new CreateIndexModel<ProductQueryModel>(indexDefinition, DefaultCreateIndexOptions);

            //var collection = GetCollection<ProductQueryModel>();

            //var indexName = await collection.Indexes.CreateOneAsync(indexModel);

            //_logger.LogInformation("----- MongoDB: indexes successfully created - {indexName}", indexName);

            var database = _mongoClient.GetDatabase(DatabaseName);

            await DbSeeder.SeedAsyncProduct(database);
            await DbSeeder.SeedAsyncCustomer(database);
        }

        private static List<string> GetCollectionNamesFromAssembly() =>
            [.. Assembly
            .GetExecutingAssembly()
            .GetAllTypesOf<IQueryModel>()
            .Select(impl => impl.Name)
            .Distinct()];

        #endregion

        #region ISynchronizeDb

        public async Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upsertFilter)
            where TQueryModel : IQueryModel
        {
            var collection = GetCollection<TQueryModel>();

            await _mongoRetryPolicy.ExecuteAsync(async () =>
                await collection.ReplaceOneAsync(upsertFilter, queryModel, DefaultReplaceOptions));
        }


        public async Task DeleteAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> deleteFilter)
            where TQueryModel : IQueryModel
        {
            var collection = GetCollection<TQueryModel>();
            await _mongoRetryPolicy.ExecuteAsync(async () => await collection.DeleteOneAsync(deleteFilter));
        }

        private static AsyncRetryPolicy CreateRetryPolicy(ILogger logger) =>
            Policy
                .Handle<MongoException>()
                .WaitAndRetryAsync(
                    RetryCount,
                    (retryAttempt) => SleepDurationProvider(retryAttempt, logger),
                    (ex, _) => OnRetry(logger, ex));

        private static TimeSpan SleepDurationProvider(int retryAttempt, ILogger logger)
        {
            // Retry with jitter
            // A well-known retry strategy is exponential backoff, allowing retries to be made initially quickly,
            // but then at progressively longer intervals: for example, after 2, 4, 8, 15, then 30 seconds.
            // REF: https://github.com/App-vNext/Polly/wiki/Retry-with-jitter#simple-jitter
            var sleepDuration =
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));

            logger.LogWarning("----- MongoDB: Retry #{Count} with delay {Delay}", retryAttempt, sleepDuration);

            return sleepDuration;
        }

        private static void OnRetry(ILogger logger, Exception ex) =>
            logger.LogError(ex, "An unexpected exception occurred while saving to MongoDB: {Message}", ex.Message);

        #endregion

        #region IDisposable

        // To detect redundant calls.
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        ~NoSqlDbContext() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Dispose managed state (managed objects).
            if (disposing)
                _mongoClient.Dispose();

            _disposed = true;
        }

        public async Task InsertAsync<TQueryModel>(TQueryModel queryModel) where TQueryModel : IQueryModel
        {
            var collection = GetCollection<TQueryModel>();

            await _mongoRetryPolicy.ExecuteAsync(async () =>
                await collection.InsertOneAsync(queryModel));
        }

        #endregion
    }
}
