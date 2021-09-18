using Basket.Domain;
using Mapster;

namespace Basket.WebService
{
    /// <summary>
    /// Mapper.
    /// </summary>
    public class Mapper : IRegister
    {
        /// <inheritdoc/>
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Basket, Dto.BasketDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Items, src => src.Items);

            config.NewConfig<BasketItem, Dto.BasketItemDto>();

            config.NewConfig<Dto.BasketItemDto, BasketItem>()
                .ConstructUsing(dto => BasketItem.Create(dto.Id, dto.Quantity).Value);
        }
    }
}