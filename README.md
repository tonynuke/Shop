# Shop
ASP.NET Core shop on microservices

# Requirements
1. Docker
1. Kubernetes
1. Helm

# Build

## Concrete service
1. Open at git repository root
1. Execute `docker build -f "$pwd\Services\Catalog\Catalog.WebService\Dockerfile" --force-rm -t "tony/catalog:1.0.0" .`

## All services
1. Open at git repository root
1. Execute `docker-compose -f docker-compose.db.yml -f docker-compose.yml up -d -b`

# Deploy

1. Open service directory `Shop\helm\basket`
1. Execute `helm dependency update`
1. Execute `helm upgrade --install basket . --namespace=local --dry-run`

# Tests

## Run specific test suit
`dotnet test --filter DisplayName~Unit`
`dotnet test --filter DisplayName~Integration`
`dotnet test --filter DisplayName~Component`

# Run tests with coverage
`dotnet test --collect:"XPlat Code Coverage"`
