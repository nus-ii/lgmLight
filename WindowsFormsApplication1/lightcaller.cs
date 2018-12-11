using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class lightcaller
    {
        public delegate void vDelegate();

        public event vDelegate ButtonC;

        public void clickClick()
        {
            ButtonC();
        }

    }
}
