# 07 - .NET Dockerization Labs

Modern .NET uygulamalarının Docker ortamında containerize edilmesi, ölçeklenmesi ve dağıtık sistem mimarileriyle çalıştırılması üzerine uygulamalı laboratuvar çalışmaları.

Bu bölümde .NET Web API ve Worker Service uygulamalarının Docker ile paketlenmesi, Nginx Reverse Proxy arkasında çalıştırılması, yatay ölçeklenmesi ve Redis kullanılarak dağıtık önbellekleme mekanizmalarının uygulanması ele alınır.

Amaç, yalnızca uygulamaları container içerisinde çalıştırmak değil; servisler arası iletişim, yük dağılımı, sağlık kontrolleri, hata yönetimi ve ortak veri paylaşımı gibi konuları gerçekçi senaryolar üzerinden incelemektir.

## 🎯 Amaç ve Kapsam

Bu laboratuvar koleksiyonu aşağıdaki konulara odaklanır:

- .NET uygulamalarının Docker container'larına dönüştürülmesi.
- Dockerfile ve Docker Compose ile çok servisli uygulamaların yönetilmesi.
- Nginx Reverse Proxy ve Load Balancing yapılandırması.
- .NET Web API uygulamalarının yatay ölçeklenmesi.
- Worker Service üzerinden periyodik HTTP istekleri gönderilmesi.
- Health Checks ve servis bağımlılıklarının yönetilmesi.
- Polly ile HTTP dayanıklılık mekanizmalarının uygulanması.
- Redis ile dağıtık önbellekleme.
- Container loglarının ve kaynak tüketiminin incelenmesi.

## 🧰 Teknolojiler

| Teknoloji | Kullanım Amacı |
|---|---|
| C# | Uygulama geliştirme |
| .NET 10 | Modern .NET uygulamaları |
| ASP.NET Core Web API | HTTP servislerinin geliştirilmesi |
| .NET Worker Service | Arka plan görevleri ve periyodik HTTP istekleri |
| Docker | Uygulamaların container içerisinde çalıştırılması |
| Docker Compose | Çok servisli uygulamaların yönetilmesi |
| Nginx | Reverse proxy ve yük dengeleme |
| Redis | Dağıtık önbellekleme |
| StackExchange.Redis | .NET uygulamalarından Redis erişimi |
| Polly / Microsoft.Extensions.Http.Resilience | HTTP dayanıklılık politikaları |
| Health Checks | Servis sağlık durumlarının kontrol edilmesi |

> Not: Her teknoloji bütün laboratuvarlarda kullanılmaz. Kullanılan bileşenler ilgili laboratuvarın README dosyasında ayrıca açıklanır.

## 📂 Laboratuvarlar

### 01. Scaled .NET Web API & Worker Client

**Klasör:** `01-api-client`

.NET Web API ve Worker Client uygulamalarının Docker Compose üzerinde çalıştırılması, Nginx Reverse Proxy arkasında birden fazla API instance'ına ölçeklenmesi ve istemci tarafında HTTP dayanıklılık mekanizmalarının uygulanması ele alınır.

**İncelenen konular:**

- Docker ile .NET uygulamalarının containerize edilmesi.
- Nginx Reverse Proxy ve yük dengeleme.
- Docker Compose ile yatay ölçekleme.
- ASP.NET Core Health Checks.
- `depends_on` ve `service_healthy` koşulları.
- Polly v8 ve `AddStandardResilienceHandler()`.
- Worker Client üzerinden periyodik HTTP istekleri.
- API instance'larının `NodeId` üzerinden gözlemlenmesi.

**Çalıştırma:**

```bash
cd 07-dotnet-dockerize/01-api-client

docker compose up --build --scale dotnet-api=3
```

**Doğrulama:**

```bash
docker compose ps

docker compose logs -f dotnet-client
```

**Öğrenme hedefi:** Birden fazla .NET API instance'ının Nginx arkasında çalıştırılması ve HTTP istemci dayanıklılığının uygulanması.

[Laboratuvar detayları →](./01-api-client/README.md)

---

### 02. Distributed Caching with Redis

**Klasör:** `02-redis-cache`

Birden fazla .NET 10 Web API instance'ının merkezi Redis önbelleğini paylaşması ve Nginx Reverse Proxy arkasında çalışması ele alınır.

**İncelenen konular:**

- Redis ile dağıtık önbellekleme.
- `StackExchange.Redis` kullanımı.
- Cache Hit ve Cache Miss davranışları.
- Cache anahtarlarının TTL ile yönetilmesi.
- Nginx Reverse Proxy ve yük dengeleme.
- Docker Compose ile API instance'larının ölçeklenmesi.
- Redis ve API servisleri arasındaki bağımlılıklar.
- Container logları ve kaynak tüketiminin incelenmesi.

**Çalıştırma:**

```bash
cd 07-dotnet-dockerize/02-redis-cache

docker compose up --build --scale dotnet-api=3
```

**Doğrulama:**

```bash
curl http://localhost:5000/api/products
```

İlk istekte cache anahtarı mevcut değilse veri kaynağından veri alınarak Redis'e yazılır. Sonraki isteklerde cache anahtarı geçerliyse veri Redis üzerinden döndürülür.

