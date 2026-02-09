using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fGamePad
    {
        public static void SetControlShake(int control, int duration, int frequency)
        {
            Function.Call(Hash.SET_​CONTROL_​SHAKE, control, duration, frequency);
        }
    }
}
