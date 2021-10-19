using System;
using Basket.Domain;
using Basket.GrpcClient;
using Mapster;

namespace Basket.GrpcService
{
    /// <summary>
    /// Mapper.
    /// </summary>
    public class Mapper : IRegister
    {
        /// <inheritdoc/>
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Basket, BasketReply>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Items, src => src.Items);

            config.NewConfig<BasketItem, BasketItemDto>();

            config.NewConfig<BasketItemDto, BasketItem>()
                .ConstructUsing(dto => BasketItem.Create(Guid.Parse(dto.Id), dto.Quantity).Value);
        }
    }
}