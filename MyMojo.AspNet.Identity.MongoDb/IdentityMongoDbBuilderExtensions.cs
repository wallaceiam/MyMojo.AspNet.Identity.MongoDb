using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using MyMojo.AspNet.Identity.MongoDb.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMojo.AspNet.Identity.MongoDb
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for adding entity framework stores.
    /// </summary>
    public static class IdentityMongoDbBuilderExtensions
    {
        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddMongoDbStores<TContext>(this IdentityBuilder builder, string userCollection = "users",
            string roleCollection = "roles")
            where TContext : IMongoDatabase
        {
            builder.Services.TryAddSingleton(p => DbStoreOptionsFactory(p, userCollection, roleCollection));

            builder.Services.TryAdd(GetDefaultServices(builder.UserType, builder.RoleType, typeof(TContext)));
            return builder;
        }

        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <typeparam name="TKey">The type of the primary key used for the users and roles.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddMongoDbStores<TContext, TKey>(this IdentityBuilder builder, string userCollection = "users",
            string roleCollection = "roles")
            where TContext : IMongoDatabase
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAddSingleton(p => DbStoreOptionsFactory(p, userCollection, roleCollection));

            builder.Services.Add(GetDefaultServices(builder.UserType, builder.RoleType, typeof(TContext), typeof(TKey)));
            return builder;
        }

        private static IDbStoreOptions DbStoreOptionsFactory(
            [NotNull] IServiceProvider applicationServiceProvider,
            [NotNull] string userCollection, [NotNull] string roleCollection)
        {
            var options = new NoSqlDbStoreOptions() { UserCollection = userCollection, RoleCollection = roleCollection };

            return options;
        }


        private static IServiceCollection GetDefaultServices(Type userType, Type roleType, Type contextType, Type keyType = null)
        {
            Type userStoreType;
            Type roleStoreType;
            keyType = keyType ?? typeof(string);
            userStoreType = typeof(UserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
            roleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, contextType, keyType);

            var services = new ServiceCollection();
            services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(userType),
                userStoreType);
            services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(roleType),
                roleStoreType);
            return services;
        }
    }
}
