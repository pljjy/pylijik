namespace Pylijik.Utils;


public static class Constants
{
    public const bool clearLogsOnStart = true;
    public static readonly ulong[] admins = File.ReadAllText(projDir + "/Utils/admins_list")
        .Split("\n")
        .Select(ulong.Parse).ToArray();

    public static bool IsAdmin(ulong id) => admins.Contains(id);
}