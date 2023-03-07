using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rt004
{
    internal static class RayTracer
    {
        public static bool initialized = false;

        // Config
        public static Dictionary<string, string> definedVariables;
        private static string[] mandatoryParameters = { "width", "height" };

        // Scene
        public static Scene scene = new Scene();
        public static Light light; // only one light object at the moment
        public static Camera camera;

        public static IBrdf brdf;
       
        static RayTracer()
        {
            initialized = ConfigLoad();
            if (initialized)
                Setup(); 
        }
        private static bool AreDefinedVariablesValid()
        {
            foreach (string parameter in mandatoryParameters)
            {
                if (!definedVariables.ContainsKey(parameter))
                {
                    Console.WriteLine("Config file is invalid.");
                    return false;
                }
            }
            return true;
        }
        public static bool ConfigLoad(string configFileName = "config.txt")
        {
            String textLines;
            try
            {
                StreamReader sr = new StreamReader(configFileName);
                textLines = sr.ReadToEnd();
                RayTracer.definedVariables = textLines.Split('\n')
                                                    .Select(line => Regex.Match(line, "(.*)=(.*)"))
                                                    .ToDictionary(match => match.Groups[1].Value, match => match.Groups[2].Value);
                sr.Close();

                if (!AreDefinedVariablesValid())
                {
                    Console.WriteLine("Config is invalid");
                    return false;
                }
                    

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }
        public static void Setup()
        {
            int wid = Int32.Parse(RayTracer.definedVariables["width"]);
            int hei = Int32.Parse(RayTracer.definedVariables["height"]);

            camera = new Camera(new Vector3d(0, 0, 0), new Vector3d(0.0d, 0.0d, -1.0d), new Vector3d(0.0d, 1.0d, 0.0d), 60.0d, wid, hei);

            light = new Light(
                new Vector4d(-1, -1, -1, 0).Normalized(),
                new Vector3d(1),
                new Vector3d(1),
                new Vector3d(1)
                );

            Sphere s = new Sphere(
                new Vector3d(0, 0, -3),
                1.0d,
                new Material(
                    new Vector3d(0.1d),
                    new Vector3d(0.0d, 0.0d, 0.64d),
                    new Vector3d(0.5d),
                    96.078431d
                    )
            );

            Sphere s1 = new Sphere(
                new Vector3d(-2, 1, -5),
                2.0d,
                new Material(
                    new Vector3d(0.1d),
                    new Vector3d(0.64, 0.64d, 0.0d),
                    new Vector3d(0.5d),
                    96.078431d
                    )
            );

            Plane p = new Plane(
                new Vector3d(0, 0, -10),
                new Vector3d(0, 2, 1),
                new Material(
                    new Vector3d(0.1d),
                    new Vector3d(0.5, 0, 0),
                    new Vector3d(0.0, 1.0, 0),
                    100
                    )
            );

            scene.Add(s1);
            scene.Add(s);
            scene.Add(p);

            brdf = new Phong();
        }
        public static void OutputImage(string fileName)
        {
            int wid = Int32.Parse(RayTracer.definedVariables["width"]);
            int hei = Int32.Parse(RayTracer.definedVariables["height"]);

            // HDR image.
            FloatImage fi = new FloatImage(wid, hei, 3);

            List<List<float[]>> framebuffer = RayTracer.camera.Render();

            for (int y = 0; y < hei; y++)
            {
                for (int x = 0; x < wid; x++)
                {
                    fi.PutPixel(x, y, framebuffer[x][y]);
                }
            }

            fi.SavePFM(fileName);

            Console.WriteLine("HDR image is finished.");
        }
    }
}
