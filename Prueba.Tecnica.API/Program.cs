using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prueba.Tecnica.Aplication.AppService;
using Prueba.Tecnica.Aplication.AppService.Interfaces;
using Prueba.Tecnica.Aplication.Dto;
using Prueba.Tecnica.Aplication.Dto.Validators;
using Prueba.Tecnica.Aplication.Settings;
using Prueba.Tecnica.Domain.Entities;
using Prueba.Tecnica.Domain.IRepository;
using Prueba.Tecnica.Infrastructure.Background;
using Prueba.Tecnica.Infrastructure.EntityFramework;
using Prueba.Tecnica.Infrastructure.Repository;
using Serilog;
using System.Security.Claims;
using System.Text;
using ILogger = Serilog.ILogger;

namespace Prueba.Tecnica
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Host.UseSerilog((ctx, lc) => lc
               .ReadFrom.Configuration(ctx.Configuration));

            AddAuthorization(builder.Services);
            AddAuthentication(builder.Services, configuration);

            AddServicesToContainer(builder.Services, configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<ExpirationDateBackgroundService>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<ExpirationDateBackgroundService>());
            AddSwagger(builder);

            var app = builder.Build();

            if (bool.Parse(configuration["LaunchMigrations"]))
                LaunchMigrations(app.Services);
            SuscribeToItemExpired(app.Services);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Método para añadir la configuración de swagger
        /// </summary>
        /// <param name="builder"></param>
        private static void AddSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your Token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                             },
                             new string[] {}
                     }
                 });
            });
        }

        /// <summary>
        /// Método para añadir todo lo relativo a inyección de dependencias
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddServicesToContainer(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<PruebaTecnicaDbContext>(options =>
                 options.UseNpgsql(configuration["ConnectionStrings:Development"])
            );

            services.Configure<JWTSettings>(configuration.GetSection("JWT"));

            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IItemAppService, ItemAppService>();
            services.AddTransient<ILoginAppService, LoginAppService>();

            services.AddTransient<IValidator<ItemCreateDto>, ItemCreateDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();
        }

        /// <summary>
        /// Método para configuar la Autenticación
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddAuthentication(IServiceCollection services, ConfigurationManager configuration)
        {
            services
                   .AddAuthentication(options =>
                   {
                       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                       options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                   })
                   .AddJwtBearer(options =>
                   {
                       options.SaveToken = true;
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                       };
                   });
        }

        /// <summary>
        /// Método para configurar la Autorización, a través de un sistema de policy
        /// </summary>
        /// <param name="services"></param>
        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, new string[] { "Usuario" }));
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, new string[] { "Administrador" }));
                options.AddPolicy("All", policy => policy.RequireClaim(ClaimTypes.Role, new string[] { "Usuario", "Administrador" }));
            }); ;
        }

        /// <summary>
        /// Método para lanzar automáticamente las migrations al inicio de la app
        /// </summary>
        /// <param name="serviceProvider"></param>
        private static void LaunchMigrations(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PruebaTecnicaDbContext>();

                var logger = scope.ServiceProvider.GetService<ILogger>();

                var pendingMigrations = db.Database.GetPendingMigrations().ToList();

                logger?.Information($"Try apply migrate: {string.Join(",", pendingMigrations)}");

                db.Database.Migrate();

                logger?.Information("Applied migration.");
            }
        }

        /// <summary>
        /// Discreto método que "escucha" los eventos ExpiredTime, y los añade al log.
        /// Por sencillez se hizo este pequeño método para probar el correcto funcionamiento del evento. 
        /// </summary>
        /// <param name="serviceProvider"></param>
        private static void SuscribeToItemExpired(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var expirationDateBgService = scope.ServiceProvider.GetRequiredService<ExpirationDateBackgroundService>();
                var logger = scope.ServiceProvider.GetService<ILogger>();

                if (expirationDateBgService != null)
                    expirationDateBgService.ItemExpired += (sender, args) =>
                    {
                        logger?.Information($"El artículo {args.Name} ha expirado con {args.ExpiredTime}");
                    };
            }
        }
    }
}