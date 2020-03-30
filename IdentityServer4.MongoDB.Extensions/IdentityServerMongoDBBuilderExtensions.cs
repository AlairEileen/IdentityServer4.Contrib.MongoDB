using IdentityServer4.MongoDB;
using IdentityServer4.MongoDB.Configuration;
using IdentityServer4.MongoDB.DbContexts;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Models;
using IdentityServer4.MongoDB.Options;
using IdentityServer4.MongoDB.Services;
using IdentityServer4.MongoDB.Storage;
using IdentityServer4.MongoDB.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using MongoDB.Bson.Serialization;

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerMongoDBBuilderExtensions
    {
        public static IIdentityServerBuilder AddConfigurationStore(
           this IIdentityServerBuilder builder, Action<MongoDBConfiguration> setupAction)
        {
            _ = builder?.Services.Configure(setupAction);

            return builder.AddConfigurationStore();
        }

        public static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            _ = builder?.Services.Configure<MongoDBConfiguration>(configuration);

            return builder.AddConfigurationStore();
        }

        public static IIdentityServerBuilder AddOperationalStore(
           this IIdentityServerBuilder builder,
           Action<MongoDBConfiguration> setupAction,
           Action<TokenCleanupOptions> tokenCleanUpOptions = null)
        {
            _ = builder?.Services.Configure(setupAction);

            return builder.AddOperationalStore(tokenCleanUpOptions);
        }

        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            IConfiguration configuration,
            Action<TokenCleanupOptions> tokenCleanUpOptions = null)
        {
            _ = builder?.Services.Configure<MongoDBConfiguration>(configuration);

            return builder.AddOperationalStore(tokenCleanUpOptions);
        }

        private static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder)
        {
            ConfigureIgnoreExtraElementsConfigurationStore();

            _ = builder?.Services.AddScoped<IConfigurationDbContext, ConfigurationDbContext>();

            _ = builder?.Services.AddTransient<IClientStore, ClientStore>();
            _ = builder?.Services.AddTransient<IResourceStore, ResourceStore>();
            _ = builder?.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            return builder;
        }

        private static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            Action<TokenCleanupOptions> tokenCleanUpOptions = null)
        {
            ConfigureIgnoreExtraElementsOperationalStore();

            _ = builder?.Services.AddScoped<IPersistedGrantDbContext, PersistedGrantDbContext>();

            _ = builder?.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            var tokenCleanupOptions = new TokenCleanupOptions();
            tokenCleanUpOptions?.Invoke(tokenCleanupOptions);
            _ = builder?.Services.AddSingleton(tokenCleanupOptions);
            _ = builder?.Services.AddSingleton<TokenCleanup>();

            return builder;
        }

        public static IApplicationBuilder UseIdentityServerMongoDBTokenCleanup(this IApplicationBuilder app, IHostApplicationLifetime applicationLifetime)
        {
            var tokenCleanup = app?.ApplicationServices.GetService<TokenCleanup>();
            if (tokenCleanup == null)
            {
                throw new InvalidOperationException("AddOperationalStore must be called on the service collection.");
            }
            _ = applicationLifetime?.ApplicationStarted.Register(tokenCleanup.Start);
            _ = applicationLifetime?.ApplicationStopping.Register(tokenCleanup.Stop);

            return app;
        }

        private static void ConfigureIgnoreExtraElementsConfigurationStore()
        {
            _ = BsonClassMap.RegisterClassMap<Client>(cm =>
              {
                  cm.AutoMap();
                  cm.SetIgnoreExtraElements(true);
              });
            _ = BsonClassMap.RegisterClassMap<IdentityResource>(cm =>
              {
                  cm.AutoMap();
                  cm.SetIgnoreExtraElements(true);
              });
            _ = BsonClassMap.RegisterClassMap<ApiResource>(cm =>
              {
                  cm.AutoMap();
                  cm.SetIgnoreExtraElements(true);
              });
        }

        private static void ConfigureIgnoreExtraElementsOperationalStore()
        {
            _ = BsonClassMap.RegisterClassMap<PersistedGrant>(cm =>
              {
                  cm.AutoMap();
                  cm.SetIgnoreExtraElements(true);
              });
        }
    }
}