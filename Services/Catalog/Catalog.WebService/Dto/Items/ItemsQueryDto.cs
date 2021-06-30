using Common.Pagination.Dto;

namespace Catalog.WebService.Dto.Items
{
    /// <summary>
    /// Model for items searching.
    /// </summary>
    public record ItemsQueryDto
    {
        /// <summary>
        /// Gets query.
        /// </summary>
        public string Query { get; init; }

        /// <summary>
        /// Gets page.
        /// </summary>
        public PageDto Page { get; init; }
    }
}