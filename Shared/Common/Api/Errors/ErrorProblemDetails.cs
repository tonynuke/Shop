using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Errors
{
    public class ErrorProblemDetails<TError>
        : ProblemDetails
    {
        public TError Error { get; set; }
    }
}