using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using EShopService_drugaedycja;
using Microsoft.AspNetCore.Hosting;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace EShopService.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                // Завантажуємо appsettings.Test.json, який знаходиться в каталозі з тестами
                var testSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");
                config.AddJsonFile(testSettingsPath, optional: false);  // або optional: true, якщо файл не обов'язковий
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IConnectionMultiplexer));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton<IConnectionMultiplexer>(sp =>
                {
                    return ConnectionMultiplexer.Connect("localhost:6379,defaultDatabase=15");
                });
            });
        }
    }
}
