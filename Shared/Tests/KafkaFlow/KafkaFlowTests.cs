using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.KafkaFlow
{
    public class KafkaFlowTests
    {
        [Fact]
        public async Task Test()
        {
            var services = new ServiceCollection();
            services.AddKafka(
                kafka => kafka
                    .AddCluster(
                        cluster => cluster
                            .WithBrokers(new[] { "localhost:9092" })
                            //.EnableAdminMessages("kafka-flow.admin", Guid.NewGuid().ToString())
                            .AddProducer(
                                "kafka-flow-rpoducer",
                                producer => producer
                                    .DefaultTopic("kafka-flow-topic")
                                    .WithCompression(CompressionType.Gzip)
                            //.AddMiddlewares(
                            //    middlewares => middlewares
                            //        .AddSerializer<ProtobufNetSerializer>()
                            //)
                            //.WithAcks(Acks.All)
                            )
                            .AddConsumer(
                                consumer => consumer
                                    .Topic("kafka-flow-topic")
                                    .WithGroupId("kafka-flow-group")
                                    .WithName("kafka-flow-consumer")
                                    .WithBufferSize(100)
                                    .WithWorkersCount(20)
                                    //.WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Latest)
                                    .WithPendingOffsetsStatisticsHandler(
                                        (resolver, offsets) =>
                                            resolver.Resolve<ILogHandler>().Verbose(
                                                "Offsets pending to be committed",
                                                new
                                                {
                                                    Offsets = offsets.Select(o =>
                                                        new
                                                        {
                                                            Partition = o.Partition.Value,
                                                            Offset = o.Offset.Value,
                                                            o.Topic
                                                        })
                                                }),
                                        new TimeSpan(0, 0, 1))
                                    .AddMiddlewares(
                                        middlewares => middlewares
                                            //.AddSerializer<ProtobufNetSerializer>()
                                            .AddTypedHandlers(
                                                handlers => handlers
                                                    .WithHandlerLifetime(InstanceLifetime.Singleton)
                                                    .AddHandler<IntegerEventHandler>())
                                    )
                            )
                    )
            );
        }
    }
}
