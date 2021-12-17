using MediatR;
using Tests.ConfluentKafka.Events;
using Xunit.Abstractions;

namespace Tests.ConfluentKafka
{
    public class TestHandler :
        INotificationHandler<IntegerEvent>,
        INotificationHandler<StringEvent>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestHandler(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public Task Handle(IntegerEvent notification, CancellationToken cancellationToken)
        {
            _testOutputHelper.WriteLine(notification.Int.ToString());
            return Task.CompletedTask;
        }

        public Task Handle(StringEvent notification, CancellationToken cancellationToken)
        {
            _testOutputHelper.WriteLine(notification.String);
            return Task.CompletedTask;
        }
    }
}