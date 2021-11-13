using System.Threading.Tasks;
using Common.Outbox;
using Common.Outbox.Consumer;
using Xunit.Abstractions;

namespace Tests.ConfluentKafka
{
    public class TestConsumer :
        IConsumer<IntegerEvent>,
        IConsumer<StringEvent>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestConsumer(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public Task Consume(IntegerEvent message)
        {
            _testOutputHelper.WriteLine(message.Int.ToString());
            return Task.CompletedTask;
        }

        public Task Consume(StringEvent message)
        {
            _testOutputHelper.WriteLine(message.String);
            return Task.CompletedTask;
        }
    }
}