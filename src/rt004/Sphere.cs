using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal class Sphere : Solid
    {
        private double r;

        public Sphere(Vector3d position, double radius, Material material) : base(position, material)
        {
            this.r = radius;
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            double a = Vector3d.Dot(ray.d, ray.d);
            double b = 2.0d * Vector3d.Dot(ray.o - position, ray.d);
            double c = Vector3d.Dot(ray.o - position, ray.o - position) - r * r;

            double d = b * b - 4 * a * c;
            
            if (d < 0)
                return false;

            double x1 = (-b - Math.Sqrt(d)) / (2 * a);
            double x2 = (-b + Math.Sqrt(d)) / (2 * a);

            hit.position = x1 < x2 ? ray.At(x1) : ray.At(x2);
            hit.normal = (hit.position - position).Normalized();
            hit.solid = this;

            return x1 > 0 && x2 > 0;
        }
    }
}
