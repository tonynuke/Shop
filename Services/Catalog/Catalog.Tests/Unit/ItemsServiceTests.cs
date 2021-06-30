using System;
using System.Threading.Tasks;
using AutoFixture;
using Catalog.Domain;
using Catalog.Services.Brands;
using Catalog.Services.Items;
using Catalog.Services.Items.Dto;
using FluentAssertions;
using Moq;
using Nest;
using TestUtils;
using Xunit;
using Name = Catalog.Domain.Name;

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
                Mock.Of<IElasticClient>(),
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