using MediatR;
using Notifications.Domain;

namespace Notifications.Services.Users.Subscriptions
{
    /// <summary>
    /// Command for unsubscribe device <see cref="DeviceToken"/> from all <see cref="User"/> subscriptions.
    /// </summary>
    public class UnsubscribeFromAllCommand : IRequest<Unit>
    {
        public UnsubscribeFromAllCommand(User user, string deviceToken)
        {
            User = user;
            DeviceToken = deviceToken;
        }

        public User User { get; }

        public string DeviceToken { get; }
    }
}