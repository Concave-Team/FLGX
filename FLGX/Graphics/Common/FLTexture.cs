using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Common
{
    public abstract class FLTexture
    {
        internal abstract void INT_GX_CreateTexture();
        internal abstract void INT_GX_CreateTexturePathed(string path, bool genMipmaps = true);
        public abstract void Use(int texUnit = 1);
        public abstract void Destroy();

        public FLTexture(string path, bool mipmapped = true)
        {
            INT_GX_CreateTexturePathed(path, mipmapped);
        }

        public FLTexture()
        {
            INT_GX_CreateTexture();
        }
    }
}
