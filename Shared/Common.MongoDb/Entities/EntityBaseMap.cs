using CSharpFunctionalExtensions;
using Domain;
using MongoDB.Bson.Serialization;

namespace Common.MongoDb.Entities
{
    public class EntityMap : IMongoEntityMap<Entity<Guid>>
    {
        public void Map(BsonClassMap<Entity<Guid>> map)
        {
            map.AutoMap();
            map.MapIdProperty(x => x.Id);
        }
    }

    public class DomainEntityMap : IMongoEntityMap<DomainEntity>
    {
        public void Map(BsonClassMap<DomainEntity> map)
        {
            map.AutoMap();
            map.UnmapMember(x => x.DomainEvents);
        }
    }

    public class EntityBaseMap : IMongoEntityMap<EntityBase>
    {
        public void Map(BsonClassMap<EntityBase> map)
        {
            map.AutoMap();
            map.MapProperty(x => x.OccVersion).SetIgnoreIfDefault(true).SetElementName("occVersion");
        }
    }
}
