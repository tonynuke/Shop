using System;
using System.Threading.Tasks;
using AutoFixture;
using Catalog.Brands;
using Catalog.Items;
using Catalog.Items.Dto;
using FluentAssertions;
using TestUtils;
using Xunit;
using Name = Catalog.Items.Name;

namespace Catalog.Tests.Unit
{
    public class ItemsServiceTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Can_create_an_item()
        {
            var catalogContext = InMemoryCatalogContext.Create();
            var brand = new Brand(Name.Create(_fixture.Create<string>()).Value);
            catalogContext.BrandsMock.SetupCursorResponse(brand);

            var service = new CatalogItemsService(
                new BrandsService(catalogContext),
                catalogContext);

            var name = Name.Create(_fixture.Create<string>()).Value;
            var dto = new CreateItemDto(
                Guid.NewGuid(),
                name,
                _fixture.Create<string>(),
                _fixture.Create<decimal>());

            var result = await service.Create(dto);
            result.IsSuccess.Should().BeTrue();
        }
    }
}