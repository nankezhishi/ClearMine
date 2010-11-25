namespace ClearMine.Common.Utilities
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// 
    /// </summary>
    public static class WindowsApi
    {
        public static uint GetDoubleClickInterval()
        {
            return RetrieveDoubleClickTime();
        }

        public static bool MoveMouseTo(int x, int y)
        {
            return SetCursorPosition(x, y);
        }

        [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime")]
        private static extern uint RetrieveDoubleClickTime();

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPosition(int x, int y);
    }
}
