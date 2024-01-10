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
COPY ["Unipr.Imu.Api/Unipr.Imu.Api.csproj", "Unipr.Imu.Api/"]
COPY ["Unipr.Imu.Business/Unipr.Imu.Business.csproj", "Unipr.Imu.Business/"]
COPY ["Unipr.Imu.Repository/Unipr.Imu.Repository.csproj", "Unipr.Imu.Repository/"]
COPY ["Unipr.Imu.Shared/Unipr.Imu.Shared.csproj", "Unipr.Imu.Shared/"]
RUN dotnet restore "./Unipr.Imu.Api/./Unipr.Imu.Api.csproj"
COPY . .
WORKDIR "/src/Unipr.Imu.Api"
RUN dotnet build "./Unipr.Imu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unipr.Imu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unipr.Imu.Api.dll"]