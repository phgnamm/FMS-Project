FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release

COPY ["API/API.csproj", "API/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
RUN dotnet restore API/API.csproj
COPY . .
WORKDIR "/API"
RUN dotnet build API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /API
RUN dotnet publish API.csproj -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV PORT=8080
ENTRYPOINT ["dotnet", "API.dll"]
