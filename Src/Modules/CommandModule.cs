using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Serilog;

namespace Pylijik.Modules;

public class CommandModule : BaseCommandModule
{
    [Command("str")]
    public async Task WriteString(CommandContext cmd, [RemainingText] string text)
    {
        if (cmd.User.IsBot) return;
        await cmd.RespondAsync("Text copied: " + text);
        Log.Logger.Information("!str by " + cmd.User);
    }
    
    [Command("id")]
    public async Task GetIds(CommandContext cmd)
    {
        if (cmd.User.IsBot) return;
        await cmd.RespondAsync($"chat_id = {cmd.Channel.Id}\nuser_id = {cmd.User.Id}");
        Log.Logger.Information("!id by " + cmd.User);
    }

    [Command("stop")]
    public async Task StopBot(CommandContext cmd)
    {
        if (cmd.User.IsBot || !IsAdmin(cmd.User.Id)) return;
        var task = cmd.RespondAsync("Bot stopped by " + cmd.User);
        await Task.WhenAny(task);
        Program.cancelTokenSrc.Cancel();
    }

    [Command("file")]
    public async Task SendFile(CommandContext cmd)
    {
        if(cmd.User.IsBot) return;
        await using var fs = new FileStream(@"C:\Users\pljjy22\Downloads\Telegram Desktop\IMG_9761.MP4", 
            FileMode.Open,
            FileAccess.Read);
        
        await new DiscordMessageBuilder()
            .WithContent("кошатина забавный ну который первый")
            .AddFile("sillious_kitty.mp4", fs)
            .SendAsync(cmd.Channel);
        
        Log.Logger.Information("!file by " + cmd.User);
    }
}