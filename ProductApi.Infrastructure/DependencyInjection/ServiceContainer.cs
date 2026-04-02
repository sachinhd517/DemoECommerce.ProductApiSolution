using eCommerce.SharedLibrary.DependancyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity
            

            // Add authentication scheme

            SharedServiceContainer.AddSharedService<ProductDBContext>(services, config, config["MySerilog:FineName"]!);

            // Create Dependency Injection (DI)
            services.AddScoped<IProduct, ProductRepository>();

            return services;
            
        }

        public static IApplicationBuilder UseInfrastructrePolicy(this IApplicationBuilder app)
        {
            // Register middelware such as:
            // Global Exception: handles external errors.
            // Listen to only Api Gateway: blocks all outsider calls;

            return app;
        }
    }
}
