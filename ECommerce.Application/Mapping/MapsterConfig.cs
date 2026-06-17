using Mapster;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Mapping;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateCategoryDto, Category>
            .NewConfig();

        TypeAdapterConfig<UpdateCategotyDto, Category>
            .NewConfig();
    }
}