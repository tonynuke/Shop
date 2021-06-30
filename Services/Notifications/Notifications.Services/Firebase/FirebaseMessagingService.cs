using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace Notifications.Services.Firebase
{
    /// <summary>
    /// Firebase messaging service.
    /// </summary>
    public class FirebaseMessagingService : IFirebaseMessaging
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseMessagingService"/> class.
        /// </summary>
        /// <param name="firebaseMessaging">Entry point to all server-side Firebase Cloud Messaging operations.</param>
        public FirebaseMessagingService(FirebaseMessaging firebaseMessaging)
        {
            _firebaseMessaging = firebaseMessaging ?? throw new ArgumentNullException(nameof(firebaseMessaging));
        }

        /// <inheritdoc/>
        public Task<BatchResponse> SendAllAsync(IEnumerable<Message> messages)
        {
            return _firebaseMessaging.SendAllAsync(messages);
        }

        /// <inheritdoc/>
        public Task<string> SendAsync(Message message)
        {
            return _firebaseMessaging.SendAsync(message);
        }

        /// <inheritdoc/>
        public Task<BatchResponse> SendMulticastAsync(MulticastMessage message)
        {
            return _firebaseMessaging.SendMulticastAsync(message);
        }

        /// <inheritdoc/>
        public Task<TopicManagementResponse> SubscribeToTopicAsync(
            IReadOnlyList<string> registrationTokens, string topic)
        {
            return _firebaseMessaging.SubscribeToTopicAsync(registrationTokens, topic);
        }

        /// <inheritdoc/>
        public Task<TopicManagementResponse> UnsubscribeFromTopicAsync(
            IReadOnlyList<string> registrationTokens, string topic)
        {
            return _firebaseMessaging.UnsubscribeFromTopicAsync(registrationTokens, topic);
        }
    }
}