using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rt004
{
    internal class Config
    {
        [JsonInclude]
        public Scene scene;
        [JsonInclude]
        public List<Light> lights;
        [JsonInclude]
        public Light ambientLight;
        [JsonInclude]
        public Camera camera;
        [JsonInclude]
        public Brdf brdf;
    }
}
