using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using SwaggerUI.Center.Components.Queries;

namespace SwaggerUI.Center.Controllers;

/// <summary>
/// 取得 OpenApi 文件
/// </summary>
[Route("api/doc")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class OpenApiDocumentController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// ctor
    /// </summary>
    public OpenApiDocumentController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    /// <summary>
    /// 取得特定服務
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    [HttpGet("{serviceName}")]
    public async Task<IActionResult> Get([FromRoute] string serviceName)
    {
        var httpContextRequest = this.HttpContext.Request;
        var requestUri = $"{httpContextRequest.Scheme}://{httpContextRequest.Host}";
        var jsonAsync = await this._mediator.Send(new OpenApiDocumentQuery(serviceName, requestUri));

        return this.Ok(jsonAsync.SerializeAsJson<OpenApiDocument>(OpenApiSpecVersion.OpenApi3_0));
    }
}