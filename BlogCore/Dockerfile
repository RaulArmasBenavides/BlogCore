# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia csproj a sus carpetas correctas
COPY ["BlogCore/BlogCore.csproj", "BlogCore/"]
COPY ["BlogCore.AccesoDatos/BlogCore.AccesoDatos.csproj", "BlogCore.AccesoDatos/"]
COPY ["BlogCore.Models/BlogCore.Models.csproj", "BlogCore.Models/"]
COPY ["BlogCore.Utilidades/BlogCore.Utilidades.csproj", "BlogCore.Utilidades/"]

# Restaura SOLO el proyecto web (evitas el .dcproj)
RUN dotnet restore "BlogCore/BlogCore.csproj"

# Copia el resto del código
COPY . .

# Limpia bin/obj que vengan del host (evita errores raros)
RUN find . -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +

# Publica
RUN dotnet publish "BlogCore/BlogCore.csproj" -c Release -o /app/out /p:UseAppHost=false

# ---- Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "BlogCore.dll"]
