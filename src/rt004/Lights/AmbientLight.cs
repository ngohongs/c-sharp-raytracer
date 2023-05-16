using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004.Lights
{
    internal class AmbientLight : Light
    {
        public AmbientLight(Vector3d Ka)
            : base(Vector3d.Zero, Ka, Vector3.Zero, Vector3.Zero)
        { }

        public override Vector3d GetDirection(Vector3d point)
        {
            return -position;
        }

    }
}
