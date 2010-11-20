﻿namespace ClearMine.Common.Utilities
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// 
    /// </summary>
    public static class Win32
    {
        [DllImport("user32.dll")]
        public static extern uint GetDoubleClickTime();

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
    }
}
