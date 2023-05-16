using OpenTK.Mathematics;
using rt004.Rays;
using rt004.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Hierarchy
{
    internal class Scene
    {
        public List<Solid> solids = new List<Solid>();
        [JsonInclude]
        public SceneNode root = new SceneNode();

        public void Add(Solid solid)
        {
            solids.Add(solid);
        }

        public bool SetSolidMaterials()
        {
            try
            {
                foreach (Solid solid in solids)
                {
                    solid.material = Raytracer.materials[solid.materialName];
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Intersect(Ray ray, ref RayHit hit)
        {
            RayHit temp = new RayHit();
            RayHit closest = new RayHit();
            bool hitSomething = false;
            double closestDistance = double.MaxValue;

            foreach (Solid solid in solids)
            {
                if (!solid.Intersect(ray, ref temp))
                    continue;

                hitSomething = true;

                double tempDistance = Vector3d.Distance(temp.position, Raytracer.camera.GetPosition());

                if (closestDistance > tempDistance)
                {
                    closest = temp;
                    closestDistance = tempDistance;

                }
            }
            hit = closest;
            return hitSomething;
        }

        private List<Solid> TransformedSolids(SceneNode root, Matrix4d parentTransform)
        {
            List<Solid> result = new List<Solid>();

            Matrix4d rootModelMatrix = root.GetModelMatrix() * parentTransform;

            if (root.Solid != null)
                result.Add(root.Solid.Transformed(rootModelMatrix));

            foreach (var child in root.Children)
            {
                result.AddRange(TransformedSolids(child, rootModelMatrix));
            }

            return result;
        }

        public void TransformToDirect()
        {
            solids = TransformedSolids(root, Matrix4d.Identity);
        }
    }
}
