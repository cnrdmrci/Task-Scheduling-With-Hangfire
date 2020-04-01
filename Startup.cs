using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskSchedulingWithHangfire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHangfire(x =>
                x.UseSqlServerStorage("Server=localhost;Database=HangfireDb;User Id=userName;Password=password;"));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHangfireDashboard();
            //Üst satýrdaki gibi default ayarlarý kullanabilir yada aþaðýdaki gibi ayarlama yapabiliriz.
            app.UseHangfireDashboard("/hangfire",new DashboardOptions()
            {
                DashboardTitle = "Dashboard Title",
                AppPath = "Home/About", //Dashboard üzerinden ana siteye dönme adresi
                Authorization = new []{new HangfireDashboardAuthorizationFilter()}
            });

            //Yadaa----------------------------------------------
            /*
            //app.UseHangfireServer();
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                //Yapýlacak görevler default olarak 15 saniyede bir kontrol edilir. Burada 60 saniye yaptýk.
                SchedulePollingInterval = TimeSpan.FromSeconds(60),

                //Arkaplanda çalýþacak görev sayýsýný güncelleyebiliriz.
                WorkerCount = Environment.ProcessorCount * 3
            });
            */

            HangfireConfig.Initialize();
            //Her gün çalýþsýn ve veritabanýnýn yedeðini alsýn.
            HangfireConfig.DatabaseBackUp();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
