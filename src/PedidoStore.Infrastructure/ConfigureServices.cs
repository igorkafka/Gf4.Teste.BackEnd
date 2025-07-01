using Microsoft.Extensions.DependencyInjection;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Repositories;
using PedidoStore.Infrastructure.Data;
using PedidoStore.Infrastructure.Data.Repositories;
using PedidoStore.Infrastructure.Data.Services;
using System.Diagnostics.CodeAnalysis;

namespace PedidoStore.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureServices
    {
        /// <summary>
        /// Adds the memory cache service to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddMemoryCacheService(this IServiceCollection services) =>
            services.AddScoped<ICacheService, MemoryCacheService>();

        /// <summary>
        /// Adds the distributed cache service to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddDistributedCacheService(this IServiceCollection services) =>
            services.AddScoped<ICacheService, DistributedCacheService>();

        /// <summary>
        /// Adds the infrastructure services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
            services
                .AddScoped<WriteDbContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

        /// <summary>
        /// Adds the write-only repositories to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection AddWriteOnlyRepositories(this IServiceCollection services) =>
             services
                .AddScoped<IOrderWriteOnlyRepository, OrderWriteOnlyRepository>()
                .AddScoped<IOrderItemWriteOnlyRepository, OrderItemWriteOnlyRepository>()
                 .AddScoped<ICustomerWriteOnlyRepository, CustomerWriteOnlyRepository>()
            .AddScoped<IProductWriteOnlyRepository, ProductWriteOnlyRepository>();


    }
}
