#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/UniprExample.Api/UniprExample.Api.csproj", "src/UniprExample.Api/"]
COPY ["src/UniprExample.Business/UniprExample.Business.csproj", "src/UniprExample.Business/"]
COPY ["src/UniprExample.Repository/UniprExample.Repository.csproj", "src/UniprExample.Repository/"]
COPY ["src/UniprExample.Shared/UniprExample.Shared.csproj", "src/UniprExample.Shared/"]
RUN dotnet restore "src/UniprExample.Api/UniprExample.Api.csproj"
COPY . .
WORKDIR "/src/src/UniprExample.Api"
RUN dotnet build "UniprExample.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniprExample.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniprExample.Api.dll"]