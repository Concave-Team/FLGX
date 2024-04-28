using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLGX.Graphics.Common
{
    public abstract class FLBuffer
    {
        public BufferType DataType { get; }
        public int Size { get; private set; } = 0;
        public bool ContainsData = false;

        internal abstract void INT_GX_CreateBuffer();
        public abstract void Bind();
        public abstract void SetBufferData<T>(T[] data, int size) where T : struct;
        public abstract void DestroyBuffer();

        public FLBuffer(BufferType type)
        {
            DataType = type;

            FLBufferManager.RegisterBuffer(this);

            INT_GX_CreateBuffer();
        }    
    }

    public enum BufferType
    {
        Vertex,
        Index
    }
}
