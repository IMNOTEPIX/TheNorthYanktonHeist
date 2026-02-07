using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fScript
    {
        public static void TerminateThisThread()
        {
            Function.Call(Hash.TERMINATE_THIS_THREAD);
        }
        public static void SetThisScriptCanRemoveBlipsCreatedByAnyScript(bool toggle)
        {
            Function.Call(Hash.SET_THIS_SCRIPT_CAN_REMOVE_BLIPS_CREATED_BY_ANY_SCRIPT, toggle);
        }
        public static void StartNewScriptWithNameHashAndArgs(Hash scriptHash, int args, int argCount, int stackSize)
        {
            Function.Call<int>(Hash.START_​NEW_​SCRIPT_​WITH_​NAME_​HASH_​AND_​ARGS, scriptHash, args, argCount, stackSize);
        }
        public static void SetThisScriptCanBePaused(bool toggle)
        {
            Function.Call(Hash.SET_THIS_SCRIPT_CAN_BE_PAUSED, toggle);
        }
        public static void StartScript(string script, int stackSize)
        {
            Function.Call(Hash.REQUEST_SCRIPT, script);
            Script.Wait(stackSize > 3000 ? 500 : 100);
            if (Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, script))
            {
                Function.Call(Hash.START_NEW_SCRIPT, script, stackSize);
                Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, script);
            }
        }
    }
}
