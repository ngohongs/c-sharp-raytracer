using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Materials
{
    internal class Material
    {
        protected Vector3d ka;
        [JsonInclude]
        public Vector3d Ka { get { return GetKa(); } }

        protected Vector3d kd;
        [JsonInclude]
        public Vector3d Kd { get { return GetKd(); } }

        protected Vector3d ks;
        [JsonInclude]
        public Vector3d Ks { get { return GetKs(); } }

        protected double ns;
        [JsonInclude]
        public double Ns { get { return GetNs(); } }

        protected double kr;
        [JsonInclude]
        public double Kr { get { return GetKr(); } }

        protected double kt;
        [JsonInclude]
        public double Kt { get { return GetKt(); } }

        protected double ior;
        [JsonInclude]
        public double Ior { get { return GetIor(); } }

        public Material(Vector3d Ka, Vector3d Kd, Vector3d Ks, double Ns, double Kr = 0, double Kt = 0, double Ior = 1)
        {
            ka = Ka;
            kd = Kd;
            ks = Ks;
            ns = Ns;
            kr = Kr;
            kt = Kt;
            ior = Ior;
        }

        public bool IsGlossy() { return Kr > 0; }

        public bool IsTransparent() { return Kt > 0; }


        public virtual Vector3d GetKa()
        {
            return ka;
        }

        public virtual Vector3d GetKd()
        {
            return kd;
        }

        public virtual Vector3d GetKs()
        {
            return ks;
        }
        public virtual double GetNs()
        {
            return ns;
        }

        public virtual double GetKr()
        {
            return kr;
        }

        public virtual double GetKt()
        {
            return kt;
        }

        public virtual double GetIor()
        {
            return ior;
        }
    }
}
