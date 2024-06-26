﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flgx
{
    public static class FLGXUtils
    {
        public static OpenTK.Mathematics.Vector3 ToOTKVec3(this System.Numerics.Vector3 v)
        {
            return new OpenTK.Mathematics.Vector3(v.X, v.Y, v.Z);
        }

        public static OpenTK.Mathematics.Vector4 ToOTKVec4(this System.Numerics.Vector4 v)
        {
            return new OpenTK.Mathematics.Vector4(v.X, v.Y, v.Z, v.W);
        }

        public static OpenTK.Mathematics.Vector2 ToOTKVec2(this System.Numerics.Vector2 v)
        {
            return new OpenTK.Mathematics.Vector2(v.X, v.Y);
        }

        public static Vector4 NormalizeColor(Vector4 color)
            => new Vector4(color.X / 255, color.Y / 255, color.Z / 255, color.W / 255);

        public static Matrix4 ToOTKMat4(this System.Numerics.Matrix4x4 source)
        {
            return new Matrix4(
                source.M11, source.M12, source.M13, source.M14,
                source.M21, source.M22, source.M23, source.M24,
                source.M31, source.M32, source.M33, source.M34,
                source.M41, source.M42, source.M43, source.M44
            );
        }

        public static System.Numerics.Vector2 ToSNV2(this OpenTK.Mathematics.Vector2 vec)
        {
            return new System.Numerics.Vector2(vec.X, vec.Y);
        }

        public static System.Numerics.Vector2 ToSNV2(this OpenTK.Mathematics.Vector2i vec)
        {
            return new System.Numerics.Vector2(vec.X, vec.Y);
        }

        public static System.Numerics.Vector3 ToSNV2(this Assimp.Vector3D vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
        }

        public static Silk.NET.Maths.Vector2D<int> ToSilkInt(this System.Numerics.Vector2 vec)
        {
            return new Silk.NET.Maths.Vector2D<int>((int)vec.X, (int)vec.Y);
        }

        public static System.Numerics.Vector2 ToSilk2D(this Silk.NET.Maths.Vector2D<int> vec)
        {
            return new System.Numerics.Vector2(vec.X, vec.Y);
        }

        public static Silk.NET.Maths.Vector2D<float> ToSilkFloat(this System.Numerics.Vector2 vec)
        {
            return new Silk.NET.Maths.Vector2D<float>(vec.X, vec.Y);
        }
    }
}
