using System;
using System.Collections.Generic;

namespace ApiGateway.WebService.Dto
{
    public record UserBasketDto
    {
        public Guid Id { get; init; }

        public IReadOnlyCollection<UserBasketItemDto> Items { get; init; }

        public decimal Price { get; init; }
    }
}