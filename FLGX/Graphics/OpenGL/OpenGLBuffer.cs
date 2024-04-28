using FLGX.Graphics.Common;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FLGX.Graphics.OpenGL
{
    public class OpenGLBuffer : FLBuffer
    {
        internal int BufferId;
        internal override void INT_GX_CreateBuffer()
        {
            GL.GenBuffers(1, out BufferId);
        }

        public override void SetBufferData<T>(T[] data, int size) where T : struct
        {
            if (this.DataType == BufferType.Vertex)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, BufferId);
                GL.BufferData(BufferTarget.ArrayBuffer, size, data, BufferUsageHint.StaticDraw);
            }
            else if(this.DataType == BufferType.Index)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferId);
                GL.BufferData(BufferTarget.ElementArrayBuffer, size, data, BufferUsageHint.StaticDraw);
            }
        }

        public override void Bind()
        {
            if (this.DataType == BufferType.Vertex)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, BufferId);
            }
            else if (this.DataType == BufferType.Index)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferId);
            }
        }

        public override void DestroyBuffer()
        {
            GL.DeleteBuffer(BufferId);
            FLBufferManager.UnregisterBuffer(this);
        }

        public OpenGLBuffer(BufferType type) : base(type)
        {
        }
    }
}
