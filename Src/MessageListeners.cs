using DSharpPlus;
using DSharpPlus.EventArgs;
using static Pylijik.Program;

namespace Pylijik;

public class MessageListeners
{
    public static async Task Echo(DiscordClient client, MessageCreateEventArgs msg)
    {
        if (msg.Author.IsBot) return;

        await msg.Message.RespondAsync(msg.Message.Content);
        Console.WriteLine($"responded to {msg.Author}\ntext - \"{msg.Message.Content}\"");
    }

    // TODO: find a better way to stop bot, this one is cringe asf AND not asynchronous
    public static Task StopBot(DiscordClient client, MessageCreateEventArgs msg)
    {
        if (msg.Author.Id == 843378250033528852 && msg.Message.Content.ToLower() == "exit")
        {
            msg.Channel.SendMessageAsync("Bot stopped");
            cancelTokenSrc.Cancel();
        }
        return Task.CompletedTask;
    }
}