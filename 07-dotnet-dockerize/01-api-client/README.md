# 07 - Scaled .NET Web API & Worker Client Lab with Nginx & Polly

Bu laboratuvar çalışması, modern .NET ekosisteminde geliştirilen mikroservislerin ve dağıtık sistemlerin Docker üzerinde **ölçeklenebilir (scalable), dayanıklı (resilient) ve üretim ortamına uygun (production-ready)** bir şekilde nasıl yapılandırılacağını ele alır.

Çalışma kapsamında birden fazla .NET Web API instance'ı, Nginx Reverse Proxy ve periyodik HTTP istekleri gönderen bir .NET Worker Client kullanılarak yük dengeleme, sağlık kontrolleri ve HTTP dayanıklılığı uygulamaları incelenir.

## 🏗️ Mimari ve Bileşenler

Sistem, istemci isteklerini karşılayan bir Nginx Reverse Proxy, yatayda ölçeklenebilen birden fazla .NET Web API instance'ı ve arka planda periyodik HTTP istekleri gönderen bir .NET Worker Client bileşeninden oluşur.

```text
                 +-----------------------+
                 |     dotnet-client     |
                 |    (.NET Worker)      |
                 +-----------------------+
                            |
                            | HTTP
                            | API_BASE_URL
                            v
                 +-----------------------+
                 |      nginx-proxy      |
                 |    Load Balancer      |
                 +-----------------------+
                      /      |      \
                     /       |       \
                    v        v        v
              +----------+ +----------+ +----------+
              |  API #1  | |  API #2  | |  API #3  |
              |  .NET    | |  .NET    | |  .NET    |
              +----------+ +----------+ +----------+
                     Horizontal Scaling
```

### Kullanılan Teknolojiler

| Teknoloji | Kullanım Amacı |
|---|---|
| C# / .NET | Web API ve Worker Client geliştirme |
| ASP.NET Core Web API | HTTP endpoint'lerinin oluşturulması |
| .NET Worker Service | Periyodik HTTP isteklerinin gönderilmesi |
| Docker | Uygulamaların container ortamında çalıştırılması |
| Docker Compose | Servislerin ve ağ yapılandırmasının yönetilmesi |
| Nginx | Reverse proxy ve yük dengeleme |
| Polly v8 | HTTP dayanıklılığı ve hata yönetimi |
| Health Checks | Servis sağlık durumlarının izlenmesi |

## 🛠️ Tasarım Kararları

### 1. Nginx Reverse Proxy ve Load Balancing

**Ne yapıldı?**

İstemci ile API instance'ları arasına Nginx Reverse Proxy katmanı yerleştirildi. İstemci, API container'larına doğrudan bağlanmak yerine `http://nginx-proxy` adresi üzerinden istek gönderir.

**Neden yapıldı?**

- **Yük dengeleme:** Gelen HTTP istekleri birden fazla API instance'ına dağıtılır.
- **Soyutlama:** İstemcinin API container'larının adreslerini bilmesine gerek kalmaz.
- **Esneklik:** API instance'larının sayısı değiştiğinde istemci yapılandırmasının değiştirilmesi gerekmez.
- **Merkezi erişim:** API servislerine erişim tek bir proxy noktası üzerinden sağlanır.

Nginx, uygun upstream yapılandırmasıyla varsayılan Round-Robin yük dengeleme algoritmasını kullanabilir.

API instance'larının sayısını artırmak için:

```bash
docker compose up --build --scale dotnet-api=3
```

> **Not:** Nginx yapılandırmasının ölçeklenen API instance'larını doğru şekilde keşfetmesi gerekir. Docker Compose servis adlarının DNS çözümlemesi ve Nginx upstream yapılandırması bu noktada önemlidir.

### 2. Health Checks ve Servis Bağımlılıkları

**Ne yapıldı?**

API projesine Health Checks desteği eklenerek `/health` endpoint'i oluşturuldu. Docker Compose üzerinden servislerin sağlık durumlarının kontrol edilmesi ve servis bağımlılıklarının yönetilmesi hedeflendi.

API container'ının sağlık kontrolü için Dockerfile içerisinde `curl` aracı kullanılabilir.

Docker Compose yapılandırmasında `depends_on` ve `condition: service_healthy` kullanılarak ilgili servislerin sağlıklı duruma gelmesi beklenebilir.

