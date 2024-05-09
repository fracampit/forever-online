using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ForeverOnline;

public static class KeyboardSimulator
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
    
    [DllImport("user32.dll")]
    static extern uint MapVirtualKey(uint uCode, uint uMapType);
    
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetForegroundWindow(IntPtr hWnd);
    
    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint type;
        public INPUTUNION u;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct INPUTUNION
    {
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    const uint KEYEVENTF_KEYUP = 0x0002;
    const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    const uint INPUT_KEYBOARD = 1;

    public static void PressKey(byte keyCode)
    {
        var scanCode = (ushort)MapVirtualKey(keyCode, 0);
        var inputDown = new INPUT
        {
            type = INPUT_KEYBOARD,
            u = new INPUTUNION
            {
                ki = new KEYBDINPUT
                {
                    wVk = keyCode,
                    wScan = scanCode,
                    dwFlags = KEYEVENTF_EXTENDEDKEY,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };

        var inputUp = inputDown;
        inputUp.u.ki.dwFlags = KEYEVENTF_KEYUP;

        var inputsSent = SendInput(1, ref inputDown, Marshal.SizeOf(typeof(INPUT)));
        if (inputsSent != 1)
        {
            Console.WriteLine($"Failed to send key down. Last Error: {Marshal.GetLastWin32Error()}");
        }

        Thread.Sleep(100);

        inputsSent = SendInput(1, ref inputUp, Marshal.SizeOf(typeof(INPUT)));
        if (inputsSent != 1)
        {
            Console.WriteLine($"Failed to send key up. Last Error: {Marshal.GetLastWin32Error()}");
        }
        
        Console.Write((char)keyCode);
    }
}
