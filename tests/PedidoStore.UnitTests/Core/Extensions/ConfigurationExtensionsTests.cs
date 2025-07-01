﻿using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit.Categories;
using PedidoStore.Core.Extensions;
using PedidoStore.Core.AppSettings;

namespace PedidoStore.UnitTests.Core.Extensions;

[UnitTest]
public class ConfigurationExtensionsTests
{
    [Fact]
    public void Should_ReturnsClassOptions_WhenGetOptions()
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

        // Act
        var act = configuration.GetOptions<CacheOptions>();

        // Assert
        act.Should().NotBeNull();
        act.AbsoluteExpirationInHours.Should().Be(absoluteExpirationInHours);
        act.SlidingExpirationInSeconds.Should().Be(slidingExpirationInSeconds);
    }
}