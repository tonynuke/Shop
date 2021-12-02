using System.Linq;
using System.Threading.Tasks;
using Catalog.Brands;
using Catalog.Items;
using Common.MongoDb.Migrations;

namespace Catalog.Persistence.Migrations
{
    public class Init : Migration<CatalogContext>
    {
        public Init(CatalogContext context)
            : base(context)
        {
        }

        public override string Id => "Init";

        /// <inheritdoc/>
        public override async Task Up()
        {
            var adidas = new Brand(Name.Create("Adidas").Value);
            var reebok = new Brand(Name.Create("Reebok").Value);
            var nike = new Brand(Name.Create("Nike").Value);

            var brands = new[]
            {
                adidas,
                reebok,
                nike,
            };
            await Context.Brands.InsertManyAsync(brands);

            var items = new[]
            {
                adidas.CreateItem(Name.Create("Ultraboost").Value, "Amazing", 500),
                adidas.CreateItem(Name.Create("Superstar").Value, "Cool", 1500),
                adidas.CreateItem(Name.Create("Ozweego").Value, "The best", 2500),
                reebok.CreateItem(Name.Create("Maison").Value, "Strong", 300),
                reebok.CreateItem(Name.Create("Outlet").Value, "Stormy", 900),
                nike.CreateItem(Name.Create("Air").Value, "Max", 2700),
            };

            await Context.Items.InsertManyAsync(items);
            await Context.Events.InsertManyAsync(items.SelectMany(item => item.DomainEvents));
        }

        /// <inheritdoc/>
        public override Task Down()
        {
            return Task.CompletedTask;
        }
    }
}