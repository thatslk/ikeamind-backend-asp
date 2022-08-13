using ikeamind_backend.Core.ExtensionsMethods;
using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Services;
using ikeamind_backend.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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


            services.AddDbContext<IkeaProductsContext>(
                options => {
                    options.UseSqlite(Configuration.GetConnectionString("IkeaDb"));
                });

            services.AddDbContext<IkeaUsersContext>(
                options => {
                    options.UseSqlite(Configuration.GetConnectionString("IkeaUsers"));
                });

            services.AddDbContext<IkeaJwtAccountsContext>(
                options => {
                    options.UseSqlite(Configuration.GetConnectionString("IkeaAccounts"));
                });

            services.AddScoped<IIkeaDbContext>(provider =>
                provider.GetService<IkeaProductsContext>());

            services.AddScoped<IIkeaUsersContext>(provider =>
                provider.GetService<IkeaUsersContext>());

            services.AddScoped<IIkeaAccountsContext>(provider =>
                provider.GetService<IkeaJwtAccountsContext>());

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
