namespace SwaggerUI.Center.Authorization;

/// <summary>
/// permission validator
/// </summary>
public class PermissionValidator : IPermissionValidator
{
    public Task<bool> VerifyAsync(string? userName)
    {
        return Task.FromResult(true);
    }
}