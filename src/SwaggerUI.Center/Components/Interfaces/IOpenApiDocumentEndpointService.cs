using SwaggerUI.Center.Components.OptionModels;

namespace SwaggerUI.Center.Components.Interfaces;

/// <summary>
/// </summary>
public interface IOpenApiDocumentEndpointService
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OpenApiDocEndpointOption>> GetListAsync();
}