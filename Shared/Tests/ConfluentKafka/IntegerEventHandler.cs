using Common.Outbox.Consumer.Handlers;
using Tests.ConfluentKafka.Events;
using Xunit.Abstractions;

namespace Tests.ConfluentKafka
{
    public class IntegerEventHandler : SingleTypeHandler<IntegerEvent>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public IntegerEventHandler(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        protected override Task HandleInternal(IntegerEvent message)
        {
            _testOutputHelper.WriteLine(message.Int.ToString());
            return Task.CompletedTask;
        }
    }
}
