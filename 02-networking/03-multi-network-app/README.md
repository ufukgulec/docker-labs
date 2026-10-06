# Lab 03: Çok Katmanlı Ağ Mimarisi (3-Tier Architecture)

## 🎯 Amaç
Frontend sunucusunun veritabanına doğrudan erişmesini engelleyerek, tüm veri trafiğini **Backend API** üzerinden geçmeye zorlamak ve ağ izolesi sağlamak.

```text
[ User / Client ]
       |
  (Port 8080)
       v
[ tier_frontend ] <---> ( frontend-net ) <---> [ tier_api ] <---> ( backend-net ) <---> [ tier_db ]
```
## 🚀 Çalıştırma ve Izolasyon Testi
1. **Stack'i başlatın:**
```bash
    docker compose up -d
```
2. **Frontend üzerinden API ve DB durumunu test edin:**
```bash
    curl http://localhost:8080/api/data
```
- **Beklenen Çıktı:**
```bash
{"database_status":"Connected (Port 5432 Open)","service":"Backend API"}
```
- **Açıklama:** Frontend isteği API'ye proxy'ledi, API ise arka planda backend-net üzerinden DB'ye ulaştı.
3. **İzolasyon Testi: Frontend konteyneri DB'yi görebiliyor mu?**
Frontend konteyneri içerisinden doğrudan tier_db ismini çözmeyi (DNS) deneyelim:
```bash
    docker exec tier_frontend nslookup tier_db
```
- **Beklenen Çıktı:** nslookup: can't resolve 'tier_db'

- **Sonuç:** Frontend konteyneri backend-net ağında yer almadığı için veritabanının varlığından bile haberdar değildir! Güvenlik mimarisi doğrulandı.
## 🧹 Temizlik
```bash
    docker compose down
```