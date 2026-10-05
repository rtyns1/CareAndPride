using System.Text.Json;
using CarePrideSystem.Application.Exceptions;
using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex, "Invalid credentials");
                await WriteResponse(context, 401, ex.Message);
            }
            catch (AccountNotApprovedException ex)
            {
                _logger.LogWarning(ex, "Account not approved");
                await WriteResponse(context, 403, ex.Message);
            }
            catch (AccountDisabledException ex)
            {
                _logger.LogWarning(ex, "Account disabled");
                await WriteResponse(context, 403, ex.Message);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain error");
                await WriteResponse(context, 400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                await WriteResponse(context, 400, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Forbidden");
                await WriteResponse(context, 403, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteResponse(context, 500, "An unexpected error occurred.");
            }
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
        }
    }
}

