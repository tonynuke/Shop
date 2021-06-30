using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Notifications.Domain;
using Notifications.Persistence;
using Notifications.Services.Users;
using Notifications.Services.Users.Devices;
using TestUtils;
using Xunit;

namespace Notifications.Tests.Unit
{
    public class LinksHandlerTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Fact]
        public async Task Can_link_user_to_device()
        {
            var context = InMemoryNotificationsContext.Create();
            var user = _fixture.Create<User>();
            context.UsersMock.SetupCursorResponse(user);
            _fixture.Inject<NotificationsContext>(context);
            var handler = _fixture.Create<UsersHandler>();

            var command = _fixture.Create<LinkUserToDeviceCommand>();
            await handler.Handle(command);
        }

        [Fact]
        public async Task Can_unlink_user_from_device()
        {
            var context = InMemoryNotificationsContext.Create();
            var user = _fixture.Create<User>();
            context.UsersMock.SetupCursorResponse(user);
            _fixture.Inject<NotificationsContext>(context);
            var handler = _fixture.Create<UsersHandler>();

            var command = _fixture.Create<UnlinkUserFromDeviceCommand>();
            await handler.Handle(command);
        }
    }
}