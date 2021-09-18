﻿using System;
using System.Text.Json;
using System.Threading;
using Confluent.Kafka;
using Domain;

namespace Common.AspNetCore.Outbox
{
    /// <summary>
    /// Consumer.
    /// </summary>
    public class Consumer : IDisposable
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Consumer"/> class.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public Consumer(string topic, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(topic);
        }

        public void Consume(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var body = consumeResult.Message.Value;

                var @event = JsonSerializer.Deserialize<object>(body);
            }

            _consumer.Close();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}