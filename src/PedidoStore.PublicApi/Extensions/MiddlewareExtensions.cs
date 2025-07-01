using PedidoStore.PublicApi.Middlewares;

namespace PedidoStore.PublicApi.Extensions
{
    internal static class MiddlewareExtensions
    {
        public static void UseErrorHandling(this IApplicationBuilder builder) =>
            builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
