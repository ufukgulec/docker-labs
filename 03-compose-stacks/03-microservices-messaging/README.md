# Lab 03: Mikroservisler Arası Mesajlaşma (RabbitMQ Pub/Sub)

## 🎯 Amaç
RabbitMQ kullanarak iki bağımsız servisi (Producer & Consumer) asenkron biçimde birbiriyle haberleştirmek ve mikroservis mimarisindeki kuyruk yapısını tecrübe etmek.

---

## 🚀 Çalıştırma ve Test Adımları

1. **Servisleri başlatın:**
```bash
    docker compose up -d
```
2. **Mesajlaşma akışını canlı izleyin:**
Producer'ın gönderdiği mesajların Consumer tarafından anlık yakalandığını görmek için logları takip edin:
```bash
    docker logs -f msg_consumer
```
**Beklenen Çıktı:**
```bash
[CONSUMER] Islendi: {'message': 'Islem #1 kurgulandi', 'task_id': 1}
```
3. **RabbitMQ Yönetim Paneline Bağlanın:**
Tarayıcınızdan http://localhost:15672 adresini açın:

- Kullanıcı Adı: guest

- Şifre: guest

- Queues sekmesinden task_queue isimli kuyruğun trafiğini ve mesaj oranlarını görsel olarak inceleyebilirsiniz.

## 🧹 Temizlik
```bash
    docker compose down -v
```