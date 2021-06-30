using System;
using MediatR;
using Notifications.Domain;

namespace Notifications.Services.Notifications
{
    public class SendUserPushNotificationCommand : IRequest<Unit>
    {
        public SendUserPushNotificationCommand(Guid userId, Notification notification)
        {
            UserId = userId;
            Notification = notification;
        }

        public Guid UserId { get; }

        public Notification Notification { get; }
    }
}