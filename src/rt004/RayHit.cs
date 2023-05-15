using OpenTK.Mathematics;
using rt004.Solids;
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

        public RayHit Clone()
        {
            return new RayHit
            {
                solid = solid,
                position = position,
                normal = normal,
                backface = backface
            };
        }
    }
}
