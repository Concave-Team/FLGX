/*
    Updated for FLGX v2.1.0 #
 */

using flgx.Graphics.Common;
using flgx.Graphics.Common.Models;
using FLUX.Graphics;

namespace flgx.Examples.ModelLoading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FLGX Examples: 03 - Model Loading(with a texture)");

            FLGX.Init(new FLGXInitSettings(RenderingAPI.OpenGL));

            var window = FLGX.CreateWindow("Model Loading", 1600, 900);

            FLGX.MakeWindowCurrent(window);

            FLModel cubeModel = FLModel.FromFile("resources/cube.obj");
            FLTexture cubeTex = FLGX.CreateTexture("resources/container.png");

            var _3DShaders = FLGX.BuildDefaultShaders(true);

            Camera3D camera = new Camera3D(new System.Numerics.Vector3(0,0,15), new System.Numerics.Vector3(0,0,-1), System.Numerics.Quaternion.Zero, 0.5f, window.WindowSize); // Create a FLUX Camera3D object.

            FLGX.ClearColor(new System.Numerics.Vector4(0.2f, 0.1f, 0.3f, 1.0f));
            window.Run(
                (float dt) =>
                {
                    _3DShaders.SetUniform_Mat4("Projection", camera.ProjectionMatrix, false);
                    _3DShaders.SetUniform_Mat4("View", camera.ViewMatrix, false);
                    _3DShaders.SetUniform_Mat4("Model", System.Numerics.Matrix4x4.CreateTranslation(new System.Numerics.Vector3(0, 0, 0)) * System.Numerics.Matrix4x4.CreateRotationX(0.4f) * System.Numerics.Matrix4x4.CreateRotationY(0.4f), false);

                    FLGX.NewFrame();

                    _3DShaders.Use();
                    cubeTex.Use();
                    _3DShaders.SetUniform_Int("tex0", 1);
                    cubeModel.Draw();

                    FLGX.EndFrame();
                }
            );

            FLGX.Shutdown();
        }
    }
}
