using KafkaFlow;
using KafkaFlow.TypedHandler;
using Tests.ConfluentKafka.Events;

namespace Tests.KafkaFlow
{
    public class IntegerEventHandler : IMessageHandler<IntegerEvent>
    {
        public Task Handle(IMessageContext context, IntegerEvent message)
        {
            Console.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message.Int);

            return Task.CompletedTask;
        }
    }
}
