namespace SwaggerUI.Center.Authorization;

/// <summary>
/// 權限驗證器
/// </summary>
public interface IPermissionValidator
{
    /// <summary>
    /// 驗證使用者名稱
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<bool> VerifyAsync(string? userName);
}