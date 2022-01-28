using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Checkout.PaymentGateway.Api.Middleware;

/// <summary>
/// A catch all error handler middleware. This code should ideally
/// be used very seldom as errors should be caught in the service code.
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IActionResultExecutor<ObjectResult> _actionResultExecutor;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IActionResultExecutor<ObjectResult> actionResultExecutor,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _next = next;
        _logger = logger;
        _actionResultExecutor = actionResultExecutor;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in service");
            await WriteProblemDetails(context,
                _problemDetailsFactory.CreateProblemDetails(context,
                StatusCodes.Status500InternalServerError,
                "Service Error",
                null,
                "Unexpected error in service",
                 context.Request.Path));
        }
    }

    private async Task WriteProblemDetails(HttpContext context, ProblemDetails details)
    {
        var routeData = context.GetRouteData() ?? new RouteData();
        var actionContext = new ActionContext(context, routeData, new ActionDescriptor());

        var result = new ObjectResult(details)
        {
            StatusCode = details.Status ?? context.Response.StatusCode,
        };

        await _actionResultExecutor.ExecuteAsync(actionContext, result);
        await context.Response.CompleteAsync();
    }
}
