using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004.Lights
{
    internal class PointLight : Light
    {
        public PointLight(Vector3d position, Vector3d Ka, Vector3d Kd, Vector3d Ks)
            : base(position, Ka, Kd, Ks)
        {}

        public override Vector3d GetDirection(Vector3d point)
        {
            return position - point;
        }
    }
}
