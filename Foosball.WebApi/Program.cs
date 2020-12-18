using System;
using CQRSlite.Routing;
using Foosball.Domain.CommandHandlers;
using Foosball.Projections.Handlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Foosball.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new RouteRegistrarProviderFactory());
        

        public class RouteRegistrarProviderFactory : IServiceProviderFactory<IServiceCollection>
        {
            public IServiceCollection CreateBuilder(IServiceCollection services)
            {
                return services;
            }

            public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
            {
                var serviceProvider = containerBuilder.BuildServiceProvider();
                var registrar = new RouteRegistrar(new Provider(serviceProvider));
                registrar.RegisterInAssemblyOf(typeof(FoosballGameCommandHandlers));
                registrar.RegisterInAssemblyOf(typeof(FoosballGameDetailsHandler));
                return serviceProvider;
            }
        }
         public class Provider : IServiceProvider
        {
            private readonly ServiceProvider _serviceProvider;
            private readonly IHttpContextAccessor _contextAccessor;

            public Provider(ServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
                _contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            }

            public object GetService(Type serviceType)
            {
                return _contextAccessor?.HttpContext?.RequestServices.GetService(serviceType) ??
                       _serviceProvider.GetService(serviceType);
            }
        }
    }
}
