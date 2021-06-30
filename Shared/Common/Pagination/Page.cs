using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Common.Pagination
{
    /// <summary>
    /// Page.
    /// </summary>
    public class Page : ValueObject
    {
        private const int DefaultSkip = 0;
        private const int MinLimit = 1;
        private const int MaxLimit = 1_000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        /// <param name="skip">Skip.</param>
        /// <param name="limit">Limit.</param>
        private Page(int skip, int limit)
        {
            Skip = skip;
            Limit = limit;
        }

        /// <summary>
        /// Gets records count to skip.
        /// </summary>
        public int Skip { get; }

        /// <summary>
        /// Gets or sets maximum count of records to return.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Creates <see cref="Page"/>.
        /// </summary>
        /// <param name="skip">Number.</param>
        /// <param name="limit">Size.</param>
        /// <returns>Page.</returns>
        public static Result<Page, IReadOnlyCollection<string>> Create(int skip, int limit)
        {
            var validationErrors = new List<string>();

            if (skip < DefaultSkip)
            {
                var error = $"Skip should be greater than {DefaultSkip}.";
                validationErrors.Add(error);
            }

            if (limit < MinLimit || limit > MaxLimit)
            {
                var error = $"Limit should be in range from {MinLimit} to {MaxLimit}";
                validationErrors.Add(error);
            }

            return !validationErrors.Any()
                ? new Page(skip, limit)
                : Result.Failure<Page, IReadOnlyCollection<string>>(validationErrors);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Skip;
            yield return Limit;
        }
    }
}