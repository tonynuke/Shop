using MongoDB.Bson.Serialization;

namespace Common.MongoDb.Entities
{
    public class EntityBaseMap : IMongoEntityMap<EntityBase>
    {
        public void Map(BsonClassMap<EntityBase> map)
        {
            map.AutoMap();
            map.MapProperty(x => x.OccVersion).SetIgnoreIfDefault(true).SetElementName("occVersion");
        }
    }
}
