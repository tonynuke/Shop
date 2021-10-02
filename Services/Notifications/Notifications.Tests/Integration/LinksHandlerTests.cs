using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Notifications.Domain;
using Notifications.Persistence;
using Notifications.Services.Users;
using Notifications.Services.Users.Devices;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Notifications.Tests.Integration
{
    public class LinksHandlerTests : MongoClientFixture
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        public LinksHandlerTests(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
        {
        }

        [Fact]
        public async Task Can_link_user_to_device()
        {
            var context = new NotificationsContext(Database);
            var user = _fixture.Create<User>();
            _fixture.Inject<NotificationsContext>(context);
            var handler = _fixture.Create<UsersHandler>();

            var command = _fixture.Create<LinkUserToDeviceCommand>();
            await handler.Handle(command);
        }

        [Fact]
        public async Task Can_unlink_user_from_device()
        {
            var context = new NotificationsContext(Database);
            var user = _fixture.Create<User>();
            _fixture.Inject<NotificationsContext>(context);
            var handler = _fixture.Create<UsersHandler>();

            var command = _fixture.Create<UnlinkUserFromDeviceCommand>();
            await handler.Handle(command);
        }
    }
}