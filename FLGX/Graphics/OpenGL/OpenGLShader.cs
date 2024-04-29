using flgx.Graphics.Common;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace flgx.Graphics.OpenGL
{
    public class OpenGLShader : Shader
    {
        private int ProgramId;
        private Dictionary<string, int> UniformLocations = new Dictionary<string, int>();

        public override void Destroy()
        {
            if (ProgramId != 0)
            {
                GL.DeleteProgram(ProgramId);
                ShaderManager.UnregisterShader(this);
                ProgramId = 0;
            }
            else throw new Exception("ProgramId handle for shader is invalid (Destroy)");
        }

        public override void Use()
        {
            if (ProgramId != 0)
            {
                GL.UseProgram(ProgramId);
            }
            else throw new Exception("ProgramId handle for shader is invalid (Use)");
        }

        internal override void INT_GX_CreateShader(string VScode, string FScode)
        {
            // Shader Compilation

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, VScode);
            CompileShader(vertexShader);

            var fragShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShader, FScode);
            CompileShader(fragShader);

            // Program creation and linking

            ProgramId = GL.CreateProgram();

            GL.AttachShader(ProgramId, vertexShader);
            GL.AttachShader(ProgramId, fragShader);

            LinkProgram(ProgramId);

            // Detach and delete shaders

            GL.DetachShader(ProgramId, vertexShader);
            GL.DetachShader(ProgramId, fragShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragShader);

            // Load uniforms into dictionary

            GL.GetProgram(ProgramId, GetProgramParameterName.ActiveUniforms, out var uniformCount);

            for(int i = 0; i < uniformCount; i++)
            {
                var uniformName = GL.GetActiveUniform(ProgramId, i, out _, out _);
                var uniformLoc = GL.GetUniformLocation(ProgramId, uniformName);

                UniformLocations.Add(uniformName, uniformLoc);
            }
        }

        private void CheckUniformExists(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                if(UniformLocations.ContainsKey(name))
                {
                    return;
                }
            }
            throw new Exception("Uniform with name " + name + " does not exist in program [" + ProgramId + "].");
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Error occurred during the linking of program[{program}].\n\n{infoLog}");
            }
        }


        private void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"An error occured during the compilation of shader[{shader}].\n\n{infoLog}");
            }
        }

        public override void SetUniform_Float(string name, float data)
        {
            CheckUniformExists(name);
            Use();
            GL.Uniform1(UniformLocations[name], data);
        }

        public override void SetUniform_Int(string name, int data)
        {
            CheckUniformExists(name);
            Use();
            GL.Uniform1(UniformLocations[name], data);
        }

        public override void SetUniform_Vec3(string name, Vector3 vec)
        {
            CheckUniformExists(name);
            Use();
            GL.Uniform3(UniformLocations[name], vec.ToOTKVec3());
        }

        public override void SetUniform_Vec4(string name, Vector4 vec)
        {
            CheckUniformExists(name);
            Use();
            GL.Uniform4(UniformLocations[name], vec.ToOTKVec4());
        }

        public override void SetUniform_Vec2(string name, Vector2 vec)
        {
            CheckUniformExists(name);
            Use();
            GL.Uniform2(UniformLocations[name], vec.ToOTKVec2());
        }

        public override void SetUniform_Mat4(string name, Matrix4x4 mat, bool transpose = false)
        {
            CheckUniformExists(name);
            var matrix = mat.ToOTKMat4();
            Use();
            GL.UniformMatrix4(UniformLocations[name], transpose, ref matrix);
        }

        public OpenGLShader(string VScode, string FScode) : base(VScode, FScode)
        {
        }
    }
}
