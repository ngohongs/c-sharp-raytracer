using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal struct Ray
    {
        public Vector3d o, d;
        public Ray(Vector3d origin, Vector3d direction)
        {
            this.o = origin;
            this.d = direction;
        }

        public Vector3d At(double t)
        {
            return o + t * d;
        }
    }
}
