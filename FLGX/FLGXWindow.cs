using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx
{
    public class FLGXWindow : GameWindow
    {
        public int WindowId = 0; // Assigned window id in the manager.

        public static GameWindowSettings gwSettings { get { return GameWindowSettings.Default; } }
        public static NativeWindowSettings nwSettings { get { return NativeWindowSettings.Default; } }

        public void Run(Action<float> renderAction)
        {
            this.RenderFrame += (FrameEventArgs e) => { renderAction((float)e.Time); };
            this.Resize += FLGXWindow_Resize;
            this.Run();
        }

        private void FLGXWindow_Resize(ResizeEventArgs obj)
        {
            switch(FLGX.InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    GL.Viewport(0,0,obj.Width,obj.Height);
                    break;
            }
        }

        public FLGXWindow(int width, int height, string title, bool vSync) : base(gwSettings, nwSettings)
        {
            Size = new OpenTK.Mathematics.Vector2i(width, height);
            Title = title;
            VSync = VSyncMode.Off;
        }
    }
}
