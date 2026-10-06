# Lab 01: İlk Konteyner ve Temel CLI Komutları

## 🎯 Amaç
Arka planda (detached mode) bir web sunucusu çalıştırmak, port yönlendirmesi yapmak ve konteyner yaşam döngüsünü yönetmek.

## 🚀 Komutlar ve Test

1. **Nginx konteynerını arka planda (detached) ve port yönlendirmesiyle başlatın:**
```bash
   docker run -d --name my-first-web -p 8080:80 nginx:alpine
```
2. **Çalışan konteynerları listeleyin:**
```bash
   docker ps
```
3. **Sayfaya erişimi test edin:**
```bash
   curl http://localhost:8080
```
4. **Konteyner loglarını canlı takip edin:**
```bash
   docker logs -f my-first-web
```
5. **Konteyner içine terminal erişimi sağlayın:**
```bash
    docker exec -it my-first-web sh
```
6. **Konteynerı durdurun ve temizleyin:**
```bash
    docker stop my-first-web
    docker rm my-first-web
```