using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Internal
{
    public static class FLGXWindowManager
    {
        internal static Dictionary<int, FLGXWindow> Windows = new Dictionary<int, FLGXWindow>();
        public static FLGXWindow ActiveWindow { get; private set; }

        private static FLGXWindow ClosingWindow;

        public static FLGXWindow RegisterNewWindow(FLGXWindow window)
        {
            window.WindowId = Windows.Count + 1;
            window.Closing += (CancelEventArgs e) => { ClosingWindow = window; HandleWindowClosing(); };
            Windows.Add(Windows.Count + 1, window);
            return GetWindow(window.WindowId); // Return the one from the list.
        }

        public static void SetAsCurrent(FLGXWindow window) 
        {
            ActiveWindow = window;
            ActiveWindow.MakeCurrent();
        }

        /// <summary>
        /// Returns a FLGXWindow from the window manager based on the id.
        /// </summary>
        /// <param name="windowId">Id of the window</param>
        /// <returns>The window</returns>
        public static FLGXWindow GetWindow(int windowId)
        {
            if(Windows.ContainsKey(windowId))
                return Windows[windowId];
            throw new FLGXInternalStateException(FLGX.InternalState, "Attempted to get invalid/non-existing window.");
        }

        private static void HandleWindowClosing()
        {
            if(ClosingWindow != null)
            {
                Windows.Remove(ClosingWindow.WindowId);
            }
        }

        public static void KillWindow(int windowId)
        {
            var hwnd = GetWindow(windowId);

            if (hwnd != null)
            {
                hwnd.Close();
                Windows.Remove(windowId);
            }
            else
                throw new FLGXInternalStateException(FLGX.InternalState, "Attempted to kill invalid/non-existing window.");
        }

        public static void KillAllWindows()
        {
            foreach(var window in Windows)
            {
                window.Value.Close();
            }

            Windows.Clear();
        }

        public static bool WindowExists(int windowId)
            => Windows.ContainsKey(windowId);
    }
}
