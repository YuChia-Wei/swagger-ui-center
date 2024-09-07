namespace SwaggerUI.Center.Middleware;

/// <summary>
/// </summary>
public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 使用動態 Swagger Define 選單 (右上角下拉選單)
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDynamicSwaggerUi(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DynamicSwaggerUiMiddleware>();
    }
}