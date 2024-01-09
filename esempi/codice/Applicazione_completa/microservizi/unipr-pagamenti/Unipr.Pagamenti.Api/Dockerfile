#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TOOOOMM_NUGET_USERNAME
ARG TOOOOMM_NUGET_PASSWORD
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Unipr.Pagamenti.Api/Unipr.Pagamenti.Api.csproj", "Unipr.Pagamenti.Api/"]
COPY ["Unipr.Pagamenti.Business/Unipr.Pagamenti.Business.csproj", "Unipr.Pagamenti.Business/"]
COPY ["Unipr.Pagamenti.Repository/Unipr.Pagamenti.Repository.csproj", "Unipr.Pagamenti.Repository/"]
COPY ["Unipr.Pagamenti.Shared/Unipr.Pagamenti.Shared.csproj", "Unipr.Pagamenti.Shared/"]
RUN dotnet restore "./Unipr.Pagamenti.Api/./Unipr.Pagamenti.Api.csproj"
COPY . .
WORKDIR "/src/Unipr.Pagamenti.Api"
RUN dotnet build "./Unipr.Pagamenti.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unipr.Pagamenti.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unipr.Pagamenti.Api.dll"]