# Online Appointment System

Bu proje, müşterilerin çevrimiçi randevu oluşturmasını ve yönetmesini sağlayan bir sistemdir. ASP.NET Core kullanılarak geliştirilmiş, çok katmanlı bir mimari üzerine inşa edilmiştir.

## Docker ile Çalıştırma

Projeyi Docker kullanarak kolayca çalıştırabilirsiniz. Aşağıdaki adımları izleyin:

### Gereksinimler

- Docker
- Docker Compose

### Kurulum ve Çalıştırma

1. Repoyu klonlayın:
   ```
   git clone https://github.com/your-username/OnlineAppointmentSystem.git
   cd OnlineAppointmentSystem
   ```

2. Docker Compose ile uygulamayı başlatın:
   ```
   docker-compose up -d
   ```

3. Uygulamaya aşağıdaki URL'ler üzerinden erişebilirsiniz:
   - Web Uygulaması: http://localhost:5002
   - API: http://localhost:5000
   - Swagger API Dokümantasyonu: http://localhost:5000/swagger

### Servisler

- **Web**: ASP.NET Core MVC uygulaması (port 5002)
- **API**: RESTful API servisi (port 5000)
- **SQL Server**: Veritabanı (port 1433)
- **Redis**: Önbellek servisi (port 6379)

### Kullanıcı Bilgileri

Sisteme ilk erişimde oluşturulan varsayılan admin kullanıcısı:
- E-posta: admin@example.com
- Şifre: Admin123!

## Geliştirme

Eğer projeyi kendi ortamınızda geliştirmek istiyorsanız:

1. .NET 8.0 SDK'yı yükleyin
2. SQL Server kurulumunu gerçekleştirin
3. Redis kurulumunu yapın (opsiyonel - UseRedisCache=false yapabilirsiniz)
4. Bağlantı dizelerini `appsettings.json` içerisinde güncelleyin
5. Projeyi çalıştırın:
   ```
   dotnet run --project OnlineAppointmentSystem.Web
   ```

## Migrasyon

Docker ortamında migrasyon otomatik olarak uygulanacaktır. Kendi ortamınızda aşağıdaki komutu çalıştırabilirsiniz:

```
dotnet ef database update --project OnlineAppointmentSystem.DataAccess
```
