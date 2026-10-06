# Lab 01: Web + DB & Healthcheck Bağımlılığı

## 🎯 Amaç
Sadece `depends_on: [db]` yazmanın yetersiz olduğunu görmek ve veritabanı motoru istek kabul etmeye tam hazır olmadan uygulamanın başlamasını engelleyen `condition: service_healthy` yapısını öğrenmek.

---

## 🚀 Çalıştırma ve Test Adımları

1. **Servisleri başlatın:**
```bash
    docker compose up -d
```
2. **Konteynerların durumunu ve sağlık kontrolünü izleyin:**
```bash
    docker compose ps
```

**Gözlem:** healthcheck_db konteynerının durum kısmında (starting) yazar. Birkaç saniye sonra (healthy) durumuna geçtiği anda healthcheck_web servisinin başlatıldığını göreceksiniz.

3. **Web servisinin veritabanına erişimini test edin:**
```bash
    curl http://localhost:8000/
```

**Beklenen Çıktı:**
```bash
{"message":"DB baglantisi basarili! (Healthy DB Startup)","status":"success"}
```

## 🧹 Temizlik
```bash
    docker compose down -v
```