using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTestLib
{
    public class Color :IEquatable<Color>
    {
        public int R;
        public int G;
        public int B;

        public Color(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }      

        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B;
        }
    }
}
