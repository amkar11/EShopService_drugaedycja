using EShop.Application;
using EShop.Domain.Repositories;
using EShop.Domain.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using User.Domain.Models;
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

        //JWT Config
        var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../UserService"));
        var config = new ConfigurationBuilder()
            .SetBasePath(rootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var jwtSettings = config.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSettings);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText("../data/public.key"));
                var publicKey = new RsaSecurityKey(rsa);

                var jwtConfig = jwtSettings.Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig!.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = publicKey,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
            options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
        });

        builder.Services.AddControllers().AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Wpisz token w formacie: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                        Scheme="Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

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


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}


