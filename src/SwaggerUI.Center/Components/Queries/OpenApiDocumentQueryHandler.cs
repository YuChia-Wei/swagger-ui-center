using Mediator;
using Microsoft.OpenApi.Models;
using SwaggerUI.Center.Components.Domain;
using SwaggerUI.Center.Components.Interfaces;

namespace SwaggerUI.Center.Components.Queries;

/// <summary>
/// open api document query handler
/// </summary>
public class OpenApiDocumentQueryHandler : IQueryHandler<OpenApiDocumentQuery, OpenApiDocument>
{
    private readonly IOpenApiDocumentRepository _openApiDocumentRepository;
    private readonly IWebApiEndpointRepository _webApiEndpointRepository;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="webApiEndpointRepository"></param>
    /// <param name="openApiDocumentRepository"></param>
    public OpenApiDocumentQueryHandler(IWebApiEndpointRepository webApiEndpointRepository,
                                       IOpenApiDocumentRepository openApiDocumentRepository)
    {
        this._openApiDocumentRepository = openApiDocumentRepository;
        this._webApiEndpointRepository = webApiEndpointRepository;
    }

    /// <summary>
    /// handle
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<OpenApiDocument> Handle(OpenApiDocumentQuery query, CancellationToken cancellationToken)
    {
        var apiJsonEndpoint = this.GetOpenApiDocEndpoint(query.ServiceName, query.RequestUri);

        var openApiServers = apiJsonEndpoint.GetOpenApiServerList();

        var openApiDocument = await this._openApiDocumentRepository.GetOpenApiDocumentAsync(apiJsonEndpoint);

        FillApiServerList(openApiDocument, openApiServers);

        // return openApiDocument.SerializeAsJson<OpenApiDocument>(OpenApiSpecVersion.OpenApi3_0);
        return openApiDocument;
    }

    private static void FillApiServerList(OpenApiDocument openApiDocument, IEnumerable<OpenApiServer> openApiServers)
    {
        foreach (var openApiServer in openApiServers.Where(o => openApiDocument.Servers.All<OpenApiServer>(doc => doc.Url != o.Url)))
        {
            openApiDocument.Servers.Add(openApiServer);
        }
    }

    private WebApiEndpoint GetOpenApiDocEndpoint(string serviceName, string requestUri)
    {
        var apiJsonEndpoint = this._webApiEndpointRepository.GetAsync(serviceName);

        apiJsonEndpoint.ParseRelateUri(requestUri);
        return apiJsonEndpoint;
    }
}