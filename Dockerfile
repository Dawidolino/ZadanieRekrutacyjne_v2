

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5049
EXPOSE 7182

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ZadanieRekrutacyjne/ZadanieRekrutacyjne.csproj", "ZadanieRekrutacyjne/"]
RUN dotnet restore "./ZadanieRekrutacyjne/ZadanieRekrutacyjne.csproj"
COPY . .
WORKDIR "/src/ZadanieRekrutacyjne"
RUN dotnet build "./ZadanieRekrutacyjne.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ZadanieRekrutacyjne.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZadanieRekrutacyjne.dll"]