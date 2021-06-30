using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Catalog.Domain
{
    /// <summary>
    /// Name.
    /// </summary>
    public class Name : ValueObject
    {
        private const string EmptyErrorMessage = "Value can't be empty.";

        private Name(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Implicitly converts the to the string.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Name string representation.</returns>
        public static implicit operator string(Name name)
        {
            return name.Value;
        }

        /// <summary>
        /// Creates <see cref="Name"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Name.</returns>
        public static Result<Name> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Name>(EmptyErrorMessage);
            }

            return new Name(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}