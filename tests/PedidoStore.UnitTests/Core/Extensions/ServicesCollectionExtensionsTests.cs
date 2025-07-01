using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PedidoStore.Core.AppSettings;
using PedidoStore.Core.Extensions;
using Xunit.Categories;
using PedidoStore.Core;
using FluentAssertions;

namespace PedidoStore.UnitTests.Core.Extensions
{
    [UnitTest]
    public class ServicesCollectionExtensionsTests
    {
        [Fact]
        public void Should_ReturnClassOptions_WhenConfigureAppSettings()
        {
            // Arrange
            const int absoluteExpirationInHours = 4;
            const int slidingExpirationInSeconds = 120;

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "CacheOptions:AbsoluteExpirationInHours", absoluteExpirationInHours.ToString() },
            { "CacheOptions:SlidingExpirationInSeconds", slidingExpirationInSeconds.ToString() }
        });

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.ConfigureAppSettings();
            var serviceProvider = services.BuildServiceProvider(true);

            // Act
            var act = serviceProvider.GetOptions<CacheOptions>();

            // Assert
            act.Should().NotBeNull();
            act.AbsoluteExpirationInHours.Should().Be(absoluteExpirationInHours);
            act.SlidingExpirationInSeconds.Should().Be(slidingExpirationInSeconds);
        }
    }
}
