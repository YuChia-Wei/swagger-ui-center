namespace SwaggerUI.Center.Middleware;

/// <summary>
/// permission validator
/// </summary>
public class SwaggerUiPermissionValidator : ISwaggerUiPermissionValidator
{
    public Task<bool> VerifyAsync(string? userName)
    {
        return Task.FromResult(true);
    }
}