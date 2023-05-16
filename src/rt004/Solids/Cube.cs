using OpenTK.Mathematics;
using rt004.Rays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Solids
{
    internal class Cube : Solid
    {
        [JsonInclude]
        public double a;

        private List<Triangle> triangles;

        [JsonConstructor]
        public Cube(Vector3d position, double a, string materialName) : base(position, materialName)
        {
            this.a = a;
            GenerateTriangles(position, a);
        }

        public Cube(Vector3d position, List<Triangle> triangles, double a, string materialName) : base(position, materialName)
        {
            this.a = a;
            this.triangles = new List<Triangle>(triangles);
        }

        public void GenerateTriangles(Vector3d center, double sideLength)
        {
            triangles = new List<Triangle>();

            // Define the eight vertices of the cube
            Vector3d frontBottomLeft = new Vector3d(center.X - sideLength / 2, center.Y - sideLength / 2, center.Z + sideLength / 2);
            Vector3d frontBottomRight = new Vector3d(center.X + sideLength / 2, center.Y - sideLength / 2, center.Z + sideLength / 2);
            Vector3d frontTopLeft = new Vector3d(center.X - sideLength / 2, center.Y + sideLength / 2, center.Z + sideLength / 2);
            Vector3d frontTopRight = new Vector3d(center.X + sideLength / 2, center.Y + sideLength / 2, center.Z + sideLength / 2);
            Vector3d backBottomLeft = new Vector3d(center.X - sideLength / 2, center.Y - sideLength / 2, center.Z - sideLength / 2);
            Vector3d backBottomRight = new Vector3d(center.X + sideLength / 2, center.Y - sideLength / 2, center.Z - sideLength / 2);
            Vector3d backTopLeft = new Vector3d(center.X - sideLength / 2, center.Y + sideLength / 2, center.Z - sideLength / 2);
            Vector3d backTopRight = new Vector3d(center.X + sideLength / 2, center.Y + sideLength / 2, center.Z - sideLength / 2);

            // Define the triangles to cover the cube
            Triangle[] cubeTriangles = new Triangle[] {
                // Front face
                new Triangle(frontBottomLeft, frontTopLeft, frontBottomRight, materialName),
                new Triangle(frontBottomRight, frontTopLeft, frontTopRight, materialName),
                // Right face
                new Triangle(frontBottomRight, backTopRight, backBottomRight, materialName),
                new Triangle(frontBottomRight, frontTopRight, backTopRight, materialName),
                // Back face
                new Triangle(backBottomRight, backTopLeft, backBottomLeft, materialName),
                new Triangle(backBottomRight, backTopRight, backTopLeft, materialName),
                // Left face
                new Triangle(frontBottomLeft, backBottomLeft, backTopLeft, materialName),
                new Triangle(frontBottomLeft, backTopLeft, frontTopLeft, materialName),
                // Bottom face
                new Triangle(frontBottomLeft, backBottomRight, frontBottomRight, materialName),
                new Triangle(frontBottomLeft, backBottomLeft, backBottomRight, materialName),
                // Top face
                new Triangle(frontTopLeft, backTopRight, frontTopRight, materialName),
                new Triangle(frontTopLeft, backTopLeft, backTopRight, materialName)
            };

            triangles.AddRange(cubeTriangles);
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            int hitCount = 0;
    
            RayHit tmpHit = new RayHit();

            double distance = double.MaxValue;

            foreach (Triangle triangle in triangles)
            {
                if (triangle.Intersect(ray, ref tmpHit))
                {
                    hitCount++;
                    double d = Vector3d.Distance(ray.o, tmpHit.position);
                    if (d < distance)
                    {
                        distance = d;
                        hit = tmpHit.Clone();
                    }
                }
            }

            if (hitCount <= 0)
                return false;

            hit.backface = hitCount < 2;
            hit.solid = this;

            return true;
        }

        public override Solid Transformed(Matrix4d modelMatrix)
        {
            Vector3d position = Vector3d.TransformPosition(this.position, modelMatrix);

            List<Triangle> newTriangles = new List<Triangle>();
            foreach (Triangle triangle in triangles)
            {
                newTriangles.Add((Triangle)triangle.Transformed(modelMatrix));
            }

            double a = Vector3d.Distance(Vector3d.TransformPosition(this.position + new Vector3d(this.a, 0, 0), modelMatrix), position);

            return new Cube(position, newTriangles, a, materialName);
        }
    }
}
