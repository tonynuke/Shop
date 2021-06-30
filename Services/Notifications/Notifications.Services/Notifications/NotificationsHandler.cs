using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Notifications.Domain;
using Notifications.Persistence;
using Notifications.Services.Firebase;

namespace Notifications.Services.Notifications
{
    /// <summary>
    /// Notifications handler.
    /// </summary>
    public class NotificationsHandler :
        IRequestHandler<SendUserPushNotificationCommand, Unit>,
        IRequestHandler<SendTopicPushNotificationCommand, Unit>
    {
        private readonly NotificationsContext _notificationsContext;
        private readonly IFirebaseMessaging _firebaseMessaging;
        private readonly ILogger<NotificationsHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsHandler"/> class.
        /// </summary>
        /// <param name="notificationsContext"></param>
        /// <param name="firebaseMessaging"></param>
        /// <param name="logger"></param>
        public NotificationsHandler(
            NotificationsContext notificationsContext,
            IFirebaseMessaging firebaseMessaging,
            ILogger<NotificationsHandler> logger)
        {
            _notificationsContext = notificationsContext;
            _firebaseMessaging = firebaseMessaging;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Unit> Handle(SendUserPushNotificationCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> userOrNothing = await _notificationsContext.Users
                .Find(u => u.Id == request.UserId)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return await userOrNothing
                .ToResult(Errors.UserNotFound)
                .OnFailure(error => _logger.LogError(error, request.UserId))
                .Map(user => MessagesGenerator.GenerateMulticastMessage(user, request.Notification))
                .Tap(_firebaseMessaging.SendMulticastAsync)
                .Finally(_ => Unit.Value);
        }

        /// <inheritdoc/>
        public async Task<Unit> Handle(SendTopicPushNotificationCommand request, CancellationToken cancellationToken)
        {
            var message = MessagesGenerator.GenerateMessage(request.Topic, request.Notification);
            await _firebaseMessaging.SendAsync(message);
            return Unit.Value;
        }
    }
}