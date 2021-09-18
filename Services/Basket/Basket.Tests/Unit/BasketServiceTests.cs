using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.Domain;
using Basket.Services;
using Basket.Services.Dto;
using FluentAssertions;
using TestUtils;
using Xunit;

namespace Basket.Tests.Unit
{
    public class BasketServiceTests
    {
        [Fact]
        public async Task Can_add_an_item_to_the_basket_when_the_basket_is_empty()
        {
            var catalogItems = Enumerable
                .Range(1, 3)
                .Select(i => BasketItem.Create(Guid.NewGuid(), i).Value)
                .ToList();
            var userId = Guid.NewGuid();
            var emptyBasket = new Domain.Basket(userId, DateTime.UtcNow);

            var dto = new UpdateBasketDto(userId, catalogItems);

            var context = InMemoryBasketContext.Create();
            context.BasketsMock.SetupUpdateOneResult();
            context.BasketsMock.SetupCursorResponse(emptyBasket);

            var service = new BasketsService(context);
            var result = await service.UpdateBasket(dto);
            result.IsSuccess.Should().BeTrue();

            context.BasketsMock.VerifyAll();
        }
    }
}
