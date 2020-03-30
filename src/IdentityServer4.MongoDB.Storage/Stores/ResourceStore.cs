using IdentityServer4.Models;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.MongoDB.Storage
{
    public class ResourceStore : IResourceStore
    {
        private readonly IConfigurationDbContext _context;
        private readonly ILogger<ResourceStore> _logger;

        public ResourceStore(IConfigurationDbContext context, ILogger<ResourceStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apis =
                from apiResource in _context.ApiResources
                where apiResource.Name == name
                select apiResource;

            var api = apis.FirstOrDefault();

            if (api != null)
            {
                _logger.LogDebug("Found {api} API resource in database", name);
            }
            else
            {
                _logger.LogDebug("Did not find {api} API resource in database", name);
            }

            return Task.FromResult(api.ToIdentityModel());
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();

            var apis =
                from api in _context.ApiResources
                where api.Scopes.Where(x => names.Contains(x.Name)).Any()
                select api;

            var results = apis.ToArray();
            var models = results.Select(x => x.ToIdentityModel()).ToArray();

            _logger.LogDebug("Found {scopes} API scopes in database", models.SelectMany(x => x.Scopes).Select(x => x.Name));

            return Task.FromResult(models.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var scopes = scopeNames.ToArray();

            var resources =
                from identityResource in _context.IdentityResources
                where scopes.Contains(identityResource.Name)
                select identityResource;

            var results = resources.ToArray();

            _logger.LogDebug("Found {scopes} identity scopes in database", results.Select(x => x.Name));

            return Task.FromResult(results.Select(x => x.ToIdentityModel()).ToArray().AsEnumerable());
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var identity = _context.IdentityResources;

            var apis = _context.ApiResources;

            var result = new Resources(
                identity.ToArray().Select(x => x.ToIdentityModel()).AsEnumerable(),
                apis.ToArray().Select(x => x.ToIdentityModel()).AsEnumerable());

            _logger.LogDebug("Found {scopes} as all scopes in database", result.IdentityResources.Select(x => x.Name).Union(result.ApiResources.SelectMany(x => x.Scopes).Select(x => x.Name)));

            return Task.FromResult(result);
        }
    }
}