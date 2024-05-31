using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FLVertex
    {
        public Vector3 Position;
        public Vector3 Normal = new Vector3(0, 0, 0);
        public Vector2 TexCoord = new Vector2(0, 0);

        public FLVertex(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }

        public FLVertex(Vector3 position) : this()
        {
            Position = position;
        }
    }

    public static class FLVertices
    {
        public static FLVertex[] QuadVertices = new FLVertex[]
        {
            new FLVertex(new Vector3(-1.0f,  1.0f, 0.0f), new Vector3(0, 0, 0), new Vector2(0.0f, 1.0f)), // Top Left
            new FLVertex(new Vector3( 1.0f,  1.0f, 0.0f), new Vector3(0, 0, 0), new Vector2(1.0f, 1.0f)), // Top Right
            new FLVertex(new Vector3( 1.0f, -1.0f, 0.0f), new Vector3(0, 0, 0), new Vector2(1.0f, 0.0f)), // Bottom Right
            new FLVertex(new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(0, 0, 0), new Vector2(0.0f, 0.0f))  // Bottom Left
        };
        public static int[] QuadIndices = new int[]
        {
            0, 1, 2, // First Triangle
            0, 2, 3  // Second Triangle
        };
        public static FLVertex[] TriangleVertices = new FLVertex[]
        {
           new FLVertex(new Vector3(-1.0f, -1.0f, 0.0f)),
           new FLVertex(new Vector3(1.0f, -1.0f, 0.0f)),
           new FLVertex(new Vector3(0.0f, 1.0f, 0.0f))
        };

        public static int[] TriangleIndices = new int[]
        {
            0, 1, 2
        };

        public static int[] DXTriIndicies = new int[]
        {
            2, 1, 0
        };
    }
}
