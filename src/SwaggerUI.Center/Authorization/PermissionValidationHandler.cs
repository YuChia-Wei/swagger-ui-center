using Microsoft.AspNetCore.Authorization;

namespace SwaggerUI.Center.Authorization;

public class PermissionValidationHandler : AuthorizationHandler<PermissionValidationRequirement>
{
    private readonly IPermissionValidator _validator;

    public PermissionValidationHandler(IPermissionValidator validator)
    {
        this._validator = validator;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionValidationRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            if (await this._validator.VerifyAsync(context.User.Identity.Name))
            {
                context.Succeed(requirement);
            }
        }

        // 如果未成功授權，會自動返回 403 Forbidden
    }
}