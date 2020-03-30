using IdentityServer4.MongoDB.Configuration;
using IdentityServer4.MongoDB.Models;
using IdentityServer4.MongoDB.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityServer4.MongoDB.DbContexts
{
    public class PersistedGrantDbContext : MongoDBContextBase, IPersistedGrantDbContext
    {
        private readonly IMongoCollection<PersistedGrant> persistedGrants;

        public PersistedGrantDbContext(IOptions<MongoDBConfiguration> settings)
            : base(settings)
        {
            persistedGrants = Database.GetCollection<PersistedGrant>(Constants.TableNames.PersistedGrant);
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexOptions = new CreateIndexOptions() { Background = true };
            var builder = Builders<PersistedGrant>.IndexKeys;

            var keyIndexModel = new CreateIndexModel<PersistedGrant>(builder.Ascending(x => x.Key), indexOptions);
            var subIndexModel = new CreateIndexModel<PersistedGrant>(builder.Ascending(x => x.SubjectId), indexOptions);
            var clientIdSubIndexModel = new CreateIndexModel<PersistedGrant>(
              builder.Combine(
                  builder.Ascending(x => x.ClientId),
                  builder.Ascending(x => x.SubjectId)),
              indexOptions);

            var clientIdSubTypeIndexModel = new CreateIndexModel<PersistedGrant>(
              builder.Combine(
                  builder.Ascending(x => x.ClientId),
                  builder.Ascending(x => x.SubjectId),
                  builder.Ascending(x => x.Type)),
              indexOptions);

            persistedGrants.Indexes.CreateOne(keyIndexModel);
            persistedGrants.Indexes.CreateOne(subIndexModel);
            persistedGrants.Indexes.CreateOne(clientIdSubIndexModel);
            persistedGrants.Indexes.CreateOne(clientIdSubTypeIndexModel);
        }

        public IQueryable<PersistedGrant> PersistedGrants
        {
            get { return persistedGrants.AsQueryable(); }
        }

        public Task Remove(Expression<Func<PersistedGrant, bool>> filter)
        {
            return persistedGrants.DeleteManyAsync(filter);
        }

        public Task RemoveExpired()
        {
            return Remove(x => x.Expiration < DateTime.UtcNow);
        }

        public Task InsertOrUpdate(Expression<Func<PersistedGrant, bool>> filter, PersistedGrant entity)
        {
            return persistedGrants.ReplaceOneAsync(filter, entity, new UpdateOptions() { IsUpsert = true });
        }

        public void Dispose()
        {
            //TODO
        }
    }
}