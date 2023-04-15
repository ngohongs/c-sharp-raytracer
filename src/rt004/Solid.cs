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
        public Material material;
        [JsonInclude]
        public string materialName;
        [JsonInclude]
        public Vector3d position;
        public Solid(Vector3d position, string materialName)
        {
            this.position = position;
            this.materialName = materialName;
        }
            
        public abstract bool Intersect(Ray ray, ref RayHit hit);

        public virtual bool IsHollow() => true;
    }
}
