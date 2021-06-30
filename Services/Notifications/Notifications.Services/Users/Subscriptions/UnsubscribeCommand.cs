using System;
using MediatR;

namespace Notifications.Services.Users.Subscriptions
{
    public class UnsubscribeCommand : IRequest<Unit>
    {
        public UnsubscribeCommand(Guid userId, string topic)
        {
            UserId = userId;
            Topic = topic;
        }

        public Guid UserId { get; }

        public string Topic { get; }
    }
}