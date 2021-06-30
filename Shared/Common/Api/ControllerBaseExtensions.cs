using Common.Api.Errors;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api
{
    public static class ControllerBaseExtensions
    {
        private const string ErrorKey = "error";

        public static IActionResult FromResult<T>(
            this ControllerBase controller, Result<T> result)
        {
            return result.IsSuccess
                ? controller.Ok(result.Value)
                : controller.Conflict(CreateProblemDetails(
                        controller.HttpContext, StatusCodes.Status409Conflict, result.Error));
        }

        public static IActionResult FromResult(
            this ControllerBase controller, Result result)
        {
            return result.IsSuccess
                ? controller.NoContent()
                : controller.Conflict(CreateProblemDetails(
                    controller.HttpContext, StatusCodes.Status409Conflict, result.Error));
        }

        public static IActionResult FromResult(
            this ControllerBase controller, Error error)
        {
            if (error is NotFoundError)
            {
                var notFound = CreateProblemDetails(
                    controller.HttpContext, StatusCodes.Status404NotFound, error);
                return controller.NotFound(notFound);
            }

            var conflict = CreateProblemDetails(
                controller.HttpContext, StatusCodes.Status409Conflict, error);
            return controller.Conflict(conflict);
        }

        public static IActionResult FromResult<T, TDetails>(
            this ControllerBase controller, Error<TDetails> error)
        {
            var conflict = CreateProblemDetails(
                controller.HttpContext, StatusCodes.Status409Conflict, error);
            return controller.Conflict(conflict);
        }

        private static ProblemDetails CreateProblemDetails(
            HttpContext context, int statusCode, Error error)
        {
            var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = factory.CreateProblemDetails(
                context, statusCode, error.Code, detail: error.Detail);
            return problemDetails;
        }

        private static ProblemDetails CreateProblemDetails(
            HttpContext context, int statusCode, string error)
        {
            var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = factory.CreateProblemDetails(
                context, statusCode, detail: error);
            return problemDetails;
        }

        private static ProblemDetails CreateProblemDetails<TExtension>(
            HttpContext context, int statusCode, Error<TExtension> error)
        {
            var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = factory.CreateProblemDetails(
                context, statusCode, error.Code, detail: error.Detail);
            problemDetails.Extensions.Add(ErrorKey, error.Extension);
            return problemDetails;
        }
    }
}