using System;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Core.Services;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AspNetCoreMentoring.DI
{
    public static class DependencyManager
    {
        public static void InstallApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ISupplierService, SuppliersService>();
        }

        public static void InstallInfrastractureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("NorthwindConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));

            services.AddScoped<IProductsQuery, ProductsQuery>();
        }
    }
}
