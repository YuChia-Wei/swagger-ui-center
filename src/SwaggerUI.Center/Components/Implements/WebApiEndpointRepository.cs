using Microsoft.Extensions.Options;
using SwaggerUI.Center.Components.Domain;
using SwaggerUI.Center.Components.Interfaces;

namespace SwaggerUI.Center.Components.Implements;

/// <summary>
/// OpenApiDocumentEndpoint 紀錄
/// </summary>
public class WebApiEndpointRepository : IWebApiEndpointRepository
{
    private readonly ILogger _logger;
    private readonly IOptionsMonitor<WebApiEndpoints> _optionsMonitor;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public WebApiEndpointRepository(IOptionsMonitor<WebApiEndpoints> optionsMonitor, ILoggerFactory loggerFactory)
    {
        this._optionsMonitor = optionsMonitor;
        this._logger = loggerFactory.CreateLogger<WebApiEndpointRepository>();
    }

    /// <summary>
    /// 取得 OpenApi 文件的 Url 資料
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public WebApiEndpoint GetAsync(string serviceName)
    {
        return this.GetApiEndpoints()
                   .First(o => o.ServiceName.Equals(serviceName, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// 取得目前記錄的 OpenApi 文件 Url 清單
    /// </summary>
    /// <returns></returns>
    public IEnumerable<WebApiEndpoint> GetList()
    {
        return this.GetApiEndpoints();
    }

    private IEnumerable<WebApiEndpoint> GetApiEndpoints()
    {
        try
        {
            // var apiEndpointsSettingOption = this._optionsMonitor.Get(nameof(WebApiEndpoints));
            var apiEndpointsSettingOption = this._optionsMonitor.CurrentValue;

            return apiEndpointsSettingOption.Endpoints;
        }
        catch (Exception e)
        {
            this._logger.Log(LogLevel.Warning, $"無法取得 OpenApi 設定資料，無法使用\n例外訊息: {e}");

            return new[] { new WebApiEndpoint(AppDomain.CurrentDomain.FriendlyName, new Uri("/swagger/v1/swagger.json", UriKind.Relative)) };
        }
    }
}