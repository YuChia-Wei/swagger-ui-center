namespace SwaggerUI.Center.Middleware;

/// <summary>
/// </summary>
public static class ApplicationBuilderExtension
{
    /// <summary>
    /// Uses the middleware log.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static IApplicationBuilder PipeLineLog(this IApplicationBuilder builder, string msg)
    {
        return builder.UseMiddleware<PipeLineLogMiddleware>(msg);
    }

    /// <summary>
    /// 使用動態 Swagger Define 選單 (右上角下拉選單)
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDynamicSwaggerUi(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DynamicSwaggerUiMiddleware>();
    }

    /// <summary>
    /// Swagger Ui Authorization
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseSwaggerUiAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwaggerUiAuthorizationMiddleware>();
    }
}