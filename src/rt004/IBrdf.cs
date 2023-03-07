using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal interface IBrdf
    {
        public Vector3d Shade(RayHit hit);
    }
}
