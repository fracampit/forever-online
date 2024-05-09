using System.Text.Json;
using ForeverOnline;

try
{
    AppUtilities.InitializeWindow();

    var appPath = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();

    var configPath = Path.Combine(appPath, "appsettings.json");
    var config = JsonDocument.Parse(File.ReadAllText(configPath)).RootElement;
    var delay = config.GetProperty("Delay").GetInt32();
    var moveMouse = config.GetProperty("MoveMouse").GetBoolean();

    AppUtilities.PrintSettings(delay, moveMouse);

    Console.WriteLine();
    Console.WriteLine("Opening Notepad...");
    KeyboardSimulator.FocusOrLaunchNotepad();
    Console.WriteLine("Pressing keys...");
    Console.WriteLine();

    if (moveMouse) new Thread(RandomMouseMover.MoveMouseRandomly).Start();

    while (true)
    {
        AppUtilities.AttemptToFocusOrLaunchNotepad();
        AppUtilities.PerformKeyPressInNotepad();
        Thread.Sleep(delay);
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}