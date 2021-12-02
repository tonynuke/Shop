using System;

namespace Common.Hosting.Configuration
{
    /// <summary>
    /// Конфигурация RabbitMq.
    /// </summary>
    public class RabbitMqConfiguration
    {
        public const string Key = "RabbitMq";

        /// <summary>
        /// Время ожидания между повторной отправкой сообщения.
        /// </summary>
        public TimeSpan RedeliveryInterval { get; set; }

        /// <summary>
        /// Максимальное количество попыток повторной отправкой сообщения.
        /// </summary>
        /// <remarks>Используется для отложенной отправки сообщений.</remarks>
        public int MaxRedeliveryCount { get; set; }

        /// <summary>
        /// Retry limit.
        /// </summary>
        public int RetryLimit { get; set; }

        /// <summary>
        /// Timeout before message send retry.
        /// </summary>
        public TimeSpan RetryInterval { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public string Username { get; set; }

        public string Password { get; set; }

        public Uri HostAddress { get; set; }
    }
}