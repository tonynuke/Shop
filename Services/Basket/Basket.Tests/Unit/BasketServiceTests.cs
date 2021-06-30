using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Basket.Services;
using Basket.Services.Dto;
using Catalog.Client.V1;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using TestUtils;
using Xunit;

namespace Basket.Tests.Unit
{
    public class BasketServiceTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Fact]
        public async Task Can_add_an_item_to_the_basket_when_the_basket_is_empty()
        {
            var catalogItem = _fixture.Create<ItemDto>();
            var userId = Guid.NewGuid();
            var emptyBasket = new Domain.Basket(userId);

            var command = new AddOrUpdateBasketItemDto(userId, catalogItem.Id, 1);

            var context = InMemoryBasketContext.Create();
            context.BasketsMock.SetupUpdateOneResult();
            context.BasketsMock.SetupCursorResponse(emptyBasket);

            var catalogClient = new Mock<ICatalogClient>();
            catalogClient.Setup(client => client.FindItemByIdAsync(
                    It.Is<Guid>(id => id == command.CatalogItemId)))
                .ReturnsAsync(catalogItem);

            var service = new BasketsService(context, catalogClient.Object);
            var result = await service.AddOrUpdateBasketItem(command);
            result.IsSuccess.Should().BeTrue();

            context.BasketsMock.VerifyAll();
        }
    }
}
