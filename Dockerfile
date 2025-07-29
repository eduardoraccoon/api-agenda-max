# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos necesarios y restaura dependencias
COPY ["api-iso-med-pg.csproj", "."]
RUN dotnet restore "api-iso-med-pg.csproj"

# Copia todo el código fuente
COPY . .

# Construye la aplicación en modo Release
RUN dotnet build "api-iso-med-pg.csproj" -c Release -o /app/build

# Publica la aplicación
RUN dotnet publish "api-iso-med-pg.csproj" -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia solo los archivos publicados
COPY --from=build /app/publish .

# Elimina cualquier archivo o directorio appsettings.json para evitar conflictos de montaje
RUN rm -rf /app/appsettings.json

# Establece la variable de entorno para entorno de producción
ENV ASPNETCORE_ENVIRONMENT=Production

# Expone los puertos necesarios
EXPOSE 80
EXPOSE 5138

# Punto de entrada
ENTRYPOINT ["dotnet", "api-iso-med-pg.dll"]