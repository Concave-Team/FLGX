using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx
{
    public enum RenderingAPI
    {
        /// <summary>
        /// OpenGL Rendering - Supported.
        /// </summary>
        OpenGL,
        /// <summary>
        /// Not supported yet.
        /// </summary>
        Direct3D11,
        /// <summary>
        /// Not supported yet.
        /// </summary>
        Vulkan
    }
    public class FLGXInitSettings
    {
        public RenderingAPI renderingAPI { get; set; }


        public FLGXInitSettings(RenderingAPI renderingAPI)
        {
            this.renderingAPI = renderingAPI;
        }

        public FLGXInitSettings()
        {
        }
    }
}
