using OpenTK.Mathematics;
using rt004.Rays;
using System.Text.Json.Serialization;

namespace rt004.Solids
{
    internal class Plane : Solid
    {
        [JsonInclude]
        public Vector3d normal;

        public Plane(Vector3d position, Vector3d normal, string materialName) : base(position, materialName)
        {
            this.normal = normal.Normalized();
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            double denom = Vector3d.Dot(normal, ray.d);
            if (Math.Abs(denom) > 1e-6)
            {
                Vector3d numer = position - ray.o;
                double t = Vector3d.Dot(numer, normal) / denom;

                hit.position = ray.At(t);
                hit.normal = denom < 0 ? normal : -normal;
                hit.solid = this;
                hit.backface = false;

                return t > 1.0e-6;
            }

            return false;
        }

        public override bool IsHollow() => false;

        public override Solid Transformed(Matrix4d modelMatrix)
        {
            Vector3d position = Vector3d.TransformPosition(this.position, modelMatrix);
            Vector3d normal = Vector3d.TransformNormal(this.normal, modelMatrix).Normalized();
            return new Plane(position, normal, materialName);
        }

    }
}
