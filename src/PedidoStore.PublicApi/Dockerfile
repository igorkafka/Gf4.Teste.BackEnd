FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

# Specifies whether .NET welcome and telemetry messages are displayed on the first run.
ENV DOTNET_NOLOGO=1

# Specifies whether data about the .NET tools usage is collected and sent to Microsoft.
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["src/Pedido.PublicApi/Pedido.PublicApi.csproj", "src/Pedido.PublicApi/"]
COPY ["src/Pedido.Application/Pedido.Application.csproj", "src/Pedido.Application/"]
COPY ["src/Pedido.Domain/Pedido.Domain.csproj", "src/Pedido.Domain/"]
COPY ["src/Pedido.Core/Pedido.Core.csproj", "src/Pedido.Core/"]
COPY ["src/Pedido.Infrastructure/Pedido.Infrastructure.csproj", "src/Pedido.Infrastructure/"]
COPY ["src/Pedido.Query/Pedido.Query.csproj", "src/Pedido.Query/"]
RUN dotnet restore "src/Pedido.PublicApi/Pedido.PublicApi.csproj"
COPY . .
WORKDIR "/src/src/Pedido.PublicApi"
RUN dotnet build "Pedido.PublicApi.csproj" --configuration Release --no-restore --nologo --output /app/build 

FROM build AS publish
RUN dotnet publish "Pedido.PublicApi.csproj" --configuration Release --no-restore --nologo --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pedido.PublicApi.dll"]