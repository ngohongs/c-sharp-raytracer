using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004
{
    [JsonDerivedType(typeof(Sphere), typeDiscriminator: "sphere")]
    [JsonDerivedType(typeof(Plane), typeDiscriminator: "plane")]
    internal abstract class Solid
    {
        [JsonInclude]
        public Material material;
        [JsonInclude]
        public Vector3d position;
        public Solid(Vector3d position, Material material)
        {
            this.position = position;
            this.material = material;
        }
            
        public abstract bool Intersect(Ray ray, ref RayHit hit);
    }
}
