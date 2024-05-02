using flgx.Graphics.Common;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using StbImageSharp;

namespace flgx.Graphics.OpenGL
{
    public class OpenGLTexture : FLTexture
    {
        internal int Id;
        public override void Destroy()
        {
            GL.DeleteTexture(Id);
        }

        public override void Use(int texUnit = 1)
        {
            GL.ActiveTexture((TextureUnit)(33984+texUnit));
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        internal override void INT_GX_CreateTexture()
        {
            GL.GenTextures(1, out Id);
        }

        internal override void INT_GX_CreateTexturePathed(string path, bool genMipmaps = true)
        {
            GL.GenTextures(1, out Id);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult img = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, img.Width, img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, img.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if(genMipmaps)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public OpenGLTexture() : base()
        {
        }

        public OpenGLTexture(int id)
        {
            Id = id;
        }

        public OpenGLTexture(string path, bool mipmapped = true) : base(path, mipmapped)
        {
        }
    }
}
