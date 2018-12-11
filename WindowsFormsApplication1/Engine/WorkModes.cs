using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.Engine
{
    /// <summary>
    /// Перечисление рабочих режимов приложения
    /// </summary>
    public enum WorkModes
    {
        OutsideControl,
        Black,
        SingleColor,
        SingleColorBlink,
        Pattern,
        Random,
        Pomodoro,
        AlarmInAction,
        KeyReaction
    }
}
