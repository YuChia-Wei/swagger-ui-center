namespace SwaggerUI.Center.Middleware;

public interface ISwaggerUiPermissionValidator
{
    Task<bool> VerifyAsync(string? userName);
}