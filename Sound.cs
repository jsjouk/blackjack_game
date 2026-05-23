using System.Diagnostics;
internal static class Sound
{
    static void Play(string path)
    {
        Process.Start("afplay", path);
    }
    static string cwd = Directory.GetCurrentDirectory();
    static internal string firstPath = "cwd//sounds";
}