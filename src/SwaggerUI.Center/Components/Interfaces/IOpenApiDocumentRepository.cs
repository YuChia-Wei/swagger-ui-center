using Microsoft.OpenApi.Models;
using SwaggerUI.Center.Components.OptionModels;

namespace SwaggerUI.Center.Components.Interfaces;

/// <summary>
/// Open Api 文件儲存庫
/// </summary>
public interface IOpenApiDocumentRepository
{
    /// <summary>
    /// 取得 Open Api 文件
    /// </summary>
    /// <param name="apiJsonEndpointOption"></param>
    /// <returns></returns>
    Task<OpenApiDocument> GetOpenApiDocumentAsync(OpenApiDocEndpointOption apiJsonEndpointOption);
}