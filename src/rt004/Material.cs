using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004
{
    internal struct Material
    {
        [JsonInclude]
        public Vector3d Ka;
        [JsonInclude]
        public Vector3d Kd;
        [JsonInclude]
        public Vector3d Ks;
        [JsonInclude]
        public double Ns;
        [JsonInclude]
        public double Kr;
        [JsonInclude]
        public double Kt;
        public Material(Vector3d Ka, Vector3d Kd, Vector3d Ks, double Ns, double Kr = 0, double Kt = 0)
        {
            this.Ka = Ka;
            this.Kd = Kd;
            this.Ks = Ks;
            this.Ns = Ns;
            this.Kr = Kr;
            this.Kt = Kt;
        }

        public bool IsGlossy() { return Kr > 0; }
        public bool IsTransparent() { return Kt > 0; }
    }
}
