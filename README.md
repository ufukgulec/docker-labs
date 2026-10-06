# Docker Labs

Docker ve Docker Compose konularını uygulamalı olarak öğrenmek, geliştirmek ve gerçek dünya senaryolarına yakın yapılar oluşturmak amacıyla hazırlanmış laboratuvar çalışmaları.

Bu repository; temel Docker kullanımından başlayarak container networking, volume yönetimi, Docker Compose, production-ready container yapıları ve hazır stack senaryolarına kadar ilerleyen örnekler içerir.

## 🎯 Amaç

Bu repository'nin amacı Docker ekosistemini yalnızca teorik olarak değil, gerçek uygulama senaryoları üzerinden ele almaktır.

Çalışmalar içerisinde;

* Docker temel komutları ve container yönetimi
* Image ve container lifecycle
* Volume ve persistent data yönetimi
* Container networking
* Container'lar arası iletişim
* Docker Compose
* Multi-container uygulamalar
* Environment configuration
* Production-ready Docker yapıları
* Hazır Docker stack'leri
* Docker ortamlarının temizlenmesi ve yönetimi

gibi konular uygulamalı olarak incelenmektedir.

---

## 📁 Repository Structure

```text
docker-labs/
│
├── 01-basics/
│   └── Docker'ın temel kullanımına yönelik lab çalışmaları
│
├── 02-networking/
│   └── Docker network ve container iletişimi
│
├── 03-compose-stacks/
│   └── Docker Compose ve multi-container uygulamalar
│
├── 04-production-ready/
│   └── Production ortamlarına uygun Docker yapılandırmaları
│
├── 05-ready-stacks/
│   └── Hazır ve tekrar kullanılabilir Docker stack örnekleri
│
├── 06-docker-cleaner/
│   └── Docker ortamı temizleme ve bakım araçları
│
└── README.md
```

---

## 🧱 01 - Basics

Docker'ın temel yapı taşlarını anlamaya yönelik çalışmalar.

Ele alınan konular:

* Docker image
* Container oluşturma ve çalıştırma
* Container lifecycle
* Port mapping
* Environment variables
* Volume kullanımı
* Bind mount
* Container logları
* Container yönetimi
* Image yönetimi

Örnek:

```bash
docker ps
docker images
docker logs <container>
docker exec -it <container> /bin/bash
```

---

## 🌐 02 - Networking

Docker container'larının birbirleriyle ve host sistemiyle nasıl iletişim kurduğunu inceleyen çalışmalar.

Konular:

* Docker bridge network
* Custom network
* Container-to-container communication
* DNS resolution
* Port publishing
* Internal vs external communication
* Network isolation

Örnek:

```bash
docker network ls
docker network create app-network
docker network inspect app-network
```

---

## 🧩 03 - Compose Stacks

Docker Compose kullanılarak birden fazla servisin birlikte yönetildiği örnekler.

Örneğin:

```text
Application
    │
    ├── API
    │
    ├── Database
    │
    └── Supporting Services
```

Bu bölümde;

* `compose.yaml`
* Services
* Networks
* Volumes
* Environment variables
* Service dependencies
* Multi-container applications

gibi konular ele alınmaktadır.

Temel komutlar:

```bash
docker compose up -d
```

```bash
docker compose down
```

```bash
docker compose ps
```

```bash
docker compose logs -f
```

---

## 🚀 04 - Production Ready

Production ortamlarına daha yakın Docker yapılarını ele alan çalışmalar.

Odak noktaları:

* Persistent storage
* Environment configuration
* Health checks
* Restart policies
* Resource management
* Network isolation
* Container security
* Service dependency management
* Production-oriented Compose configurations

Amaç yalnızca container çalıştırmak değil, sürdürülebilir ve yönetilebilir container altyapıları oluşturmaktır.

---

## 📦 05 - Ready Stacks

Belirli kullanım senaryolarında doğrudan kullanılabilecek Docker Compose stack örnekleri.

Bu bölüm zaman içerisinde;

* Database
* Web application
* API
* Reverse proxy
* Monitoring
* Development environments
* Supporting services

gibi farklı servis kombinasyonlarını içerecek şekilde genişletilecektir.

---

## 🧹 06 - Docker Cleaner

Docker ortamında kullanılmayan kaynakların tespit edilmesi ve temizlenmesine yönelik çalışmalar.

Örneğin:

```bash
docker system df
```

Kullanılmayan kaynakların temizlenmesi için:

```bash
docker system prune
```

> `prune` komutlarını production ortamlarında kullanmadan önce silinecek kaynaklar dikkatlice kontrol edilmelidir.

---

## 🛠️ Kullanılan Teknolojiler

* Docker
* Docker Compose
* Linux
* Bash
* Container Networking
* Persistent Volumes
* Reverse Proxy
* Microsoft SQL Server
* .NET / ASP.NET Core

---

## 💻 Gereksinimler

Lab çalışmalarını çalıştırabilmek için temel olarak aşağıdaki araçların kurulu olması gerekir:

* Docker Engine
* Docker Compose
* Git
* Linux / WSL / macOS / Windows

Docker kurulumunun ardından kontrol:

```bash
docker --version
```

```bash
docker compose version
```

---

## 🚦 Başlangıç

Repository'yi klonlayın:

```bash
git clone https://github.com/ufukgulec/docker-labs.git
```

Repository'ye geçin:

```bash
cd docker-labs
```

İlgili lab klasörüne girerek kendi README veya dokümantasyonundaki adımları uygulayın.

Örneğin:

```bash
cd 03-compose-stacks
```

Bir Compose projesini başlatmak için:

```bash
docker compose up -d
```

Çalışan servisleri kontrol etmek için:

```bash
docker compose ps
```

Logları takip etmek için:

```bash
docker compose logs -f
```

Çalışmayı durdurmak için:

```bash
docker compose down
```

---

## 📚 Öğrenme Yaklaşımı

Repository içerisindeki çalışmalar basitten karmaşığa doğru ilerleyecek şekilde organize edilmiştir:

```text
Docker Basics
      │
      ▼
Networking
      │
      ▼
Docker Compose
      │
      ▼
Multi-Container Applications
      │
      ▼
Production-Ready Configurations
      │
      ▼
Reusable Docker Stacks
      │
      ▼
Maintenance & Cleanup
```

Her lab mümkün olduğunca bağımsız tutulur ve ilgili konu için çalıştırılabilir örnekler ile dokümantasyon içerir.

---

## 🔗 Related Labs

Bu repository, farklı teknoloji alanlarındaki uygulamalı çalışmaların bir parçasıdır.

Örneğin SQL Server tarafındaki çalışmalar:

**sql-server-labs**

SQL Server kurulumu, database tasarımı, ilişkiler, indeksler, performans ve bakım senaryolarını içerir.

---

## 📌 Not

Bu repository sürekli geliştirilen bir laboratuvar alanıdır.

Yeni Docker senaryoları, Compose stack'leri, production yapılandırmaları ve farklı servis entegrasyonları zaman içerisinde eklenecektir.

Amaç yalnızca Docker komutlarını listelemek değil; **containerization, networking, orchestration ve production operasyonları hakkında pratik bir referans oluşturmak**tır.
