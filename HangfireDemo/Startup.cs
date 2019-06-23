using Hangfire;
using HangfireDemo.Data;
using HangfireDemo.Hangfire.Auth;
using HangfireDemo.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HangfireDemo
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IGenerateDailySalesReport, GenerateDailySalesReport>();

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DataContext")));
            services.AddHangfireServer();

            services.AddDbContext<HangfireDemoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("HangfireDemoContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var hangFireAuth = new DashboardOptions()
            {
                Authorization = new[]
                {
                    new HangFireAuthorization(app.ApplicationServices.GetService<IAuthorizationService>(),
                        app.ApplicationServices.GetService<IHttpContextAccessor>()     ),
                }
            };
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate<IGenerateDailySalesReport>(s => s.ForAllCustomers(), Cron.Daily);

            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("a Fire-and-forget job!"));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("a Continuation!"));

            app.UseHangfireDashboard("/hangfire", hangFireAuth);
        }
    }
}