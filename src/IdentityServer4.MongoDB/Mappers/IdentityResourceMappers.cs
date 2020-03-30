



using AutoMapper;
using IdentityServer4.MongoDB.Entities;

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

        public static Models.IdentityResource ToModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<Models.IdentityResource>(resource);
        }

        public static IdentityResource ToEntity(this Models.IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }
    }
}