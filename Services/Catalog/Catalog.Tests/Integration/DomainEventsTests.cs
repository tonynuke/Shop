using System.Threading.Tasks;
using AutoFixture;
using Catalog.Domain;
using Catalog.Persistence;
using DataAccess;
using TestUtils;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Tests.Integration
{
    public class DomainEventsTests : MongoClientFixture
    {
        private readonly Fixture _fixture = new();

        public DomainEventsTests(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
        {
        }

        [Fact]
        public async Task C()
        {
            var repo = new CatalogContext(Database);

            var name = Name.Create(_fixture.Create<string>()).Value;
            var item = new CatalogItem(new Brand(name), name, "", 100);
            await repo.Items.InsertOneAsync(item);

            item.Price = 300;

            await repo.Items.ReplaceOneOcc(item);
        }
    }
}