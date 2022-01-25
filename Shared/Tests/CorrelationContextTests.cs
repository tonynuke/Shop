using Common.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class CorrelationContextTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CorrelationContextTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private async Task Function()
        {
            TrySetCorrelationId("function 1");

            await Task.Delay(1);

            TrySetCorrelationId("function 2");
        }

        private void TrySetCorrelationId(string id)
        {
            try
            {
                CorrelationContext.CorrelationId = id;
                _testOutputHelper.WriteLine(CorrelationContext.CorrelationId);
            }
            catch (Exception exception)
            {
                _testOutputHelper.WriteLine(exception.Message);
                _testOutputHelper.WriteLine(CorrelationContext.CorrelationId);
            }
        }

        [Fact]
        public async Task CorrelationContextTest()
        {
            TrySetCorrelationId("base");

            await Function();

            _testOutputHelper.WriteLine("Task run");
            await Task.Run(() => Function());
        }
    }
}
