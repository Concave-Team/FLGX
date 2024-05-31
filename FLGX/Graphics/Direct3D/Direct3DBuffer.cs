using flgx.Graphics.Common;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Direct3D
{
    public class Direct3DBuffer : FLBuffer
    {
        public BufferDesc bufferDesc;
        public ComPtr<ID3D11Buffer> buffer = default;
        private bool BufferCreated = false;
        public override void Bind(uint strides = 0, uint offsets = 0)
        {
            if (this.DataType == BufferType.Vertex)
                FLGX.InternalState.deviceContext.IASetVertexBuffers(0, 1, ref buffer, strides, offsets);
            else if (this.DataType == BufferType.Index)
                FLGX.InternalState.deviceContext.IASetIndexBuffer(buffer, Silk.NET.DXGI.Format.FormatR32Uint, 0u);
        }

        public override void DestroyBuffer()
        {
            buffer.Dispose();
        }

        public override void SetBufferData<T>(T[] data, int size)
        {
            bufferDesc = new BufferDesc
            {
                ByteWidth = (uint)(size),
                Usage = Usage.Default,
                BindFlags = (uint)(this.DataType == BufferType.Index ? BindFlag.IndexBuffer : BindFlag.VertexBuffer)
            };

            var state = FLGX.InternalState;

            unsafe
            {
                var d3d11 = state.d3d11;
                var dxgi = state.dxgi;
                var compiler = state.compiler;
                fixed (T* pData = data)
                {
                    var subresData = new SubresourceData
                    {
                        PSysMem = pData
                    };

                    SilkMarshal.ThrowHResult(state.device.CreateBuffer(in bufferDesc, in subresData, ref buffer));
                }
            }
        }

        public override void SetEmptyData<T>()
        {
            var state = FLGX.InternalState;

            unsafe
            {
                var d3d11 = state.d3d11;
                var dxgi = state.dxgi;
                var compiler = state.compiler;
                BufferDesc desc = new BufferDesc((uint)sizeof(T), Usage.Dynamic, (uint)BindFlag.ConstantBuffer, (uint)CpuAccessFlag.Write, 0, 0);
                SilkMarshal.ThrowHResult(state.device.CreateBuffer(in desc, (SubresourceData*)null, ref buffer));
            }

            BufferCreated = true;
        }    

        public override void SetBufferData<T>(T data, int size = 0)
        {
            var state = FLGX.InternalState;
            if (this.DataType == BufferType.Constant)
            {
                if(BufferCreated)
                {
                    unsafe
                    {
                        MappedSubresource subRes = new MappedSubresource(null, 0, 0);
                        SilkMarshal.ThrowHResult(state.deviceContext.Map(buffer, 0, Map.WriteDiscard, 0, ref subRes));
                        Unsafe.CopyBlock(subRes.PData, Unsafe.AsPointer(ref data), (uint)Unsafe.SizeOf<T>());
                        state.deviceContext.Unmap(buffer, 0);
                    }
                }
            }
            else
                throw new flgx.Internal.FLGXInternalStateException(FLGX.InternalState, "[ERROR/D3D11]: Cannot set buffer data of one element for non-constant buffer.");
        }

        internal override void INT_GX_CreateBuffer()
        {
            // UNUSED FOR DIRECT3D11 BACKEND
        }

        public Direct3DBuffer(BufferType type) : base(type)
        {
        }
    }
}
