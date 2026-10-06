# Lab 02: Dockerfile Temelleri

## 🎯 Amaç
Sıfırdan basit bir Dockerfile yazmak, `ENV` ile çevre değişkeni tanımlamak ve imaj derlemek.

## 🚀 Çalıştırma

1. **İmajı build edin:**
```bash
    docker build -t basic-python-app .
```
2. **Varsayılan ENV değerleriyle çalıştırın:**
```bash
    docker run -d --name app1 -p 8000:8000 basic-python-app
    curl http://localhost:8000
```
3. **ENV değerini dışarıdan ezerek başka bir portta çalıştırın:**
```bash
    docker run -d --name app2 -p 9090:9090 -e PORT=9090 -e APP_NAME="Custom App" basic-python-app
    curl http://localhost:9090
```
4. **Temizlik:**
```bash
   docker rm -f app1 app2
```