/*
    Updated for FLGX v2.1.0 #
 */

using flgx;
using flgx.Graphics;
using flgx.Graphics.Common;
using Silk.NET.Core.Attributes;
using System.Numerics;
using System.Runtime.InteropServices;

namespace flgx.Examples.Direct3D11.RedTriangle
{
    [StructLayout(LayoutKind.Sequential)]
    struct Data
    {
        public Vector3 draw_color;
        public float padding; // This is needed because Vector3 is 12 bytes and Direct3D11 requires that the size of this struct be a multiple of 16 bytes. (12+4=16)
    }

    internal class Program
    {
        public static VertexStructure vertexSpec;
        public static FLMesh triangleMesh;
        public static Shader _2DShaders;
        public static Data shaderData = new Data(); // We will be creating an object of our constant buffer(uniform data) descriptor struct.

        static void Main(string[] args)
        {
            Console.WriteLine("FLGX Examples: 02 - Red Triangle");

            FLGX.Init(new FLGXInitSettings(RenderingAPI.Direct3D11));

            var window = FLGX.CreateWindow("Red Triangle", 1600, 900);
            
            window.OnLoad = () =>
            {
                FLGX.MakeWindowCurrent(window);
                _2DShaders = FLGX.CreateHLSLShader("shaders_triangle.hlsl"); // Now, since we use Direct3D11, we must create a HLSL shader instead of a GLSL shader.
                shaderData.draw_color = new Vector3(1, 0, 0); // Now we can set the draw color to red.
                shaderData.padding = 0.0f; // Initialize any other values to avoid any issues.

                _2DShaders.InitUniformContext<Data>(); // We will be initializing the shader to use this descriptor struct.

                _2DShaders.SetUniformStruct(shaderData); // And we then pass the data object to the shader.

                vertexSpec = FLGX.CreateFLVertexStructure(); // Create a Vertex Structure(OpenGL equivalent to VAO) to describe how your vertices are structured(this one is FLVertex-specific).

                vertexSpec.Bind(); // Make sure to bind your vertex structure before creating a mesh.
                triangleMesh = new FLMesh(FLVertices.DXTriIndicies, FLVertices.TriangleVertices); // It is best to use an FLMesh when you can, since it handles the buffers for you.

                FLGX.ClearColor(new System.Numerics.Vector4(0.2f, 0.1f, 0.3f, 1.0f)); // Set our clear color to a purple-ish color.
            };

            window.Run(
                (float dt) => // Let's define our render loop here. The render loop function takes in a float that describes the deltatime.
                {
                    FLGX.NewFrame(); // Begin a new drawing/rendering frame - this tells FLGX to clear the screen and prepare it for rendering.

                    vertexSpec.Bind(); // Bind your VertexStructure before drawing, so FLGX knows your vertex specification.
                    FLGX.UseShader(_2DShaders);
                    triangleMesh.Draw(); // And draw your triangle mesh!
                    FLGX.EndFrame(); // End the current frame.
                }
            );

            FLGX.Shutdown(); // After you're done, make sure to run Shutdown, as it clears up buffers, windows and shaders.
        }
    }
}
