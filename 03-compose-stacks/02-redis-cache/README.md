# Lab 02: Flask + Redis Caching (Hit Counter)

## 🎯 Amaç
Redis bellek içi (in-memory) veri deposunu Docker Compose üzerinde bir web uygulamasıyla konfigüre etmek ve verileri volume kullanarak kalıcı hale getirmek.

---

## 🚀 Çalıştırma ve Test Adımları

1. **Stack'i başlatın:**
```bash
    docker compose up -d
```
2. **Sayfaya art arda istek atarak sayacın arttığını doğrulayın:**
```bash
    curl http://localhost:5000/
    curl http://localhost:5000/
```
**Beklenen Çıktı:**
```bash
{"message":"Bu sayfa toplam 2 kez ziyaret edildi!","status":"success","visit_count":2}
```
3. **Veri Kalıcılığı Testi (Redis'i Yeniden Başlatın):**
Konteyneri durdurup yeniden başlattığımızda verinin silinmediğini test edelim:

```bash
    docker compose restart redis
    curl http://localhost:5000/
```
**Sonuç:** Sayaç sıfırlanmaz ve 3 değerinden devam eder. redis_data volume'u veriyi korumuştur.

## 🧹 Temizlik
```bash
    docker compose down -v
```