using OpenTK.Mathematics;
using rt004.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace rt004.Hierarchy
{
    internal class SceneNode
    {
        public Vector3d Position { get; set; }
        public double Scale { get; set; }
        public Vector3d _Rotation;
        public Vector3d Rotation
        {
            get
            {
                return _Rotation * 180 / Math.PI;
            }
            set
            {
                _Rotation = value * Math.PI / 180.0;
            }
        }
        public Solid? Solid { get; set; }
        public List<SceneNode> Children { get; set; }

        public SceneNode()
        {
            Children = new List<SceneNode>();
            Position = Vector3d.Zero;
            _Rotation = Vector3d.Zero;
            Scale = 1;
        }

        public Matrix4d GetModelMatrix()
        {

            Matrix4d modelMatrix = Matrix4d.Identity;

            modelMatrix *= Matrix4d.Scale(Scale);

            modelMatrix *= Matrix4d.CreateRotationX(_Rotation.X);
            modelMatrix *= Matrix4d.CreateRotationY(_Rotation.Y);
            modelMatrix *= Matrix4d.CreateRotationZ(_Rotation.Z);

            modelMatrix *= Matrix4d.CreateTranslation(Position);
            return modelMatrix;
        }
    }

}
