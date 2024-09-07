using Microsoft.AspNetCore.Authorization;

namespace SwaggerUI.Center.Authorization;

/// <summary>
/// authorization policy builder 的擴充方法
/// </summary>
public static class AuthorizationPolicyBuilderExtension
{
    /// <summary>
    /// 需要使用外部權限驗證
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static AuthorizationPolicyBuilder RequirePermissionValidation(this AuthorizationPolicyBuilder builder)
    {
        builder.Requirements.Add(new PermissionValidationRequirement());

        return builder;
    }
}