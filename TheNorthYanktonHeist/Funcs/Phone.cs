using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fPhone
    {
        public static int Handle
        {
            get
            {
                uint hash = (uint)Game.Player.Character.Model.Hash;
                uint num = hash;
                int result;
                if (num != 225514697U)
                {
                    if (num != 2602752943U)
                    {
                        if (num != 2608926626U)
                        {
                            int num2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
                            while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num2))
                            {
                                Script.Yield();
                            }
                            result = num2;
                        }
                        else
                        {
                            int num2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_facade");
                            while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num2))
                            {
                                Script.Yield();
                            }
                            result = num2;
                        }
                    }
                    else
                    {
                        int num2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_badger");
                        while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num2))
                        {
                            Script.Yield();
                        }
                        result = num2;
                    }
                }
                else
                {
                    int num2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
                    while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num2))
                    {
                        Script.Yield();
                    }
                    result = num2;
                }
                return result;
            }
        }

        public static void PlayTextArriveTone()
        {
            uint hash = (uint)Game.Player.Character.Model.Hash;
            uint num = hash;
            if (num != 225514697U/*michael*/)
            {
                if (num != 2602752943U/*franklin*/)
                {
                    if (num != 2608926626U/*trevor*/)
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Default", true);
                    else
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Trevor", true);
                }
                else
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Franklin", true);
            }
            else
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Michael", true);
        }

        public static void SetTextHeader(string text)
        {
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_HEADER");
            Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, text, -1);
            Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }

        public static void SetNewText(string contact, string message, string contactTXD = "char_default")
        {

            SetSoftKeyIcon(1, SoftKeyIcons.Yes);
            SetSoftKeyColor(1, Color.Green);
            SetSoftKeyIcon(2, SoftKeyIcons.Blank);
            SetSoftKeyIcon(3, SoftKeyIcons.No);
            SetTextHeader("Texts");
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_DATA_SLOT_EMPTY");
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 7);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_DATA_SLOT");
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 7);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, contact);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, "<textarea rows=\"1000\" cols=\"1000\">" + message + "</textarea>");
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, contactTXD);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "DISPLAY_VIEW");
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 7);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, -1082130432);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, -1082130432);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, -1082130432);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }

        public static int GetSelectedIndex()
        {
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "GET_CURRENT_SELECTION");
            int methodReturn = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
            while (!Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, methodReturn))
            {
                Script.Wait(0);
            }
            return Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, methodReturn);
        }

        public static void SetSoftKeyColor(int buttonID, Color color)
        {
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_SOFT_KEYS_COLOUR");
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, buttonID);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)color.R);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)color.G);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)color.B);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }
        public static void SetSoftKeyIcon(int buttonID, SoftKeyIcons icon)
        {
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_SOFT_KEYS");
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, buttonID);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)icon);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }
        public enum SoftKeyIcons
        {
            Blank = 1,
            Select,
            Pages,
            Back,
            Call,
            Hangup,
            HangupHuman,
            Week,
            Keypad,
            Open,
            Reply,
            Delete,
            Yes,
            No,
            Sort,
            Website,
            Police,
            Ambulance,
            Fire,
            Pages2
        }
    }
}
