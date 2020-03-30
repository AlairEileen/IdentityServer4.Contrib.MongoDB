


using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.MongoDB.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IConfigurationDbContext context;
        private readonly ILogger<CorsPolicyService> logger;

        public CorsPolicyService(IConfigurationDbContext context, ILogger<CorsPolicyService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger;
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            // If we use SelectMany directly, we got a NotSupportedException inside MongoDB driver.
            // Details:
            // System.NotSupportedException: Unable to determine the serialization information for the collection
            // selector in the tree: aggregate([]).SelectMany(x => x.AllowedCorsOrigins.Select(y => y.Origin))
            var origins = context.Clients.Select(x => x.AllowedCorsOrigins.Select(y => y.Origin)).ToList();

            // As a workaround, we use SelectMany in memory.
            var distinctOrigins = origins.SelectMany(o => o).Where(x => x != null).Distinct();

            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return Task.FromResult(isAllowed);
        }
    }
}