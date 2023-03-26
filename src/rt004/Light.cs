using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004
{
    internal struct Light
    {
        [JsonInclude]
        public Vector4d position;
        [JsonInclude]
        public Vector3d Ka;
        [JsonInclude]
        public Vector3d Kd;
        [JsonInclude]
        public Vector3d Ks;
        public Light(Vector4d position, Vector3d Ka, Vector3d Kd, Vector3d Ks)
        {
            this.position = position;
            this.Ka = Ka;
            this.Kd = Kd;
            this.Ks = Ks;
        }
        public Vector3d GetDirection(Vector3d point)
        {
            if (position.W > 0)
                return position.Xyz - point;
            return -position.Xyz;
        }

        public bool IsDirectional() { return position.W <= 0; } 
    }
}
