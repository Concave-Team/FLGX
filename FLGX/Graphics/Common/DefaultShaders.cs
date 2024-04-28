using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLGX.Graphics.Common
{
    public static class DefaultShaders
    {
        public static string DefaultGLSLShaders_FS =
            @"
                #version 330 core
                uniform vec4 DrawColor;
                out vec4 FragColor;

                void main()
                {
                    FragColor = DrawColor;
                } 
            ";
        public static string DefaultGLSLShaders_VS =
            @"
                #version 330 core
                layout (location = 0) in vec3 aPos;
                layout (location = 1) in vec3 aNormal;
                layout (location = 2) in vec2 aTexCoord;

                void main()
                {
                    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                }
            ";
        public static string DefaultGLSLShaders3D_FS =
            @"
                #version 330 core
                uniform vec4 DrawColor;
                out vec4 FragColor;
                in vec3 normal;

                void main()
                {
                    FragColor = DrawColor * vec4(normal, 1.0);
                } 
            ";
        public static string DefaultGLSLShaders3D_VS =
            @"
                #version 330 core
                layout (location = 0) in vec3 aPos;
                layout (location = 1) in vec3 aNormal;
                layout (location = 2) in vec2 aTexCoord;

                uniform mat4 Model;
                uniform mat4 Projection;
                uniform mat4 View;

                out vec3 normal;
                void main()
                {
                    gl_Position = Projection * View * Model * vec4(aPos.x, aPos.y, aPos.z, 1.0);
                    normal = aNormal;
                }
            ";
    }
}
