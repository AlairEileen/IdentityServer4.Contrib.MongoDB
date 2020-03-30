using IdentityServer4.MongoDB.Configuration;
using IdentityServer4.MongoDB.Models;
using IdentityServer4.MongoDB.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System;

namespace IdentityServer4.MongoDB.DbContexts
{
    public class ConfigurationDbContext : MongoDBContextBase, IConfigurationDbContext
    {
        private readonly IMongoCollection<Client> clients;
        private readonly IMongoCollection<IdentityResource> identityResources;
        private readonly IMongoCollection<ApiResource> apiResources;

        public ConfigurationDbContext(IOptions<MongoDBConfiguration> settings)
            : base(settings)
        {
            clients = Database.GetCollection<Client>(Constants.TableNames.Client);
            identityResources = Database.GetCollection<IdentityResource>(Constants.TableNames.IdentityResource);
            apiResources = Database.GetCollection<ApiResource>(Constants.TableNames.ApiResource);

            CreateClientsIndexes();
            CreateIdentityResourcesIndexes();
            CreateApiResourcesIndexes();
        }

        private void CreateClientsIndexes()
        {
            var indexOptions = new CreateIndexOptions() { Background = true };

            var builder = Builders<Client>.IndexKeys;
            var clientIdIndexModel = new CreateIndexModel<Client>(builder.Ascending(_ => _.ClientId), indexOptions);
            clients.Indexes.CreateOne(clientIdIndexModel);
        }

        private void CreateIdentityResourcesIndexes()
        {
            var indexOptions = new CreateIndexOptions() { Background = true };

            var builder = Builders<IdentityResource>.IndexKeys;
            var nameIndexModel = new CreateIndexModel<IdentityResource>(builder.Ascending(_ => _.Name), indexOptions);
            identityResources.Indexes.CreateOne(nameIndexModel);
        }

        private void CreateApiResourcesIndexes()
        {
            var indexOptions = new CreateIndexOptions() { Background = true };

            var builder = Builders<ApiResource>.IndexKeys;
            var nameIndexModel = new CreateIndexModel<ApiResource>(builder.Ascending(_ => _.Name), indexOptions);
            var scopesIndexModel = new CreateIndexModel<ApiResource>(builder.Ascending(_ => _.Scopes), indexOptions);
            apiResources.Indexes.CreateOne(nameIndexModel);
            apiResources.Indexes.CreateOne(scopesIndexModel);
        }

        public IQueryable<Client> Clients
        {
            get { return clients.AsQueryable(); }
        }

        public IQueryable<IdentityResource> IdentityResources
        {
            get { return identityResources.AsQueryable(); }
        }

        public IQueryable<ApiResource> ApiResources
        {
            get { return apiResources.AsQueryable(); }
        }

        public async Task AddClient(Client entity, CancellationToken cancellationToken = default)
        {
            await clients.InsertOneAsync(entity, null, cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveClient(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken = default)
        {
            return clients.DeleteManyAsync(filter, null, cancellationToken);
        }

        public Task InsertOrUpdateClient(Expression<Func<Client, bool>> filter, Client entity, CancellationToken cancellationToken = default)
        {
            return clients.ReplaceOneAsync(filter, entity, new ReplaceOptions() { IsUpsert = true }, cancellationToken);
        }

        public async Task AddIdentityResource(IdentityResource entity, CancellationToken cancellationToken = default)
        {
            await identityResources.InsertOneAsync(entity, null, cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveIdentityResource(Expression<Func<IdentityResource, bool>> filter, CancellationToken cancellationToken = default)
        {
            return identityResources.DeleteManyAsync(filter, null, cancellationToken);
        }

        public Task InsertOrUpdateIdentityResource(Expression<Func<IdentityResource, bool>> filter, IdentityResource entity, CancellationToken cancellationToken = default)
        {
            return identityResources.ReplaceOneAsync(filter, entity, new ReplaceOptions() { IsUpsert = true }, cancellationToken);
        }

        public async Task AddApiResource(ApiResource entity, CancellationToken cancellationToken = default)
        {
            await apiResources.InsertOneAsync(entity, null, cancellationToken).ConfigureAwait(false);
        }

        public Task RemovedApiResource(Expression<Func<ApiResource, bool>> filter, CancellationToken cancellationToken = default)
        {
            return apiResources.DeleteManyAsync(filter, null, cancellationToken);
        }

        public Task InsertOrUpdateApiResource(Expression<Func<ApiResource, bool>> filter, ApiResource entity, CancellationToken cancellationToken = default)
        {
            return apiResources.ReplaceOneAsync(filter, entity, new ReplaceOptions() { IsUpsert = true }, cancellationToken);
        }
    }
}