**Neden yapıldı?**

- Servislerin hazır olmadan birbirlerine istek göndermesini önlemek.
- Başlangıç sırasından kaynaklanan bağlantı hatalarını azaltmak.
- Servislerin sağlık durumlarını izlemek.
- Dağıtık sistemlerde başlangıç aşamasındaki bağlantı problemlerini daha kolay tespit etmek.

> **Önemli:** `depends_on` ile `service_healthy` koşulu, servislerin başlangıç sırasını ve ilk sağlık kontrolünü yönetir. Daha sonra oluşabilecek arızaları otomatik olarak çözmez. Ayrıca Docker Compose'un sağlık kontrolü, Nginx'in arızalı bir API instance'ını otomatik olarak upstream listesinden çıkaracağını garanti etmez.

### 3. Polly v8 ile HTTP Dayanıklılığı

**Ne yapıldı?**

Worker Client tarafında `Microsoft.Extensions.Http.Resilience` paketi ve `AddStandardResilienceHandler()` metodu kullanılarak standart HTTP dayanıklılık politikaları yapılandırıldı.

Örnek kullanım:

```csharp
builder.Services
    .AddHttpClient("ApiClient", client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["API_BASE_URL"]!
        );
    })
    .AddStandardResilienceHandler();
```

**Neden yapıldı?**

Dağıtık sistemlerde ağ bağlantısı sorunları, geçici servis hataları ve container yeniden başlatmaları gibi durumlarda HTTP isteklerinin daha kontrollü yönetilmesi amaçlanır.

Standart dayanıklılık işleyicisi şu mekanizmaları içerir:

- **Retry:** Uygun geçici hatalarda isteğin yeniden denenmesi.
- **Circuit Breaker:** Belirli hata koşullarında başarısız servise yapılan isteklerin geçici olarak durdurulması.
- **Timeout:** İsteklerin belirlenen süre sınırları içinde tamamlanmasının sağlanması.
- **Rate Limiting:** Yapılandırmaya bağlı olarak eşzamanlı ve yoğun isteklerin kontrol edilmesi.

> **Not:** `AddStandardResilienceHandler()` standart bir dayanıklılık yapılandırması sağlar. Tek tek politikaların davranışları, eşik değerleri ve zaman aşımı süreleri kullanılan paket sürümüne ve yapılandırmaya bağlıdır. Yeniden denemeler, özellikle POST gibi yan etkili HTTP işlemlerinde idempotency dikkate alınarak tasarlanmalıdır.

### 4. Yatay Ölçekleme (Horizontal Scaling)

**Ne yapıldı?**

Docker Compose kullanılarak aynı Web API servisinin birden fazla container instance'ı çalıştırılması sağlandı.

```bash
docker compose up --build --scale dotnet-api=3
```

Bu komut, `dotnet-api` servisinden üç instance oluşturmayı hedefler.

**Neden yapıldı?**

- İstek yükünün birden fazla API instance'ına dağıtılmasını sağlamak.
- Yatay ölçekleme yaklaşımını uygulamalı olarak incelemek.
- Container tabanlı servis mimarisini anlamak.
- Yük dağılımını ve API instance'larının davranışlarını gözlemlemek.

API yanıtlarında `Environment.MachineName` kullanılarak isteği işleyen container'ın kimliği gösterilebilir.

Örnek yanıt:

```json
{
  "message": "Request processed successfully.",
  "nodeId": "container-instance-id"
}
```

Farklı `nodeId` değerlerinin gözlemlenmesi, isteklerin farklı instance'lar tarafından işlendiğini doğrulamaya yardımcı olur.

> **Not:** Docker Compose ile yatay ölçekleme yapmak tek başına yüksek erişilebilirlik veya daha yüksek performans garantisi vermez. Gerçek kazanım; uygulamanın durum yönetimine, kaynak kapasitesine, yük dağılımına ve altyapı yapılandırmasına bağlıdır.

## 🚀 Projeyi Çalıştırma

### Gereksinimler

Başlamadan önce aşağıdaki araçların kurulu olması gerekir:

- Docker Engine veya Docker Desktop
- Docker Compose
- .NET SDK (uygulamaları container dışında geliştirmek ve çalıştırmak için)

### 1. Proje Dizinine Gitme

Terminal üzerinden ilgili proje dizinine geçin:

```bash
cd 07-dotnet-dockerize/01-api-client
```

