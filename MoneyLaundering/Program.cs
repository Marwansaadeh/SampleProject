using BankAppData.Models;
using BankWebbApp.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyLaundering.Service;
using System;

namespace MoneyLaundering
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();
            Configuration = config;
            using IHost host = CreateHostBuilder(Configuration).Build();
            RunMoneyLundary(host.Services);
            host.Run();

        }
        static IHostBuilder CreateHostBuilder(IConfiguration config) =>
           Host.CreateDefaultBuilder()
               .ConfigureServices((services) =>
                   services.AddTransient<ICustomerRepository, CustomerRepository>()
                   .AddTransient<ITransctionsRepository, TransctionsRepository>()
                    .AddTransient<IBatchService, BatchService>()
                    .AddTransient<ISuspectPeople, SuspectPeople>()
                     .AddTransient<IEmailSender, MailSender>()
                    .AddTransient<MoneyLundary>()
               .AddDbContext<BankAppDataContext>(options =>
              options.UseSqlServer(config.GetConnectionString("DefaultConnection")))

               );
        static void RunMoneyLundary(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            MoneyLundary money = provider.GetRequiredService<MoneyLundary>();
            money.Run();

        }

    }
}
