FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Siena.slnx ./
COPY src/Siena.Api/Siena.Api.csproj src/Siena.Api/
COPY src/Siena.Application/Siena.Application.csproj src/Siena.Application/
COPY src/Siena.Domain/Siena.Domain.csproj src/Siena.Domain/
COPY src/Siena.Infrastructure/Siena.Infrastructure.csproj src/Siena.Infrastructure/
COPY tests/Siena.Api.Tests/Siena.Api.Tests.csproj tests/Siena.Api.Tests/
COPY tests/Siena.Application.Tests/Siena.Application.Tests.csproj tests/Siena.Application.Tests/
RUN dotnet restore Siena.slnx
COPY . .
RUN dotnet publish src/Siena.Api/Siena.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS development
WORKDIR /src
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
COPY . .
EXPOSE 8080
CMD ["dotnet", "watch", "--project", "src/Siena.Api/Siena.Api.csproj", "run", "--urls", "http://0.0.0.0:8080"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Siena.Api.dll"]
