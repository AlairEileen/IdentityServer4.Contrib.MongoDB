using IdentityServer4.MongoDB.Models;

using System.Linq;
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
    }
}