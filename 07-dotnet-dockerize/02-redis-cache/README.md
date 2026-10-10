# 02 - Distributed Caching & Scaling with .NET 10, Redis, Nginx & Docker

Bu laboratuvar çalışması, .NET 10 ile geliştirilen Web API servislerinde **dağıtık önbellekleme (Distributed Caching)** ve **yatay ölçekleme (Horizontal Scaling)** yaklaşımlarını ele alır.

Docker üzerinde birden fazla API instance'ı, Nginx Reverse Proxy ve merkezi Redis önbelleği kullanılarak birden fazla servisin ortak cache verisine nasıl eriştiği uygulamalı olarak incelenir.

Çalışmanın temel amacı, API instance'larının bağımsız çalıştığı ve ortak önbellek verisini merkezi bir Redis sunucusu üzerinden paylaştığı bir mimari oluşturmaktır.

## 🏗️ Mimari ve Bileşenler

Sistem aşağıdaki bileşenlerden oluşur:

- **Nginx Reverse Proxy:** İstemci isteklerini API instance'larına yönlendirir.
- **.NET 10 Web API:** Ürün verilerini sunar ve Redis önbelleğini kullanır.
- **Redis:** API instance'larının ortak kullandığı dağıtık önbellek katmanıdır.
- **Docker Compose:** Servislerin, ağ bağlantılarının ve bağımlılıkların yönetilmesini sağlar.

### Mimari Diyagramı

```text
               +---------------------------------------+
               |                 Client                |
               +---------------------------------------+
                                   |
                                   | HTTP :5000
                                   v
               +---------------------------------------+
               |              nginx-proxy              |
               |           Reverse Proxy               |
               +---------------------------------------+
                    /              |              \
                   /               |               \
                  v                v                v
            +-----------+    +-----------+    +-----------+
            |  API #1   |    |  API #2   |    |  API #3   |
            |  .NET 10  |    |  .NET 10  |    |  .NET 10  |
            +-----------+    +-----------+    +-----------+
                  \                |                /
                   \               |               /
                    v              v              v
               +---------------------------------------+
               |              redis-cache              |
               |         Distributed Cache              |
               +---------------------------------------+
```

### Kullanılan Teknolojiler

| Teknoloji | Kullanım Amacı |
|---|---|
| C# / .NET 10 | Web API geliştirme |
| ASP.NET Core Web API | HTTP endpoint'lerinin oluşturulması |
| Redis | Merkezi dağıtık önbellekleme |
| StackExchange.Redis | .NET uygulamalarından Redis erişimi |
| Nginx | Reverse proxy ve yük dengeleme |
| Docker | Uygulamaların container ortamında çalıştırılması |
| Docker Compose | Servislerin ve ağ yapılandırmasının yönetilmesi |

## 🛠️ Tasarım Kararları

### 1. Merkezi Dağıtık Önbellekleme (Distributed Caching with Redis)

**Ne yapıldı?**

API uygulamasına `StackExchange.Redis` kütüphanesi entegre edilerek Redis üzerinden ortak bir önbellek mekanizması oluşturuldu.

`/api/products` endpoint'ine gelen isteklerde şu işlem sırası uygulanır:

1. Redis üzerinde ilgili cache anahtarı aranır.
2. Veri bulunursa (`Cache Hit`) doğrudan Redis üzerinden döndürülür.
3. Veri bulunamazsa (`Cache Miss`) simüle edilmiş veri kaynağından ürün bilgileri alınır.
4. Elde edilen veri Redis'e 30 saniyelik bir yaşam süresiyle (TTL) kaydedilir.
5. Ürün verisi istemciye döndürülür.

**Neden yapıldı?**

Birden fazla API instance'ının çalıştığı mimarilerde her instance'ın kendi belleğinde ayrı bir cache tutması, önbellek verilerinin farklılaşmasına neden olabilir.

Redis kullanılarak:

- API instance'ları arasında ortak cache verisi paylaşılır.
- Tekrarlanan veri erişimlerinde gecikme azaltılabilir.
- Veri kaynağına yapılan gereksiz erişimler azaltılabilir.
- Uygulama instance'larının birbirinden bağımsız ölçeklenmesi kolaylaştırılır.

> **Not:** Bu laboratuvarda veri kaynağı simüle edilmektedir. Gerçek bir veritabanı kullanıldığında cache invalidation, veri güncelleme stratejileri ve tutarlılık gereksinimleri ayrıca ele alınmalıdır.

### 2. Nginx Load Balancing ve Stateless Architecture

**Ne yapıldı?**

Nginx Reverse Proxy, istemci isteklerini `dotnet-api:8080` servisine yönlendirecek şekilde yapılandırıldı.

Docker Compose ile API servisinden üç instance çalıştırılması hedeflenir.

**Neden yapıldı?**

