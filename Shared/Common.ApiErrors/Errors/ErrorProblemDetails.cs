using Microsoft.AspNetCore.Mvc;

namespace Common.ApiErrors.Errors
{
    public class ErrorProblemDetails<TError>
        : ProblemDetails
    {
        public TError Error { get; set; }
    }
}