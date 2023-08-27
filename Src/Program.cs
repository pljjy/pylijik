using DSharpPlus;
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
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.MessageContents | DiscordIntents.AllUnprivileged
        });
        client.MessageCreated += StopBot; // should be on top since its synchronous
        client.MessageCreated += Echo;

        await client.ConnectAsync();
        await Task.Delay(-1, cancelToken);
    }
}