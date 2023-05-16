using OpenTK.Mathematics;
using rt004.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004.Lights
{
    [JsonDerivedType(typeof(AmbientLight), typeDiscriminator: "ambient")]
    [JsonDerivedType(typeof(PointLight), typeDiscriminator: "point")]
    [JsonDerivedType(typeof(DirectionalLight), typeDiscriminator: "directional")]
    internal abstract class Light
    {
        [JsonInclude]
        public Vector3d position;
        [JsonInclude]
        public Vector3d Ka;
        [JsonInclude]
        public Vector3d Kd;
        [JsonInclude]
        public Vector3d Ks;

        public Light(Vector3d position, Vector3d Ka, Vector3d Kd, Vector3d Ks)
        {
            this.position = position;
            this.Ka = Ka;
            this.Kd = Kd;
            this.Ks = Ks;
        }

        public abstract Vector3d GetDirection(Vector3d point);
    }
}
