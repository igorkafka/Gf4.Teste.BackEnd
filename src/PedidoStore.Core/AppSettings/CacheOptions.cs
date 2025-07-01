

using PedidoStore.Core.SharedKernel;

namespace PedidoStore.Core.AppSettings
{
    public class CacheOptions : IAppOptions
    {
        static string IAppOptions.ConfigSectionPath => nameof(CacheOptions);

        public int AbsoluteExpirationInHours { get; private init; }
        public int SlidingExpirationInSeconds { get; private init; }
    }
}
