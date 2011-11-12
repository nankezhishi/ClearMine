namespace ClearMine.Common.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 
    /// </summary>
    public static class NativeMethods
    {
        public static int DoubleClickInterval
        {
            get
            {
                return Convert.ToInt32(RetrieveDoubleClickTime());
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public static bool MoveMouseTo(int x, int y)
        {
            return SetCursorPosition(x, y);
        }

        [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime")]
        private static extern uint RetrieveDoubleClickTime();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPosition(int x, int y);
    }
}
