using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using SwaggerUI.Center.Components.Domain;
using SwaggerUI.Center.Components.Interfaces;

namespace SwaggerUI.Center.Components.Implements;

/// <summary>
/// Open Api 文件儲存庫
/// </summary>
public class OpenApiDocumentRepository : IOpenApiDocumentRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public OpenApiDocumentRepository(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 取得 Open Api 文件
    /// </summary>
    /// <param name="apiJsonEndpoint"></param>
    /// <returns></returns>
    public async Task<OpenApiDocument> GetOpenApiDocumentAsync(WebApiEndpoint apiJsonEndpoint)
    {
        var openApiDocument = await this.DownloadOpenApiDocumentAsync(apiJsonEndpoint);

        return openApiDocument;
    }

    /// <summary>
    /// 下載外部的 OpenApi 文件
    /// </summary>
    /// <param name="apiJsonEndpoint"></param>
    /// <returns></returns>
    private async Task<OpenApiDocument> DownloadOpenApiDocumentAsync(WebApiEndpoint apiJsonEndpoint)
    {
        var httpClient = this._httpClientFactory.CreateClient();

        var stream = await httpClient.GetStreamAsync(apiJsonEndpoint.JsonUri);

        return new OpenApiStreamReader().Read(stream, out _);
    }
}