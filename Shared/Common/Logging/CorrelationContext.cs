using System;
using System.Threading;

namespace Common.Logging
{
    /// <summary>
    /// Correlation context.
    /// </summary>
    /// <remarks>Propagates correlation id.</remarks>
    public static class CorrelationContext
    {
        private const string CorrelationIdIsAlreadySpecified = "Correlation id is already specified.";
        private static readonly AsyncLocal<string> Id = new ();

        /// <summary>
        /// Gets correlation id.
        /// </summary>
        public static string CorrelationId
        {
            get => Id.Value;
            internal set
            {
                if (Id.Value == null)
                {
                    Id.Value = value;
                }
                else
                {
                    throw new InvalidOperationException(CorrelationIdIsAlreadySpecified);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether correlation id is specified.
        /// </summary>
        internal static bool IsSpecified => Id.Value != null;
    }
}