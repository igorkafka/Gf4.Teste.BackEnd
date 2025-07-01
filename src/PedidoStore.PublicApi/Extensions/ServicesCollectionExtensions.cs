using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MongoDB.Driver;
using PedidoStore.Core.AppSettings;
using PedidoStore.Core.Extensions;
using PedidoStore.Infrastructure.Data;
using System.Diagnostics.CodeAnalysis;
using PedidoStore.Infrastructure;

namespace PedidoStore.PublicApi.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class ServicesCollectionExtensions
    {
        private const int DbMaxRetryCount = 3;
        private const int DbCommandTimeout = 30;
        private const string DbMigrationAssemblyName = "PedidoStore.PublicApi";
        private const string RedisInstanceName = "master";
        private const string TestingEnvironmentName = "Testing";

        private static readonly string[] DbRelationalTags = ["database", "ef-core", "sql-server", "relational"];
        private static readonly string[] DbNoSqlTags = ["database", "mongodb", "no-sql"];

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetOptions<ConnectionOptions>();

            var healthCheckBuilder = services
                .AddHealthChecks()
                .AddDbContextCheck<WriteDbContext>(tags: DbRelationalTags)
                .AddMongoDb(clientFactory: _ => new MongoClient(options.NoSqlConnection), tags: DbNoSqlTags);

            if (!options.CacheConnectionInMemory())
                healthCheckBuilder.AddRedis(options.CacheConnection);

            return services;
        }

        public static IServiceCollection AddWriteDbContext(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (!environment.IsEnvironment(TestingEnvironmentName))
            {
                services.AddDbContextPool<WriteDbContext>((serviceProvider, optionsBuilder) =>
                    ConfigureDbContext<WriteDbContext>(
                        serviceProvider, optionsBuilder, QueryTrackingBehavior.TrackAll));

            }

            return services;
        }

        public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetOptions<ConnectionOptions>();
            if (options.CacheConnectionInMemory())
            {
                services.AddMemoryCacheService();
                services.AddMemoryCache(memoryOptions => memoryOptions.TrackStatistics = true);
            }
            else
            {
                services.AddDistributedCacheService();
                services.AddStackExchangeRedisCache(redisOptions =>
                {
                    redisOptions.InstanceName = RedisInstanceName;
                    redisOptions.Configuration = options.CacheConnection;
                });
            }

            return services;
        }

        private static void ConfigureDbContext<TDbContext>(
            IServiceProvider serviceProvider,
            DbContextOptionsBuilder optionsBuilder,
            QueryTrackingBehavior queryTrackingBehavior) where TDbContext : DbContext
        {
            var logger = serviceProvider.GetRequiredService<ILogger<TDbContext>>();
            var options = serviceProvider.GetOptions<ConnectionOptions>();
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
            var envIsDevelopment = environment.IsDevelopment();

            optionsBuilder
                .UseSqlServer(options.SqlConnection, sqlServerOptions =>
                {
                    sqlServerOptions
                        .MigrationsAssembly(DbMigrationAssemblyName)
                        .EnableRetryOnFailure(DbMaxRetryCount)
                        .CommandTimeout(DbCommandTimeout);
                })
                .EnableDetailedErrors(envIsDevelopment)
                .EnableSensitiveDataLogging(envIsDevelopment)
                .UseQueryTrackingBehavior(queryTrackingBehavior)
                .LogTo((eventId, _) => eventId.Id == CoreEventId.ExecutionStrategyRetrying, eventData =>
                {
                    if (eventData is not ExecutionStrategyEventData retryEventData)
                        return;

                    var exceptions = retryEventData.ExceptionsEncountered;

                    logger.LogWarning(
                        "----- DbContext: Retry #{Count} with delay {Delay} due to error: {Message}",
                        exceptions.Count,
                        retryEventData.Delay,
                        exceptions[^1].Message);
                });

            if (envIsDevelopment)
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
