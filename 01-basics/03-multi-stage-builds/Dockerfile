# ---------------------------------------------------
# Stage 1: Build Aşaması (SDK içeren büyük imaj ~800MB)
# ---------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Önce sadece projeyi kopyalayıp restore yapıyoruz (Layer Caching)
COPY WebApi.csproj .
RUN dotnet restore

# Kalan tüm dosyaları kopyalayıp derliyoruz
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ---------------------------------------------------
# Stage 2: Runtime Aşaması (Sadece Runtime ~200MB)
# ---------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Build aşamasındaki derlenmiş çıktıları kopyala
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApi.dll"]