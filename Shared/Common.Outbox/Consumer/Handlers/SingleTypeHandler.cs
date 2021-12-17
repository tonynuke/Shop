using Confluent.Kafka;
using System.Text.Json;

namespace Common.Outbox.Consumer.Handlers
{
    public abstract class SingleTypeHandler<TMessage> : IConsumeResultHandler<string, string>
    {
        protected abstract Task HandleInternal(TMessage message);

        public Task Handle(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Deserialize<TMessage>(consumeResult.Message.Value);
            return HandleInternal(message!);
        }
    }
}
