# build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY "./src/IdentityWebApi/IdentityWebApi.csproj" ./
RUN dotnet restore "IdentityWebApi.csproj"
COPY . .

WORKDIR ./src/IdentityWebApi
RUN dotnet build "IdentityWebApi.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "IdentityWebApi.csproj" -c Release -o /app/publish

# launch
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityWebApi.dll"]
