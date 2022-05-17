using System;

namespace Common.ApiErrors.Errors
{
    /// <summary>
    /// Error.
    /// </summary>
    public class Error
    {
        public Error(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public Error(string code, string detail)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Detail = detail ?? throw new ArgumentNullException(nameof(code));
        }

        public string Code { get; }

        public string Detail { get; }
    }

    /// <summary>
    /// Generic error.
    /// </summary>
    /// <typeparam name="TExtension">Error payload.</typeparam>
    public class Error<TExtension> : Error
    {
        public Error(string code, TExtension extension)
            : base(code)
        {
            Extension = extension;
        }

        public Error(string code, string detail, TExtension extension)
            : base(code, detail)
        {
            Extension = extension;
        }

        public TExtension Extension { get; }
    }
}