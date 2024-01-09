#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ConsumerExample.Api/ConsumerExample.Api.csproj", "src/ConsumerExample.Api/"]
COPY ["src/ConsumerExample.Business/ConsumerExample.Business.csproj", "src/ConsumerExample.Business/"]
COPY ["src/ConsumerExample.Repository/ConsumerExample.Repository.csproj", "src/ConsumerExample.Repository/"]
COPY ["src/ConsumerExample.Shared/ConsumerExample.Shared.csproj", "src/ConsumerExample.Shared/"]
RUN dotnet restore "src/ConsumerExample/ConsumerExample.csproj"
COPY . .
WORKDIR "/src/src/ConsumerExample"
RUN dotnet build "ConsumerExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsumerExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerExample.dll"]