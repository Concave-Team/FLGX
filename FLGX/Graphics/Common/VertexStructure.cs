﻿using flgx.Graphics.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Common
{
    public abstract class VertexStructure
    {
        internal abstract void INT_GX_CreateVStruct();
        internal virtual void INT_GX_CreateVStruct(InputElement[] elements) { }

        public abstract void Bind();
        public abstract void AddAttribPointer(int index, int size, FLVertexAttribType type, bool normalized, int stride, int offset);
        public abstract void Destroy();
        public abstract void EnableAttrib(int index);
        public abstract void DisableAttrib(int index);

        public VertexStructure()
        {
            INT_GX_CreateVStruct();
        }

        public VertexStructure(InputElement[] elements)
        {
            INT_GX_CreateVStruct(elements);
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
