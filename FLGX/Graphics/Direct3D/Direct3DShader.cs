using flgx.Graphics.Common;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Direct3D
{
    public class Direct3DShader : Shader
    {
        private ComPtr<ID3D11VertexShader> vertexShader = default;
        private ComPtr<ID3D11PixelShader> pixelShader = default;
        public ComPtr<ID3D10Blob> VertexShaderBytecode;
        public Direct3DBuffer UniformBuffer;

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void SetUniform_Float(string name, float data)
        {
            throw new NotImplementedException();
        }

        public override void SetUniform_Int(string name, int data)
        {
            throw new NotImplementedException();
        }

        public override void SetUniform_Mat4(string name, Matrix4x4 mat, bool transpose)
        {
            throw new NotImplementedException();
        }

        public override void SetUniform_Vec2(string name, Vector2 vec)
        {
            throw new NotImplementedException();
        }

        public override void SetUniform_Vec3(string name, Vector3 vec)
        {
            throw new NotImplementedException();
        }

        public override void SetUniform_Vec4(string name, Vector4 vec)
        {
            throw new NotImplementedException();
        }

        public override void SetUniformStruct<T>(T data, int register)
        {
            UniformBuffer.SetBufferData<T>(data);

            unsafe
            {
                FLGX.InternalState.deviceContext.PSSetConstantBuffers((uint)register, 1, ref UniformBuffer.buffer);
                FLGX.InternalState.deviceContext.VSSetConstantBuffers((uint)register, 1, ref UniformBuffer.buffer);
            }
        }

        public override void InitUniformContext<T>()
        {
            UniformBuffer = new Direct3DBuffer(BufferType.Constant);
            UniformBuffer.SetEmptyData<T>();
        }

        public override void Use()
        {
            FLGX.InternalState.deviceContext.VSSetShader(vertexShader, ref Unsafe.NullRef<ComPtr<ID3D11ClassInstance>>(), 0);
            FLGX.InternalState.deviceContext.PSSetShader(pixelShader, ref Unsafe.NullRef<ComPtr<ID3D11ClassInstance>>(), 0);
        }

        internal override void INT_GX_CreateShader(string VScode, string FScode)
        {
            // Not Used
        }

        internal override void INT_GX_CreateShader(string shaderCode)
        {
            unsafe
            {
                var shaderCodeBytes = Encoding.ASCII.GetBytes(shaderCode);

                ComPtr<ID3D10Blob> vertexCode = default;
                ComPtr<ID3D10Blob> vertexErrors = default;

                HResult hr = FLGX.InternalState.compiler.Compile
                (
                    in shaderCodeBytes[0],
                    (nuint)shaderCodeBytes.Length,
                    nameof(shaderCode),
                    null,
                    ref Unsafe.NullRef<ID3DInclude>(),
                    "vs_main",
                    "vs_5_0",
                    0,
                    0,
                    ref vertexCode,
                    ref vertexErrors
                );

                if(hr.IsFailure)
                {
                    if(vertexErrors.Handle is not null)
                    {
                        Console.WriteLine(SilkMarshal.PtrToString((nint)vertexErrors.GetBufferPointer()));
                    }

                    hr.Throw();
                }

                VertexShaderBytecode = vertexCode;

                // Compile pixel shader.
                ComPtr<ID3D10Blob> pixelCode = default;
                ComPtr<ID3D10Blob> pixelErrors = default;
                hr = FLGX.InternalState.compiler.Compile
                (
                    in shaderCodeBytes[0],
                    (nuint)shaderCodeBytes.Length,
                    nameof(shaderCode),
                    null,
                    ref Unsafe.NullRef<ID3DInclude>(),
                    "ps_main",
                    "ps_5_0",
                    0,
                    0,
                    ref pixelCode,
                    ref pixelErrors
                );

                // Check for compilation errors.
                if (hr.IsFailure)
                {
                    if (pixelErrors.Handle is not null)
                    {
                        Console.WriteLine(SilkMarshal.PtrToString((nint)pixelErrors.GetBufferPointer()));
                    }

                    hr.Throw();
                }

                // Create vertex shader.
                SilkMarshal.ThrowHResult
                (
                    FLGX.InternalState.device.CreateVertexShader
                    (
                        vertexCode.GetBufferPointer(),
                        vertexCode.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref vertexShader
                    )
                );

                // Create pixel shader.
                SilkMarshal.ThrowHResult
                (
                    FLGX.InternalState.device.CreatePixelShader
                    (
                        pixelCode.GetBufferPointer(),
                        pixelCode.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref pixelShader
                    )
                );
            }
        }

        public Direct3DShader(string VScode, string FScode) : base(VScode, FScode)
        {
            // Not Used
        }

        public Direct3DShader(string shaderCode) : base(shaderCode)
        {
            
        }
    }
}
