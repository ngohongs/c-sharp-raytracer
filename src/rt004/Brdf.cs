using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004
{
    [JsonDerivedType(typeof(Phong), typeDiscriminator: "phong")]
    internal abstract class Brdf
    {
        public abstract Vector3d Shade(RayHit hit, Light light, bool inShadow = false);
    }
}
