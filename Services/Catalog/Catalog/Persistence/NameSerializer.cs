using Catalog.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalog.Persistence
{
    /// <summary>
    /// <see cref="Name"/> serializer.
    /// </summary>
    public class NameSerializer : SerializerBase<Name>
    {
        /// <inheritdoc/>
        public override void Serialize(
            BsonSerializationContext context, BsonSerializationArgs args, Name value)
        {
            context.Writer.WriteString(value.Value);
        }

        /// <inheritdoc/>
        public override Name Deserialize(
            BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Name.Create(context.Reader.ReadString()).Value;
        }
    }
}