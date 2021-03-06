﻿using AutoMapper;
using IdentityServer4.Models;

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

        public static ApiResource ToIdentityModel(this Models.ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<ApiResource>(resource);
        }

        public static Models.ApiResource ToMongoDbIdentityModel(this ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<Models.ApiResource>(resource);
        }
    }
}