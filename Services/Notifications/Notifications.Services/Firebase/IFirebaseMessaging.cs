using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace Notifications.Services.Firebase
{
    /// <summary>
    /// Firebase messaging interface.
    /// </summary>
    /// <remarks>For unit testing purpose.</remarks>
    public interface IFirebaseMessaging
    {
        /// <summary>
        /// Sends all the messages in the given list via Firebase Cloud Messaging. Employs batching to
        /// send the entire list as a single RPC call. Compared to the <see cref="M:FirebaseAdmin.Messaging.FirebaseMessaging.SendAsync(FirebaseAdmin.Messaging.Message)" />
        /// method, this is a significantly more efficient way to send multiple messages.
        /// </summary>
        /// <exception cref="T:FirebaseAdmin.Messaging.FirebaseMessagingException">If an error occurs while sending the
        /// messages.</exception>
        /// <param name="messages">Up to 100 messages to send in the batch. Cannot be null.</param>
        /// <returns>A <see cref="T:FirebaseAdmin.Messaging.BatchResponse" /> containing details of the batch operation's
        /// outcome.</returns>
        Task<BatchResponse> SendAllAsync(IEnumerable<Message> messages);

        /// <summary>
        /// Sends a message to the FCM service for delivery. The message gets validated both by
        /// the Admin SDK, and the remote FCM service. A successful return value indicates
        /// that the message has been successfully sent to FCM, where it has been accepted by the
        /// FCM service.
        /// </summary>
        /// <returns>A task that completes with a message ID string, which represents
        /// successful handoff to FCM.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the message argument is null.</exception>
        /// <exception cref="T:System.ArgumentException">If the message contains any invalid
        /// fields.</exception>
        /// <exception cref="T:FirebaseAdmin.Messaging.FirebaseMessagingException">If an error occurs while sending the
        /// message.</exception>
        /// <param name="message">The message to be sent. Must not be null.</param>
        Task<string> SendAsync(Message message);

        /// <summary>
        /// Sends the given multicast message to all the FCM registration tokens specified in it.
        /// </summary>
        /// <exception cref="T:FirebaseAdmin.Messaging.FirebaseMessagingException">If an error occurs while sending the
        /// messages.</exception>
        /// <param name="message">The message to be sent. Must not be null.</param>
        /// <returns>A <see cref="T:FirebaseAdmin.Messaging.BatchResponse" /> containing details of the batch operation's
        /// outcome.</returns>
        Task<BatchResponse> SendMulticastAsync(MulticastMessage message);

        /// <summary>Subscribes a list of registration tokens to a topic.</summary>
        /// <param name="registrationTokens">A list of registration tokens to subscribe.</param>
        /// <param name="topic">The topic name to subscribe to. /topics/ will be prepended to the topic name provided if absent.</param>
        /// <returns>A task that completes with a <see cref="T:FirebaseAdmin.Messaging.TopicManagementResponse" />, giving details about the topic subscription operations.</returns>
        Task<TopicManagementResponse> SubscribeToTopicAsync(IReadOnlyList<string> registrationTokens, string topic);

        /// <summary>
        /// Unsubscribes a list of registration tokens from a topic.
        /// </summary>
        /// <param name="registrationTokens">A list of registration tokens to unsubscribe.</param>
        /// <param name="topic">The topic name to unsubscribe from. /topics/ will be prepended to the topic name provided if absent.</param>
        /// <returns>A task that completes with a <see cref="T:FirebaseAdmin.Messaging.TopicManagementResponse" />, giving details about the topic unsubscription operations.</returns>
        Task<TopicManagementResponse> UnsubscribeFromTopicAsync(IReadOnlyList<string> registrationTokens, string topic);
    }
}