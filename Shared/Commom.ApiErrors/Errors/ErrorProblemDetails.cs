using Microsoft.AspNetCore.Mvc;

namespace Commom.ApiErrors.Errors
{
    public class ErrorProblemDetails<TError>
        : ProblemDetails
    {
        public TError Error { get; set; }
    }
}