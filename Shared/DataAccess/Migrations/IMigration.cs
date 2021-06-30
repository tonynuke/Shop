using System.Threading.Tasks;

namespace DataAccess.Migrations
{
    /// <summary>
    /// Migration interface.
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Up migration.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        Task Up();

        /// <summary>
        /// Down migration.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        Task Down();
    }
}