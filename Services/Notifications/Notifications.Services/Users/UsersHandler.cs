using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Notifications.Domain;
using Notifications.Persistence;
using Notifications.Services.Firebase;
using Notifications.Services.Users.Devices;
using Notifications.Services.Users.Subscriptions;

namespace Notifications.Services.Users
{
    public class UsersHandler :
        IRequestHandler<LinkUserToDeviceCommand, Unit>,
        IRequestHandler<UnlinkUserFromDeviceCommand, Unit>,
        IRequestHandler<FindUserByIdQuery, Maybe<User>>,
        IRequestHandler<SubscribeCommand, Unit>,
        IRequestHandler<UnsubscribeCommand, Unit>,
        IRequestHandler<UnsubscribeFromAllCommand, Unit>
    {
        private readonly NotificationsContext _notificationsContext;
        private readonly IFirebaseMessaging _firebaseMessaging;
        private readonly ILogger<UsersHandler> _logger;

        public UsersHandler(
            NotificationsContext notificationsContext,
            IFirebaseMessaging firebaseMessaging,
            ILogger<UsersHandler> logger)
        {
            _notificationsContext = notificationsContext;
            _firebaseMessaging = firebaseMessaging;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Unit> Handle(LinkUserToDeviceCommand request, CancellationToken cancellationToken = default)
        {
            var previousUser = await _notificationsContext.Users.Find(
                u => u.Devices.Any(device => device.Token == request.Token))
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            if (previousUser != null)
            {
                bool isSameUser = previousUser.Id == request.UserId;
                if (isSameUser)
                {
                    return Unit.Value;
                }

                _logger.LogInformation(
                    "Previous user {PreviousUserId} will be unlinked due to new user {NewUserId}. Device token {Token}",
                    previousUser.Id,
                    request.UserId,
                    request.Token);

                await UnlinkUserDeviceAndSave(previousUser, request.Token);
            }

            await FindUser(request.UserId)
                .OnFailureCompensate(_ => new User(request.UserId))
                .Bind(user => Device.Create(request.Token)
                    .Ensure(user.LinkToDevice, Errors.UserIsAlreadyLinked)
                    .Tap(_ => SaveUser(user)));

            return Unit.Value;
        }

        /// <inheritdoc/>
        public Task<Unit> Handle(UnlinkUserFromDeviceCommand request, CancellationToken cancellationToken = default)
        {
            return FindUser(request.UserId)
                .Tap(user => UnlinkUserDeviceAndSave(user, request.Token))
                .Finally(_ => Unit.Value);
        }

        /// <inheritdoc/>
        public async Task<Maybe<User>> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _notificationsContext.Users
                .Find(u => u.Id == request.UserId)
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Unit> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            return FindUser(request.UserId)
                .Ensure(user => user.SubscribeToTopic(request.Topic), Errors.UserIsAlreadySubscribed)
                .Tap(user => SubscribeUserDevicesToTopic(user, request.Topic))
                .Tap(SaveUser)
                .Finally(_ => Unit.Value);

            Task SubscribeUserDevicesToTopic(User user, string topic)
            {
                var tokens = user.Devices.Select(device => device.Token).ToList();
                return _firebaseMessaging.SubscribeToTopicAsync(tokens, topic);
            }
        }

        /// <inheritdoc/>
        public Task<Unit> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            return FindUser(request.UserId)
                .Ensure(user => user.UnsubscribeFromTopic(request.Topic), Errors.UserIsAlreadyUnsubscribed)
                .Tap(user => UnsubscribeUserDevicesFromTopic(user, request.Topic))
                .Tap(SaveUser)
                .Finally(_ => Unit.Value);

            Task UnsubscribeUserDevicesFromTopic(User user, string topic)
            {
                var tokens = user.Devices.Select(device => device.Token).ToList();
                return _firebaseMessaging.UnsubscribeFromTopicAsync(tokens, topic);
            }
        }

        /// <inheritdoc/>
        public async Task<Unit> Handle(UnsubscribeFromAllCommand request, CancellationToken cancellationToken = default)
        {
            var tokens = new[] { request.DeviceToken };
            var tasks = request.User.Subscriptions.Select(
                topic => _firebaseMessaging.UnsubscribeFromTopicAsync(tokens, topic));
            await Task.WhenAll(tasks);
            return Unit.Value;
        }

        private Task UnlinkUserDeviceAndSave(User user, string deviceToken)
        {
            return user.FindDeviceByToken(deviceToken)
                .ToResult(Errors.DeviceNotFound)
                .Ensure(user.UnlinkFromDevice, Errors.UserIsAlreadyUnlinked)
                .Map(_ => new UnsubscribeFromAllCommand(user, deviceToken))
                .Tap(command => Handle(command))
                .Tap(_ => SaveUser(user));
        }

        private async Task<Result<User>> FindUser(Guid userId)
        {
            Maybe<User> user = await _notificationsContext.Users
                .Find(u => u.Id == userId)
                .SingleOrDefaultAsync();
            return user
                .ToResult(Errors.UserNotFound)
                .OnFailure(error => _logger.LogError(error, userId));
        }

        private Task SaveUser(User user)
        {
            var replaceOptions = new ReplaceOptions
            {
                IsUpsert = true
            };
            return _notificationsContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user, replaceOptions);
        }
    }
}