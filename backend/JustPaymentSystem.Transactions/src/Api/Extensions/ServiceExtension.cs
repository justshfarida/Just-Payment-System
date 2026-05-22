using Application;
using Application.Common.Interfaces;
using Infrastructure.Repositories;

namespace Api.Extensions;

public static class ServiceExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterRepositories()
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionReadRepository, TransactionReadRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
