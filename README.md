# FLGX - Flexible Lightweight Graphics Library
[![NuGet Badge](https://buildstats.info/nuget/FLGX?includePreReleases=true)](https://www.nuget.org/packages/FLGX/1.0.1-beta)
[![NuGet Badge](https://buildstats.info/nuget/FLUXUtils?includePreReleases=true)](https://www.nuget.org/packages/FLUXUtils/1.0.0)

<b>FLGX</b>(Flexible Lightweight Graphics Library) is an open-source C#(.NET) graphics rendering library meant to streamline graphics programming and let you work more on your games rather than spend tons of time on graphics!
FLGX is licensed under MIT, so you don't have to worry about licensing or anything! Just download it and use it!

FLGX will support three graphics APIs(OpenGL, DirectX 11 and Vulkan) in the future, so that you could code once and use any one of those three graphics APIs!
Right now, FLGX supports both DirectX 11 and OpenGL, with Vulkan possibly coming in the future.

# Features

The list of features in FLGX is evergrowing, but currently FLGX has got:

* Buffer Management
* Vertex Structures
* A windowing system
* Shader support(including rudimentary GLSL default ones)
* Meshes
* Loading models from files(and from vertex/index data)
* A backend for OpenGL
* A backend for Direct3D11
* Framebuffers(currently OpenGL only)
* An easy-to-use API.

In the future we would like to add Vulkan backend support, deferred rendering support, a lighting library & more!

# FLUX

FLUX(Flexible Lightweight Utilities Library) is another open-source library that comes with FLGX. It will implement additional utilities to help your development with FLGX!
Currenty FLUX has some basic features like a type-UniqueList, a simple ECS and a handy Camera3D class!
FLUX is able to be used standalone as well, as it doesn't actually depend on any FLGX functions directly.

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

```
    FLGX.Init(new FLGXInitSettings(RenderingAPI.OpenGL)); // Initialize the FLGX library with your selected RenderingAPI(currently only OpenGL and Direct3D11 is supported.)

    var window = FLGX.CreateWindow("Triangle Example", 1300, 900); // Create an FLGX window with your selected title and size.

    FLGX.MakeWindowCurrent(window); // Make your window the current window, so that FLGX knows where to draw to.

    var vertexStructure = FLGX.CreateFLVertexStructure(); // Creates a vertex structure(equivalent to VAO in OpenGL) to describe how your vertices are written(in this case it creates one specific to FLVertex)
    var mesh = new FLMesh(FLVertices.TriangleIndices, FLVertices.TriangleVertices); // Create an FLMesh object to automatically create a mesh for drawing

    var defaultShaders = FLGX.BuildDefaultShaders(); // Build and compile the default shaders. You can pass 'true' to this functions to build 3D default shaders too.
    defaultShaders.SetUniform_Vec4("DrawColor", new System.Numerics.Vector4(255,0,0,0)); // The default shaders have you pass the draw color through a uniform.

    // Start the window render loop with your render function.
    window.Run(
    (float dt) =>
    {
        FLGX.NewFrame(); // Begin a new FLGX frame for drawing.
        FLGX.UseShader(defaultShaders); // Use the default shaders.

        vertexStructure.Bind(); // Bind the vertex structure, so it knows the specification you set.
        mesh.Draw(); // Draw the triangle FLMesh to the screen.

        FLGX.EndFrame(); // Then end the frame.
    }
    );

    vertexStructure.Destroy(); // Make sure to destroy your vertex structure after.
    FLGX.Shutdown(); // Lastly, shutdown FLGX. This deinitializes the library, and destroys any unfreed buffers, windows and shaders.</code>
```

# Direct3D11/OpenGL Backend Differences

Despite the fact that this is a "code-once, use everywhere"-type library, the FLGX code used for Direct3D11 is slightly different than with OpenGL.
This is mainly due to the fact that the two graphics APIs are structured differently and have a different order of operations.
However, FLGX seeks to minimize this difference as much as possible, yet there's still a few things you have to do between each backend.

**Window Initialization**
(This part is unnecessary if you're going just for OpenGL)

Direct3D11 requires you to initialize everything in the OnLoad function hook given in the standardized window class object(IFLGXWindow)
This is because Direct3D11 uses Silk.NET's windowing system(FLGXSNWindow) and it works differently. But this interface has been standardized for both backends.

The fix is really simple, you just need to set IFLGXWindow.OnLoad to a lambda function and inside of the function, you initialize all of your textures, shaders, models, etc.

```
    var window = FLGX.CreateWindow(...); // Your IFLGXWindow object

    window.OnLoad = () => 
    {
        // Initialize your resources here...
    };
```

**Shader Uniforms**
While OpenGL uses uniforms as it's way of CPU-to-Shader data communication, Direct3D11 uses something called constant buffers.
This means that the regular SetUniform_type() functions will not work with Direct3D11(it will throw a NotImplementedException)
Also, as a side-note, Direct3D11 requires you to use HLSL shaders. Make sure you aren't using GLSL shaders with Direct3D11.

For Direct3D11 the process of using constant buffers is a bit more convoluted, but still quite simple:

First of all, create a struct, detailing what data you will be sending:

```
    struct Matrices // Say we want to send our 3D camera data.
    {
        public Matrix4x4 model;
        public Matrix4x4 view;
        public Matrix4x4 projection;
    }
```

Then initialize the uniform context and load in your data.
```
    var shader = FLGX.CreateHLSLShader(...); // Your HLSL(Direct3D11) shader

    shader.InitUniformContext<Matrices>();

    var MatrixStructObject = new Matrices();
    MatrixStructObject.model = Matrix4x4.CreateTranslation(new Vector3(0, 0, -10)) * Matrix4x4.CreateRotationY(0.1f);
    MatrixStructObject.view = OurCamera.ViewMatrix; MatrixStructObject.projection = OurCamera.ProjectionMatrix;

    shader.SetUniformStruct(mats); // And send your data to the constant buffer. Whenever you update your data in the struct object, just call this function again to update it on the shader.
```

# Acknowledgements

Libraries used while creating FLGX:

* AssimpNet
* OpenTK
* Serilog
* System.Numerics
* StbImageSharp
* Silk.NET

#
Copyright © 2024 Concave Studios