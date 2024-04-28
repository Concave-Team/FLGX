using FLGX.Graphics.Common;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLGX.Graphics.OpenGL
{
    public class OpenGLVertexStructure : VertexStructure
    {
        private uint StructureId;
        internal override void INT_GX_CreateVStruct()
        {
            GL.GenVertexArrays(1, out StructureId);
        }

        private VertexAttribPointerType ConvertFLtoGL(FLVertexAttribType ty)
        {
            switch (ty)
            {
                case FLVertexAttribType.Float:  return VertexAttribPointerType.Float;
                case FLVertexAttribType.Double: return VertexAttribPointerType.Double;
                case FLVertexAttribType.UInt:   return VertexAttribPointerType.UnsignedInt;
                case FLVertexAttribType.Int:    return VertexAttribPointerType.Int;
                case FLVertexAttribType.Byte:   return VertexAttribPointerType.Byte;
                case FLVertexAttribType.Short:  return VertexAttribPointerType.Short;
                default: throw new Exception("Couldn't convert FLVertexAttribType to OpenGL type.");
            }
        }

        public override void Destroy()
        {
            GL.DeleteVertexArray(StructureId);
        }

        public override void AddAttribPointer(int index, int size, FLVertexAttribType type, bool normalized, int stride, int offset)
        {
            GL.BindVertexArray(StructureId);
            GL.VertexAttribPointer(index, size, ConvertFLtoGL(type), normalized, stride, offset);
            GL.EnableVertexAttribArray(index);
        }

        public override void EnableAttrib(int index)
        {
            GL.BindVertexArray(StructureId);
            GL.EnableVertexAttribArray(index);
        }

        public override void DisableAttrib(int index)
        {
            GL.BindVertexArray(StructureId);
            GL.DisableVertexAttribArray(index);
        }

        public override void Bind()
        {
            GL.BindVertexArray(StructureId);
        }
    }
}
