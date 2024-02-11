using Mediator;
using Microsoft.OpenApi.Models;

namespace SwaggerUI.Center.Components.Queries;

/// <summary>
/// open api document query
/// </summary>
public class OpenApiDocumentQuery : IQuery<OpenApiDocument>
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="serviceName"></param>
    /// <param name="requestUri"></param>
    public OpenApiDocumentQuery(string serviceName, string requestUri)
    {
        this.ServiceName = serviceName;
        this.RequestUri = requestUri;
    }

    /// <summary>
    /// service name
    /// </summary>
    public string ServiceName { get; private set; }

    /// <summary>
    /// Api Uri
    /// </summary>
    public string RequestUri { get; private set; }
}