- HTTP trafiğini birden fazla API instance'ına dağıtmak.
- API instance'larının bağımsız çalışmasını sağlamak.
- Merkezi Redis önbelleği üzerinden ortak verilere erişmek.
- İstemcinin hangi API instance'ına bağlandığından bağımsız bir erişim noktası sunmak.

API yanıtında bulunan `nodeId` alanı, isteği işleyen container'ın kimliğini gösterir.

Farklı `nodeId` değerlerinin gözlemlenmesi, isteklerin farklı API instance'ları tarafından işlendiğini doğrulamaya yardımcı olur.

> **Önemli:** Redis kullanmak tek başına uygulamayı stateless yapmaz. Uygulamanın oturum, kimlik doğrulama ve diğer kullanıcıya özel durumları da instance'lar arasında doğru şekilde yönetilmelidir. Ayrıca Nginx'in ölçeklenmiş API instance'larını doğru keşfettiği doğrulanmalıdır.

### 3. Docker Health Checks ve Servis Bağımlılıkları

**Ne yapıldı?**

Docker Compose üzerinden Redis, API ve Nginx servisleri arasında başlangıç bağımlılıkları oluşturuldu.

Hedeflenen başlangıç sırası:

1. Redis servisi başlatılır ve sağlık kontrolünden geçmesi beklenir.
2. API servisleri Redis hazır olduktan sonra başlatılır.
3. Nginx, API servisleri sağlıklı duruma geldikten sonra başlatılır.

**Neden yapıldı?**

Dağıtık sistemlerde servisler farklı zamanlarda hazır hale gelebilir. Bir servisin diğer servis hazır olmadan bağlantı kurmaya çalışması, başlangıç sırasında bağlantı hatalarına neden olabilir.

Health check ve `depends_on` yapılandırmalarıyla:

- Servislerin başlangıç sırası kontrol edilir.
- Başlangıç aşamasındaki bağlantı problemleri azaltılır.
- Servislerin sağlık durumları izlenebilir.

> **Not:** `depends_on` ve `condition: service_healthy` başlangıç bağımlılıklarını yönetir. Redis veya API sonradan erişilemez hale geldiğinde uygulamanın otomatik olarak iyileşmesini garanti etmez. Bunun için ayrıca hata yönetimi, yeniden bağlantı ve dayanıklılık mekanizmaları gerekir.

## 🚀 Projeyi Çalıştırma

### Gereksinimler

- Docker Engine veya Docker Desktop
- Docker Compose
- .NET 10 SDK (uygulamayı container dışında geliştirmek ve çalıştırmak için)

### 1. Proje Dizinine Gitme

Terminal üzerinden ilgili proje dizinine geçin:

```bash
cd 07-dotnet-dockerize/02-redis-cache
```

### 2. Servisleri Başlatma

Üç API instance'ı ve Redis servisiyle sistemi build edip başlatın:

```bash
docker compose up --build --scale dotnet-api=3
```

Servisleri arka planda çalıştırmak için:

```bash
docker compose up --build --scale dotnet-api=3 -d
```

### 3. Container Durumlarını Kontrol Etme

Çalışan servisleri listeleyin:

```bash
docker compose ps
```

Servis loglarını takip edin:

```bash
docker compose logs -f
```

Yalnızca API loglarını incelemek için:

```bash
docker compose logs -f dotnet-api
```

Redis loglarını incelemek için:

```bash
docker compose logs -f redis-cache
```

## 🧪 Redis Önbelleğini Test Etme

Sistem çalışırken Nginx üzerinden `/api/products` endpoint'ine istek gönderin.

### 1. İlk İstek: Cache Miss

```bash
curl -i http://localhost:5000/api/products
```

Redis'te ilgili cache anahtarı bulunmuyorsa API, simüle edilmiş veri kaynağından ürünleri alır ve Redis'e kaydeder.

Örnek yanıt:

```json
{
  "source": "Database (Cache Miss - .NET 10)",
  "nodeId": "3091a4db5329",
  "data": "Laptop, Mouse, Keyboard, Monitor (Fetched from DB)"
}
```

**Açıklama:**

- `source`: Verinin cache yerine simüle edilmiş veri kaynağından alındığını gösterir.
- `nodeId`: İsteği işleyen API container'ının kimliğidir.
- `data`: Döndürülen ürün verisidir.

### 2. Sonraki İstekler: Cache Hit

Aynı endpoint'e tekrar istek gönderin:

```bash
curl -i http://localhost:5000/api/products
```

Cache anahtarı Redis'te mevcutsa API veriyi Redis üzerinden döndürür.

Örnek yanıt:

```json
{
  "source": "Redis Cache (.NET 10)",
  "nodeId": "263cc2945dbd",
  "data": "Laptop, Mouse, Keyboard, Monitor (Fetched from DB)"
}
```

**Açıklama:**

- `source`: Verinin Redis üzerinden alındığını gösterir.
- `nodeId`: İsteği işleyen API container'ının kimliğidir.
- `data`: Redis'te saklanan ürün verisidir.

