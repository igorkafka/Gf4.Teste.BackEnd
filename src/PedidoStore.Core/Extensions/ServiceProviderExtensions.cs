using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PedidoStore.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoStore.Core.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static TOptions GetOptions<TOptions>(this IServiceProvider serviceProvider)
            where TOptions : class, IAppOptions =>
            serviceProvider.GetService<IOptions<TOptions>>()?.Value;
    }
}
