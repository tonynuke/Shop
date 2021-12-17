using Confluent.Kafka;

namespace Common.Outbox.Consumer.Handlers
{
    /// <summary>
    /// Consume result handler interface.
    /// </summary>
    public interface IConsumeResultHandler<TKey, TValue>
    {
        /// <summary>
        /// Handles consume result.
        /// </summary>
        /// <typeparam name="TKey">Key.</typeparam>
        /// <typeparam name="TValue">Value.</typeparam>
        /// <param name="consumeResult">Consume result.</param>
        /// <returns>Asynchronous operation.</returns>
        Task Handle(ConsumeResult<TKey, TValue> consumeResult, CancellationToken cancellationToken);
    }
}
