using MongoDB.Driver;

namespace PedidoStore.Query.Abstractions
{
    public interface IReadDbContext : IDisposable
    {
        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the collection for the specified query model.
        /// </summary>
        /// <typeparam name="TQueryModel">The type of the query model.</typeparam>
        /// <returns>The MongoDB collection for the specified query model.</returns>
        IMongoCollection<TQueryModel> GetCollection<TQueryModel>() where TQueryModel : IQueryModel;

        /// <summary>
        /// Creates collections in the database for all query models.
        /// </summary>
        /// <returns>A task representing the asynchronous creation of collections.</returns>
        Task CreateCollectionsAsync();
    }
}
