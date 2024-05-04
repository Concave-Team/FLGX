using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using flgx.Graphics.Common;

namespace flgx.Graphics.Common
{
    public class FLMesh
    {
        public int[] Indices;
        public FLVertex[] Vertices;

        public FLBuffer VertexBuffer;
        public FLBuffer IndexBuffer;

        /// <summary>
        /// Draws the Mesh
        /// </summary>
        public void Draw()
        {
            FLGX.DrawIndexed(VertexBuffer, IndexBuffer, Indices.Length);
        }

        public void DrawInstances(int count)
        {
            FLGX.DrawInstanced(VertexBuffer, IndexBuffer, Indices.Length, count);
        }

        // No need for a destroy function, since all buffers get destroyed anyway.

        /// <summary>
        /// Creates an FLMesh object for usage with rendering objects.
        /// </summary>
        /// <param name="indices">The indices of the mesh</param>
        /// <param name="vertices">The vertices of the mesh</param>
        public FLMesh(int[] indices, FLVertex[] vertices)
        {
            Indices = indices;
            Vertices = vertices;

            VertexBuffer = FLGX.CreateVertexBuffer<FLVertex>(vertices, vertices.Length * Marshal.SizeOf<FLVertex>());
            IndexBuffer = FLGX.CreateIndexBuffer<int>(Indices, Indices.Length * sizeof(int));
        }
    }
}
