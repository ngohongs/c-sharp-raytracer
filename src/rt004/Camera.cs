using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector3d = OpenTK.Mathematics.Vector3d;
using Vector4d = OpenTK.Mathematics.Vector4d;

namespace rt004
{
    internal class Camera
    {
        [JsonInclude]
        public Vector3d position;
        [JsonInclude]
        public Vector3d forward;
        [JsonInclude]
        public Vector3d up;

        private double _fov;        

        [JsonInclude]
        public double fov { get => _fov / Math.PI * 180; set => _fov = value / 180 * Math.PI; }

        [JsonInclude]
        public double height;
        [JsonInclude]
        public double width;

        private Matrix4d inverseViewMatrix;
        public Camera(Vector3d position, Vector3d forward, Vector3d up, double fov, double width, double height)
        {
            this.position = position;
            this.forward = forward.Normalized();
            this.up = up.Normalized();

            this._fov = fov * Math.PI / 180.0d;
            this.height = height;
            this.width = width;

            Vector3d right = Vector3d.Cross(forward, up).Normalized();

            inverseViewMatrix = new Matrix4d();
            inverseViewMatrix.Column0 = new Vector4d(right, 0.0d);
            inverseViewMatrix.Column1 = new Vector4d(up, 0.0d);
            inverseViewMatrix.Column2 = new Vector4d(-forward, 0.0d);
            inverseViewMatrix.Column3 = new Vector4d(-Vector3d.Dot(right, position), -Vector3d.Dot(up, position), Vector3d.Dot(forward, position), 1.0d);
            inverseViewMatrix.Invert();
        }

        public List<List<float[]>> Render()
        {
            List<List<float[]>> framebuffer = new List<List<float[]>>((int) width);

            // Background
            for (int x = 0; x < framebuffer.Capacity; x++)
            {
                framebuffer.Insert(x, new List<float[]>((int)height));
                for (int y = 0; y < framebuffer[x].Capacity; y++)
                {
                    float t = (float)(y / height);
                    Vector3d white = new Vector3(1);
                    Vector3d lightBlue = new Vector3(121.0f / 255.0f, 190.0f / 255.0f, 219.0f / 255.0f);
                    Vector3 f = (Vector3)((1.0f - t) * lightBlue + t * white);
                    framebuffer[x].Insert(y, new float[] { f.X, f.Y, f.Z });
                }
            }

            for (int y = 0; y < (int) height; y++)
            {
                for (int x = 0; x < (int) width; x++)
                {
                    Ray ray = CastRay(x, y);

                    Vector4d color = Trace(ray, 1);

                    if (color.W != 1)
                        continue;

                    Vector3 floatColor = (Vector3)color.Xyz;

                    framebuffer[x][y] = new float[] { floatColor.X, floatColor.Y, floatColor.Z };
                }
            }
            return framebuffer;
        }

        public Vector4d Trace(Ray ray, int depth)
        {
            RayHit hit = new RayHit();

            if (!Raytracer.scene.Intersect(ray, ref hit))
                return Vector4d.Zero;

            Vector3d color = Raytracer.brdf.Shade(hit, Raytracer.ambientLight, true);
            foreach (Light light in Raytracer.lights)
            {
                Ray shadowRay = new Ray(hit.position, light.GetDirection(hit.position));
                RayHit shadowHit = new RayHit();

                color += Raytracer.brdf.Shade(hit, light);
            }

            if (depth >= Raytracer.MAX_DEPTH)
                return new Vector4d(color, 1.0d);


            //if (hit.solid.material.IsGlossy())
            //{
            //    Vector3d r = ray.d.Reflect(hit.normal).Normalized();
            //    Ray reflectionRay = new Ray(hit.position, r);
            //    color += hit.solid.material.Kr * Trace(reflectionRay, depth + 1).Xyz;
            //}

            // TODO
            //if (hit.solid.material.IsTransparent())
            //{
            //    Vector3 t = 
            //}

            return new Vector4d(color, 1.0d); 
        }

        private Ray CastRay(double x, double y)
        {
            double aspectRatio = width / height;
            double px = (2 * ((x + 0.5) / width) - 1) * Math.Tan(_fov / 2);
            double py = (1 - 2 * ((y + 0.5) / height)) * Math.Tan(_fov / 2);

            if (width > height)
                px *= aspectRatio;
            else
                py *= 1.0d / aspectRatio;
            
            Vector4d rayOrigin = new Vector4d(0.0d, 0.0d, 0.0d, 1.0d);
            Vector4d rayDirection = new Vector4d(px, py, -1.0d, 0.0d);

            Vector3d rayWorldOrigin = (inverseViewMatrix * rayOrigin).Xyz;
            Vector3d rayWorldDirection = (inverseViewMatrix * rayDirection).Xyz.Normalized();

            return new Ray(rayWorldOrigin, rayWorldDirection);

        }

        public Vector3d GetPosition() { return position; }
    }
}
