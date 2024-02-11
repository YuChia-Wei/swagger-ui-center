using Microsoft.Extensions.Options;
using SwaggerUI.Center.Components.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerUI.Center.Swagger;

/// <summary>
/// Swagger Ui Options Uri 設定
/// </summary>
/// <remarks>https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1093</remarks>
public class SwaggerUiOptionsConfigure : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IWebApiEndpointRepository _webApiEndpointRepository;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="webApiEndpointRepository"></param>
    public SwaggerUiOptionsConfigure(
        IWebApiEndpointRepository webApiEndpointRepository)
    {
        this._webApiEndpointRepository = webApiEndpointRepository;
    }

    /// <summary>
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerUIOptions options)
    {
        var services = this._webApiEndpointRepository.GetList();

        // options.RoutePrefix = "";

        // Clear the list of services before adding more
        options.ConfigObject.Urls = null;

        foreach (var service in services.Where(o => o.IsSwaggerEnabled))
        {
            options.SwaggerEndpoint($"/api/doc/{service.ServiceName}",
                                    $"{service.ServiceName}");
        }
    }
}