#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG APP_VERSION=1.0.0

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.WebApplication/Identity.WebApplication.csproj", "Services/Identity/Identity.WebApplication/"]
RUN dotnet restore "Services/Identity/Identity.WebApplication/Identity.WebApplication.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.WebApplication"
RUN dotnet build "Identity.WebApplication.csproj" -c Release -o /app/build

FROM build AS publish
ARG APP_VERSION
RUN dotnet publish "Identity.WebApplication.csproj" -c Release -o /app/publish /p:Version=$APP_VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.WebApplication.dll"]