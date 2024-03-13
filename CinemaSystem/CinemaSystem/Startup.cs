using CinemaSystem.Core;
using CinemaSystem.Core.Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;

namespace CinemaSystem
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CinemaSystem", Version = "v1" });
            });
            var connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<CinemaSystemContext>(x => x.UseSqlServer(connectionString));
            services.AddScoped<EMailSender>();
            services.AddScoped<TicketManager>();
            services.AddAutoMapper(typeof(Startup));
            services.AddCors(x =>
            {
                x.AddPolicy("local", y =>
                {
                    y.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            // проверять, что издатель токена совпадает с заданной в параметре validissuer строкой
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = AuthOptions.ISSUER,

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = AuthOptions.AUDIENCE,

                            // проверяется истекло ли время действия токена или нет.
                            ValidateLifetime = true,

                            // ключ безопасности, которым подписывается токен
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaSystem v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("local");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "qr",
                    pattern: "{controller=QR}/{action=Index}/{id?}");
            });

            var pathToImg = Path.Combine(env.WebRootPath, "posters");
            if (!Directory.Exists(pathToImg))
            {
                Directory.CreateDirectory(pathToImg);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(pathToImg),
                RequestPath = "/posters"
            });
            
        }
    }
}
