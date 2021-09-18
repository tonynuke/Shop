using System;
using System.Threading.Tasks;
using AutoFixture;
using Catalog.Domain;
using FluentAssertions;
using Nest;
using Xunit;
using Xunit.Abstractions;
using Name = Catalog.Domain.Name;

namespace Catalog.Tests.Integration
{
    public class ElasticSearchTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly IElasticClient _elasticClient;
        const string DefaultIndex = "items";

        public ElasticSearchTests(ITestOutputHelper testOutputHelper)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                .DefaultIndex(DefaultIndex)
                .DefaultMappingFor<Indexing.Item>(descriptor => descriptor)
                .EnableDebugMode(details => testOutputHelper.WriteLine(details.DebugInformation));
            _elasticClient = new ElasticClient(settings);
            var createIndexResponse = _elasticClient.Indices.Create(
                DefaultIndex,
                index => index.Map<Indexing.Item>(x => x.AutoMap()));
        }

        [Fact(Skip = "manual elastic test")]
        public async Task Index_item()
        {
            var brand = new Brand(Name.Create("adidas").Value, string.Empty);

            var item1 = brand.CreateItem(Name.Create("штаны").Value, "kek", 100);
            await _elasticClient.IndexDocumentAsync(Map(item1));

            var item2 = brand.CreateItem(Name.Create("кроссовки").Value, "no", 400);
            await _elasticClient.IndexDocumentAsync(Map(item2));

            var response = await _elasticClient.SearchAsync<Indexing.Item>(
                selector => selector
                    .From(0)
                    .Size(10)
                    .Query(query => query.MultiMatch(
                        c => c.Query("kek")
                            .Fields(descriptor => descriptor.Fields(
                                item => item.Name,
                                item => item.Description,
                                item => item.Brand
                            )))));

            var items = response.Documents;
            items.Should().NotBeEmpty();
        }

        private static Indexing.Item Map(CatalogItem item)
        {
            return new()
            {
                Name = item.Name.Value,
                Description = item.Description,
                Brand = item.Brand.Name.Value,
                Price = item.Price
            };
        }
    }
}