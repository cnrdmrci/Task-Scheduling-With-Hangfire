using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace TaskSchedulingWithHangfire
{
    public class HangfireConfig
    {
        public static void Initialize()
        {
            //Görev çalıştırma sırasında hata alındığında default olarak 10 kez daha deneme yapar.
            //Bu kod tekrar deneme sayısını günceller. 3 deneme sonrasında başarısız olarak atanır.
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute(){Attempts = 3});

            //Fire and Forget
            var jobIdFireForget = BackgroundJob.Enqueue(
                () => Console.WriteLine("Fire And Forget!")
                );
            var jobIdFireForget2 = BackgroundJob.Enqueue<Example>(x=>
                x.Run()
            );

            //Delayed
            var jobIdDelayed = BackgroundJob.Schedule(
                () => Console.WriteLine("Delayed!"), 
                TimeSpan.FromSeconds(10)
                );

            var jobIdDelayed2 = BackgroundJob.Schedule<Example>(x=>
                    x.Run(),
                    TimeSpan.FromSeconds(10)
                    );

            //Recurring
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Minutely);
            RecurringJob.AddOrUpdate<Example>(x=> x.Run(), Cron.Hourly);

            //Continuation
            BackgroundJob.ContinueJobWith(jobIdDelayed, () => Console.WriteLine("Continuation!"));
            BackgroundJob.ContinueJobWith<Example>(jobIdDelayed, x=>x.Run());

            //-----
            RecurringJob.AddOrUpdate(
                () =>
                    Console.WriteLine("Her salı 15:00'da çalışırım."),
                Cron.Weekly(DayOfWeek.Tuesday, 15),
                TimeZoneInfo.Local);

        }

        //Hata alınması durumunda 5 kez daha başarılı oluncaya kadar deneme yapar.
        //Başarısız olursa, başarısız olarak atanır.
        [AutomaticRetry(Attempts = 5)]
        public static void doSomething()
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Fire And Forget!"));
        }

        public static void DatabaseBackUp()
        {
            RecurringJob.AddOrUpdate<Database>(x => x.BackUp(), Cron.Daily);
        }
    }
}
