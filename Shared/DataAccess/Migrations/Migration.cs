using System.Threading.Tasks;

namespace DataAccess.Migrations
{
    /// <summary>
    /// Migration.
    /// </summary>
    /// <typeparam name="TContext">Context.</typeparam>
    public abstract class Migration<TContext> : IMigration
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration{TContext}"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        protected Migration(TContext context)
        {
            Context = context;
        }

        /// <inheritdoc/>
        public abstract string Id { get; }

        /// <summary>
        /// Gets context.
        /// </summary>
        protected TContext Context { get; }

        /// <inheritdoc/>
        public abstract Task Up();

        /// <inheritdoc/>
        public abstract Task Down();
    }
}