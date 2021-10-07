#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG APP_VERSION=1.0.0

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ApiGateways/ApiGateway.WebService/ApiGateway.WebService.csproj", "ApiGateways/ApiGateway.WebService/"]
RUN dotnet restore "ApiGateways/ApiGateway.WebService/ApiGateway.WebService.csproj"
COPY . .
WORKDIR "/src/ApiGateways/ApiGateway.WebService"
RUN dotnet build "ApiGateway.WebService.csproj" -c Release -o /app/build

FROM build AS publish
ARG APP_VERSION
RUN dotnet publish "ApiGateway.WebService.csproj" -c Release -o /app/publish /p:Version=$APP_VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.WebService.dll"]