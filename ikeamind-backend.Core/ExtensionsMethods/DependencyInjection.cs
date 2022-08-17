using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ikeamind_backend.Core.ExtensionsMethods
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreInjections(
            this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<TokenGenerators>();
            return services;
        }
    }
}
