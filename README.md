# Shop
ASP.NET Core shop on microservices

# Requirements
1. Docker
2. Kubernetes
3. Helm

# Build

## Concrete service
1. Open at git repository root
2. Execute `docker build -f "$pwd\Services\Catalog\Catalog.WebService\Dockerfile" --force-rm -t "tony/catalog:1.0.0" .`

## All services
1. Open at git repository root
2. Execute `docker-compose -f docker-compose.db.yml -f docker-compose.yml up -d -b`

# Tests

## Run specific test suit
`dotnet test --filter DisplayName~Unit`
`dotnet test --filter DisplayName~Integration`
`dotnet test --filter DisplayName~Component`

# Run tests with coverage
`dotnet test --collect:"XPlat Code Coverage"`
