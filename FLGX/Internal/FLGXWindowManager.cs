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
        internal static Dictionary<int, IFLGXWindow> Windows = new Dictionary<int, IFLGXWindow>();
        public static IFLGXWindow ActiveWindow { get; private set; }

        private static IFLGXWindow ClosingWindow;

        public static IFLGXWindow RegisterNewWindow(IFLGXWindow window)
        {
            window.WindowId = Windows.Count + 1;
            window.OnClosing = () => { ClosingWindow = window; HandleWindowClosing(); };
            Windows.Add(Windows.Count + 1, window);
            return GetWindow(window.WindowId); // Return the one from the list.
        }

        public static void SetAsCurrent(IFLGXWindow window) 
        {
            ActiveWindow = window;
            ActiveWindow.MakeWindowCurrent();
        }

        /// <summary>
        /// Returns a FLGXWindow from the window manager based on the id.
        /// </summary>
        /// <param name="windowId">Id of the window</param>
        /// <returns>The window</returns>
        public static IFLGXWindow GetWindow(int windowId)
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
                ClosingWindow = null;
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
