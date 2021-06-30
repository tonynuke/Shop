using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MongoDB.Bson.Serialization.Attributes;

namespace Notifications.Domain
{
    /// <summary>
    /// Device.
    /// </summary>
    public class Device : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="token">Token.</param>
        public Device(string token)
        {
            Token = token;
        }

        /// <summary>
        /// Gets push platform token.
        /// </summary>
        [BsonElement("token")]
        public string Token { get; private set; }

        /// <summary>
        /// Creates device.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>Device.</returns>
        public static Result<Device> Create(string token)
        {
            return string.IsNullOrWhiteSpace(token)
                ? Result.Failure<Device>("Token can't be null or whitespace.")
                : new Device(token);
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Token;
        }
    }
}