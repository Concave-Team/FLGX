using flgx.Graphics.Common;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.OpenGL
{
    public class OpenGLFrameBuffer : FLFrameBuffer
    {
        private int Id;
        public OpenGLTexture ColorAttachment;
        private int RBId;

        public override void Bind()
        {
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
            else
                FLGX.log.Error("Framebuffer is not yet complete, cannot bind.");
        }

        public override void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override void BindRenderbuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBId);
        }

        public override void AttachAttachments(int width, int height)
        {
            Bind();
            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                int tex = 0;
                GL.GenTextures(1, out tex);
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, tex, 0);

                ColorAttachment = new OpenGLTexture(tex);

                GL.GenRenderbuffers(1, out RBId);
                BindRenderbuffer();
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBId);
            }
        }

        public override void BindColorTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, ColorAttachment.Id);
        }

        public override void Destroy()
        {
            GL.DeleteFramebuffer(Id);
        }

        internal override void INT_GX_CreateFB()
        {
            GL.GenFramebuffers(1, out Id);
        }
    }
}