İstekler farklı API instance'larına yönlendirilse bile aynı cache anahtarına erişildiği sürece ortak Redis verisi kullanılabilir.

> **Not:** Örnek container kimlikleri temsilidir. Cache süresi dolduğunda veya anahtar silindiğinde yeni bir `Cache Miss` oluşabilir.

### 3. İstekleri Art Arda Gönderme

Birden fazla istek göndererek cache davranışını gözlemleyin:

```bash
for i in $(seq 1 10); do
    echo "Request $i"
    curl -s http://localhost:5000/api/products
    echo
done
```

Beklenen davranış:

- İlk istek cache anahtarı yoksa `Cache Miss` oluşturur.
- Sonraki istekler cache anahtarı mevcut olduğu sürece `Cache Hit` döndürür.
- `nodeId` değişse bile cache verisi ortak Redis sunucusundan okunur.

### 4. Redis İçeriğini Kontrol Etme

Redis container'ına bağlanın:

```bash
docker compose exec redis-cache redis-cli
```

Cache anahtarlarını incelemek için:

```redis
SCAN 0
```

Bir cache anahtarının değerini görmek için:

```redis
GET <cache-key>
```

Cache anahtarının kalan yaşam süresini kontrol etmek için:

```redis
TTL <cache-key>
```

> `<cache-key>` ifadesini uygulamanın gerçekten kullandığı Redis anahtarıyla değiştirin. Redis üzerinde üretim ortamlarında tüm anahtarları listelemek için `KEYS *` kullanmak yerine `SCAN` tercih edilmelidir.

### 5. Container Kaynaklarını İzleme

API ve Redis servislerinin kaynak tüketimini inceleyin:

```bash
docker stats
```

Bu komut, container'ların CPU ve bellek kullanımını gözlemlemeye yardımcı olur.

## 🧪 Test Senaryoları

| Senaryo | İşlem | Beklenen Sonuç |
|---|---|---|
| İlk istek | Cache anahtarı yokken `/api/products` çağrılır. | `Cache Miss` oluşur ve veri Redis'e yazılır. |
| Tekrarlanan istek | Aynı endpoint tekrar çağrılır. | Cache anahtarı mevcutsa `Cache Hit` oluşur. |
| Farklı API instance'ı | İstek başka bir API container'ına yönlendirilir. | Ortak Redis anahtarı kullanılır. |
| Cache süresinin dolması | Cache anahtarının TTL değeri sona erer. | Sonraki istek yeni bir `Cache Miss` oluşturabilir. |
| Redis bağlantı hatası | Redis servisine erişim kesilir. | Uygulamanın tanımlanmış hata yönetimi davranışı gözlemlenir. |
| API ölçekleme | API instance sayısı değiştirilir. | Proxy yapılandırması desteklediği ölçüde istekler instance'lara dağıtılır. |

## 🧹 Projeyi Durdurma

Çalışan servisleri durdurmak ve Compose tarafından oluşturulan container'ları kaldırmak için:

```bash
docker compose down
```

Redis verilerini saklamak için kullanılan named volume'lar varsayılan olarak korunur.

Volume'ları da kaldırmak isterseniz:

```bash
docker compose down -v
```

> **Dikkat:** `-v` seçeneği, Compose tarafından yönetilen named volume'ları da kaldırır. Redis verilerinin kalıcı olarak silinmesine neden olabileceğinden bu komutu yalnızca verileri temizlemek istediğinizde kullanın.

## 📚 Öğrenme Hedefleri

Bu laboratuvar çalışması sonunda aşağıdaki konularda uygulamalı deneyim kazanılması hedeflenir:

- .NET 10 Web API geliştirme.
- Redis ve `StackExchange.Redis` kullanımı.
- Dağıtık önbellekleme ve cache hit/miss davranışı.
- Nginx Reverse Proxy ve yük dengeleme.
- Docker Compose ile yatay ölçekleme.
- Servis sağlık kontrolleri ve bağımlılık yönetimi.
- Cache TTL ve yaşam döngüsü yönetimi.
- Container loglarının ve kaynak tüketiminin incelenmesi.
- Dağıtık sistemlerde ortak durum yönetimi.

## 🎯 Sonuç

Bu laboratuvar, birden fazla .NET 10 Web API instance'ının merkezi Redis önbelleği üzerinden ortak veriye erişmesini ve Nginx Reverse Proxy arkasında çalışmasını uygulamalı olarak ele alır.

Redis ile dağıtık önbellekleme, Docker Compose ile yatay ölçekleme ve Nginx ile yük dengeleme bir araya getirilerek dağıtık uygulamaların temel mimari yaklaşımları incelenir.

Bu yapı, daha ileri aşamalarda gerçek veritabanı entegrasyonu, cache invalidation, dayanıklılık politikaları, gözlemlenebilirlik ve yük testleriyle genişletilebilir.