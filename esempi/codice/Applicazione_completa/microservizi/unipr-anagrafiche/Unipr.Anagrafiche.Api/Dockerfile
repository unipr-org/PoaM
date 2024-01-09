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
COPY ["Unipr.Anagrafiche.Api/Unipr.Anagrafiche.Api.csproj", "Unipr.Anagrafiche.Api/"]
COPY ["Unipr.Anagrafiche.Business/Unipr.Anagrafiche.Business.csproj", "Unipr.Anagrafiche.Business/"]
COPY ["Unipr.Anagrafiche.Repository/Unipr.Anagrafiche.Repository.csproj", "Unipr.Anagrafiche.Repository/"]
COPY ["Unipr.Anagrafiche.Shared/Unipr.Anagrafiche.Shared.csproj", "Unipr.Anagrafiche.Shared/"]
RUN dotnet restore "./Unipr.Anagrafiche.Api/./Unipr.Anagrafiche.Api.csproj"
COPY . .
WORKDIR "/src/Unipr.Anagrafiche.Api"
RUN dotnet build "./Unipr.Anagrafiche.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unipr.Anagrafiche.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unipr.Anagrafiche.Api.dll"]