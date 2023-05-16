using OpenTK.Mathematics;
using rt004.Lights;
using rt004.Rays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Brdfs
{
    [JsonDerivedType(typeof(Phong), typeDiscriminator: "phong")]
    internal abstract class Brdf
    {
        public abstract Vector3d Shade(RayHit hit, Light light, bool inShadow = false);
    }
}
