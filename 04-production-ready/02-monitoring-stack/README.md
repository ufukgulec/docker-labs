# Lab 02: Production Monitoring (cAdvisor + Prometheus + Grafana)

## 🎯 Amaç
Docker altyapısındaki tüm konteynerların CPU, RAM ve Network tüketimlerini cAdvisor ile toplayıp Grafana üzerinde görselleştirmek.
## 🚀 Çalıştırma ve Panel Erişimi
1. **Monitoring Stack'i başlatın:**
```bash
    docker compose up -d
```
2. **Panellere Erişin:**
- cAdvisor (Ham Metrikler): http://localhost:8080

- Prometheus: http://localhost:9090

- Grafana: http://localhost:3000 (Kullanıcı adı: admin, Şifre: admin)
3. **Grafana Konfigürasyonu:**
- Grafana'ya giriş yaptıktan sonra Data Sources kısmından Prometheus'u seçin.

- Connection URL kısmına http://prometheus:9090 yazıp kaydedin.

- Dashboard ekle kısmından Docker Container Metrics şablonunu (Örn: Dashboard ID 14282) içe aktarın (Import).
## 🧹 Temizlik
```bash
    docker compose down
```