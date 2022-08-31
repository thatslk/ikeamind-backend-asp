using ikeamind_backend.Core.ExtensionsMethods;
using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Services;
using ikeamind_backend.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ikeamind_backend.JwtAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            var authOptionsConfiguration = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsConfiguration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ikeamind_backend.JwtAuth", Version = "v1" });
            });

            services.AddCors(config =>
            {
                config.AddPolicy("AllowAll", p =>
                    p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddDbContext<IkeaMindAccountsContext>(
                options => {
                    options.UseNpgsql(Configuration.GetConnectionString("IkeaAccounts"));
                });

            services.AddDbContext<IkeaProductsAndUsersContext>(
                options => {
                    options.UseNpgsql(Configuration.GetConnectionString("IkeaProductsAndUsers"));
                });


            services.AddScoped<IIkeaMindAccountsContext>(provider =>
                provider.GetService<IkeaMindAccountsContext>());

            services.AddScoped<IIkeaProductsAndUsersContext>(provider =>
                provider.GetService<IkeaProductsAndUsersContext>());

            services.AddCoreInjections();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ikeamind_backend.JwtAuth v1"));
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
