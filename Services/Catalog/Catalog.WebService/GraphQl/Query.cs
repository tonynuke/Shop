using System;
using Catalog.Items;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using MongoDB.Driver;

public class Query
{
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IExecutable<CatalogItem> GetPersons(
        [Service] IMongoCollection<CatalogItem> collection)
        => collection.AsExecutable();

    [UseFirstOrDefault]
    public IExecutable<CatalogItem> GetPersonById(
        [Service] IMongoCollection<CatalogItem> collection,
        [ID] Guid id)
        => collection.Find(x => x.Id == id)
            .AsExecutable();
}