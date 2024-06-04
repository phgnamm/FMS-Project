FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release

COPY ["ChillDe.FMS.API/ChillDe.FMS.API.csproj", "ChillDe.FMS.API/"]
COPY ["ChillDe.FMS.Services/ChillDe.FMS.Services.csproj", "ChillDe.FMS.Services/"]
COPY ["ChillDe.FMS.Repositories/ChillDe.FMS.Repositories.csproj", "ChillDe.FMS.Repositories/"]
RUN dotnet restore API/ChillDe.FMS.API.csproj
COPY . .
WORKDIR "/ChillDe.FMS.API"
RUN dotnet build ChillDe.FMS.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /ChillDe.FMS.API
RUN dotnet publish ChillDe.FMS.API.csproj -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV PORT=8080
ENTRYPOINT ["dotnet", "ChillDe.FMS.API.dll"]
