using System;

namespace Order.WebService.Dto
{
    public record MakeAnOrder
    {
        public Guid BasketId { get; init; }

        public Delivery Delivery { get; init; }
    }
}