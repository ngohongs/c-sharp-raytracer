using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal struct Material
    {
        public Vector3d Ka;
        public Vector3d Kd;
        public Vector3d Ks;
        public double Ns;
        public Material(Vector3d Ka, Vector3d Kd, Vector3d Ks, double Ns)
        {
            this.Ka = Ka;
            this.Kd = Kd;
            this.Ks = Ks;
            this.Ns = Ns;
        }
    }
}
