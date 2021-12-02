using System.Threading.Tasks;

namespace Common.Outbox.Consumer
{
    /// <summary>
    /// Message consumer interface.
    /// </summary>
    /// <typeparam name="TMessage">Message.</typeparam>
    public interface IConsumer<in TMessage>
        where TMessage : class
    {
        /// <summary>
        /// Consumes the <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Asynchronous operation.</returns>
        Task Consume(TMessage message);
    }
}