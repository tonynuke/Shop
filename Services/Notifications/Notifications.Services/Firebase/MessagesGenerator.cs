using System.Collections.Generic;
using System.Linq;
using FirebaseAdmin.Messaging;
using Notifications.Domain;

namespace Notifications.Services.Firebase
{
    /// <summary>
    /// Messages generator.
    /// </summary>
    public static class MessagesGenerator
    {
        /// <summary>
        /// Generates multicast message.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="notification">Notification.</param>
        /// <returns>Multicast message.</returns>
        public static MulticastMessage GenerateMulticastMessage(
            User user, Domain.Notification notification)
        {
            return new()
            {
                Tokens = user.Devices.Select(device => device.Token).ToList(),
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Title,
                    Body = notification.Body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        {
                            "apns-priority", "5"
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Generates message.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="notification">Notification.</param>
        /// <returns>Message.</returns>
        public static Message GenerateMessage(
            string topic, Domain.Notification notification)
        {
            return new Message
            {
                Topic = topic,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Title,
                    Body = notification.Body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        {
                            "apns-priority", "5"
                        }
                    }
                }
            };
        }
    }
}