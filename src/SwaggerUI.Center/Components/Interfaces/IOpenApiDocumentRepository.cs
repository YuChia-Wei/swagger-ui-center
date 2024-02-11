using Microsoft.OpenApi.Models;
using SwaggerUI.Center.Components.Domain;

namespace SwaggerUI.Center.Components.Interfaces;

/// <summary>
/// Open Api 文件儲存庫
/// </summary>
public interface IOpenApiDocumentRepository
{
    /// <summary>
    /// 取得 Open Api 文件
    /// </summary>
    /// <param name="apiJsonEndpoint"></param>
    /// <returns></returns>
    Task<OpenApiDocument> GetOpenApiDocumentAsync(WebApiEndpoint apiJsonEndpoint);
}