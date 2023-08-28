using System.Globalization;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Microsoft.Extensions.Logging;
using Pylijik.Modules;
using Serilog;
using Serilog.Events;

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
            if (!Directory.Exists(projDir + "/Logs"))
                Directory.CreateDirectory(projDir + "/Logs");
            
            if (clearLogsOnStart)
            {
                var dirs = Directory.GetFiles(projDir + "/Logs/");
                foreach (var dir in dirs)
                    File.Delete(dir);
            }
            
            // Init Lavalink
            System.Diagnostics.Process.Start("CMD.exe",
                "java -jar ../../../Src/Lavalink/Lavalink.jar");
            Thread.Sleep(15000); // wait until it launches
            
            await new Program().InitBot();
        }
        finally
        {
            Log.Logger.Information("Bot stopped");
        }
    }

    public Program()
    {
        cancelToken = cancelTokenSrc.Token;
    }
    
    // fanati id  = 1144730372165337209
    // me id = 843378250033528852
    async Task InitBot()
    {
        #region Configs setup

        string logFile = projDir + $"/Logs/log-{DateTime.Now:MMM_dd_yyyy__hh-mm-ss_tt}.txt" ;
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.ColoredConsole(formatProvider: CultureInfo.CurrentCulture)
                    .WriteTo.File(logFile, LogEventLevel.Information, formatProvider: CultureInfo.CurrentCulture)
                    .CreateLogger();
                
                var logFactory = new LoggerFactory().AddSerilog();
                
                var endpoint = new ConnectionEndpoint
                {
                    Hostname = "127.0.0.1", // From your server configuration.
                    Port = 2333 // From your server configuration
                };
        
                var lavalinkConfig = new LavalinkConfiguration
                {
                    Password = "youshallnotpass", // From your server configuration.
                    RestEndpoint = endpoint,
                    SocketEndpoint = endpoint
                };

        #endregion
        
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.MessageContents | DiscordIntents.AllUnprivileged,
            LogTimestampFormat = "MMM dd yyyy - hh:mm:ss tt",
            LoggerFactory = logFactory
        });

        var commands = client.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] {"!"}
        });
        commands.RegisterCommands<CommandModule>();
        var lavalink = client.UseLavalink();
        
        
        await client.ConnectAsync();
        await lavalink.ConnectAsync(lavalinkConfig);
        await Task.Delay(-1, cancelToken);
    }
    
}