using OpenTK.Mathematics;
using rt004.Rays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Cameras
{
    internal class PerspectiveCamera : Camera
    {
        private double _fov;
        [JsonInclude]
        public double fov { get => _fov / Math.PI * 180; set => _fov = value / 180 * Math.PI; }

        public PerspectiveCamera(Vector3d position, Vector3d forward, Vector3d up, double fov, double width, double height) : base(position, forward, up, width, height)
        {
            _fov = fov * Math.PI / 180.0d;
        }

        protected override Ray CastRay(double x, double y)
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
    }


}
