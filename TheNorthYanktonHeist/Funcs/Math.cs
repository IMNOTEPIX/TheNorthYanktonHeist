using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fMath
    {
        public static int CEIL(float value)
        {
            return Function.Call<int>(Hash.CEIL, value);
        }
    }
}
