namespace MasstTransitTests
{
    using MassTransit;
    using System.Threading.Tasks;

    class KafkaMessageConsumer :
        IConsumer<KafkaMessage>
    {
        public Task Consume(ConsumeContext<KafkaMessage> context)
        {
            Console.WriteLine($"Consume {context.Message.Text}");
            var x = context.Offset();
            return Task.CompletedTask;
        }
    }
}