using MyMojo.AspNet.Identity.MongoDb.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMojo.AspNet.Identity.MongoDb
{
    /// <summary>
    ///     Extension methods for setting up Entity Framework related services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MongoDbServiceCollectionExtensions
    {
        /// <summary>
        ///     Registers the given context as a service in the <see cref="IServiceCollection" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services) 
        ///         {
        ///             var connectionString = "connection string to database";
        /// 
        ///             services.AddDbContext&lt;MyContext&gt;(ServiceLifetime.Scoped); 
        ///         }
        ///     </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static IServiceCollection AddMongoDbContext<TContext>(
            [NotNull] this IServiceCollection serviceCollection,
            ServiceLifetime contextLifetime)
            where TContext : IMongoDatabase
            => AddMongoDbContext<TContext>(serviceCollection, null, contextLifetime);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="IServiceCollection" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction"/> that provides the applications <see cref="IServiceProvider"/>.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own <see cref="IServiceProvider"/>
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services) 
        ///         {
        ///             var connectionString = "connection string to database";
        /// 
        ///             services
        ///                 .AddEntityFrameworkMongoDb()
        ///                 .AddDbContext&lt;MyContext&gt;((serviceProvider, options) => 
        ///                     options.UseMongoDb(connectionString)
        ///                            .UseInternalServiceProvider(serviceProvider)); 
        ///         }
        ///     </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="NoSqlDbContextOptions" /> and passes it to the base constructor of <see cref="DbContext"/>.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static IServiceCollection AddMongoDbContext<TContext>(
            [NotNull] this IServiceCollection serviceCollection,
            [CanBeNull] string connectionString,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
            where TContext : IMongoDatabase
        {
            serviceCollection.TryAddSingleton(p => DbContextOptionsFactory(p, connectionString));

            serviceCollection.AddSingleton<IDbContextOptions>(p => DbContextOptionsFactory(p, connectionString));
            
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContext),
                p => DbContextFactory<IMongoDatabase>(p.GetRequiredService<IDbContextOptions>()),
                contextLifetime));

            return serviceCollection;
        }

        private static TContext DbContextFactory<TContext>(IDbContextOptions noSqlDbContextOptions)
            where TContext : IMongoDatabase
        {
            var url = new MongoUrl(noSqlDbContextOptions.ConnectionString);
            var client = new MongoClient(url);

            return (TContext)client.GetDatabase(url.DatabaseName);
        }

        private static NoSqlDbContextOptions DbContextOptionsFactory(
            [NotNull] IServiceProvider applicationServiceProvider,
            [CanBeNull] string connectionString)
        {
            var options = new NoSqlDbContextOptions() { ConnectionString = connectionString };

            return options;
        }
    }

}


