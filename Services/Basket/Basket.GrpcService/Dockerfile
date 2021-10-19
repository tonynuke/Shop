#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG APP_VERSION=1.0.0

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Basket/Basket.GrpcService/Basket.GrpcService.csproj", "Services/Basket/Basket.GrpcService/"]
RUN dotnet restore "Services/Basket/Basket.GrpcService/Basket.GrpcService.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Basket.GrpcService"
RUN dotnet build "Basket.GrpcService.csproj" -c Release -o /app/build

FROM build AS publish
ARG APP_VERSION
RUN dotnet publish "Basket.GrpcService.csproj" -c Release -o /app/publish /p:Version=$APP_VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.GrpcService.dll"]