**Öğrenme hedefi:** Yatayda ölçeklenen API instance'ları arasında ortak önbellek kullanımı ve dağıtık cache mekanizmasının çalışma mantığının anlaşılması.

[Laboratuvar detayları →](./02-redis-cache/README.md)

---

## 🏗️ Genel Mimari Yaklaşım

Laboratuvarlarda uygulanan mimari, ihtiyaca göre birden fazla API instance'ı ve yardımcı servislerden oluşabilir.

Örnek mimari:

```text
                    +----------------------+
                    |        Client        |
                    +----------------------+
                              |
                              | HTTP
                              v
                    +----------------------+
                    |     Nginx Proxy      |
                    |   Load Balancing     |
                    +----------------------+
                       /       |       \
                      /        |        \
                     v         v         v
                +--------+ +--------+ +--------+
                | API #1 | | API #2 | | API #3 |
                +--------+ +--------+ +--------+
                     \        |        /
                      \       |       /
                       v      v      v
                    +------------------+
                    |      Redis       |
                    | Distributed Cache|
                    +------------------+
```

Bu diyagram, Redis kullanan ikinci laboratuvarın mimarisini temsil eder. İlk laboratuvarda Redis bulunmaz; API ve Worker Client arasındaki iletişim Nginx üzerinden gerçekleştirilir.

## 🚀 Başlangıç

### Gereksinimler

Laboratuvarları çalıştırmak için aşağıdaki araçlar önerilir:

- Docker Engine veya Docker Desktop.
- Docker Compose.
- Git.
- .NET 10 SDK.

.NET SDK, uygulamaları container dışında geliştirmek ve çalıştırmak için gereklidir. Yalnızca önceden hazırlanmış container imajlarını çalıştırmak için SDK'nın yerel makinede kurulu olması zorunlu değildir.

### Depoyu Klonlama

```bash
git clone https://github.com/ufukgulec/docker-labs.git

cd docker-labs/07-dotnet-dockerize
```

İlgili laboratuvarın klasörüne geçerek o laboratuvara ait README dosyasındaki adımları izleyin.

## 🔍 Genel Doğrulama Komutları

### Çalışan Container'ları Listeleme

```bash
docker compose ps
```

### Servis Loglarını Görüntüleme

```bash
docker compose logs -f
```

### Container Kaynaklarını İzleme

```bash
docker stats
```

### Servisleri Durdurma

İlgili laboratuvar klasöründe aşağıdaki komutu çalıştırın:

```bash
docker compose down
```

> Her laboratuvarın kendi Docker Compose yapılandırması bulunabilir. Komutları çalıştırmadan önce ilgili laboratuvarın dizininde olduğunuzdan emin olun.

## 📚 Öğrenme Kazanımları

Bu laboratuvar koleksiyonu tamamlandığında aşağıdaki konularda uygulamalı deneyim kazanılması hedeflenir:

- .NET uygulamalarının Docker ile paketlenmesi.
- Docker Compose ile çok servisli mimarilerin kurulması.
- Reverse proxy ve yük dengeleme mekanizmalarının anlaşılması.
- API instance'larının yatay ölçeklenmesi.
- Servis bağımlılıkları ve sağlık kontrollerinin yapılandırılması.
- HTTP istemcilerinde retry, timeout ve circuit breaker mekanizmalarının uygulanması.
- Redis üzerinden dağıtık önbellekleme.
- Cache Hit, Cache Miss ve TTL kavramlarının uygulanması.
- Container logları ve kaynak tüketiminin incelenmesi.
- Dağıtık uygulamalarda servis iletişimi ve ortak veri yönetimi.

## 🛣️ Geliştirme Yol Haritası

Laboratuvar koleksiyonu aşağıdaki konularla genişletilebilir:

- [ ] Multi-stage Dockerfile optimizasyonları.
- [ ] Docker image boyutunun azaltılması.
- [ ] Non-root container çalıştırma ve güvenlik iyileştirmeleri.
- [ ] Docker Compose profilleri ve ortam bazlı yapılandırma.
- [ ] Graceful shutdown ve container yaşam döngüsü yönetimi.
- [ ] OpenTelemetry ile distributed tracing ve metrics.
- [ ] Prometheus ve Grafana ile gözlemlenebilirlik.
- [ ] Yük testi ve yatay ölçekleme performans karşılaştırmaları.
- [ ] Redis cache invalidation stratejileri.
- [ ] CI/CD pipeline ile otomatik build ve test süreçleri.

## 📌 Notlar

Bu depo, Docker ve .NET ekosistemindeki farklı mimari yaklaşımları uygulamalı olarak incelemek amacıyla hazırlanmıştır.

Örnekler, öğrenme ve deney yapma odaklıdır. Üretim ortamında kullanılmadan önce güvenlik, erişilebilirlik, kaynak sınırları, izleme, hata yönetimi ve veri kalıcılığı gereksinimleri ayrıca değerlendirilmelidir.

Her laboratuvarın çalışma mantığı, kullanılan teknolojileri ve test adımları kendi README dosyasında açıklanır.