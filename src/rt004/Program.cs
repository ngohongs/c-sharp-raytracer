//using System.Numerics;

using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace rt004
{
    internal class Program
    {
        static int Main(string[] args)
        {     
            // Parameters.
            string fileName;
            fileName = args.Length < 1 ? "out.pfm" : args[0] + ".pfm";

            if (!Raytracer.initialized)
            {
                return 1;
            }

            Raytracer.OutputImage(fileName);
 
            return 0;
        }
    }
}
