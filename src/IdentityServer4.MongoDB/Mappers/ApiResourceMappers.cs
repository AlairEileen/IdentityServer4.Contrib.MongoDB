



using AutoMapper;
using IdentityServer4.MongoDB.Entities;

namespace IdentityServer4.MongoDB.Mappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Models.ApiResource ToModel(this ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<Models.ApiResource>(resource);
        }

        public static ApiResource ToEntity(this Models.ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<ApiResource>(resource);
        }
    }
}