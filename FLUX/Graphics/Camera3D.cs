using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FLUX.Graphics
{
    public class Camera3D
    {
        public Vector3 Position { get; set; } = new Vector3(0,0,0);
        public Vector3 Target { get; set; } = Vector3.UnitX;
        public Vector3 Up { get; set; } = Vector3.UnitY;
        public Quaternion Rotation { get; set; } = new Quaternion(0, 0, 0, 0);
        public float FOV { get; set; } = 50f;
        public float FarClip { get; set; } = 1000f;
        public float NearClip { get; set; } = 0.001f;

        public Vector2 ScreenDimensions { get; set; } = new Vector2(800, 600);

        public Matrix4x4 ProjectionMatrix 
        { 
            get
            {
                return Matrix4x4.CreatePerspectiveFieldOfView(FOV, ScreenDimensions.X / ScreenDimensions.Y, NearClip, FarClip);
            }
        }

        public Matrix4x4 ViewMatrix
        {
            get
            {
                return Matrix4x4.CreateLookAt(Position, Target, Up);
            }
        }

        public Matrix4x4 ProjectionViewMatrix
        {
            get
            {
                return ProjectionMatrix * ViewMatrix;
            }
        }

        public Camera3D(Vector3 position, Vector3 target, Quaternion rotation, float fOV, Vector2 screenDimensions)
        {
            Position = position;
            Target = target;
            Rotation = rotation;
            FOV = fOV;
            ScreenDimensions = screenDimensions;
        }
    }
}
