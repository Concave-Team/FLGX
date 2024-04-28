using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FLGX.Graphics.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FLVertex
    {
        public Vector3 Position;
        public Vector3 Normal = new Vector3(0,0,0);
        public Vector2 TexCoord = new Vector2(0,0);

        public FLVertex(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }

        public FLVertex(Vector3 position) : this()
        {
            Position = position;
        }
    }
}
