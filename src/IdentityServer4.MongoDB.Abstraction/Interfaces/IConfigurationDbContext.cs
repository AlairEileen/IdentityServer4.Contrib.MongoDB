using IdentityServer4.MongoDB.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.MongoDB.Interfaces
{
    public interface IConfigurationDbContext
    {
        IQueryable<Client> Clients { get; }
        IQueryable<IdentityResource> IdentityResources { get; }
        IQueryable<ApiResource> ApiResources { get; }

        Task AddClient(Client entity, CancellationToken cancellationToken = default);

        Task AddIdentityResource(IdentityResource entity, CancellationToken cancellationToken = default);

        Task AddApiResource(ApiResource entity, CancellationToken cancellationToken = default);

        Task RemoveClient(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken = default);

        Task InsertOrUpdateClient(Expression<Func<Client, bool>> filter, Client entity, CancellationToken cancellationToken = default);

        Task InsertOrUpdateIdentityResource(Expression<Func<IdentityResource, bool>> filter, IdentityResource entity, CancellationToken cancellationToken = default);

        Task RemoveIdentityResource(Expression<Func<IdentityResource, bool>> filter, CancellationToken cancellationToken = default);

        Task RemovedApiResource(Expression<Func<ApiResource, bool>> filter, CancellationToken cancellationToken = default);

        Task InsertOrUpdateApiResource(Expression<Func<ApiResource, bool>> filter, ApiResource entity, CancellationToken cancellationToken = default);
    }
}