using DocsManager.Core.Application;
using Microsoft.Extensions.DependencyInjection;

namespace DocsManager.Core
{
    public static class IoC
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<JwtService>();
            services.AddScoped<DocumentService>();

            return services;
        }
    }
}
