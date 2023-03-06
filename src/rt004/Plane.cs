using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal class Plane : Solid
    {
        private Vector3d normal;
        public Plane(Vector3d position, Vector3d normal)
        {
            base.position = position;
            this.normal = normal.Normalized();
        }

        public override bool Intersect(Ray ray, ref Vector3d rayHit, ref Vector3d surfaceNormal)
        {
            double denom = Vector3d.Dot(normal, ray.d);
            if (Math.Abs(denom) > 1e-6)
            {
                Vector3d numer = position - ray.o;
                double t = Vector3d.Dot(numer, normal) / denom;

                rayHit = ray.At(t);
                surfaceNormal = normal;

                return t > 0;
            }

            return false;
        }
    }
}
