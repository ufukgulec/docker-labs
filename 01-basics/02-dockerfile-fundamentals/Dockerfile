FROM python:3.11-alpine

# Çalışma dizinini ayarla
WORKDIR /app

# Çevre değişkenlerini tanımla (Varsayılan değerler)
ENV PORT=8000
ENV APP_NAME="Fundamentals Lab"

# Dosyaları kopyala
COPY app.py .

# Port bilgisini dökümante et
EXPOSE 8000

# Uygulamayı başlat
CMD ["python", "app.py"]