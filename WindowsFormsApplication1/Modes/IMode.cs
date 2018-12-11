using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightTestLib;

namespace WindowsFormsApplication1.Modes
{
    public interface IMode
    {
        LightTask Process();
    }
}
