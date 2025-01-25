﻿# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
COPY . .
WORKDIR /src/DY.Auth.Identity.Api
RUN dotnet restore "DY.Auth.Identity.Api.csproj"
RUN dotnet publish "DY.Auth.Identity.Api.csproj" -c Release -o /publish --no-restore

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
RUN groupadd -r nonrootgroup && useradd -r -g nonrootgroup nonrootuser
WORKDIR /app
COPY --from=build /publish ./
USER nonrootuser
ENTRYPOINT ["dotnet", "DY.Auth.Identity.Api.dll"]
