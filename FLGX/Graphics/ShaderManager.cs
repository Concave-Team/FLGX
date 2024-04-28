using FLGX.Graphics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLGX.Graphics
{
    public static class ShaderManager
    {
        public static List<Shader> shaders = new List<Shader>();

        public static void RegisterShader(Shader shader)
        {
            shaders.Add(shader);
        }

        public static void UnregisterShader(Shader shader)
        {
            shaders.Remove(shader);
        }

        public static void DestroyAllShaders()
        {
            foreach(var shader in shaders)
            {
                shader.Destroy();
            }
        }
    }
}
