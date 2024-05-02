using flgx.Graphics;
using flgx.Graphics.Common;
using flgx.Graphics.OpenGL;
using flgx.Internal;
using OpenTK.Graphics.OpenGL;
using Serilog;
using Serilog.Core;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace flgx
{
    public static class FLGX
    {
        /// <summary>
        /// The internal state of the FLGX library. This stores data needed to be kept during runtime.
        /// </summary>
        public static FLGXInternalState InternalState;
        /// <summary>
        /// A variable that tells whether or not the library has been initialized.
        /// </summary>
        public static bool IsInitialized = false;
        internal static Logger log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        /// <summary>
        /// Initialized the FLGX library. You must call this to be able to use the library, otherwise it will not work.
        /// </summary>
        /// <param name="settings">The initialization settings for FLGX</param>
        /// <returns>A bool indicating initialization success or failure.</returns>
        public static bool Init(FLGXInitSettings settings)
        {
            InternalState = new FLGXInternalState();

            InternalState.RenderingAPI = settings.renderingAPI;

            IsInitialized = true;

            InternalState.CreateStateVariable("ActiveShader", null);

            log.Information("[FLGX]: Successfully initialized FLGX.");

            return true;
        }

        /// <summary>
        /// Creates a new window for rendering. To run a window, use the FLGXWindow.Run(Action[float]) function, specifying your render/draw loop.
        /// </summary>
        /// <param name="title">The title of the window</param>
        /// <param name="w">The width of the window (in pixels)</param>
        /// <param name="h">The height of the window (in pixels)</param>
        /// <returns>The constructed window.</returns>
        public static FLGXWindow CreateWindow(string title, int w, int h)
        {
            return FLGXWindowManager.RegisterNewWindow(new FLGXWindow(w, h, title, true));
        }

        /// <summary>
        /// Makes the window be the current active window for rendering. You must call this before rendering to a window.
        /// </summary>
        /// <param name="window">The window to make active.</param>
        public static void MakeWindowCurrent(FLGXWindow window)
        {
            FLGXWindowManager.SetAsCurrent(window);
        }

        /// <summary>
        /// Creates a populated index buffer.
        /// </summary>
        /// <typeparam name="T">The datatype of the buffer data contents.</typeparam>
        /// <param name="data">The data you want to populate the buffer with</param>
        /// <param name="size">The size(in bytes) of the buffer data.</param>
        /// <returns>A populated index buffer</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLBuffer? CreateIndexBuffer<T>(T[] data, int size) where T : struct
        {
            FLBuffer? idxBuf = null;

            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    idxBuf = new OpenGLBuffer(BufferType.Index);
                    idxBuf.SetBufferData(data, size);
                    break;
            }

            if (idxBuf == null)
            {
                throw new FLGXInternalStateException(InternalState, "Could not successfully create an index buffer.");
            }

            return idxBuf;
        }

        /// <summary>
        /// Creates a populated vertex buffer.
        /// </summary>
        /// <typeparam name="T">The datatype of the buffer data contents.</typeparam>
        /// <param name="data">The data you want to populate the buffer with</param>
        /// <param name="size">The size(in bytes) of the buffer data.</param>
        /// <returns>A populated vertex buffer</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLBuffer? CreateVertexBuffer<T>(T[] data, int size) where T : struct
        {
            FLBuffer? vtxBuf = null;

            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    vtxBuf = new OpenGLBuffer(BufferType.Vertex);
                    vtxBuf.SetBufferData(data, size);
                    break;
            }

            if (vtxBuf == null)
            {
                throw new FLGXInternalStateException(InternalState, "Could not successfully create a vertex buffer.");
            }

            return vtxBuf;
        }

        /// <summary>
        /// Creates a dataless vertex buffer for later population [overload]
        /// </summary>
        /// <returns>A dataless vertex buffer</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLBuffer? CreateVertexBuffer()
        {
            FLBuffer? vtxBuf = null;

            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    vtxBuf = new OpenGLBuffer(BufferType.Vertex);
                    break;
            }

            if (vtxBuf == null)
            {
                throw new FLGXInternalStateException(InternalState, "Could not successfully create a vertex buffer.");
            }

            return vtxBuf;
        }

        /// <summary>
        /// Creates a dataless index buffer for later population [overload]
        /// </summary>
        /// <returns>A dataless index buffer</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLBuffer? CreateIndexBuffer()
        {
            FLBuffer? idxBuf = null;

            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    idxBuf = new OpenGLBuffer(BufferType.Index);
                    break;
            }

            if (idxBuf == null)
            {
                throw new FLGXInternalStateException(InternalState, "Could not successfully create an index buffer.");
            }

            return idxBuf;
        }

        /// <summary>
        /// Creates a vertex structure for use with buffers. Make sure to destroy it during shutdown, as FLGX will not do it automatically.
        /// </summary>
        /// <returns>The vertex structure.</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static VertexStructure CreateVertexStructure()
        {
            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    return new OpenGLVertexStructure();
            }

            throw new FLGXInternalStateException(InternalState, "Unsupported rendering API detected when trying to create vertex structure.");
        }

        public static void SetFLVertexAttributes(VertexStructure vertexStructure)
        {
            int sizeOfFloat = sizeof(float);
            int sizeOfVec3 = Marshal.SizeOf<System.Numerics.Vector3>();

            // Define the stride and offsets for each attribute in FLVertex
            int positionOffset = 0;
            int normalOffset = sizeOfFloat * 3;
            int texCoordOffset = sizeOfFloat * 6; // Assuming texCoord is Vec2

            // Add attribute pointers for each attribute in FLVertex
            vertexStructure.AddAttribPointer(0, 3, FLVertexAttribType.Float, false, sizeOfFloat * 8, positionOffset);
            vertexStructure.AddAttribPointer(1, 3, FLVertexAttribType.Float, false, sizeOfFloat * 8, normalOffset);
            vertexStructure.AddAttribPointer(2, 2, FLVertexAttribType.Float, false, sizeOfFloat * 8, texCoordOffset);
        }

        /// <summary>
        /// Creates an empty texture.
        /// </summary>
        /// <returns>An empty texture</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLTexture CreateTexture()
        {
            switch(InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    return new OpenGLTexture();
                    break;
                default:
                    throw new FLGXInternalStateException(InternalState, "This rendering API does not support textures yet.");
            }
        }

        /// <summary>
        /// Creates a texture based on an image at a certain path
        /// </summary>
        /// <param name="path">The path to the image</param>
        /// <param name="mipmaps">Whether or not to generate mipmaps (if the rendering API supports them)</param>
        /// <returns>A texture</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLTexture CreateTexture(string path, bool mipmaps = true)
        {
            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    return new OpenGLTexture(path, mipmaps);
                    break;
                default:
                    throw new FLGXInternalStateException(InternalState, "This rendering API does not support textures yet.");
            }
        }

        /// <summary>
        /// Creates a framebuffer based on your size inputs.
        /// </summary>
        /// <param name="width">The width of the framebuffer</param>
        /// <param name="height">The height of the framebuffer</param>
        /// <returns>A framebuffer</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static FLFrameBuffer CreateFramebuffer(int width, int height)
        {
            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    var oglFb = new OpenGLFrameBuffer();
                    oglFb.AttachAttachments(width, height);
                    return oglFb;
                default:
                    throw new FLGXInternalStateException(InternalState, "This rendering API does not support framebuffers yet.");
            }
        }

        /// <summary>
        /// Creates a shader based on GLSL vertex and fragment shader file paths. (Only on the OpenGL rendering API/backend)
        /// </summary>
        /// <param name="vsPath">The file path to the vertex shader</param>
        /// <param name="fsPath">The file path to the fragment shader</param>
        /// <returns>A GLSL shader</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static Shader CreateGLSLShader(string vsPath, string fsPath)
        {
            if (InternalState.RenderingAPI == RenderingAPI.OpenGL)
            {
                var vsCode = File.ReadAllText(vsPath);
                var fsCode = File.ReadAllText(fsPath);

                Shader shader = new OpenGLShader(vsCode, fsCode);

                return shader;
            }
            throw new FLGXInternalStateException(InternalState, "Cannot create GLSL shaders when not using the OpenGL rendering backend.");
        }

        /// <summary>
        /// Creates a shader based on GLSL vertex and fragment shader code data. (Only on the OpenGL rendering API/backend)
        /// </summary>
        /// <param name="vsPath">The code data of the vertex shader</param>
        /// <param name="fsPath">The code data of the fragment shader</param>
        /// <returns>A GLSL shader</returns>
        /// <exception cref="FLGXInternalStateException"></exception>
        public static Shader CreateGLSLShaderFromMemory(string vsCode, string fsCode)
        {
            if (InternalState.RenderingAPI == RenderingAPI.OpenGL)
            {
                Shader shader = new OpenGLShader(vsCode, fsCode);

                return shader;
            }
            throw new FLGXInternalStateException(InternalState, "Cannot create GLSL shaders when not using the OpenGL rendering backend.");
        }

        public static void UseShader(Shader shader)
        {
            InternalState.SetStateVariable("ActiveShader", shader);
            shader.Use();
        }

        public static Shader BuildDefaultShaders(bool _3D = false, bool OFS = false)
        {
            switch(InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    if(_3D == true)
                        return CreateGLSLShaderFromMemory(DefaultShaders.DefaultGLSLShaders3D_VS, DefaultShaders.DefaultGLSLShaders3D_FS);
                    else if(OFS == true)
                        return CreateGLSLShaderFromMemory(DefaultShaders.DefaultGLSLShadersOSR_VS, DefaultShaders.DefaultGLSLShadersOSR_FS);
                    else
                        return CreateGLSLShaderFromMemory(DefaultShaders.DefaultGLSLShaders_VS, DefaultShaders.DefaultGLSLShaders_FS);

            }

            throw new FLGXInternalStateException(InternalState, "Cannot create default shaders for this API, as it is not supported.");
        }

        public static void ClearColor(Vector4 color)
        {
            switch (InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    GL.ClearColor(color.X, color.Y, color.Z, color.W);
                    break;
            }
        }

        /// <summary>
        /// Denotes the beginning of a FLGX frame, call this to start a draw frame.
        /// </summary>
        public static void NewFrame()
        {
            var currentWindow = FLGXWindowManager.ActiveWindow;

            if (currentWindow != null)
            {
                currentWindow.ProcessEvents(0);

                switch (InternalState.RenderingAPI)
                {
                    case RenderingAPI.OpenGL:
                        GL.Enable(EnableCap.DepthTest);
                        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                        break;
                }
            }
        }

        public static void NewFrameWODB()
        {
            var currentWindow = FLGXWindowManager.ActiveWindow;

            if (currentWindow != null)
            {
                currentWindow.ProcessEvents(0);

                switch (InternalState.RenderingAPI)
                {
                    case RenderingAPI.OpenGL:
                        GL.Disable(EnableCap.DepthTest);
                        GL.Clear(ClearBufferMask.ColorBufferBit);
                        break;
                }
            }
        }

        public static void DrawIndexed(FLBuffer VertexBuffer, FLBuffer IndexBuffer, int indiceCount)
        {
            VertexBuffer.Bind();
            IndexBuffer.Bind();

            switch(InternalState.RenderingAPI)
            {
                case RenderingAPI.OpenGL:
                    GL.DrawElements(BeginMode.Triangles, indiceCount, DrawElementsType.UnsignedInt, 0);
                    GL.BindVertexArray(0);
                    return;
            }

            throw new FLGXInternalStateException(InternalState, "This rendering API does not support indexed drawing.");
        }

        /// <summary>
        /// Denotes the end of a FLGX frame, call this to end a draw frame.
        /// </summary>
        public static void EndFrame()
        {
            var currentWindow = FLGXWindowManager.ActiveWindow;

            if (currentWindow != null)
            {
                currentWindow.SwapBuffers();
            }
        }

        /// <summary>
        /// Shuts down FLGX and frees resources.
        /// </summary>
        public static async void Shutdown()
        {
            if (IsInitialized)
            {
                ShaderManager.DestroyAllShaders();
                FLBufferManager.ClearBuffers();
                FLGXWindowManager.KillAllWindows();
                await log.DisposeAsync();
            }
            else
            {
                log.Warning("[FLGX]: Attempted to shutdown FGLX, even though it was not initialized.");
            }
        }
    }
}