### 2. Servisleri Başlatma

Üç API instance'ı ile uygulamayı build edip çalıştırın:

```bash
docker compose up --build --scale dotnet-api=3
```

Komut, Docker Compose yapılandırmasındaki servisleri oluşturur ve başlatır.

### 3. Container Durumlarını Kontrol Etme

Çalışan servislerin durumunu görüntüleyin:

```bash
docker compose ps
```

Container loglarını takip etmek için:

```bash
docker compose logs -f
```

Yalnızca Worker Client loglarını incelemek için:

```bash
docker compose logs -f dotnet-client
```

### 4. Yük Dağılımını Doğrulama

Worker Client loglarını takip edin ve periyodik HTTP isteklerinin sonuçlarını inceleyin.

Kontrol edilmesi gerekenler:

- Worker Client'ın belirlenen aralıklarla HTTP isteği göndermesi.
- İsteklerin Nginx Reverse Proxy üzerinden API servislerine ulaşması.
- API yanıtlarında farklı `NodeId` değerlerinin görülmesi.
- Servislerin Docker sağlık durumlarının `healthy` olması.
- Geçici bağlantı hatalarında dayanıklılık politikalarının beklenen şekilde çalışması.

Örnek log çıktısı:

```text
Request successful | Node: api-instance-1
Request successful | Node: api-instance-2
Request successful | Node: api-instance-3
Request successful | Node: api-instance-1
```

> Örnek loglar temsili niteliktedir. İsteklerin dağılımı, Nginx yapılandırmasına ve istek sayısına bağlıdır; her instance'ın sırayla çağrılması garanti edilmez.

## 🧪 Test Senaryoları

Aşağıdaki senaryolarla sistemin davranışı incelenebilir.

| Senaryo | İşlem | Beklenen Sonuç |
|---|---|---|
| Normal çalışma | Servisleri başlatın. | Worker Client, API'ye istek gönderir. |
| Yük dağılımı | Birden fazla API instance'ı çalıştırın. | İstekler yapılandırmaya uygun şekilde farklı instance'lara dağıtılır. |
| Servis başlangıcı | Servisleri yeniden başlatın. | Sağlık kontrolü ve bağımlılık yapılandırması başlangıç davranışını etkiler. |
| Geçici HTTP hatası | API'ye erişimi geçici olarak kesin. | Yapılandırmaya uygun dayanıklılık politikaları devreye girer. |
| Ölçek artırma | API instance sayısını artırın. | Proxy yapılandırması destekliyorsa yeni instance'lar trafiğe dahil edilir. |
| Log inceleme | Container loglarını takip edin. | İstek sonuçları ve hata durumları incelenebilir. |

## 🧹 Projeyi Durdurma

Çalışan servisleri durdurmak için:

```bash
docker compose down
```

Bu komut, Compose tarafından oluşturulan container'ları ve ağları kaldırır. Named volume'lar varsayılan olarak korunur.

## 📚 Öğrenme Hedefleri

Bu laboratuvar çalışması sonunda aşağıdaki konularda uygulamalı deneyim kazanılması hedeflenir:

- Docker Compose ile çok servisli .NET uygulamalarının yönetimi.
- Nginx Reverse Proxy ve yük dengeleme yapılandırması.
- .NET Worker Service ile periyodik HTTP istekleri gönderilmesi.
- Polly v8 ve `Microsoft.Extensions.Http.Resilience` ile HTTP dayanıklılığı.
- ASP.NET Core Health Checks ve Docker sağlık kontrolleri.
- Docker Compose üzerinden yatay ölçekleme.
- Container loglarıyla servis davranışlarının izlenmesi.
- Dağıtık sistemlerde başlangıç sırası, bağlantı hataları ve servis bağımlılıklarının yönetimi.

## 🎯 Sonuç

Bu çalışma, .NET tabanlı servislerin Docker ortamında bir proxy katmanı arkasında çalıştırılmasını, birden fazla API instance'ına ölçeklenmesini ve HTTP istemci tarafında dayanıklılık mekanizmalarının kullanılmasını ele alır.

Nginx, Docker Compose, Health Checks ve Polly bileşenleri birlikte değerlendirilerek dağıtık uygulamalarda servis iletişimi, yük dağılımı ve hata yönetimi konularında uygulamalı bir altyapı oluşturulur.