

using MongoDB.Driver;
using PedidoStore.Domain.Entities;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query;
public static class DbSeeder
{
    public static async Task SeedAsyncProduct(IMongoDatabase database)
    {
        var collection = database.GetCollection<ProductQueryModel>(typeof(ProductQueryModel).Name);

        bool hasAny = await collection.Find(FilterDefinition<ProductQueryModel>.Empty).AnyAsync();

        if (!hasAny)
        {
            var products = new List<ProductQueryModel>
            {
                new ProductQueryModel(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Laptop", 1500 ),
                new ProductQueryModel (Guid.Parse("b8a0f735-c46b-4e63-84de-88a8ab5cbe02"), "Nintendo Switch", 2250),
                new ProductQueryModel (Guid.Parse("d1c3a881-2a4d-4e9f-9214-8cbf6ff88e44"), "PC Gamer", 2250)
            };

            await collection.InsertManyAsync(products);
        }
    }
    public static async Task SeedAsyncCustomer(IMongoDatabase database)
    {
        var collection = database.GetCollection<CustomerQueryModel>(typeof(CustomerQueryModel).Name);

        bool hasAny = await collection.Find(FilterDefinition<CustomerQueryModel>.Empty).AnyAsync();

        if (!hasAny)
        {

            await collection.InsertOneAsync(new CustomerQueryModel() { Id = Guid.Parse("4a5f1c24-b68a-46f3-b1a3-1b2f0ea3dc08"), Name = "user", Phone = "(69) 99297-1717", Email = "user@gmail.com" });
        }
    }
}