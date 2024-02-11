using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerUI.Center.Middleware;

/// <summary>
/// swagger ui 的身分驗證
/// </summary>
public class SwaggerUiAuthorizationMiddleware : IMiddleware
{
    private readonly ILogger<SwaggerUiAuthorizationMiddleware> _logger;
    private readonly ISwaggerUiPermissionValidator _permissionValidator;
    private readonly SwaggerUIOptions _swaggerUiOptions;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    /// <param name="permissionValidator"></param>
    public SwaggerUiAuthorizationMiddleware(ILogger<SwaggerUiAuthorizationMiddleware> logger,
                                            IOptionsSnapshot<SwaggerUIOptions> options,
                                            ISwaggerUiPermissionValidator permissionValidator)
    {
        this._logger = logger;
        this._permissionValidator = permissionValidator;
        this._swaggerUiOptions = options.Value;
    }

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var httpMethod = context.Request.Method;
        var path = context.Request.Path.Value;

        // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
        if (httpMethod == "GET" && this.IsMatchSwaggerUi(path))
        {
            if (IsUnauthenticated(context))
            {
                RespondWithRedirect(context.Response, "/");
                return;
            }

            if (await this.IsForbiddenAsync(context))
            {
                ResponseWithForbidden(context);
                return;
            }
        }

        await next.Invoke(context);
    }

    private async Task<bool> IsForbiddenAsync(HttpContext context)
    {
        var verifyAsync = await this._permissionValidator.VerifyAsync(context.User.Identity?.Name);
        return !verifyAsync;
    }

    private bool IsMatchSwaggerUi(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        return Regex.IsMatch(path, $"^/?{Regex.Escape(this._swaggerUiOptions.RoutePrefix)}/?$", RegexOptions.IgnoreCase) ||
               Regex.IsMatch(path, $"^/{Regex.Escape(this._swaggerUiOptions.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase);
    }

    private static bool IsUnauthenticated(HttpContext context)
    {
        return !context.User.Identity?.IsAuthenticated ?? true;
    }

    private static void RespondWithRedirect(HttpResponse response, string location)
    {
        response.StatusCode = (int)HttpStatusCode.Moved;
        response.Headers.Location = location;
    }

    private static void ResponseWithForbidden(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    }
}