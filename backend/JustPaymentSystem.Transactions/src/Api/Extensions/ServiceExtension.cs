using Application;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Api.Extensions;

public static class ServiceExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterRepositories()
        {
            services.AddMemoryCache()
            services.AddSingleton<ICacheService, MemoryCacheService>(); // Need change lifetime to Scoped for Redis!!!!!
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionReadRepository, TransactionReadRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPagedResponseFactory, PagedResponseFactory>();
            return services;
        }

        public IServiceCollection RegisterServices()
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);
            return services;
        }

        public IServiceCollection RegisterMapper()
        {
            var assemblies = typeof(ApplicationAssembly).Assembly;
            var types = assemblies.GetTypes().Where(x => x is { IsClass: true, IsAbstract: false });

            foreach (var implementation in types)
            {
                var interfaces = implementation
                    .GetInterfaces()
                    .Where(i =>
                        i.Name.EndsWith("Mapper") &&
                        i != typeof(IDisposable));

                foreach (var service in interfaces)
                {
                    services.AddSingleton(service, implementation);
                }
            }

            return services;
        }
    }
}
