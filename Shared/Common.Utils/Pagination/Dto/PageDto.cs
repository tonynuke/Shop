namespace Common.Utils.Pagination.Dto
{
    /// <summary>
    /// Page dto.
    /// </summary>
    [Page]
    public record PageDto
    {
        /// <summary>
        /// Gets records count to skip.
        /// </summary>
        public int Skip { get; init; }

        /// <summary>
        /// Gets maximum count of records to return.
        /// </summary>
        public int Limit { get; init; }
    }
}