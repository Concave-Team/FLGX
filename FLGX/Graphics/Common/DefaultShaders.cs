using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics.Common
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
        public static string DefaultGLSLShaders3D_FS =
            @"
                #version 330 core
                out vec4 FragColor;

                uniform sampler2D tex0;
                in vec2 texCoord;

                void main()
                {
                    FragColor = texture(tex0, texCoord);
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

        public static string DefaultGLSLShaders3D_VS =
            @"
                #version 330 core
                layout (location = 0) in vec3 aPos;
                layout (location = 1) in vec3 aNormal;
                layout (location = 2) in vec2 aTexCoord;

                uniform mat4 Model;
                uniform mat4 Projection;
                uniform mat4 View;

                out vec2 texCoord;
                void main()
                {
                    gl_Position = Projection * View * Model * vec4(aPos.x, aPos.y+(gl_InstanceID*2.3), aPos.z, 1.0);
                    texCoord = aTexCoord;
                }
            ";
        public static string DefaultGLSLShadersOSR_VS =
            @"
                #version 330 core
                layout (location = 0) in vec3 aPos;
                layout (location = 1) in vec3 aNormal;
                layout (location = 2) in vec2 aTexCoord;

                out vec2 texCoord;

                void main()
                {
                    gl_Position = vec4(aPos.x, aPos.y, 0.0, 1.0);
                    texCoord = aTexCoord;
                }
            ";
        public static string DefaultGLSLShadersOSR_FS =
        @"
            #version 330 core
            out vec4 FragColor;
            in vec2 texCoord;

            uniform sampler2D screenTex;

            void main()
            {
                FragColor = texture(screenTex, texCoord);
            } 
        ";
    }
}
