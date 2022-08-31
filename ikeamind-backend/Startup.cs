using ikeamind_backend.Core.ExtensionsMethods;
using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Services;
using ikeamind_backend.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;

namespace ikeamind_backend
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

            var authOptionsConfiguration = Configuration.GetSection("Auth").Get<AuthOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptionsConfiguration.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptionsConfiguration.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = authOptionsConfiguration.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "dist";
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ikeamind_backend", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ikeamind_backend v1"));
            }

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpaStaticFiles();

            app.UseSpa(builder =>
            {
                if (env.IsDevelopment())
                    builder.UseProxyToSpaDevelopmentServer("http://localhost:3000/");
            });
        }
    }
}
