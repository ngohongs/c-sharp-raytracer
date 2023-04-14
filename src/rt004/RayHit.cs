using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal struct RayHit
    {
        public Solid solid;
        public Vector3d position;
        public Vector3d normal;
        public bool backface;
    }
}
