﻿using System.Runtime.InteropServices;

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
    static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;

    public static void InitializeWindow()
    {
        var handle = GetConsoleWindow();
        if (handle == IntPtr.Zero)
        {
            Console.WriteLine("Error: Unable to get console window handle.");
            return;
        }

        var screenWidth = GetSystemMetrics(SM_CXSCREEN);
        var screenHeight = GetSystemMetrics(SM_CYSCREEN);

        var success = MoveWindow(handle, 0, 0, screenWidth / 2, screenHeight, true);
        if (!success)
        {
            Console.WriteLine("Error: Unable to set window position and size.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(AsciiArtCollection.Brand);
        Console.WriteLine($"Version: {typeof(AppUtilities).Assembly.GetName().Version}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(AsciiArtCollection.Logo);
        Console.ResetColor();
    }
}