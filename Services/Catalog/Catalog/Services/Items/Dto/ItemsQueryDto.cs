using Common.Pagination;

namespace Catalog.Services.Items.Dto
{
    /// <summary>
    /// Items query dto.
    /// </summary>
    public class ItemsQueryDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsQueryDto"/> class.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="page">Page.</param>
        public ItemsQueryDto(string query, Page page)
        {
            Query = query;
            Page = page;
        }

        /// <summary>
        /// Gets query.
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Gets page.
        /// </summary>
        public Page Page { get; }
    }
}