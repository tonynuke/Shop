using System;

namespace Order.WebService.Dto
{
    public record Delivery
    {
        public string Address { get; init; }

        public DateTime DateTime { get; init; }
    }
}