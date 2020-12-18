using System.Linq;
using System.Reflection;
using CQRSlite.Caching;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Messages;
using CQRSlite.Queries;
using CQRSlite.Routing;
using Foosball.Domain.CommandHandlers;
using Foosball.Projections.Handlers;
using Foosball.Persistance.EventStore;
using Foosball.Persistance.ProjectionStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Foosball.Domain.Commands;
using Foosball.Domain.Events;
using Foosball.Persistance.ProjectionStore.Entites;
using Foosball.Projections.Queries;
using System.Collections.Generic;

namespace Foosball.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();

            //Add Cqrs services
            services.AddSingleton<Router>(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
            services.AddSingleton<IQueryProcessor>(y => y.GetService<Router>());
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            services.AddScoped<ISession, Session>();
            
            //CommandHandlers
            services.AddScoped<FoosballGameCommandHandlers>();
            services.AddScoped<ICommandHandler<CreateGame>, FoosballGameCommandHandlers>();
            services.AddScoped<ICommandHandler<RecordGoal>, FoosballGameCommandHandlers>();
            
            //Event Handlers
            services.AddScoped<FoosballGameDetailsHandler>();
            services.AddScoped<FoosballGameListHandler>();
            services.AddScoped<IEventHandler<FoosballGameCreated>, FoosballGameDetailsHandler>();
            services.AddScoped<IEventHandler<GoalScored>, FoosballGameDetailsHandler>();
            services.AddScoped<ICancellableEventHandler<FoosballGameCreated>, FoosballGameListHandler>();
            services.AddScoped<ICancellableEventHandler<SetFinished>, FoosballGameListHandler>();
            
            //Query Handlers
            services.AddScoped<IQueryHandler<GetFoosballGameDetails, FoosballGameDetails>, FoosballGameDetailsHandler>();
            services.AddScoped<ICancellableQueryHandler<GetFoosballGames, IReadOnlyCollection<FoosballGameListItem>>, FoosballGameListHandler>();

            //Persistance - Use In Memory Store
           
            services.AddSingleton<IProjectionStore, InMemoryProjectionStore>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Foosball.WebApi", Version = "v1" });
            });
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foosball.WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
