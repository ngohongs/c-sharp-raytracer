using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal class Ray
    {
        public Vector3d o, d;
        public Stack<double> s;
        public Ray(Vector3d origin, Vector3d direction, Stack<double>? space = null)
        {
            this.o = origin;
            this.d = direction;
            s = space != null ? space : new Stack<double>();
        }

        public Vector3d At(double t)
        {
            return o + t * d;
        }
    }
}
