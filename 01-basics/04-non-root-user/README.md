# Lab 04: Non-Root User (Konteyner Güvenliği)

## 🎯 Amaç
Konteynerlerin varsayılan olarak `root` yetkisiyle çalışmasının yarattığı güvenlik riskini önlemek ve kısıtlı yetkili kullanıcı çalıştırmak.

## 🚀 Çalıştırma ve Test

1. **İmajı derleyin:**
```bash
    docker build -t secure-node-app .
```
2. **Konteynerı çalıştırıp çalışan kullanıcı ID'sini görün:**
```bash
    docker run -d --name secure-app -p 3000:3000 secure-node-app
    curl http://localhost:3000
```

(Çıktı UID: 1000 veya root olmayan bir ID olacaktır).

3. **Konteyner içinde yetki testi yapın (Dosya oluşturma engeli):**
```bash
   docker exec secure-app whoami
```

(Root sistem dizinlerine yazmaya çalıştığında izin vermediğini doğrulayın).

4. **Temizlik:**
```bash
   docker rm -f secure-app
```