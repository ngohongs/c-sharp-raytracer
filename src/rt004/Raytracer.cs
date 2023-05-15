using OpenTK.Mathematics;
using rt004.Solids;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rt004
{
    internal static class Raytracer
    {
        public static bool initialized = false;

        // Scene
        public static Scene scene = new Scene();
        public static List<Light> lights = new List<Light>(); 
        public static Dictionary<string, Material> materials = new Dictionary<string, Material>();
        public static Dictionary<string, Solid> solids = new Dictionary<string, Solid>();
        public static Light ambientLight;
        public static Camera camera;
        public static Brdf brdf;

        public static int MAX_DEPTH;
        public static int SAMPLES_K;
       
        static Raytracer()
        {
            initialized = ConfigLoad();
            if (initialized)
                Setup(); 
        }

        public static bool ConfigLoad(string configFileName = "config.json")
        {
            StreamReader sr = new StreamReader(configFileName);
            string json = sr.ReadToEnd();

            try
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new Vector3dConverter());
                options.Converters.Add(new Vector4dConverter());
                Config? config = JsonSerializer.Deserialize<Config>(json, options);

                if (config == null)
                {
                    Console.WriteLine("Config file is invalid");
                    return false;
                }

                MAX_DEPTH = config.maxDepth;
                SAMPLES_K = config.samplesK;
                materials = config.materials;
                scene = config.scene;
                lights = config.lights;
                ambientLight = config.ambientLight;
                camera = config.camera;
                brdf = config.brdf;

            }
            catch (Exception e)
            {
                Console.WriteLine("Config file is invalid (" + e.Message + ")");
                return false;
            }

            return true;
        }

        public static void ConfigSave(string configFileName = "config.json")
        {
            Config config = new Config();
            config.maxDepth = MAX_DEPTH;
            config.samplesK = SAMPLES_K;
            config.materials = materials;
            config.scene = scene;
            config.brdf = brdf;
            config.camera = camera;
            config.lights = lights;
            config.ambientLight = ambientLight;

            var options = new JsonSerializerOptions();
            options.Converters.Add(new Vector3dConverter());
            options.Converters.Add(new Vector4dConverter());
            options.WriteIndented = true;

            string jsonString = JsonSerializer.Serialize<Config>(config, options);
            File.WriteAllText(configFileName, jsonString);
        }

        public static void Setup()
        {
            if (!(initialized = MAX_DEPTH > 0 && SAMPLES_K > 0))
            {
                Console.WriteLine("Config file is invalid (maxDepth or samplesK is less than 1)");
                return;
            }

            scene.TransformToDirect();

            if (!(initialized = scene.SetSolidMaterials())) {
                Console.WriteLine("Config file is invalid (materialName of a solid is null or doesn't exist)");
                return;
            }
        }

        public static List<Solid> TransformedSolids(SceneNode root, Matrix4d parentTransform)
        {
            List<Solid> result = new List<Solid>();

            Matrix4d rootModelMatrix = root.GetModelMatrix() * parentTransform;

            if (root.Solid != null)
                result.Add(root.Solid.Transformed(rootModelMatrix));
       
            foreach (var child in root.Children)
            {
                result.AddRange(TransformedSolids(child, rootModelMatrix));
            }

            return result;
        }

        public static void OutputImage(string fileName)
        {
            int wid = (int) camera.width;
            int hei = (int) camera.height;

            // HDR image.
            FloatImage fi = new FloatImage(wid, hei, 3);

            Framebuffer framebuffer = Raytracer.camera.Render();

            for (int y = 0; y < hei; y++)
            {
                for (int x = 0; x < wid; x++)
                {
                    fi.PutPixel(x, y, framebuffer.GetPixel(x, y));
                }
            }

            fi.SavePFM(fileName);

            Console.WriteLine("HDR image is finished.");
        }
    }
}
