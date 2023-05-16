using OpenTK.Mathematics;
using rt004.Brdfs;
using rt004.Cameras;
using rt004.Hierarchy;
using rt004.Lights;
using rt004.Materials;
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
        public int maxDepth;
        [JsonInclude]
        public int samplesK;
        [JsonInclude]
        public int chunkSize;
        [JsonInclude]
        public Dictionary<string, Material> materials;
        [JsonInclude]
        public Scene scene;
        [JsonInclude]
        public List<Light> lights;
        [JsonInclude]
        public AmbientLight ambientLight;
        [JsonInclude]
        public Camera camera;
        [JsonInclude]
        public Brdf brdf;
        [JsonInclude]
        public Vector3d background;
    }
}
