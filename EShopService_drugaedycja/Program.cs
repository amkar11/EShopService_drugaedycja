using EShop.Application;
using EShop.Domain.Repositories;
using EShop.Domain.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Threading.Tasks;
namespace EShopService_drugaedycja;
public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("MyDB").EnableSensitiveDataLogging());
        builder.Services.AddTransient<ICreditCardService, CreditCardService>();
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse("localhost:6379", true);
            return ConnectionMultiplexer.Connect(configuration);
        });

        builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
        builder.Services.AddHostedService<RedisCleanupService>();
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddControllers().AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seeder = new EShopSeeder(context);
            await seeder.Initialize();
        }
        // Configure the HTTP request pipeline

        
        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }


        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}


