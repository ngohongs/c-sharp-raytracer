using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004
{
    internal class Sphere : Solid
    {
        [JsonInclude]
        public double radius;

        public Sphere(Vector3d position, double radius, Material material) : base(position, material)
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

            hit.position = x1 < x2 ? ray.At(x1) : ray.At(x2);
            hit.normal = (hit.position - position).Normalized();
            hit.solid = this;

            return x1 > 1.0e-6 && x2 > 1.0e-6;
        }
    }
}
