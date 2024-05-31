/*
    Updated for FLGX v2.1.0 #
 */

using flgx.Graphics.Common;

namespace flgx.Examples.RedTriangle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FLGX Examples: 02 - Red Triangle");

            FLGX.Init(new FLGXInitSettings(RenderingAPI.OpenGL));

            var window = FLGX.CreateWindow("Red Triangle", 1600, 900);

            FLGX.MakeWindowCurrent(window);

            VertexStructure vertexSpec = FLGX.CreateVertexStructure(); // Create a Vertex Structure(OpenGL equivalent to VAO) to describe how your vertices are structured.

            vertexSpec.Bind(); // Make sure to bind your vertex structure before creating a mesh.
            FLMesh triangleMesh = new FLMesh(FLVertices.TriangleIndices, FLVertices.TriangleVertices); // It is best to use an FLMesh when you can, since it handles the buffers for you.

            FLGX.SetFLVertexAttributes(vertexSpec); // Since we are using FLVertex anyway, we can use this built-in function to set up the VertexStructure for us. (Use this function only after creating a mesh or buffer to avoid errors!)

            Shader _2DShaders = FLGX.BuildDefaultShaders(); // By default, FLGX has basic built-in shaders for off-screen rendering(screen quad), basic 3D(textured) and 2D(colors-only). Here we are using the 2D ones.
            _2DShaders.SetUniform_Vec4("DrawColor", new System.Numerics.Vector4(1.0f, 0, 0, 1.0f)); // The 2D shader has a uniform DrawColor that we set to define the drawing color. Here we choose red.

            FLGX.ClearColor(new System.Numerics.Vector4(0.2f, 0.1f, 0.3f, 1.0f)); // Set our clear color to a purple-ish color.
            window.Run(
                (float dt) => // Let's define our render loop here. The render loop function takes in a float that describes the deltatime.
                {
                    FLGX.NewFrame(); // Begin a new drawing/rendering frame - this tells FLGX to clear the screen and prepare it for rendering.

                    vertexSpec.Bind(); // Bind your VertexStructure before drawing, so FLGX knows your vertex specification.
                    _2DShaders.Use(); // Use your shaders, so they can be used for drawing.
                    triangleMesh.Draw(); // And draw your triangle mesh!
                    FLGX.EndFrame(); // End the current frame.
                }
            );

            vertexSpec.Destroy(); // Make sure to destroy your VertexStructure after you are done with it.
            FLGX.Shutdown(); // After you're done, make sure to run Shutdown, as it clears up buffers, windows and shaders.
        }
    }
}
