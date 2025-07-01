
namespace PedidoStore.Query.Abstractions
{
    public interface IReadDbMapping
    {
        /// <summary>
        /// Configures the mappings for reading from the database.
        /// </summary>
        void Configure();
    }
}
