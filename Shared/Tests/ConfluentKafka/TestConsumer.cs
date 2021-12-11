using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Xunit.Abstractions;

namespace Tests.ConfluentKafka
{
    public class TestConsumer :
        INotificationHandler<IntegerEvent>,
        INotificationHandler<StringEvent>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestConsumer(ITestOutputHelper testOutputHelper)
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