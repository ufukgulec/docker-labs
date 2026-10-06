# Lab 02: Network Isolation (Ağ İzolasyonu)

## 🎯 Amaç
`internal: true` parametresiyle veritabanı konteynerının dış internetle bağlantısını tamamen kesmek ve sadece izin verilen `web` konteyneri üzerinden erişilmesini sağlamak.

---

## 🚀 Çalıştırma ve Test Adımları

1. **Ağ yapısını ve servisleri başlatın:**
```bash
    docker compose up -d
```
2. **Veritabanının (isolated_db) dış internete çıkamadığını doğrulayın:**
```bash
    docker exec isolated_db ping -c 2 google.com
```
**Beklenen Çıktı:** ping: bad address 'google.com' veya Network is unreachable hatası. DB tamamen izoledir.
3. **Web servisinin hem dışarıya yanıt verdiğini hem DB ile konuşabildiğini görün:**

- Web Servisi Testi:
```bash
    curl http://localhost:8080/
    #Çıktı: Web Servisi Ayakta!
```
- Web -> DB Bağlantı Testi:
```bash
    curl http://localhost:8080/test-db
    # Çıktı: DB Sunucusuna Erisim Basarili (Port 5432 Open)
```
4. **Dışarıdan doğrudan DB'ye erişilemediğini doğrulayın:**

Lokal makinenizden 5432 portuna bağlanmayı deneyin. Port dışarıya expose edilmediği ve ağ izole olduğu için bağlantı reddedilecektir.

## 🧹 Temizlik

```bash
   docker compose down
```
