﻿using System;
using System.Runtime.InteropServices;
using N8Engine.Inputs;

namespace N8Engine.External.User
{
    static class UserInput
    {
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeystate
        [DllImport("user32.dll")]
        static extern short GetKeyState(Key key);
        
        // https://stackoverflow.com/questions/6331868/using-getkeystate
        const int IS_KEY_PRESSED = 0x8000;

        public static bool IsKeyDown(Key key) => Convert.ToBoolean(GetKeyState(key) & IS_KEY_PRESSED);
    }
}