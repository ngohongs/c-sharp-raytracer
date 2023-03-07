using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal class Phong : IBrdf
    {
        public Vector3d Shade(RayHit hit)
        {
            Vector3d toLight = Raytracer.light.GetDirection(hit.position);
            Vector3d a = hit.solid.material.Ka * Raytracer.light.Ka;

            Vector3d d = Vector3d.Zero;
            double fd = Vector3d.Dot(hit.normal, toLight);
            if (fd > 0)
                d = hit.solid.material.Kd * Raytracer.light.Kd * fd;

            Vector3d s = Vector3d.Zero;
            Vector3d r = toLight.Reflect(hit.normal).Normalized();
            Vector3d v = (Raytracer.camera.GetPosition() - hit.position).Normalized();
            double fs = Vector3d.Dot(r, v);
            if (fs > 0)
                s = hit.solid.material.Ks * Raytracer.light.Ks * Math.Pow(fs, hit.solid.material.Ns);

            return a + d + s;
        }
    }
}
