using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Common
{
    public abstract class FLBuffer
    {
        public BufferType DataType { get; }
        public int Size { get; private set; } = 0;
        public bool ContainsData = false;

        internal abstract void INT_GX_CreateBuffer();
        public virtual void Bind() { }
        public virtual void Bind(uint strides = 0, uint offsets = 0) { }
        public abstract void SetBufferData<T>(T[] data, int size) where T : struct;
        public virtual void SetBufferData<T>(T data, int size) where T : struct { }
        public virtual void SetEmptyData<T>() where T : struct { }
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
        Index,
        Constant
    }
}
