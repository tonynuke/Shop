using Common.Utils.Pagination.Dto;
using System.ComponentModel.DataAnnotations;

namespace Common.Utils.Pagination
{
    /// <summary>
    /// <see cref="Page"/> validation attribute.
    /// </summary>
    public class PageAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var model = value as PageDto;

            var pageResult = Page.Create(model.Skip, model.Limit);

            return pageResult.IsFailure
                ? new ValidationResult(null, pageResult.Error)
                : ValidationResult.Success;
        }
    }
}