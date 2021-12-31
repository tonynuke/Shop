using Common.Outbox.Consumer;
using Common.Outbox.Exceptions;
using Confluent.Kafka;
using Domain;
using System.Text;

namespace Common.Outbox
{
    public static class HeadersUtils
    {
        public const string TypeHeader = "Type";

        public static bool TryGetTypeHeader(this Headers headers, out string? header)
        {
            header = null;
            var typeHeader = headers.SingleOrDefault(x => x.Key == TypeHeader);
            if (typeHeader == null)
            {
                return false;
            }

            header = GetTypeName(typeHeader);
            return true;
        }

        private static string GetTypeName(IHeader header)
        {
            var typeHeaderBytes = header.GetValueBytes();
            return Encoding.UTF8.GetString(typeHeaderBytes);
        }

        public static Header GetTypeHeader(this DomainEventBase @event)
        {
            // TODO: consider typemap approach for non .net producers, consumers
            var eventType = @event.GetType();
            var typeHeader = $"{eventType.FullName}, {eventType.Assembly.GetName().Name}";
            var typeAsBytes = Encoding.UTF8.GetBytes(typeHeader);
            return new Header(TypeHeader, typeAsBytes);
        }

        public static Type GetMessageType(this Headers headers)
        {
            var isHeaderSpecified = headers.TryGetTypeHeader(out var typeName);
            if (!isHeaderSpecified)
            {
                throw new TypeHeaderIsNotSpecified();
            }

            return Type.GetType(typeName);
        }
    }
}
