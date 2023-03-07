using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    public static class VectorExtensionMethods
    {
        public static Vector3d Reflect(this Vector3d vector, Vector3d normal)
        {
            return 2.0 * Vector3d.Dot(normal, vector) * normal - vector;
        }

        public static Vector3d Refract(this Vector3d vector, Vector3d normal, double eta)
        {
            double nDotV = Vector3d.Dot(normal, vector);
            double k = 1.0d - eta * eta * (1.0d - nDotV * nDotV);
            if (k < 0.0d)
                return Vector3d.Zero;
            return eta * vector - (eta * nDotV * Math.Sqrt(k)) * normal;
        }
    }
}
