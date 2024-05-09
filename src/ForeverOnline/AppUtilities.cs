using System.Runtime.InteropServices;

namespace ForeverOnline;

public static class AppUtilities
{
    public static void PrintSettings(int delay, bool moveMouse)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Settings:");
        Console.WriteLine($" - Delay: {delay} ms");
        Console.WriteLine($" - Random mouse movement enabled: {moveMouse}");
        Console.WriteLine("-------------------------------------------------");
        Console.ResetColor();
    }
    
    public static void AttemptToFocusOrLaunchNotepad()
    {
        if (KeyboardSimulator.IsNotepadFocused()) return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Notepad is not focused or not open. Attempting to focus or launch...");
        KeyboardSimulator.FocusOrLaunchNotepad();
        Console.ResetColor();
        Thread.Sleep(3000); // Wait a bit for Notepad to open or gain focus
    }

    public static void PerformKeyPressInNotepad()
    {
        if (!KeyboardSimulator.IsNotepadFocused())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Notepad is still not focused.");
            Console.ResetColor();
            return;
        }

        var randomKeySelector = new VirtualKeyCodeSelector(new Random());
        KeyboardSimulator.PressKey((byte)randomKeySelector.SelectRandomLetterKey());
    }
    
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
    public static void InitializeWindow()
    {
        var handle = GetConsoleWindow();
        if (handle == IntPtr.Zero)
        {
            Console.WriteLine("Error: Unable to get console window handle.");
            return;
        }

        var success = SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 0x0004 | 0x0001);
        if (!success)
        {
            Console.WriteLine("Error: Unable to set window position and size.");
            return;
        }

        var brandLines = AsciiArtCollection.Brand.Split('\n').Length;
        var logoLines = AsciiArtCollection.Logo.Split('\n').Length;
        const int settingsLines = 5;

        var totalLines = brandLines + logoLines + settingsLines;

        // add a few extra lines for buffer
        totalLines += 10;

        try
        {
            if (totalLines > Console.BufferHeight)
            {
                Console.BufferHeight = totalLines; // Try to increase the buffer height
                Console.WriteLine($"Warning: Desired window height ({totalLines}) exceeds buffer height. Buffer height has been increased to {totalLines}.");
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            try
            {
                // If the desired buffer height is too large, try to set it to the maximum allowed value
                Console.BufferHeight = short.MaxValue - 1;
                Console.WriteLine($"Warning: Desired window height ({totalLines}) exceeds maximum buffer height. Buffer height has been set to maximum allowed value.");
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the maximum allowed value is also too large, set the buffer height to a default value
                Console.BufferHeight = 30; // Or any other value less than the maximum allowed value on your system
                Console.WriteLine($"Warning: Desired window height ({totalLines}) exceeds maximum buffer height. Buffer height has been set to default value.");
            }
        }

        Console.WindowHeight = Math.Min(totalLines, Console.BufferHeight);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(AsciiArtCollection.Brand);
        Console.WriteLine($"Version: {typeof(AppUtilities).Assembly.GetName().Version}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(AsciiArtCollection.Logo);
        Console.ResetColor();
    }
}