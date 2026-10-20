namespace Harden_a_CRUD_Web_API.Handlers
{
    using Harden_a_CRUD_Web_API.Exceptions;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    public class GlobalExceptionHandler
        : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler>
            _logger;

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
                    (404, "Resource not found"),

                ConflictException =>
                    (409, "Conflict"),

                _ =>
                    (500, "An unexpected error occurred")
            };

            _logger.LogError(
                ex,
                "Unhandled exception");

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = status == 500
                    ? null
                    : ex.Message
            };

            context.Response.StatusCode = status;

            await context.Response
                .WriteAsJsonAsync(problem, ct);

            return true;
        }
    }
}
