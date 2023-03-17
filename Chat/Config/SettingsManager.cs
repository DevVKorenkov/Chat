namespace Chat.Config;

public class SettingsManager
{
    /// <summary>
    /// Property to getting fields from appsettings.json
    /// </summary>
    public static IConfiguration AppSettings { get; private set; }

    static SettingsManager()
    {
        AppSettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();
    }
}
