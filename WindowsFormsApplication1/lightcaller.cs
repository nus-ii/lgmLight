using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class lightcaller
    {
        public delegate int vDelegate(List<LightTestLib.Color> cList);

        public event vDelegate ButtonC;

        public void clickClick(List<LightTestLib.Color> cList)
        {
            ButtonC(cList);
        }

    }
}
