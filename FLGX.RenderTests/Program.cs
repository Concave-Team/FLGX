using flgx;
using FLUX;
using flgx.Internal;
using flgx.Graphics.Common;
using FLUX.Graphics;
using Matrix4x4 = System.Numerics.Matrix4x4;
using flgx.Graphics.Common.Models;

namespace flgx.RenderTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[FLGX RenderTests]\nMeant to test features of FLGX as development goes.");
            FLGX.Init(new FLGXInitSettings(RenderingAPI.OpenGL));
            var window = FLGX.CreateWindow("test", 1300, 900);
            FLVertex[] vertices = new FLVertex[]
            {
                new FLVertex(new System.Numerics.Vector3(-1.0f, -1.0f, 0.0f)), // Bottom-left vertex
                new FLVertex(new System.Numerics.Vector3(1.0f, -1.0f, 0.0f)),  // Bottom-right vertex
                new FLVertex(new System.Numerics.Vector3(0.0f, 1.0f, 0.0f))    // Top vertex
            };

            int[] indices = new int[]
            {
                0, 1, 2 
            };

            Camera3D camera = new Camera3D(new System.Numerics.Vector3(0,0,12), new System.Numerics.Vector3(0,0,-1), new System.Numerics.Quaternion(0,0,0,0), 1, window.Size.ToSNV2());

            FLGX.MakeWindowCurrent(window);

            FLModel model = FLModel.FromFile("meshes/Test.obj");

            var defaultShaders = FLGX.BuildDefaultShaders(true);

            defaultShaders.SetUniform_Vec4("DrawColor", new System.Numerics.Vector4(255,255,255,0));

            var rot = 0f;
            FLGX.ClearColor(new System.Numerics.Vector4(0.1f, 0.05f, 0.2f, 0));
            window.Run(
                (float dt) =>
                {
                    rot += 0.001f;
                    FLGX.NewFrame();
                    FLGX.UseShader(defaultShaders);
                    defaultShaders.SetUniform_Mat4("Projection", camera.ProjectionMatrix, false);
                    defaultShaders.SetUniform_Mat4("View", camera.ViewMatrix, false);
                    defaultShaders.SetUniform_Mat4("Model", Matrix4x4.CreateTranslation(new System.Numerics.Vector3(0, 0, -3f)) * Matrix4x4.CreateRotationZ(rot) * Matrix4x4.CreateRotationY(rot) * Matrix4x4.CreateRotationX(rot) * Matrix4x4.CreateScale(1f), false);
                    model.Draw();
                    FLGX.EndFrame();
                }
            );

            model.VtxStruct.Destroy();
            FLGX.Shutdown();
        }
    }
}
