using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using DataAccess.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Notifications.Domain
{
    /// <summary>
    /// User.
    /// </summary>
    public sealed class User : EntityBase
    {
        /// <summary>
        /// Subscriptions.
        /// </summary>
        private HashSet<string> _subscriptions = new ();

        /// <summary>
        /// Devices.
        /// </summary>
        private HashSet<Device> _devices = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userId">User id.</param>
        public User(Guid userId)
        {
            Id = userId;
        }

        /// <summary>
        /// Gets subscriptions.
        /// </summary>
        [BsonElement("subscriptions")]
        public IReadOnlyCollection<string> Subscriptions
        {
            get => _subscriptions;
            private set => _subscriptions = value.ToHashSet();
        }

        /// <summary>
        /// Gets devices.
        /// </summary>
        [BsonElement("devices")]
        public IReadOnlyCollection<Device> Devices
        {
            get => _devices;
            private set => _devices = value.ToHashSet();
        }

        /// <summary>
        /// Add link between user and device.
        /// </summary>
        /// <param name="device">Device.</param>
        /// <returns>
        /// <see langword="true" /> if the device is added; <see langword="false" /> if the device is already present.
        /// </returns>
        public bool LinkToDevice(Device device)
        {
            return _devices.Add(device);
        }

        /// <summary>
        /// Removes the link between user and device.
        /// </summary>
        /// <param name="device">Device.</param>
        /// <returns>
        /// <see langword="true" /> if the device is unlinked; <see langword="false" /> if the device is already unlinked.
        /// </returns>
        public bool UnlinkFromDevice(Device device)
        {
            return _devices.Remove(device);
        }

        /// <summary>
        /// Finds device by the token.
        /// </summary>
        /// <param name="token">Device token.</param>
        /// <returns>Device.</returns>
        public Maybe<Device> FindDeviceByToken(string token)
        {
            return _devices.SingleOrDefault(device => device.Token == token);
        }

        /// <summary>
        /// Subscribes user to the topic.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <returns>
        /// <see langword="true"/> if the user subscribed, <see langword="false"/> if the user is already subscribed.
        /// </returns>
        public bool SubscribeToTopic(string topic)
        {
            return _subscriptions.Add(topic);
        }

        /// <summary>
        /// Unsubscribes the user from the topic.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <returns>
        /// <see langword="true"/> if the user unsubscribed, <see langword="false"/> if the subscription is not found.
        /// </returns>
        public bool UnsubscribeFromTopic(string topic)
        {
            return _subscriptions.Remove(topic);
        }
    }
}
