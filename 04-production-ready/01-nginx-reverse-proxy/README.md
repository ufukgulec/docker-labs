# Lab 01: Nginx Reverse Proxy & Load Balancing

## 🎯 Amaç
`docker compose scale` kullanarak uygulamanın instancelerini çoğaltmak ve Nginx üzerinden gelen yükü bu instancelara dağıtmak.

## 🚀 Çalıştırma ve Test Adımları
1. **Uygulamayı 3 adet instance (örnek) olacak şekilde scale ederek başlatın:**
```bash
    docker compose up -d --scale app=3
```
2. **Sayfaya art arda curl istekleri atarak yükün dağıtıldığını görün:**
```bash
    curl http://localhost/
    curl http://localhost/
    curl http://localhost/
```
**Beklenen Çıktı:** Her istekte yanıt veren Hostname (konteyner ID'si) değişecektir. Nginx isteği 3 farklı app konteynerına Round-Robin mantığıyla yönlendirir.

## 🧹 Temizlik
```bash
    docker compose down
```