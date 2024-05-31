using flgx.Graphics.Common;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StbImageSharp;
using Silk.NET.DXGI;

namespace flgx.Graphics.Direct3D
{
    public class Direct3DTexture : FLTexture
    {
        ComPtr<ID3D11Texture2D> texture = default;
        ComPtr<ID3D11SamplerState> textureSampler = default;
        ComPtr<ID3D11ShaderResourceView> textureResourceView = default;

        public override void Destroy()
        {
            texture.Dispose();
            textureSampler.Dispose();
            textureResourceView.Dispose();
        }

        public override void Use(int texUnit = 1)
        {
            unsafe
            {
                FLGX.InternalState.deviceContext.PSSetSamplers((uint)texUnit, 1u, ref textureSampler);
                FLGX.InternalState.deviceContext.PSSetShaderResources((uint)texUnit, 1u, ref textureResourceView);
            }
        }

        internal override void INT_GX_CreateTexture()
        {
            throw new NotImplementedException();
        }

        internal override void INT_GX_CreateTexturePathed(string path, bool genMipmaps = true)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult img = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            var textureDesc = new Texture2DDesc
            {
                Width = (uint)img.Width,
                Height = (uint)img.Height,
                Format = Format.FormatR8G8B8A8Unorm,
                MipLevels = 1,
                BindFlags = (uint)BindFlag.ShaderResource,
                Usage = Usage.Default,
                CPUAccessFlags = 0,
                MiscFlags = (uint)ResourceMiscFlag.None,
                SampleDesc = new SampleDesc(1, 0),
                ArraySize = 1
            };

            unsafe
            {
                fixed (byte* pixelData = img.Data)
                {
                    var subresourceData = new SubresourceData
                    {
                        PSysMem = pixelData,
                        SysMemPitch = (uint)img.Width * sizeof(int),
                        SysMemSlicePitch = (uint)(img.Width * sizeof(int) * img.Height)
                    };

                    SilkMarshal.ThrowHResult
                    (
                        FLGX.InternalState.device.CreateTexture2D
                        (
                            in textureDesc,
                            in subresourceData,
                            ref texture
                        )
                    );
                }
            }

            var srvDesc = new ShaderResourceViewDesc
            {
                Format = textureDesc.Format,
                ViewDimension = D3DSrvDimension.D3DSrvDimensionTexture2D,
                Anonymous = new ShaderResourceViewDescUnion
                {
                    Texture2D =
                    {
                        MostDetailedMip = 0,
                        MipLevels = 1
                    }
                }
            };

            SilkMarshal.ThrowHResult
            (
                FLGX.InternalState.device.CreateShaderResourceView
                (
                    texture,
                    in srvDesc,
                    ref textureResourceView
                )
            );

            // Create a sampler.
            var samplerDesc = new SamplerDesc
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                MipLODBias = 0,
                MaxAnisotropy = 1,
                MinLOD = float.MinValue,
                MaxLOD = float.MaxValue,
            };

            unsafe
            {
                // Black border color.
                samplerDesc.BorderColor[0] = 0.0f;
                samplerDesc.BorderColor[1] = 0.0f;
                samplerDesc.BorderColor[2] = 0.0f;
                samplerDesc.BorderColor[3] = 1.0f;
            }

            SilkMarshal.ThrowHResult
            (
                FLGX.InternalState.device.CreateSamplerState
                (
                    in samplerDesc,
                    ref textureSampler
                )
            );

            if(genMipmaps)
            {
                FLGX.InternalState.deviceContext.GenerateMips(textureResourceView);
            }
        }


        public Direct3DTexture(string path, bool mipmapped = true) : base(path, mipmapped)
        {
        }
    }
}
