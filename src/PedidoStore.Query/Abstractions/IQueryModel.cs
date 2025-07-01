
using MongoDB.Bson.Serialization.Attributes;

namespace PedidoStore.Query.Abstractions
{
    public interface IQueryModel;

    /// <summary>
    /// Represents the interface for a query model with a generic key type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IQueryModel<out TKey> : IQueryModel where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the ID of the query model.
        /// </summary>
        /// 

        [BsonId] // maps to _id automatically

        TKey Id { get; }
    }
}
