﻿namespace ForeverOnline;

public enum VirtualKeyCode
{
    Backspace = 0x08,
    Tab = 0x09,
    Enter = 0x0D,
    Shift = 0x10,
    Ctrl = 0x11,
    Alt = 0x12,
    PauseBreak = 0x13,
    CapsLock = 0x14,
    Escape = 0x1B,
    Space = 0x20,
    PageUp = 0x21,
    PageDown = 0x22,
    End = 0x23,
    Home = 0x24,
    LeftArrow = 0x25,
    UpArrow = 0x26,
    RightArrow = 0x27,
    DownArrow = 0x28,
    PrintScreen = 0x2C,
    Insert = 0x2D,
    Delete = 0x2E,
    Key0 = 0x30,
    Key1 = 0x31,
    Key2 = 0x32,
    Key3 = 0x33,
    Key4 = 0x34,
    Key5 = 0x35,
    Key6 = 0x36,
    Key7 = 0x37,
    Key8 = 0x38,
    Key9 = 0x39,
    A = 0x41,
    B = 0x42,
    C = 0x43,
    D = 0x44,
    E = 0x45,
    F = 0x46,
    G = 0x47,
    H = 0x48,
    I = 0x49,
    J = 0x4A,
    K = 0x4B,
    L = 0x4C,
    M = 0x4D,
    N = 0x4E,
    O = 0x4F,
    P = 0x50,
    Q = 0x51,
    R = 0x52,
    S = 0x53,
    T = 0x54,
    U = 0x55,
    V = 0x56,
    W = 0x57,
    X = 0x58,
    Y = 0x59,
    Z = 0x5A,
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    F6 = 0x75,
    F7 = 0x76,
    F8 = 0x77,
    F9 = 0x78,
    F10 = 0x79,
    F11 = 0x7A,
    F12 = 0x7B,
    NumLock = 0x90,
    ScrollLock = 0x91,
    LeftShift = 0xA0,
    RightShift = 0xA1,
    LeftControl = 0xA2,
    RightControl = 0xA3,
    LeftMenu = 0xA4,
    RightMenu = 0xA5
}

public class VirtualKeyCodeSelector(Random random)
{
    public static readonly List<VirtualKeyCode> LetterKeys = [
        VirtualKeyCode.A,
        VirtualKeyCode.B,
        VirtualKeyCode.C,
        VirtualKeyCode.D,
        VirtualKeyCode.E,
        VirtualKeyCode.F,
        VirtualKeyCode.G,
        VirtualKeyCode.H,
        VirtualKeyCode.I,
        VirtualKeyCode.J,
        VirtualKeyCode.K,
        VirtualKeyCode.L,
        VirtualKeyCode.M,
        VirtualKeyCode.N,
        VirtualKeyCode.O,
        VirtualKeyCode.P,
        VirtualKeyCode.Q,
        VirtualKeyCode.R,
        VirtualKeyCode.S,
        VirtualKeyCode.T,
        VirtualKeyCode.U,
        VirtualKeyCode.V,
        VirtualKeyCode.W,
        VirtualKeyCode.X,
        VirtualKeyCode.Y,
        VirtualKeyCode.Z,
        VirtualKeyCode.Space
    ];
    
    public VirtualKeyCode SelectRandomLetterKey()
    {
        return LetterKeys[random.Next(LetterKeys.Count)];
    }
}