using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal abstract class Solid
    {
        public Material material;
        protected Vector3d position;
        public Solid(Vector3d position, Material material)
        {
            this.position = position;
            this.material = material;
        }
            
        public abstract bool Intersect(Ray ray, ref RayHit hit);
    }
}
