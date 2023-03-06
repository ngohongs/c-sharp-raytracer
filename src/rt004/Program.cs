//using System.Numerics;

using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace rt004
{
    internal class Program
    {
        static string[] MandatoryParameters = { "width", "height" };
        static Dictionary<string, string> ConfigLoad(string configFileName = "config.txt")
        {
            String textLines;
            try
            {
                StreamReader sr = new StreamReader(configFileName);
                textLines = sr.ReadToEnd();
                Dictionary<string, string> definedVariables = textLines.Split('\n')
                                                                       .Select(line => Regex.Match(line, "(.*)=(.*)"))
                                                                       .ToDictionary(match => match.Groups[1].Value, match => match.Groups[2].Value);
                return definedVariables;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return new Dictionary<string, string>(); 
            }
        }
        static int Main(string[] args)
        {     
            // Parameters.
            int wid = 600;
            int hei = 450;
            string fileName;

            Dictionary<string, string> definedVariables = ConfigLoad();

            foreach (string parameter in MandatoryParameters)
            {
                if (!definedVariables.ContainsKey(parameter))
                {
                    Console.WriteLine("Config file is invalid.");
                    return 1;
                }
            }

            wid = Int32.Parse(definedVariables["width"]);
            hei = Int32.Parse(definedVariables["height"]);

            fileName = args.Length < 1 ? "out.pfm" : args[0] + ".pfm";

            // HDR image.
            FloatImage fi = new FloatImage(wid, hei, 3);

            Camera camera = new Camera(new Vector3d(0, 0, 0), new Vector3d(0.0d, 0.0d, -1.0d), new Vector3d(0.0d, 1.0d, 0.0d), 60.0d, wid, hei);

            List<List<float[]>> framebuffer = camera.Render();

            for (int y = 0; y < hei; y++)
            {
                for (int x = 0; x < wid; x++)
                {
                    fi.PutPixel(x, y, framebuffer[x][y]);
                }
            }

            //fi.SaveHDR(fileName);   // Doesn't work well yet...
            fi.SavePFM(fileName);

            Console.WriteLine("HDR image is finished.");
            return 0;
        }
    }
}
