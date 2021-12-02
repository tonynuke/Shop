using System.Collections.Generic;

namespace Common.Utils.Pagination
{
    /// <summary>
    /// Page with content.
    /// </summary>
    /// <typeparam name="TContent">Content type.</typeparam>
    public class PageContent<TContent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageContent{TContent}"/> class.
        /// </summary>
        /// <param name="records">Records.</param>
        /// <param name="totalRecords">Total records count.</param>
        public PageContent(IReadOnlyCollection<TContent> records, long totalRecords)
        {
            Records = records;
            TotalRecords = totalRecords;
        }

        /// <summary>
        /// Gets records.
        /// </summary>
        public IReadOnlyCollection<TContent> Records { get; }

        /// <summary>
        /// Gets total records count.
        /// </summary>
        public long TotalRecords { get; }
    }
}