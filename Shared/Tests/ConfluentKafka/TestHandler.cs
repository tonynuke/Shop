using MediatR;
using Microsoft.Extensions.Logging;
using Tests.ConfluentKafka.Events;

namespace Tests.ConfluentKafka
{
    public class TestHandler :
        INotificationHandler<IntegerEvent>,
        INotificationHandler<StringEvent>
    {
        private readonly ILogger<TestHandler> _logger;

        public TestHandler(ILogger<TestHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(IntegerEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(notification.Int.ToString());
            return Task.CompletedTask;
        }

        public Task Handle(StringEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(notification.String);
            return Task.CompletedTask;
        }
    }
}