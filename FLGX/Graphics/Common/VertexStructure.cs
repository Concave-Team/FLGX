using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLGX.Graphics.Common
{
    public abstract class VertexStructure
    {
        internal abstract void INT_GX_CreateVStruct();

        public abstract void Bind();
        public abstract void AddAttribPointer(int index, int size, FLVertexAttribType type, bool normalized, int stride, int offset);
        public abstract void Destroy();
        public abstract void EnableAttrib(int index);
        public abstract void DisableAttrib(int index);

        public VertexStructure()
        {
            INT_GX_CreateVStruct();
        }
    }

    public enum FLVertexAttribType
    {
        Float,
        Int,
        UInt,
        Byte,
        Double,
        Short
    }
}
