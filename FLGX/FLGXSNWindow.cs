using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D.Compilers;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.Windowing;

namespace flgx
{
    public class FLGXSNWindow : IFLGXWindow
    {
        public IWindow windowHandle;

        public int WindowId { get; set; }
        public Vector2 WindowSize { get { return windowHandle.Size.ToSilk2D(); } set { windowHandle.Size = value.ToSilkInt(); } }
        public string Title { get; set; }

        public Action OnLoad { get; set; }

        public void DoEvents()
        {
            windowHandle.ContinueEvents();
        }

        public void Initialize()
        {

        }

        public void _OnLoad()
        {
            if (FLGX.InternalState.RenderingAPI == RenderingAPI.Direct3D11)
            {
                FLGX.InternalState.d3d11 = D3D11.GetApi(windowHandle);
                FLGX.InternalState.dxgi = DXGI.GetApi(windowHandle);
                FLGX.InternalState.compiler = D3DCompiler.GetApi();

                // TO-DO: Implement input event handling.

                var state = FLGX.InternalState;

                unsafe
                {
                    var d3d11 = state.d3d11;
                    var dxgi = state.dxgi;
                    var compiler = state.compiler;

                    SilkMarshal.ThrowHResult
                        (
                            d3d11.CreateDevice
                            (
                                default(ComPtr<IDXGIAdapter>),
                                D3DDriverType.Hardware,
                                Software: default,
                                (uint)CreateDeviceFlag.Debug,
                                null,
                                0,
                                D3D11.SdkVersion,
                                ref state.device,
                                null,
                                ref state.deviceContext
                            )
                        );

                    var swapChainDsc = new SwapChainDesc1
                    {
                        BufferCount = 2,
                        Format = Format.FormatB8G8R8A8Unorm,
                        BufferUsage = DXGI.UsageRenderTargetOutput,
                        SwapEffect = SwapEffect.FlipDiscard,
                        SampleDesc = new SampleDesc(1, 0)
                    };

                    state.factory = dxgi.CreateDXGIFactory<IDXGIFactory2>();

                    SilkMarshal.ThrowHResult
                    (
                        state.factory.CreateSwapChainForHwnd
                        (
                            state.device,
                            windowHandle.Native!.DXHandle!.Value,
                            in swapChainDsc,
                            null,
                            ref Unsafe.NullRef<IDXGIOutput>(),
                            ref state.swapchain
                        )
                    );
                }
            }

            if (OnLoad != null)
                OnLoad();
        }

        public void Run(Action<float> renderLoop)
        {
            windowHandle.Load += OnLoad;
            windowHandle.Render += (double dt) => { renderLoop((float)dt); };
            windowHandle.Run();
        }

        public Action OnClosing
        {
            get { return OnClosing; }
            set
            {
                windowHandle.Closing += value;
            }
        }

        public void MakeWindowCurrent()
        {
            windowHandle.MakeCurrent();
        }

        public void SwapBuffers()
        {
            FLGX.InternalState.swapchain.Present(1, 0);
        }

        public void Close()
        {
            windowHandle.Close();
        }

        public FLGXSNWindow(Vector2 size, string title, bool vSync = true, bool isVulkan = false)
        {
            WindowOptions opts;

            Title = title;

            if (isVulkan)
                opts = WindowOptions.DefaultVulkan;
            else
                opts = WindowOptions.Default;

            opts.Size = size.ToSilkInt();
            opts.Title = Title;
            opts.VSync = vSync;
            opts.API = GraphicsAPI.None;
            if (isVulkan)
                opts.API = GraphicsAPI.DefaultVulkan;

            windowHandle = Window.Create(opts);

            windowHandle.Load += _OnLoad;
        }
    }
}
