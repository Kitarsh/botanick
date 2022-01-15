namespace BotANick.Discord.Services;

public class LoggingService
{
    // DiscordSocketClient are injected automatically from the IServiceProvider
    public LoggingService(DiscordSocketClient discord)
    {
        _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

        discord.Log += OnLogAsync;
    }

    private string _logDirectory { get; }

    private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

    private Task OnLogAsync(LogMessage msg)
    {
        if (!Directory.Exists(_logDirectory))     // Create the log directory if it doesn't exist
            Directory.CreateDirectory(_logDirectory);
        if (!File.Exists(_logFile))               // Create today's log file if it doesn't exist
            File.Create(_logFile).Dispose();

        string logText = $"{DateTime.Now:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
        File.AppendAllText(_logFile, logText + "\n");     // Write the log text to a file

        return Console.Out.WriteLineAsync(logText);       // Write the log text to the console
    }
}
