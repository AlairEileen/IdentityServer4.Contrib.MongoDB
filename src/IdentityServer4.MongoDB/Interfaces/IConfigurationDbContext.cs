



using IdentityServer4.MongoDB.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.MongoDB.Interfaces
{
    public interface IConfigurationDbContext : IDisposable
    {
        IQueryable<Client> Clients { get; }
        IQueryable<IdentityResource> IdentityResources { get; }
        IQueryable<ApiResource> ApiResources { get; }

        Task AddClient(Client entity);

        Task AddIdentityResource(IdentityResource entity);

        Task AddApiResource(ApiResource entity);
    }
}