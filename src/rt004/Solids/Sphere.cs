using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Solids
{
    internal class Sphere : Solid
    {
        [JsonInclude]
        public double radius;

        public Sphere(Vector3d position, double radius, string materialName) : base(position, materialName)
        {
            this.radius = radius;
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            double a = Vector3d.Dot(ray.d, ray.d);
            double b = 2.0d * Vector3d.Dot(ray.o - position, ray.d);
            double c = Vector3d.Dot(ray.o - position, ray.o - position) - radius * radius;

            double d = b * b - 4 * a * c;

            if (d < 0)
                return false;

            double x1 = (-b - Math.Sqrt(d)) / (2 * a);
            double x2 = (-b + Math.Sqrt(d)) / (2 * a);

            if (x1 <= 1.0e-6 && x2 <= 1.0e-6)
                return false;

            hit.position = x1 > 1.0e-6 ? ray.At(x1) : ray.At(x2);
            hit.normal = x1 <= 1.0e-6 ? -(hit.position - position).Normalized() : (hit.position - position).Normalized();
            hit.solid = this;
            hit.backface = x1 <= 1.0e-6;

            return true;
        }

        public override Solid Transformed(Matrix4d modelMatrix)
        {
            Vector3d position = Vector3d.TransformPosition(this.position, modelMatrix);
            double radius = Vector3d.Distance(Vector3d.TransformPosition(this.position + new Vector3d(this.radius, 0, 0), modelMatrix), position);

            return new Sphere(position, radius, materialName);
        }
    }
}
