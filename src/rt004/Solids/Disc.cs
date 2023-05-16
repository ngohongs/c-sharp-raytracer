using OpenTK.Mathematics;
using rt004.Rays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Solids
{
    internal class Disc : Solid
    {
        [JsonInclude]
        public double radius;
        [JsonInclude]
        public Vector3d normal;

        public Disc(Vector3d position, double radius, Vector3d normal, string materialName) : base(position, materialName)
        {
            this.radius = radius;
            this.normal = normal.Normalized();
        }

        public override bool IsHollow() => false;

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            Solid diskPlane = new Plane(position, normal, materialName);
            if (!diskPlane.Intersect(ray, ref hit))
                return false;

            if (Vector3d.Distance(hit.position, position) > radius)
                return false;

            hit.solid = this;

            return true;
        }

        public override Solid Transformed(Matrix4d modelMatrix)
        {
            Vector3d position = Vector3d.TransformPosition(this.position, modelMatrix);
            Vector3d normal = Vector3d.TransformNormal(this.normal, modelMatrix).Normalized();
            double radius = Vector3d.Distance(Vector3d.TransformPosition(this.position + new Vector3d(this.radius, 0, 0), modelMatrix), position);
            return new Disc(position, radius, normal, materialName);

        }
    }
}
