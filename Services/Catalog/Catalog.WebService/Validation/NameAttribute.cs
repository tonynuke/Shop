using System.ComponentModel.DataAnnotations;
using Catalog.Domain;

namespace Catalog.WebService.Validation
{
    /// <summary>
    /// <see cref="Name"/> validation attribute.
    /// </summary>
    public class NameAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var nameResult = Name.Create(value as string);

            return nameResult.IsFailure
                ? new ValidationResult(nameResult.Error)
                : ValidationResult.Success;
        }
    }
}