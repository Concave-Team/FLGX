# FLGX - Flexible Lightweight Graphics Library
[![NuGet Badge](https://buildstats.info/nuget/FLGX?includePreReleases=true)](https://www.nuget.org/packages/FLGX/1.0.1-beta)
[![NuGet Badge](https://buildstats.info/nuget/FLUXUtils?includePreReleases=true)](https://www.nuget.org/packages/FLUXUtils/1.0.0)

<b>FLGX</b>(Flexible Lightweight Graphics Library) is an open-source C#(.NET) graphics rendering library meant to streamline graphics programming and let you work more on your games rather than spend tons of time on graphics!
FLGX is licensed under MIT, so you don't have to worry about licensing or anything! Just download and use!

FLGX will support three graphics APIs(OpenGL, DirectX 11 and Vulkan) in the future, so you can code once, and use three graphics APIs!
Currently, FLGX only supports OpenGL, but in the future the other backends will come in.

# Features

The list of features in FLGX is evergrowing, but currently FLGX has got:

* Buffer Management
* Vertex Structures
* A windowing system
* Shader support(including default ones)
* Meshes
* Loading models from files
* A backend for OpenGL
* An easy-to-use API.

In the future we would like to add DirectX and Vulkan backend support, indirect and instanced drawing, an easy way to create and manage framebuffers & more!

# FLUX
FLUX(Flexible Lightweight Utilities Library) is another open-source library that comes with FLGX. It will implement additional utilities to help your development with FLGX!
Though, currently it only has a Camera3D class, it will soon contain more things like state managers and a simple ECS! 
FLUX will be able to be used standalone as well, as it doesn't actually depend on any FLGX functions directly.

# How To Get

To use FLGX in your project you can install it through NuGet or build it for yourself, which is also very simple!
All you need to do is to clone this repository and include the FLGX project file in your project's solution.
From there - add a project reference to it, and you can start using FLGX!

As for the NuGet release, just open up your NuGet package manager console and use: Install-Package FLGX
For FLUX you can do Install-Package FLUXUtils

# How To Use

Using FLGX is a little like using OpenGL or some other graphics API.
FLGX is designed to provide a rather low-level experience to graphics programming to hand most of the control to your hands.

This is how a basic program to render a triangle looks like in FLGX:

<code>FLGX.Init(new FLGXInitSettings(RenderingAPI.OpenGL)); // Initialize the FLGX library with your selected RenderingAPI(currently only OpenGL is supported.)

    var window = FLGX.CreateWindow("Triangle Example", 1300, 900); // Create an FLGX window with your selected title and size.

    float[] vertices = new float[] // Define the triangle's vertices
    {
       -1.0f, -1.0f, 0.0f, 
        1.0f, -1.0f, 0.0f,  
        0.0f, 1.0f, 0.0f    
    };

    int[] indices = new int[] // Define the triangle's indices
    {
        0, 1, 2 
    };

    FLGX.MakeWindowCurrent(window); // Make your window the current window, so that FLGX knows where to draw to.

    var vertexStructure = FLGX.CreateVertexStructure(); // Create a vertex structure to specify how your vertices are written(this is basically the equivalent to a VAO in OpenGL)

    var vertexBuffer = FLGX.CreateVertexBuffer<float>(vertices, sizeof(float) * vertices.Length); // Create a vertex buffer that stores your vertices
    var indexBuffer = FLGX.CreateIndexBuffer<int>(indices, sizeof(int) * indices.Length); // Create an index buffer to store your indices in.

    vertexBuffer.Bind(); // Bind your vertex buffer to make sure it's the current active buffer.
    vertexStructure.AddAttribPointer(0, 3, FLVertexAttribType.Float, false, 3 * sizeof(float), 0); // Create a vertex attribute pointer, so that FLGX knows how your vertices are written.

    var defaultShaders = FLGX.BuildDefaultShaders(); // Build and compile the default shaders. You can pass 'true' to this functions to build 3D default shaders too.
    defaultShaders.SetUniform_Vec4("DrawColor", new System.Numerics.Vector4(255,0,0,0)); // The default shaders have you pass the draw color through a uniform.

    // Start the window render loop with your render function.
    window.Run(
    (float dt) =>
    {
        FLGX.NewFrame(); // Begin a new FLGX frame for drawing.
        FLGX.UseShader(defaultShaders); // Use the default shaders.

        vertexStructure.Bind(); // Bind the vertex structure, so it knows the specification you set.
        FLGX.DrawIndexed(vertexBuffer, indexBuffer, indices.Length); // Draw based on the vertices and indices you set in the buffers.

        FLGX.EndFrame(); // Then end the frame.
    }
    );

    vertexStructure.Destroy(); // Make sure to destroy your vertex structure after.
    FLGX.Shutdown(); // Lastly, shutdown FLGX. This deinitializes the library, and destroys any unfreed buffers, windows and shaders.</code>

# Acknowledgements

Libraries used while creating FLGX:

* AssimpNet
* OpenTK
* Serilog
* System.Numerics