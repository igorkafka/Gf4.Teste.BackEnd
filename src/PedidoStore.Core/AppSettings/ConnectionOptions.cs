
using PedidoStore.Core.SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace PedidoStore.Core.AppSettings
{
    public class ConnectionOptions : IAppOptions
    {
        static string IAppOptions.ConfigSectionPath => "ConnectionStrings";

        [Required]
        public string SqlConnection { get; private init; }

        [Required]
        public string NoSqlConnection { get; private init; }

        [Required]
        public string CacheConnection { get; private init; }

        public bool CacheConnectionInMemory() =>
            CacheConnection.Equals("InMemory", StringComparison.InvariantCultureIgnoreCase);
    }
}
