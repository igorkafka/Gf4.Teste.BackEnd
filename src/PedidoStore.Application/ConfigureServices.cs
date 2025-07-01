﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PedidoStore.Application.Abstractions;
using PedidoStore.Application.Behaviors;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PedidoStore.Application
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureServices
    {
        /// <summary>
        /// Adds command handlers to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(IApplicationMarker));
            return services
                .AddValidatorsFromAssembly(assembly, ServiceLifetime.Singleton)
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly)
                    .AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)));
        }
    }
}
