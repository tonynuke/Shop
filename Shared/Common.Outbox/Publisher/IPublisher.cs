using Domain;

namespace Common.Outbox.Publisher
{
    /// <summary>
    /// Events publisher interface.
    /// </summary>
    /// TODO: Look at MongoDb change streams
    /// https://debezium.io/
    /// https://docs.mongodb.com/kafka-connector/current/
    public interface IPublisher
    {
        /// <summary>
        /// Publish <paramref name="events"/>.
        /// </summary>
        /// <param name="events">Events.</param>
        /// <returns>Asynchronous operation.</returns>
        Task Publish(IEnumerable<DomainEventBase> events);
    }
}