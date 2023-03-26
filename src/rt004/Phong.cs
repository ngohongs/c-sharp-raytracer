using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal class Phong : Brdf
    {

        public override Vector3d Shade(RayHit hit, Light light, bool inShadow)
        {
            Vector3d toLight = light.GetDirection(hit.position).Normalized();
            Vector3d a = hit.solid.material.Ka * light.Ka;

            if (inShadow)
                return a;

            double fd = Math.Max(Vector3d.Dot(hit.normal, toLight), 0);
            Vector3d d = hit.solid.material.Kd * light.Kd * fd;

            Vector3d r = (-toLight).Reflect(hit.normal).Normalized();
            Vector3d v = (Raytracer.camera.GetPosition() - hit.position).Normalized();
            double fs = Math.Max(Vector3d.Dot(r, v), 0);
            Vector3d s = hit.solid.material.Ks * light.Ks * Math.Pow(fs, hit.solid.material.Ns);


            return d + s;
        }
    }
}
