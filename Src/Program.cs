using System.Globalization;
using DSharpPlus;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using static Pylijik.MessageListeners;

namespace Pylijik;

public class Program
{
    public static string token = File.ReadAllText(projDir + "/botToken");
    public static CancellationTokenSource cancelTokenSrc = new CancellationTokenSource();
    CancellationToken cancelToken;

    public static async Task Main()
    {
        try
        {
            await new Program().InitBot();
        }
        finally
        {
            Console.WriteLine("Bot stopped");
        }
    }

    public Program()
    {
        cancelToken = cancelTokenSrc.Token;
    }
    
    async Task InitBot()
    {
        if (!Directory.Exists(projDir + "/Logs"))
            Directory.CreateDirectory(projDir + "/Logs");
        
        string logFile = projDir + $"/Logs/log-{DateTime.Now:MMM_dd_yyyy__hh-mm-ss_tt}.txt" ;
        Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole(formatProvider: CultureInfo.CurrentCulture)
            .WriteTo.File(logFile, LogEventLevel.Information, formatProvider: CultureInfo.CurrentCulture)
            .CreateLogger();
        
        var logFactory = new LoggerFactory().AddSerilog();
        
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.MessageContents | DiscordIntents.AllUnprivileged,
            LogTimestampFormat = "MMM dd yyyy - hh:mm:ss tt",
            LoggerFactory = logFactory
        });
        client.MessageCreated += StopBot; // should be on top since its synchronous
        client.MessageCreated += Echo;

        await client.ConnectAsync();
        await Task.Delay(-1, cancelToken);
    }
}