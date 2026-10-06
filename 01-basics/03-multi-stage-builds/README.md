# Lab 03: Multi-Stage Builds (İmaj Boyutu Optimizasyonu)

## 🎯 Amaç
Geliştirme araçlarını (SDK) üretim ortamı imajından ayırarak güvenliği artırmak ve imaj boyutunu %70+ oranında küçültmek.

## 🚀 Çalıştırma ve İnceleme

1. **İmajı derleyin:**
```bash
    docker build -t dotnet-multistage-api .
```
2. **İmaj boyutlarını karşılaştırın:**
```bash
    docker images | grep -E "dotnet-multistage-api|dotnet"
```

> [!NOTE]
> dotnet/sdk imajı yaklaşık 800MB iken, ürettiğimiz dotnet-multistage-api imajı sadece ~210MB civarında olacaktır.

3. **Konteynerı çalıştırın ve test edin:**
```bash
    docker run -d -p 8080:8080 --name test-api dotnet-multistage-api
    curl http://localhost:8080
    docker rm -f test-api
```