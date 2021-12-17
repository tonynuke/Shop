using Confluent.Kafka;
using MassTransit;
using MassTransit.KafkaIntegration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MasstTransitTests
{
    public partial class Program
    {
        private const string Topic = "masstransit-topic";
        private const string ConsumerGroupName = "masstransit-consumer-group";

        public static async Task Main()
        {
            var services = new ServiceCollection();

            services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

                x.AddRider(rider =>
                {
                    rider.AddProducer<KafkaMessage>(Topic);
                    rider.AddConsumer<KafkaMessageConsumer>(x =>
                    {
                    });

                    rider.UsingKafka((context, k) =>
                    {
                        k.Host("localhost:29092");
                        
                        k.TopicEndpoint<KafkaMessage>(
                            Topic,
                            ConsumerGroupName,
                            e =>
                            {
                                e.ConfigureConsumer<KafkaMessageConsumer>(context);
                                e.AutoOffsetReset = AutoOffsetReset.Earliest;
                                e.SetOffsetsCommittedHandler(OffsetsCommittedHandler);
                                e.CheckpointMessageCount = 1;
                                e.CheckpointInterval = TimeSpan.FromSeconds(1);
                            });
                    });
                });
            });

            await using var provider = services.BuildServiceProvider(true);
            var busControl = provider.GetRequiredService<IBusControl>();

            var startTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            await busControl.StartAsync(startTokenSource);
            try
            {
                Console.WriteLine("Started");
                await Task.Run(() => Client(provider), CancellationToken.None);
            }
            finally
            {
                await busControl.StopAsync(TimeSpan.FromSeconds(30));
            }
        }

        private static void OffsetsCommittedHandler(IConsumer<Ignore, KafkaMessage> consumer, CommittedOffsets offsets)
        {
            // TODO: Why it was not called?
            consumer.Commit();
        }

        static async Task Client(IServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("Enter something, or empty to quit");
                var text = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text))
                    break;

                using var serviceScope = provider.CreateScope();

                var producer = serviceScope.ServiceProvider.GetRequiredService<ITopicProducer<KafkaMessage>>();
                var message = new KafkaMessage { Text = text };
                await producer.Produce(message);
                Console.WriteLine($"Produce {message.Text}");
            }
        }
    }
}