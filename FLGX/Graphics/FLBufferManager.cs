using flgx.Graphics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx.Graphics
{
    public static class FLBufferManager
    {
        public static List<FLBuffer> buffers = new List<FLBuffer>();

        public static void RegisterBuffer(FLBuffer buffer) { buffers.Add(buffer); }
        public static void UnregisterBuffer(FLBuffer buffer) { buffers.Remove(buffer); }

        public static void ClearBuffers() 
        { 
            foreach (var buf in buffers) 
            { 
                buf.DestroyBuffer(); 
            } 
        }
    }
}
