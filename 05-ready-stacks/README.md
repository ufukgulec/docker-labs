# 05: Hazır Docker Compose Şablonları (Ready Stacks)

Bu klasör, günlük geliştirmelerinizde veya yeni projelerinizde anında ayağa kaldırıp kullanabileceğiniz hazır **Production/Dev** Docker Compose şablonlarını içerir.

## 🛠 Şablon Listesi

| Servis / Stack | Erişim Portu | Web UI Portu | Varsayılan Kullanıcı / Şifre |
| :--- | :--- | :--- | :--- |
| **Portainer** | 9443 (HTTPS) | 9000 (HTTP) | İlk girişte oluşturulur |
| **MS SQL Server** | 1433 | - | `sa` / `StrongPassword123!` |
| **Redis + Commander** | 6379 | 8081 | Şifresiz |
| **Postgres + pgAdmin** | 5432 | 5050 | `admin@admin.com` / `adminpassword` |
| **RabbitMQ UI** | 5672 | 15672 | `guest` / `guest` |

## 🚀 Hızlı Kullanım
Kullanmak istediğiniz servisin klasörüne girip tek komutla çalıştırın:

```bash
cd 02-mssql-server
docker compose up -d
```
