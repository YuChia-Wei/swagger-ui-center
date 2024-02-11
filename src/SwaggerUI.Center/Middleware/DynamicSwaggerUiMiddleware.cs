using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerUI.Center.Middleware;

/// <summary>
/// Wrapper over SwaggerUI middleware to support reloading the options at runtime
/// </summary>
/// <remarks>https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1093</remarks>
public class DynamicSwaggerUiMiddleware : IMiddleware
{
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly ILoggerFactory _loggerFactory;
    private readonly SwaggerUIOptions _swaggerUiOptions;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="hostingEnv"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="options"></param>
    public DynamicSwaggerUiMiddleware(IWebHostEnvironment hostingEnv,
                                      ILoggerFactory loggerFactory,
                                      IOptionsSnapshot<SwaggerUIOptions> options)
    {
        this._hostingEnv = hostingEnv;
        this._loggerFactory = loggerFactory;
        this._swaggerUiOptions = options.Value;
    }

    /// <summary>
    /// invoke
    /// </summary>
    /// <param name="context"></param>
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        return new SwaggerUIMiddleware(next, this._hostingEnv, this._loggerFactory, this._swaggerUiOptions).Invoke(context);
    }
}