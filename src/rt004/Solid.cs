using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal abstract class Solid
    {
        public Material material;
        protected Vector3d position;
        public abstract bool Intersect(Ray ray, ref Vector3d rayHit, ref Vector3d surfaceNormal);
    }
}
