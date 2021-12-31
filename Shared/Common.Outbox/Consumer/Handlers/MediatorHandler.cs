using Confluent.Kafka;
using MediatR;
using System.Text.Json;

namespace Common.Outbox.Consumer.Handlers
{
    public class MediatorHandler : IConsumeResultHandler<string, string>
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
        {
            var messageType = consumeResult.Message.Headers.GetMessageType();
            var @event = JsonSerializer.Deserialize(consumeResult.Message.Value, messageType);
            await _mediator.Publish(@event!, cancellationToken);
        }
    }
}
