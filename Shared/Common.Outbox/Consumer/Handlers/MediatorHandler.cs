using Confluent.Kafka;
using MediatR;
using System.Text.Json;

namespace Common.Outbox.Consumer.Handlers
{
    public class MediatorHandler : IConsumeResultHandler<string, string>
    {
        private readonly IReadOnlyDictionary<string, Type> _typeMap;
        private readonly IMediator _mediator;

        public MediatorHandler(IReadOnlyDictionary<string, Type> typeMap, IMediator mediator)
        {
            _typeMap = typeMap;
            _mediator = mediator;
        }

        public async Task Handle(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
        {
            var isHeaderSpecified = consumeResult.Message.Headers.TryGetTypeHeader(out var typeName);
            if (!isHeaderSpecified)
            {
                throw new TypeHeaderIsNotSpecified();
            }

            var isEventTypeFounded = _typeMap.TryGetValue(typeName, out var eventType);
            if (!isEventTypeFounded || eventType == null)
            {
                throw new EventTypeNotFound();
            }

            var @event = JsonSerializer.Deserialize(consumeResult.Message.Value, eventType);
            await _mediator.Publish(@event!, cancellationToken);
        }
    }
}
