using Catalog.Brands;
using Catalog.Items;
using Catalog.WebService.Dto.Brands;
using Catalog.WebService.Dto.Items;
using Mapster;

namespace Catalog.WebService
{
    /// <summary>
    /// Mapper.
    /// </summary>
    public class Mapper : IRegister
    {
        /// <inheritdoc/>
        public void Register(TypeAdapterConfig config)
        {
           config.NewConfig<CatalogItem, ItemDto>()
               .Map(dest => dest.Id, src => src.Id)
               .Map(dest => dest.Name, src => src.Name.Value)
               .Map(dest => dest.Price, src => src.Price)
               .Map(dest => dest.Description, src => src.Description);

           config.NewConfig<Brand, BrandDto>()
               .Map(dest => dest.Id, src => src.Id)
               .Map(dest => dest.Name, src => src.Name.Value)
               .Map(dest => dest.ImageUrl, src => src.ImageUrl);
        }
    }
}