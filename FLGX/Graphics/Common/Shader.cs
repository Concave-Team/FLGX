using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FLGX.Graphics.Common
{
    public abstract class Shader
    {
        public abstract void Destroy();
        public abstract void Use();
        public abstract void SetUniform_Float(string name, float data);
        public abstract void SetUniform_Int(string name, int data);
        public abstract void SetUniform_Vec3(string name, Vector3 vec);
        public abstract void SetUniform_Vec4(string name, Vector4 vec);
        public abstract void SetUniform_Vec2(string name, Vector2 vec);
        public abstract void SetUniform_Mat4(string name, Matrix4x4 mat, bool transpose);
        internal abstract void INT_GX_CreateShader(string VScode, string FScode);

        public Shader(string VScode, string FScode)
        {
            ShaderManager.RegisterShader(this);
            INT_GX_CreateShader(VScode, FScode);
        }
    }
}
