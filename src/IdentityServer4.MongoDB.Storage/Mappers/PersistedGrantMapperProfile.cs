using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.MongoDB.Mappers
{
    /// <summary>
    /// AutoMapper Config for PersistedGrant
    /// Between model and mongodb model
    /// <seealso cref="https://github.com/AutoMapper/AutoMapper/wiki/Configuration">
    /// </seealso>
    /// </summary>
    public class PersistedGrantMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="PersistedGrantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedGrantMapperProfile()
        {
            // mongodb model to model
            CreateMap<Models.PersistedGrant, PersistedGrant>(MemberList.Destination);

            // model to mongodb model
            CreateMap<PersistedGrant, Models.PersistedGrant>(MemberList.Source);
        }
    }
}