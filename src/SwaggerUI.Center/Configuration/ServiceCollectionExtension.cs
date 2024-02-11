namespace SwaggerUI.Center.Configuration;

/// <summary>
/// Auth 設定
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// 加入身分認證設定檔
    /// </summary>
    /// <param name="configurationManager"></param>
    public static ConfigurationManager AddAuthenticationConfigurationJsons(this ConfigurationManager configurationManager)
    {
        configurationManager.AddAuthSetting(GetRealJsonPath(Path.Combine("Configuration", "Authentication", "AuthSetting.json")));

        return configurationManager;
    }

    private static void AddAuthSetting(this ConfigurationManager configurationManager, string filePath)
    {
        configurationManager.AddJsonFile(filePath, true, false);
    }

    private static string GetRealJsonPath(string jsonPath)
    {
        var resolveLinkTarget = File.ResolveLinkTarget(jsonPath, true);
        return resolveLinkTarget?.FullName ?? jsonPath;
    }
}