FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orchestrator.ApiHost/Orchestrator.ApiHost.csproj", "Orchestrator.ApiHost/"]
RUN ls -la
RUN dotnet restore "Orchestrator.ApiHost/Orchestrator.ApiHost.csproj"
COPY . .
WORKDIR "/src/Orchestrator.ApiHost"
RUN dotnet build "Orchestrator.ApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Orchestrator.ApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orchestrator.ApiHost.dll"]
