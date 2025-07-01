﻿using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.Data.Context;
using PedidoStore.Query.Data.Mapping;
using PedidoStore.Query.Data.Repositories;
using PedidoStore.Query.Data.Repositories.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PedidoStore.Query
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureServices
    {
        /// <summary>
        /// Adds query handlers to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(IQueryMarker));
            return services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
                .AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(assembly))))
                .AddValidatorsFromAssembly(assembly);
        }

        /// <summary>
        /// Adds the read database context to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection AddReadDbContext(this IServiceCollection services)
        {
            services
                .AddScoped<ISynchronizeDb, NoSqlDbContext>()
                .AddScoped<IReadDbContext, NoSqlDbContext>()
                .AddScoped<NoSqlDbContext>();

            ConfigureMongoDb();

            return services;
        }

        /// <summary>
        /// Adds read-only repositories to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection AddReadOnlyRepositories(this IServiceCollection services) =>
                        services.AddScoped<ICustomerReadOnlyRepository, CustomerReadOnlyRepository>().
            AddScoped<IOrderReadOnlyRepository, OrderReadOnlyRepository>().
            AddScoped<IProductReadOnlyRepository, ProductReadOnlyRepository>();



        /// <summary>
        /// Configures the MongoDB settings and mappings.
        /// </summary>
        private static void ConfigureMongoDb()
        {
            try
            {
                // Step 1: Configure the serializer for Guid type.
                BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

                // Step 2: Configure the conventions to be applied to all mappings.
                // REF: https://mongodb.github.io/mongo-csharp-driver/2.0/reference/bson/mapping/conventions/
                ConventionRegistry.Register("Conventions",
                    new ConventionPack
                    {
                    new CamelCaseElementNameConvention(), // Convert element names to camel case
                    new EnumRepresentationConvention(BsonType.String), // Serialize enums as strings
                    new IgnoreExtraElementsConvention(true), // Ignore extra elements when deserializing
                    new IgnoreIfNullConvention(true) // Ignore null values when serializing
                    }, _ => true);

                // Step 3: Register the mappings configurations.
                // It is recommended to register all mappings before initializing the connection with MongoDb
                // REF: https://mongodb.github.io/mongo-csharp-driver/2.0/reference/bson/mapping/
                new OrderMap().Configure(); // Configuration for Customer class
            }
            catch
            {
                // Ignore
            }
        }
    }
}
