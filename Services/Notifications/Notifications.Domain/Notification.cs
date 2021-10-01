using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Notifications.Domain
{
    /// <summary>
    /// Notification.
    /// </summary>
    public class Notification : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="body">Body.</param>
        public Notification(string title, string body)
        {
            Title = title;
            Body = body;
        }

        /// <summary>
        /// Gets title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets body.
        /// </summary>
        public string Body { get; }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Body;
        }
    }
}