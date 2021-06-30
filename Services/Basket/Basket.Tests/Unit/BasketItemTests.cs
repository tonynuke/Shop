using System;
using Basket.Domain;
using FluentAssertions;
using Xunit;

namespace Basket.Tests.Unit
{
    public class BasketItemTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Can_not_create_an_item(int quantity)
        {
            BasketItem.Create(Guid.NewGuid(), quantity).IsSuccess.Should().BeFalse();
        }
    }
}