using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTestLib
{
     public class LightHolderSparta:LightHolder,ILightHolder
    {
        public override List<Color> GetBaseColors (bool withBlack = false)
        {
            var ColorList = new List<Color>();

            ColorList.Add(new Color(0, 0, 99));
            ColorList.Add(new Color(0, 99, 0));
            ColorList.Add(new Color(0, 99, 99));

            ColorList.Add(new Color(99, 0, 0));
            ColorList.Add(new Color(99, 0, 99));
            ColorList.Add(new Color(99, 99, 0));

            ColorList.Add(new Color(99, 99, 99));

            if (withBlack)
                ColorList.Add(new Color(0, 0, 0));

            return ColorList;
        }
    }
}
