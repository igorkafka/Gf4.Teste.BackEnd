using Asp.Versioning;
using CorrelationId;
using FluentValidation;
using FluentValidation.Resources;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using PedidoStore.PublicApi.Extensions;
using Scalar.AspNetCore;
using StackExchange.Profiling;
using PedidoStore.Core;
using PedidoStore.Core.Extensions;
using PedidoStore.Query;
using System.Globalization;
using PedidoStore.Infrastructure;
using PedidoStore.Application;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<GzipCompressionProviderOptions>(compressionOptions => compressionOptions.Level = CompressionLevel.Fastest)
    .Configure<JsonOptions>(jsonOptions => jsonOptions.JsonSerializerOptions.Configure())
    .Configure<RouteOptions>(routeOptions => routeOptions.LowercaseUrls = true)
    .AddHttpClient()
    .AddHttpContextAccessor()
    .AddResponseCompression(compressionOptions =>
    {
        compressionOptions.EnableForHttps = true;
        compressionOptions.Providers.Add<GzipCompressionProvider>();
    })
    .AddEndpointsApiExplorer()
    .AddApiVersioning(versioningOptions =>
    {
        versioningOptions.DefaultApiVersion = ApiVersion.Default;
        versioningOptions.ReportApiVersions = true;
        versioningOptions.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddApiExplorer(explorerOptions =>
    {
        explorerOptions.GroupNameFormat = "'v'VVV";
        explorerOptions.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200") // Endereço do front-end
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddOpenApi();
builder.Services.AddDataProtection();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(behaviorOptions =>
    {
        behaviorOptions.SuppressMapClientErrors = true;
        behaviorOptions.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(_ => { });

// Adding the application services in ASP.NET Core DI.
builder.Services
    .ConfigureAppSettings()
    .AddInfrastructure()
    .AddCommandHandlers()
    .AddQueryHandlers()
    .AddWriteDbContext(builder.Environment)
    .AddWriteOnlyRepositories()
    .AddReadDbContext()
    .AddReadOnlyRepositories()
    .AddCacheService(builder.Configuration)
    .AddHealthChecks(builder.Configuration)
    .AddDefaultCorrelationId();

// MiniProfiler for .NET
// https://miniprofiler.com/dotnet/
builder.Services.AddMiniProfiler(options =>
{
    // Route: /profiler/results-index
    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Dark;
    options.EnableServerTimingHeader = true;
    options.TrackConnectionOpenClose = true;
    options.EnableDebugMode = builder.Environment.IsDevelopment();
}).AddEntityFramework();

// Validating the services added in the ASP.NET Core DI.
builder.Host.UseDefaultServiceProvider((context, serviceProviderOptions) =>
{
    serviceProviderOptions.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    serviceProviderOptions.ValidateOnBuild = true;
});



// Using the Kestrel Server (linux).
builder.WebHost.UseKestrel(kestrelOptions => kestrelOptions.AddServerHeader = false);

// FluentValidation global configuration.
ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member?.Name;
ValidatorOptions.Global.LanguageManager = new LanguageManager { Enabled = true, Culture = new CultureInfo("en-US") };

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapOpenApi();

// Route: /scalar/v1
app.MapScalarApiReference(scalarOptions =>
{
    scalarOptions.DarkMode = true;
    scalarOptions.DotNetFlag = false;
    scalarOptions.HideDownloadButton = true;
    scalarOptions.HideModels = true;
    scalarOptions.Title = "Pedido API";
});

app.UseCors("AllowAngularApp");
app.UseErrorHandling();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseMiniProfiler();
app.UseCorrelationId();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAppAsync();