using System.Runtime.InteropServices;
using System.Text.Json;
using ForeverOnline;

InitializeWindow();

var config = JsonDocument.Parse(File.ReadAllText("appsettings.json")).RootElement;
var randomKeySelector = new VirtualKeyCodeSelector(new Random());
var delay = config.GetProperty("Delay").GetInt32();
var moveMouse = config.GetProperty("MoveMouse").GetBoolean();

PrintSettings();

Console.WriteLine();
Console.WriteLine("Opening Notepad...");
KeyboardSimulator.FocusOrLaunchNotepad();
Console.WriteLine("Pressing keys...");
Console.WriteLine();

if (moveMouse) new Thread(RandomMouseMover.MoveMouseRandomly).Start();

while (true)
{
    AttemptToFocusOrLaunchNotepad();
    PerformKeyPressInNotepad();
    Thread.Sleep(delay);
}

void AttemptToFocusOrLaunchNotepad()
{
    if (KeyboardSimulator.IsNotepadFocused()) return;

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Notepad is not focused or not open. Attempting to focus or launch...");
    KeyboardSimulator.FocusOrLaunchNotepad();
    Console.ResetColor();
    Thread.Sleep(3000); // Wait a bit for Notepad to open or gain focus
}

void PerformKeyPressInNotepad()
{
    if (!KeyboardSimulator.IsNotepadFocused())
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Notepad is still not focused.");
        Console.ResetColor();
        return;
    }

    KeyboardSimulator.PressKey((byte)randomKeySelector.SelectRandomLetterKey());
}

[DllImport("kernel32.dll", SetLastError = true)]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll", SetLastError = true)]
static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

void InitializeWindow()
{
    var handle = GetConsoleWindow();
    SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 0x0004 | 0x0001);

    var brandLines = AsciiArtCollection.Brand.Split('\n').Length;
    var logoLines = AsciiArtCollection.Logo.Split('\n').Length;
    const int settingsLines = 5;

    var totalLines = brandLines + logoLines + settingsLines;

    // add a few extra lines for buffer
    totalLines += 10;

    Console.WindowHeight = totalLines < Console.BufferHeight ? totalLines : Console.BufferHeight;
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(AsciiArtCollection.Brand);
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(AsciiArtCollection.Logo);
    Console.ResetColor();
}

void PrintSettings()
{
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine("Settings:");
    Console.WriteLine($" - Delay: {delay} ms");
    Console.WriteLine($" - Random mouse movement enabled: {moveMouse}");
    Console.WriteLine("-------------------------------------------------");
    Console.ResetColor();
}