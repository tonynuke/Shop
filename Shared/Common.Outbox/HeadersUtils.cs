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
            var type = @event.GetType().FullName;
            var typeAsBytes = Encoding.UTF8.GetBytes(type!);
            return new Header(TypeHeader, typeAsBytes);
        }
    }
}
