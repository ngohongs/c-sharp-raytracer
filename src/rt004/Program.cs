//using System.Numerics;

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

            for (int x = 0; x < wid; x++)
            {
                for (int y = 0; y < hei; y++)
                {
                    fi.PutPixel(x, y, new float[] { 0, x, y });
                }
            }

            for (int x = 0; x < 4 * wid; x++)
            {
                double y = hei / 2 * Math.Sin(2 * Math.PI * (x /(4.0f * wid))) + hei / 2;
                fi.PutPixel(x / 4, (int)y, new float[] { 10 * x, 0, 10*x });
            }

            //fi.SaveHDR(fileName);   // Doesn't work well yet...
            fi.SavePFM(fileName);

            Console.WriteLine("HDR image is finished.");
            return 0;
        }
    }
}
