using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fTimer
    {
        public static int TimerA() => Function.Call<int>(Hash.TIMERA);
        public static int TimerB() => Function.Call<int>(Hash.TIMERB);
        public static void SetTimerA(int value) => Function.Call(Hash.SETTIMERA, value);
        public static void SetTimerB(int value) => Function.Call(Hash.SETTIMERB, value);
    }
}
