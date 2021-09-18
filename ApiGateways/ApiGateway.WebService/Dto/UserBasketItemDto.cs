using System;

namespace ApiGateway.WebService.Dto
{
    public record UserBasketItemDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public int Quantity { get; init; }

        public decimal Price { get; init; }
    }
}