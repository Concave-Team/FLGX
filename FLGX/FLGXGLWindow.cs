using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace flgx
{
    public interface IFLGXWindow
    {
        public int WindowId { get; set; } // Assigned window id in the manager.
        public Vector2 WindowSize { get; }
        public void Run(Action<float> renderAction);
        public void MakeWindowCurrent();
        public Action OnClosing { get; set; }
        public Action OnLoad { get; set; }
        public void Close();
        public void SwapBuffers();
        public void Initialize();
        public void DoEvents();
    }

    public class FLGXGLWindow : GameWindow, IFLGXWindow
    {

        public int WindowId { get; set; }
        public Vector2 WindowSize { get { return this.Size.ToSNV2(); }  }
        public static GameWindowSettings gwSettings { get { return GameWindowSettings.Default; } }
        public static NativeWindowSettings nwSettings { get { return NativeWindowSettings.Default; } }
        public Action OnClosing {
            get {  return OnClosing; }
            set {
                this.Closing += (CancelEventArgs e) => { value(); };
            } 
        }

        public Action OnLoad { get; set; }

        public void Initialize()
        {
            // do nothing because this already happens.
        }


        public void DoEvents()
        {
            this.ProcessEvents(0);
        }

        public void MakeWindowCurrent()
        {
            this.MakeCurrent();
        }

        public void Run(Action<float> renderAction)
        {
            this.RenderFrame += (FrameEventArgs e) => { renderAction((float)e.Time); };
            this.Resize += FLGXWindow_Resize;
            this.Load += OnLoad;
            this.Run();
        }

        public void Close()
        {
            this.Close();
        }

        private void FLGXWindow_Resize(ResizeEventArgs obj)
        {
            switch (FLGX.InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    GL.Viewport(0, 0, obj.Width, obj.Height);
                    break;
            }
        }

        public FLGXGLWindow(int width, int height, string title, bool vSync) : base(gwSettings, nwSettings)
        {
            Size = new OpenTK.Mathematics.Vector2i(width, height);
            Title = title;
            if (vSync)
                VSync = VSyncMode.Off;
            else
                VSync = VSyncMode.Adaptive;
        }
    }
}
