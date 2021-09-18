using System.Threading.Tasks;

namespace Common.AspNetCore.Outbox
{
    /// <summary>
    /// Message handler interface.
    /// </summary>
    /// <typeparam name="TMessage">Message.</typeparam>
    public interface IMessageHandler<in TMessage>
        where TMessage : class
    {
        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Asynchronous operation.</returns>
        Task Handle(TMessage message);
    }
}