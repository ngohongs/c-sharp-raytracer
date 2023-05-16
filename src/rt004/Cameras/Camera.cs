using OpenTK.Mathematics;
using rt004.Helpers;
using rt004.Lights;
using rt004.Rays;
using rt004.Solids;
using System;
using System.Collections;
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

namespace rt004.Cameras
{
    [JsonDerivedType(typeof(PerspectiveCamera), typeDiscriminator: "perspective")]
    internal abstract class Camera
    {
        [JsonInclude]
        public Vector3d position;
        [JsonInclude]
        public Vector3d forward;
        [JsonInclude]
        public Vector3d up;

        [JsonInclude]
        public double height;
        [JsonInclude]
        public double width;

        protected Matrix4d inverseViewMatrix;

        public Camera(Vector3d position, Vector3d forward, Vector3d up, double width, double height)
        {
            this.position = position;

            this.forward = forward.Normalized();
            this.up = up.Normalized();
            Vector3d right = Vector3d.Cross(forward, up).Normalized();

            this.height = height;
            this.width = width;

            inverseViewMatrix = new Matrix4d();
            inverseViewMatrix.Column0 = new Vector4d(right, 0.0d);
            inverseViewMatrix.Column1 = new Vector4d(up, 0.0d);
            inverseViewMatrix.Column2 = new Vector4d(-forward, 0.0d);
            inverseViewMatrix.Column3 = new Vector4d(-Vector3d.Dot(right, position), -Vector3d.Dot(up, position), Vector3d.Dot(forward, position), 1.0d);
            inverseViewMatrix.Invert();
        }

        public Framebuffer Render()
        {
            Framebuffer framebuffer = new((int)width, (int)height);

            // Annouce render with resolution and samples and depth
            Console.WriteLine($"Rendering {width}x{height} image with {Raytracer.SAMPLES_K} samples and {Raytracer.MAX_DEPTH} depth");

            Stopwatch sw = Stopwatch.StartNew();

            //Sequential
            //for (int j = 0; j < (int)height; j += Raytracer.CHUNK_SIZE)
            //{
            //    for (int i = 0; i < (int)width; i += Raytracer.CHUNK_SIZE)
            //    {
            //        for (int y = j; y < j + Raytracer.CHUNK_SIZE && y < height; y++)
            //        {
            //            for (int x = i; x < i + Raytracer.CHUNK_SIZE && x < width; x++)
            //            {
            //                framebuffer.SetPixel(x, y, RenderPixel(x, y));
            //            }
            //        }
            //    }
            //}

            // Parallel
            var tasks = new List<Task>();
            for (int j = 0; j < (int)height; j += Raytracer.CHUNK_SIZE)
            {
                for (int i = 0; i < (int)width; i += Raytracer.CHUNK_SIZE)
                {
                    tasks.Add(RunRenderChunkTask(i, j, framebuffer));
                }
            }

            // Wait for all tasks to finish
            Task.WaitAll(tasks.ToArray());

            sw.Stop();
            Console.WriteLine($"Render time: {sw.ElapsedMilliseconds} ms");

            return framebuffer;
        }

        private Task RunRenderChunkTask(int i, int j, Framebuffer framebuffer)
        {
            return Task.Run(() =>
            {
                for (int y = j; y < j + Raytracer.CHUNK_SIZE && y < height; y++)
                {
                    for (int x = i; x < i + Raytracer.CHUNK_SIZE && x < width; x++)
                    {
                        framebuffer.SetPixel(x, y, RenderPixel(x, y));
                    }
                }
            });
        }

        private float[] RenderPixel(int x, int y)
        {
            Random random = new Random();

            Vector4d color = Vector4d.Zero;

            if (Raytracer.SAMPLES_K == 1)
            {
                Ray ray = CastRay(x, y);
                color += Trace(ray, 1);
            }
            else
            {
                double step = 1.0d / Raytracer.SAMPLES_K;

                for (int i = 0; i < Raytracer.SAMPLES_K; i++)
                {
                    for (int j = 0; j < Raytracer.SAMPLES_K; j++)
                    {
                        double dx = random.NextDouble() * step + step * i - 0.5d;
                        double dy = random.NextDouble() * step + step * j - 0.5d;
                        Ray ray = CastRay(x + dx, y + dy);
                        color += Trace(ray, 1);
                    }
                }
                color = color / (Raytracer.SAMPLES_K * Raytracer.SAMPLES_K);
            }

            Vector3 floatColor = (Vector3)color.Xyz;

            return new float[] { floatColor.X, floatColor.Y, floatColor.Z };
        }

        public Vector4d Trace(Ray ray, int depth)
        {
            RayHit hit = new RayHit();

            if (!Raytracer.scene.Intersect(ray, ref hit))
                return new Vector4d(Raytracer.background, 1.0);

            Vector3d color = Raytracer.brdf.Shade(hit, Raytracer.ambientLight, true);
            foreach (Light light in Raytracer.lights)
            {
                Ray shadowRay = new Ray(hit.position, light.GetDirection(hit.position));
                RayHit shadowHit = new RayHit();

                bool inShadow = Raytracer.scene.Intersect(shadowRay, ref shadowHit);

                if (!inShadow)
                    color += Raytracer.brdf.Shade(hit, light);
            }

            if (depth >= Raytracer.MAX_DEPTH)
                return new Vector4d(color, 1.0d);

            if (hit.solid.material.IsGlossy())
            {
                Vector3d r = ray.d.Reflect(hit.normal).Normalized();
                Ray reflectionRay = new Ray(hit.position, r);
                color += hit.solid.material.Kr * Trace(reflectionRay, depth + 1).Xyz;
            }

            if (hit.solid.material.IsTransparent())
            {
                Vector3d t = ray.d.Refract(hit.normal, GetETA(ray, hit));
                Ray refrationRay = new Ray(hit.position, t.Normalized(), ray.s.Clone());
                color += hit.solid.material.Kt * Trace(refrationRay, depth + 1).Xyz;
            }

            return new Vector4d(color, 1.0d);
        }

        protected abstract Ray CastRay(double x, double y);

        private double GetETA(Ray ray, RayHit hit)
        {
            double iorTo = hit.solid.material.Ior;
            double iorFrom = ray.s.Count > 0 ? ray.s.First() : 1; // 1 is air

            if (hit.solid.IsHollow())
            {
                if (hit.backface)
                {
                    iorFrom = ray.s.TryPop(out double iorInside) ? iorInside : hit.solid.material.Ior;
                    iorTo = ray.s.Count > 0 ? ray.s.First() : 1; // 1 is air
                }
                else
                {
                    ray.s.Push(hit.solid.material.Ior);
                }
            }

            return iorFrom / iorTo;
        }

        public Vector3d GetPosition() { return position; }
    }
}
