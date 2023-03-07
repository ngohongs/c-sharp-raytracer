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
            Vector3d toLight = RayTracer.light.GetDirection(hit.position);
            Vector3d a = hit.solid.material.Ka * RayTracer.light.Ka;

            Vector3d d = Vector3d.Zero;
            double fd = Vector3d.Dot(hit.normal, toLight);
            if (fd > 0)
                d = hit.solid.material.Kd * RayTracer.light.Kd * fd;

            Vector3d s = Vector3d.Zero;
            Vector3d r = toLight.Reflect(hit.normal).Normalized();
            Vector3d v = (RayTracer.camera.GetPosition() - hit.position).Normalized();
            double fs = Vector3d.Dot(r, v);
            if (fs > 0)
                s = hit.solid.material.Ks * RayTracer.light.Ks * Math.Pow(fs, hit.solid.material.Ns);

            return a + d + s;
        }
    }
}
