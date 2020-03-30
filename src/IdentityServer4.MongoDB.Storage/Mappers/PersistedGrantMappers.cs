using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.MongoDB.Mappers
{
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static PersistedGrant ToIdentityModel(this MongoDB.Models.PersistedGrant token)
        {
            return token == null ? null : Mapper.Map<PersistedGrant>(token);
        }

        public static Models.PersistedGrant ToMongoDbIdentityModel(this PersistedGrant token)
        {
            return token == null ? null : Mapper.Map<Models.PersistedGrant>(token);
        }

        public static void UpdateEntity(this PersistedGrant token, MongoDB.Models.PersistedGrant target)
        {
            Mapper.Map(token, target);
        }
    }
}