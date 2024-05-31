using flgx.Graphics.Common;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Direct3D
{
    public struct InputElement
    {
        public string SemanticName;
        public uint SemanticIndex;
        public Format Format;
        public uint InputSlot;
        public uint AlignedByteOffset;
        public InputClassification InputSlotClass;
        public uint InstanceDataStepRate;

        public InputElementDesc ToIEDesc()
        {
            unsafe
            {
                fixed (byte* name = SilkMarshal.StringToMemory(SemanticName))
                    return new InputElementDesc { Format = this.Format, AlignedByteOffset = this.AlignedByteOffset, InputSlot = this.InputSlot, InputSlotClass = this.InputSlotClass, InstanceDataStepRate = this.InstanceDataStepRate, SemanticIndex = this.SemanticIndex, SemanticName = name };
            }
        }

        public InputElement(string semanticName, uint semanticIndex, Format format, uint inputSlot, uint alignedByteOffset, InputClassification inputSlotClass, uint instanceDataStepRate)
        {
            SemanticName = semanticName;
            SemanticIndex = semanticIndex;
            Format = format;
            InputSlot = inputSlot;
            AlignedByteOffset = alignedByteOffset;
            InputSlotClass = inputSlotClass;
            InstanceDataStepRate = instanceDataStepRate;
        }
    }

    public class Direct3DVertexStructure : VertexStructure
    {
        private ComPtr<ID3D11InputLayout> inLayout;

        public override void AddAttribPointer(int index, int size, FLVertexAttribType type, bool normalized, int stride, int offset)
        {
            // DirectX requires us to define these during creation, so this function is not used here in this case.
        }

        public override void Bind()
        {
            FLGX.InternalState.deviceContext.IASetInputLayout(inLayout);
        }

        public override void Destroy()
        {
            inLayout.Dispose();
        }

        public override void DisableAttrib(int index)
        {
            // Not Used Here.
        }

        public override void EnableAttrib(int index)
        {
            // Not Used Here.
        }

        internal override void INT_GX_CreateVStruct()
        {
            // Not Used Here.
        }

        internal override void INT_GX_CreateVStruct(InputElement[] elements)
        {
            var inputElementDescs = new List <InputElementDesc>();

            foreach(InputElement elementDesc in elements)
            {
                inputElementDescs.Add(elementDesc.ToIEDesc());
            }

            var arrayOfIED = inputElementDescs.ToArray();
            var vertexCode = (FLGX.InternalState.GetStateVariable("ActiveShader") as Direct3DShader).VertexShaderBytecode;

            unsafe {
                fixed (InputElementDesc* descs = arrayOfIED)
                {
                    SilkMarshal.ThrowHResult
                    (
                        FLGX.InternalState.device.CreateInputLayout
                        (
                            descs,
                            (uint)arrayOfIED.Length,
                            vertexCode.GetBufferPointer(),
                            vertexCode.GetBufferSize(),
                            ref inLayout
                        )
                    );
                }
            }
        }

        public Direct3DVertexStructure()
        {
        }

        public Direct3DVertexStructure(InputElement[] elements) : base(elements)
        {

        }
    }
}
