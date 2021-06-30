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

            config.NewConfig<Domain.BasketItem, Dto.BasketItemDto>();
        }
    }
}