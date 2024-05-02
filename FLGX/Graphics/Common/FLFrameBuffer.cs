using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Common
{
    public abstract class FLFrameBuffer
    {
        protected FLFrameBuffer()
        {
            INT_GX_CreateFB();
        }

        internal abstract void INT_GX_CreateFB();
        public abstract void Bind();
        public abstract void AttachAttachments(int width, int height);
        /// <summary>
        /// Binds the created renderbuffer (OpenGL-only)
        /// </summary>
        public abstract void BindRenderbuffer();
        public abstract void BindColorTexture();
        public abstract void Unbind();
        public abstract void Destroy();
    }
}
