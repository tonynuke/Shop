using System;
using MediatR;

namespace Notifications.Services.Users.Subscriptions
{
    public class SubscribeCommand : IRequest<Unit>
    {
        public SubscribeCommand(Guid userId, string topic)
        {
            UserId = userId;
            Topic = topic;
        }

        public Guid UserId { get; }

        public string Topic { get; }
    }
}