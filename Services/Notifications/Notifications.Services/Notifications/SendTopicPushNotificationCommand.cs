using MediatR;
using Notifications.Domain;

namespace Notifications.Services.Notifications
{
    public class SendTopicPushNotificationCommand : IRequest<Unit>
    {
        public SendTopicPushNotificationCommand(string topic, Notification notification)
        {
            Topic = topic;
            Notification = notification;
        }

        /// <summary>
        /// Gets topic.
        /// </summary>
        public string Topic { get; }

        /// <summary>
        /// Gets notification.
        /// </summary>
        public Notification Notification { get; }
    }
}