# Lab 01: Default Bridge vs Custom Bridge (DNS Resolution)

## 🎯 Amaç
Docker'ın varsayılan `bridge` ağı ile özel oluşturulan `custom bridge` ağı arasındaki temel farkı görmek. Özel ağlarda konteynerlar birbirine IP adresi yerine **konteyner isimleriyle** erişebilir.

---

### Test 1: Varsayılan Bridge Ağı (DNS Çalışmaz)

1. **Varsayılan ağda iki konteyner başlatın:**
```bash
    docker run -d --name default-app1 alpine tail -f /dev/null
    docker run -d --name default-app2 alpine tail -f /dev/null
```
2. **İsim ile ping atmayı deneyin (Başarısız olacaktır):**
```bash
    docker exec default-app1 ping -c 2 default-app2
```

**Sonuç:** ping: bad address 'default-app2' hatası alırsınız çünkü varsayılan ağda otomatik DNS yoktur.

### Test 2: Custom Bridge Ağı (DNS Çalışır)

1. **Yeni bir özel ağ oluşturun:**
```bash
    docker network create my-custom-net
```
2. **Konteynerları bu özel ağa bağlayarak başlatın:**
```bash
    docker run -d --name custom-app1 --network my-custom-net alpine tail -f /dev/null
    docker run -d --name custom-app2 --network my-custom-net alpine tail -f /dev/null
```
3. **İsim ile ping atmayı deneyin (Başarılı olacaktır):**
```bash
    docker exec custom-app1 ping -c 2 custom-app2
```
**Sonuç:** Docker'ın dahili DNS sunucusu custom-app2 ismini otomatik olarak IP adresine çözer.

## 🧹 Temizlik

```bash
    docker rm -f default-app1 default-app2 custom-app1 custom-app2
    docker network rm my-custom-net
```