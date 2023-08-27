namespace Pylijik.Utils;

public static class DirHelper
{
    public static readonly string projDir = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent + ""!;
    public static readonly string binDir = Directory.GetCurrentDirectory();
}