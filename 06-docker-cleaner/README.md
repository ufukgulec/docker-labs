# 🧹 Docker Cleanup & Disk Management Guide

Docker çalışırken eski imajları, durdurulmuş konteynerları ve asılı (dangling) volume'ları sistemde tutarak disk alanını hızlıca doldurabilir. Bu dizinde hem manuel komutları hem de otomatik temizleme araçlarını bulabilirsiniz.

---

## 📊 Disk Alanı Analizi

Öncelikle Docker'ın diskte ne kadar yer kapladığını görmek için:

```bash
docker system df
```
Detaylı (hangi imaj/volume kaç GB) görmek için:
```bash
docker system df -v
```

## ⚡ En Çok Kullanılan Temizlik Komutları

1. **Güvenli Temizlik (Sadece Kullanılmayan Öğeler)**

Çalışan konteynerlara ve onların verilerine dokunmaz; sadece durdurulmuş konteynerları, kullanılmayan ağları ve etiketsiz (dangling) imajları siler:
```bash
docker system prune
```

2. **Tam Temizlik (Nükleer Seçenek ☢️)**

Durdurulmuş tüm konteynerları, hiçbir konteyner tarafından kullanılmayan tüm imajları, ağları ve volume'ları (VERİ KAYBI RİSKİ!) siler:
```bash
docker system prune -a --volumes -f
```

## 🎯 Parça Parça Temizlik Komutları
- **Konteyner Temizliği:** Durdurulmuş tüm konteynerları siler.
```bash
docker container prune
```
- **İmaj Temizliği:** Kullanılmayan (dangling/unregistered) tüm imajları siler.
```bash
docker image prune -a
```
- **Volume Temizliği:** Hiçbir konteynere bağlı olmayan tüm bağımsız volume'ları siler.
```bash
docker volume prune
```
- **Ağ Temizliği:** Kullanılmayan tüm özel ağları siler.
```bash
docker network prune
```

## 🚀 Script veya Compose İle Çalıştırma

### Yöntem A: Script İle İnteraktif Temizlik
```bash
chmod +x clean.sh
./clean.sh
```
### Yöntem B: Arka Plan Servisi Olarak Otomatik Temizlik (24 Saatlik Periyot)
```bash
docker compose up -d
```