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
    internal class Triangle : Solid
    {
        [JsonInclude]
        public Vector3d v0;
        [JsonInclude]
        public Vector3d v1;
        [JsonInclude]
        public Vector3d v2;

        public Triangle(Vector3d v0, Vector3d v1, Vector3d v2, string materialName) : base((v0 + v1 + v2) / 3.0d, materialName)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }

        public override bool IsHollow() => false;

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            Vector3d v0v1 = v1 - v0;
            Vector3d v0v2 = v2 - v0;
            Vector3d pVec = Vector3d.Cross(ray.d, v0v2);
            double det = Vector3d.Dot(v0v1, pVec);

            if (Math.Abs(det) < 1.0e-6)
                return false;

            double invDet = 1.0d / det;

            Vector3d tVec = ray.o - v0;
            double u = Vector3d.Dot(tVec, pVec) * invDet;
            if (u < 0 || u > 1)
                return false;

            Vector3d qVec = Vector3d.Cross(tVec, v0v1);
            double v = Vector3d.Dot(ray.d, qVec) * invDet;
            if (v < 0 || u + v > 1)
                return false;

            double t = Vector3d.Dot(v0v2, qVec) * invDet;

            if (t <= 1.0e-6)
                return false;

            Vector3d normal = Vector3d.Cross(v0v1, v0v2).Normalized();

            if (Vector3d.Dot(normal, ray.d.Normalized()) > 0)
                normal = -normal;

            hit.position = ray.At(t);
            hit.normal = normal;
            hit.solid = this;
            hit.backface = false;
            return true;
        }

        public override Solid Transformed(Matrix4d modelMatrix)
        {
            Vector3d v0 = Vector3d.TransformPosition(this.v0, modelMatrix);
            Vector3d v1 = Vector3d.TransformPosition(this.v1, modelMatrix);
            Vector3d v2 = Vector3d.TransformPosition(this.v2, modelMatrix);

            return new Triangle(v0, v1, v2, materialName);
        }
    }
}
