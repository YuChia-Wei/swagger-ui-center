FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SwaggerUI.Center/SwaggerUI.Center.csproj", "src/SwaggerUI.Center/"]
RUN dotnet restore "src/SwaggerUI.Center/SwaggerUI.Center.csproj"
COPY . .
WORKDIR "/src/src/SwaggerUI.Center"
RUN dotnet build "SwaggerUI.Center.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SwaggerUI.Center.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SwaggerUI.Center.dll"]
