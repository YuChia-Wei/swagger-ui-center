using System.Text.Json.Serialization;

namespace SwaggerUI.Center.Authentication.Options;

public class SwaggerAuthSettingOptions
{
    public DefaultAuthEnum Default { get; set; } = DefaultAuthEnum.Opid;

    [JsonPropertyName("Jwt")]
    public JwtAuthOptions? Jwt { get; set; }

    [JsonPropertyName("Opid")]
    public OpidAuthOptions? Opid { get; set; }
}