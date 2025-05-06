using DocsManager.Core.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
