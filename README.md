# Shop
ASP.NET Core shop on microservices

# Build

docker build -f "E:\Projects\MongoTest\Services\Catalog\WebService\Dockerfile" --force-rm -t webservice  --label "com.microsoft.created-by=visual-studio" --label "com.microsoft.visual-studio.project-name=WebService" .

docker-compose -f docker-compose.db.yml -f docker-compose.yml up -d -b

# Tests

## Run specific test suit
dotnet test --filter DisplayName~Unit
dotnet test --filter DisplayName~Integration
dotnet test --filter DisplayName~Component

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
