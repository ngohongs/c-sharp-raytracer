using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector3d = OpenTK.Mathematics.Vector3d;
using Vector4d = OpenTK.Mathematics.Vector4d;

namespace rt004
{
    internal class Camera
    {
        private Vector3d position;
        private Vector3d forward;
        private Vector3d up;

        private double fov;
        private double height;
        private double width;

        private Matrix4d inverseViewMatrix;
        public Camera(Vector3d position, Vector3d forward, Vector3d up, double fov, double width, double height)
        {
            this.position = position;
            this.forward = forward.Normalized();
            this.up = up.Normalized();

            this.fov = fov * Math.PI / 180.0d;
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
            for (int i = 0; i < framebuffer.Capacity; i++)
            {
                framebuffer.Insert(i, new List<float[]>((int) height));
            }

            Sphere s = new Sphere(new Vector3d(0, 0, -5), 1.0d);
            s.material.Ka = new Vector3d(0.1d);
            s.material.Kd = new Vector3d(0.64d);
            s.material.Ks = new Vector3d(0.5d);
            s.material.Ns = 96.078431d;

            Plane p = new Plane(new Vector3d(0, 0, -2), new Vector3d(0, 2, 1));
            p.material.Ka = new Vector3d(0.1d);
            p.material.Kd = new Vector3d(0.5, 0, 0);
            p.material.Ks = new Vector3d(0.0, 1.0, 0);
            p.material.Ns = 100;


            Vector3d lightDirection = new Vector3d(0, 1, 0);
            Vector3d lightColor = new Vector3d(1, 1, 1);

            for (int y = 0; y < (int) height; y++)
            {
                for (int x = 0; x < (int) width; x++)
                {
                    float t = (float)(y / height);
                    Vector3d w = new Vector3(1);
                    Vector3d lb = new Vector3(121.0f / 255.0f, 190.0f / 255.0f, 219.0f / 255.0f);
                    Vector3 f = (Vector3)((1.0f - t) * lb + t * w);
                    framebuffer[x].Insert(y, new float[] { f.X, f.Y, f.Z, 1.0f });



                    Ray primaryRay = CastRay(x, y);
                    Vector3d hit = new Vector3d();
                    Vector3d normal = new Vector3d();
                    if (p.Intersect(primaryRay, ref hit, ref normal))
                    {
                        Vector3d a = p.material.Ka;

                        Vector3d d = p.material.Kd * lightColor * Vector3d.Dot(normal, lightDirection);
                        if (Vector3d.Dot(normal, lightDirection) < 0)
                            d = Vector3d.Zero;

                        Vector3d r = (2.0 * Vector3d.Dot(normal, lightDirection) * normal - lightDirection).Normalized();
                        Vector3d v = (position - hit).Normalized();
                        Vector3d l = p.material.Ks * lightColor * Math.Pow(Vector3d.Dot(r, v), p.material.Ns);
                        if (Vector3d.Dot(r, v) < 0)
                            l = Vector3d.Zero;

                        Vector3 e = (Vector3)(a + d + l);

                        framebuffer[x].Insert(y, new float[] { e.X, e.Y, e.Z, 1.0f });
                    }
                    if (s.Intersect(primaryRay, ref hit, ref normal))
                    {
                        Vector3d a = s.material.Ka;

                        Vector3d d = s.material.Kd * lightColor * Vector3d.Dot(normal, lightDirection);
                        if (Vector3d.Dot(normal, lightDirection) < 0)
                            d = Vector3d.Zero;

                        Vector3d r = (2.0 * Vector3d.Dot(normal, lightDirection) * normal - lightDirection).Normalized();
                        Vector3d v = (position - hit).Normalized();
                        Vector3d l = s.material.Ks * lightColor * Math.Pow(Vector3d.Dot(r, v), s.material.Ns);
                        if (Vector3d.Dot(r, v) < 0)
                            l = Vector3d.Zero;

                        Vector3 e = (Vector3)(a + d + l);

                        framebuffer[x].Insert(y, new float[] { e.X, e.Y, e.Z, 1.0f });
                    }
                }
            }
            return framebuffer;
        }

        private Ray CastRay(double x, double y)
        {
            double aspectRatio = width / height;
            double px = (2 * ((x + 0.5) / width) - 1) * Math.Tan(fov / 2);
            double py = (1 - 2 * ((y + 0.5) / height)) * Math.Tan(fov / 2);

            if (width > height)
                px *= aspectRatio;
            else
                py *= 1.0d / aspectRatio;
            


            Matrix4d t = Matrix4d.Identity;
            t.Column3 = new Vector4d(position, 1.0d);
            

            Vector4d rayOrigin = new Vector4d(0.0d, 0.0d, 0.0d, 1.0d);
            Vector4d rayDirection = new Vector4d(px, py, -1.0d, 0.0d);
            Vector3d rayWorldOrigin = (inverseViewMatrix * rayOrigin).Xyz;
            Vector3d rayWorldDirection = (inverseViewMatrix * rayDirection).Xyz.Normalized();
            return new Ray(rayWorldOrigin, rayWorldDirection);

        }
    }
}
