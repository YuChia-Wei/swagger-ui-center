using Microsoft.AspNetCore.Authorization;

namespace SwaggerUI.Center.Authorization;

/// <summary>
/// 需要進行權限驗證的身份驗證要求
/// </summary>
public class PermissionValidationRequirement : IAuthorizationRequirement;