namespace Chat.Config;

public class SettingsManager
{
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
