using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.MongoDB.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityResource ToIdentityModel(this Models.IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }

        public static Models.IdentityResource ToMongoDbIdentityModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<Models.IdentityResource>(resource);
        }
    }
}