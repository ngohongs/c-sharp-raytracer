using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004
{
    internal class Framebuffer
    {
        private List<List<float[]>> framebuffer;
        private int width;
        private int height;
        public Framebuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            framebuffer = new List<List<float[]>>(width);
            for (int i = 0; i < width; i++)
            {
                framebuffer.Add(new List<float[]>(height));
                for (int j = 0; j < height; j++)
                {
                    framebuffer[i].Add(new float[] { 0.0f });
                }
            }
        }

        public void SetPixel(int x, int y, float[] color)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return;
            framebuffer[x][y] = color;
        }

        public float[] GetPixel(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return new float[] { -1.0f, -1.0f, -1.0f };
            return framebuffer[x][y];
        }
    }
}
