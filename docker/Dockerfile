# ==========================================
# Etapa 1: restauración y compilación
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY src/CloudForge.Api/CloudForge.Api.csproj src/CloudForge.Api/
COPY src/CloudForge.Application/CloudForge.Application.csproj src/CloudForge.Application/
COPY src/CloudForge.Domain/CloudForge.Domain.csproj src/CloudForge.Domain/
COPY src/CloudForge.Infrastructure/CloudForge.Infrastructure.csproj src/CloudForge.Infrastructure/
COPY src/CloudForge.SharedKernel/CloudForge.SharedKernel.csproj src/CloudForge.SharedKernel/

RUN dotnet restore src/CloudForge.Api/CloudForge.Api.csproj

COPY src/ src/

RUN dotnet publish src/CloudForge.Api/CloudForge.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ==========================================
# Etapa 2: imagen final de ejecución
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0

EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "CloudForge.Api.dll"]
