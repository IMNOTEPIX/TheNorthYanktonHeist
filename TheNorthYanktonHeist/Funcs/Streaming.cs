using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fStreaming
    {
        public static bool HasAnimDictLoaded(string animDict)
        {
            return Function.Call<bool>(Hash.HAS_​ANIM_​DICT_​LOADED, animDict);
        }
        public static void RemoveNamedPTFXAsset(string fxName)
        {
            Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, fxName);
        }
        public static void UseParticleFXAsset(string fxAsset)
        {
            Function.Call(Hash.USE_PARTICLE_FX_ASSET, fxAsset);
        }
        public static string RequestNamedPTFXAsset(string fxName)
        {
            while (!Function.Call<bool>(Hash.HAS_​NAMED_​PTFX_​ASSET_​LOADED, fxName))
            {
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, fxName);
                Script.Yield();
            }

            return fxName;
        }
        public static string LoadNamedPTFXAsset(string fxName)
        {
            if (!Function.Call<bool>(Hash.HAS_​NAMED_​PTFX_​ASSET_​LOADED, fxName))
            {
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, fxName);
            }

            return fxName;
        }
        /// <summary>
        /// Possible p0 values: "prologue", "Prologue_Main"
        /// </summary>
        public static void SetMapDataCullboxEnabled(string name, bool toggle)
        {
            Function.Call(Hash.SET_​MAPDATACULLBOX_​ENABLED, name, toggle);
        }
        public static string RequestAnimDict(string dict, List<string> animDictList = null)
        {
            if (dict != string.Empty)
            {
                if (animDictList != null)
                    animDictList.Add(dict);
                while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, dict);
                    Script.Yield();
                }
            }
            return dict;
        }
        public static void RemoveAnimDict(List<string> animDictList)
        {
            if (animDictList.Count < 0)
            {
                for (int i = 0; i < animDictList.Count; i++)
                {
                    while (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDictList[i]))
                    {
                        Function.Call(Hash.REMOVE_ANIM_DICT, animDictList[i]);
                        Script.Yield();
                    }
                }
            }
        }
        public static void RemoveAnimDict(string dict)
        {
            if (dict != string.Empty)
            {
                Function.Call(Hash.REMOVE_ANIM_DICT, dict);
            }
        }
        public static void RemovePTFXAsset()
        {
            Function.Call(Hash.REMOVE_PTFX_ASSET);
        }
    }
}
