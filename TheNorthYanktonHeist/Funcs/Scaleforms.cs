using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fScaleforms
    {
        public static bool HasScaleformMovieLoaded(int scaleID)
        {
            return Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleID);
        }

        protected static void pushArgs(object[] args)
        {
            foreach (object x in args)
            {
                if (x.GetType() == typeof(int)) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)x);
                else if (x.GetType() == typeof(float)) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)x);
                else if (x.GetType() == typeof(double)) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)x);
                else if (x.GetType() == typeof(bool)) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)x);
                else if (x.GetType() == typeof(TXD)) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, ((TXD)x).Texture);
                else if (x.GetType() == typeof(string))
                {
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)x);
                    Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                }
                else if (x.GetType() == typeof(char))
                {
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ((char)x).ToString());
                    Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                }
            }
        }
        public class TXD
        {
            private readonly string texture;
            public string Texture { get { return texture; } }

            public TXD(string texture)
            {
                this.texture = texture;
            }
        }

        public static void CallFunction(int Handle, string name, params object[] args)
        {
            Function.Call<bool>(Hash.BEGIN_​SCALEFORM_​MOVIE_​METHOD, Handle, name);
            pushArgs(args);
            Function.Call(Hash.END_​SCALEFORM_​MOVIE_​METHOD);
        }
        public static bool CallFunctionBool(int Handle, string name, params object[] args)
        {
            Function.Call<bool>(Hash.BEGIN_​SCALEFORM_​MOVIE_​METHOD, Handle, name);
            pushArgs(args);
            int ret = Function.Call<int>(Hash.END_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE);
            while (!Function.Call<bool>(Hash.IS_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE_​READY, ret)) Script.Yield();
            return Function.Call<bool>(Hash.GET_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE_​BOOL, ret);
        }
        public static int CallFunctionInt(int Handle, string name, params object[] args)
        {
            Function.Call<bool>(Hash.BEGIN_​SCALEFORM_​MOVIE_​METHOD, Handle, name);
            pushArgs(args);
            int ret = Function.Call<int>(Hash.END_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE);
            while (!Function.Call<bool>(Hash.IS_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE_​READY, ret)) Script.Yield();
            return Function.Call<int>(Hash.GET_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE_​INT, ret);
        }
        public static string CallFunctionString(int Handle, string name, params object[] args)
        {
            Function.Call<bool>(Hash.BEGIN_​SCALEFORM_​MOVIE_​METHOD, Handle, name);
            pushArgs(args);
            int ret = Function.Call<int>(Hash.END_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE);
            while (!Function.Call<bool>(Hash.IS_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE_​READY, ret)) Script.Yield();
            return Function.Call<string>(Hash.GET_​SCALEFORM_​MOVIE_​METHOD_​RETURN_​VALUE_​STRING, ret);
        }
    }
}
