using Harden_a_CRUD_Web_API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Harden_a_CRUD_Web_API.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception ex,
            CancellationToken ct)
        {
            var (status, title) = ex switch
            {
                NotFoundException =>
                    (StatusCodes.Status404NotFound, "Resource not found"),

                ConflictException =>
                    (StatusCodes.Status409Conflict, "Conflict"),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                     "An unexpected error occurred")
            };

            _logger.LogError(
                ex,
                "Unhandled exception occurred.");

            var problem = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{status}",
                Title = title,
                Status = status,
                Detail = status == 500
                    ? "An unexpected error occurred."
                    : ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = status;

            await context.Response.WriteAsJsonAsync(
                problem,
                ct);

            return true;
        }
    }
}
