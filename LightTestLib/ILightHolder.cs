using System.Collections.Generic;
using System.Threading.Tasks;

namespace LightTestLib
{
    public interface ILightHolder
    {
        Color ActiveColor { get; set; }

        void BlinkListAsync(List<Color> colorList, int gapMs);
        
        List<Color> BaseColors { get; }
        void PulseAsync(Color targetColor, int milliSecondsInterval);
        void RandomShowAsync(int gapMs, List<Color> colorList);
        Task<int> SetOtherColorAsync(int gapMs, List<Color> colorList);
        void Kill();
    }
}