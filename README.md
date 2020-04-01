# Hangfire ile Görev Zamanlama

### Task Scheduling (Görev Zamanlama) Nedir?
Belirli bir işi belirlenmiş bir zamanda otomatik başlatma durumuna görev zamanlama denir.

### Hangfire Nedir?
Hangfire yukarıdaki tanımda belirtilen görev zamanlama işini üstlenen bir açık kaynak kütüphanedir. Arkaplan işlemleri oluşturma, yürütme ve yönetmemize kolaylık sağlamaktadır ve mevcut arayüzü ile birlikte kullanım kolaylığı sunmaktadır. Depolama alanı olarak bir çok veritabanı : Sql server, Redis vb. ayrıca Ioc container ve unit test desteklemektedir. Oluşturulan arkaplan işlemleri mevcut kendi arayüz sisteminden silinebilir ve zamanı gelmeden de çalıştırılabilir.

### Hangfire Kurulumu
Projenize eklemeniz için aşağıdaki komutu Package Manager Console'da çalıştırmanız yeterli.

> PM> Install-Package Hangfire

### Hangfire Entegrasyon

Sql Server üzerinden işlemlerini gerçekleştirmesi için StartUp.cs içerisinde;
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHangfire(x => 
    	x.UseSqlServerStorage("Server=localhost;Database=HangfireDb;User Id=userName;Password=password;"));
    services.AddHangfireServer();
}

public void Configure(IApplicationBuilder app)
{
    app.UseHangfireDashboard();
}
```

Şeklinde ConfigureServices ve Configure fonksiyonlarına yukarıdaki bilgilerin girilmesi gerekli. Veritabanı için gerekli tablolar otomatik olarak oluşturulacaktır ve kullanıma hazır hale gelecektir.\
Programı çalıştırdığınızda arayüze erişmek için adres;

> https://<uygulama-adresi>/hangfire

### Hangfire Kullanım

##### - Bir Kez Çalışan İşler 

- Fire And Forget
	Bir kez ve hemen çalışan görev şeklidir. Kullanımı;

```csharp
var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Fire And Forget!"));
```

- Delayed
	Oluşturulmasından itibaren, belirlenen süre kadar sonra çalıştırılan görev şeklidir.

```csharp
var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"),TimeSpan.FromDays(7));
```

##### - Tekrarlayarak Çalışan İşler

- Recurring
	CRON süresine göre haftalık, günlük, saatlik vb. biçiminde tekrarlayarak çalışan görev şeklidir. 

```csharp
RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Daily);
```
	
- Continuation
	Belirli bir iş tamamlandığında belirtilen farklı bir işi çalıştıran görev şeklidir.

```csharp
BackgroundJob.ContinueWith(jobId, () => Console.WriteLine("Continuation!"));
```

### Cron Zamanlaması İncelemesi
Zamanlanmış görevlerin ne zaman çalışması gerektiği bilgisini, Hangfire kütüphanesiyle gelen Cron classı yardımıyla gerçekleştiririz. Cron classı içerisinde bulunan; saatlik, günlük, haftalık, aylık vb. methodlar yardımıyla kolayca çalışma zamanı bilgisi girebiliriz.

```csharp
RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Her ayın 5'inde saat 15:00'da çalışırım."),
	                Cron.Monthly(5,15),
	                TimeZoneInfo.Local);

RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Her salı 00:00'da çalışırım."),
	                Cron.Weekly(DayOfWeek.Tuesday),
	                TimeZoneInfo.Local);

RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Her salı 15:00'da çalışırım."),
	                Cron.Weekly(DayOfWeek.Tuesday,15),
	                TimeZoneInfo.Local);

RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Her gün saat 15:00'da çalışırım."),
	                Cron.Daily(15, 00),
	                TimeZoneInfo.Local);

RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Her saat başı çalışırım."),
	                Cron.Hourly(),
	                TimeZoneInfo.Local);

RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Dakikada bir kez çalışırım."),
	                Cron.Minutely(),
	                TimeZoneInfo.Local);
```

- Time Zone
	Cron zamanlaması yaparken gireceğimiz tarihin ve saatin doğru çalışması için mevcut time zone bilgisini mevcut bölgemiz olmalı. Local bilgisayardaki 'time zone' ayarların otomatik alınması için ; 
	> TimeZoneInfo.Local girebiliriz.