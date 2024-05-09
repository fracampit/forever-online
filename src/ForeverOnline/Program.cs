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
    
    while (true)
    {
        if (moveMouse) RandomMouseMover.MoveMouseRandomly();
        AppUtilities.PerformKeyPress();
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