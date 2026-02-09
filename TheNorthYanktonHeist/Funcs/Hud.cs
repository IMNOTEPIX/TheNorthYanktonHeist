using GTA;
using GTA.Native;
using GTA.UI;
using GTA.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fHud
    {
        public static void DisplayAmmoThisFrame(bool display)
        {
            Function.Call(Hash.DISPLAY_AMMO_THIS_FRAME, display);
        }
        public static void ShowGXTSubtitle(string GxtEntry, int duration = 10)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_PRINT, GxtEntry);
            Function.Call(Hash.END_TEXT_COMMAND_PRINT, duration, true);
        }
        /// <summary>
        /// loop is basicaly you call the function one time and the help text stays until u clear it, others are self explanatory
        /// </summary>
        public static void DisplayGXTHelpText(string GxtEntry, bool loop = false, bool beep = true, int duration = -1)
        {
            if (!Function.Call<bool>(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED) && !Function.Call<bool>(Hash.IS_HELP_MESSAGE_ON_SCREEN) && !Function.Call<bool>(Hash.IS_HELP_MESSAGE_FADING_OUT) && !Game.Player.Character.IsDead)
            {
                Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, GxtEntry);
                Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, loop, beep, duration);
            }
            else
            {
                Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, GxtEntry);
                Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, loop, false, duration);
            }
        }
        public static void ToggleNorthYanktonMap(bool toggle)
        {
            Function.Call(Hash.SET_​MINIMAP_​IN_​PROLOGUE, toggle);
        }
        public static void ShowNotification(string message, bool isImportant = true, bool cacheMessage = true)
        {
            Notification.PostTicker(message, isImportant, cacheMessage);
        }
        public static FeedPost PostMessageNotify(string text, TextureAsset textAsset, bool isImportant, FeedTextIcon icon, string senderName)
        {
            return Notification.PostMessageText(text, textAsset, isImportant, icon, senderName);
        }
        public static void DisplayHelpText(params string[] texts)
        {
            if (!IsHelpMessageBeingDisplayed() && !IsHelpMessageOnScreen() && !IsHelpMessageFadingOut() && !Game.Player.Character.IsDead)
            {
                ClearAllHelpMessages();
                BeginTextCommandDisplayHelp(CellEmailBcon);
                foreach (string text in texts)
                {
                    AddTextComponentSubstringPlayerName(text);
                }
                EndTextCommandDisplayHelp(0, false, true);
            }
            else
            {
                ClearAllHelpMessages();
                ClearHelp(true);
                BeginTextCommandDisplayHelp(CellEmailBcon);
                foreach (string text in texts)
                {
                    AddTextComponentSubstringPlayerName(text);
                }
                EndTextCommandDisplayHelp(0, false, false, 1);
            }
        }
        public static void DisplayHelpText(string text)
        {
            if (!IsHelpMessageBeingDisplayed() && !IsHelpMessageOnScreen() && !IsHelpMessageFadingOut() && !Game.Player.Character.IsDead)
            {
                ClearAllHelpMessages();
                BeginTextCommandDisplayHelp(CellEmailBcon);
                AddTextComponentSubstringPlayerName(text);
                EndTextCommandDisplayHelp(0, false, true);
            }
            else
            {
                ClearAllHelpMessages();
                ClearHelp(true);
                BeginTextCommandDisplayHelp(CellEmailBcon);
                AddTextComponentSubstringPlayerName(text);
                EndTextCommandDisplayHelp(0, false, false, 1);
            }
        }
        public static void DisplayHelpText_Duration(int helpMessageID = 0, bool loop = false, bool beep = true, int duration = 2500, params string[] texts)
        {
            if (!IsHelpMessageBeingDisplayed() && !IsHelpMessageOnScreen() && !IsHelpMessageFadingOut() && !Game.Player.Character.IsDead)
            {
                ClearAllHelpMessages();
                ClearBrief();
                BeginTextCommandDisplayHelp(CellEmailBcon);
                foreach (string text in texts)
                {
                    AddTextComponentSubstringPlayerName(text);
                }
                EndTextCommandDisplayHelp(helpMessageID, loop, beep, duration);
            }
        }
        public static IntPtr CellEmailBcon => StringToCoTaskMemUTF8("CELL_EMAIL_BCON");
        private static byte[] _strBufferForStringToCoTaskMemUTF8 = new byte[100];
        public unsafe static IntPtr StringToCoTaskMemUTF8(string s)
        {
            IntPtr result;
            if (s == null)
            {
                result = IntPtr.Zero;
            }
            else
            {
                int byteCount = Encoding.UTF8.GetByteCount(s);
                if (byteCount > _strBufferForStringToCoTaskMemUTF8.Length)
                {
                    _strBufferForStringToCoTaskMemUTF8 = new byte[byteCount * 2];
                }
                Encoding.UTF8.GetBytes(s, 0, s.Length, _strBufferForStringToCoTaskMemUTF8, 0);
                IntPtr intPtr = Marshal.AllocCoTaskMem(byteCount + 1);
                if (intPtr == IntPtr.Zero)
                {
                    throw new OutOfMemoryException();
                }
                Marshal.Copy(_strBufferForStringToCoTaskMemUTF8, 0, intPtr, byteCount);
                ((byte*)intPtr.ToPointer())[byteCount] = 0;
                result = intPtr;
            }
            return result;
        }
        public static void BeginTextCommandDisplayHelp(IntPtr GxtEntry)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, GxtEntry);
        }
        public static void BeginTextCommandDisplayHelp(string GxtEntry)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, GxtEntry);
        }
        public static void AddTextComponentSubstringPlayerName(string text)
        {
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
        }
        public static void EndTextCommandDisplayHelp(int duration, bool loop, bool beep, int shape = -1)
        {
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, duration, loop, beep, shape);
        }
        public static bool IsHelpMessageBeingDisplayed()
        {
            return Function.Call<bool>(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED);
        }
        public static bool IsHelpMessageOnScreen()
        {
            return Function.Call<bool>(Hash.IS_HELP_MESSAGE_ON_SCREEN);
        }
        public static bool IsHelpMessageFadingOut()
        {
            return Function.Call<bool>(Hash.IS_HELP_MESSAGE_FADING_OUT);
        }
        public static void ClearPrints()
        {
            Function.Call(Hash.CLEAR_PRINTS);
        }
        public static void ClearAllPrints()
        {
            Function.Call(Hash.CLEAR_SMALL_PRINTS);
            Function.Call(Hash.CLEAR_PRINTS);
        }
        public static void ClearAllHelpMessages()
        {
            Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
        }
        public static void ClearBrief()
        {
            Function.Call(Hash.CLEAR_BRIEF);
        }
        public static void ClearHelp(bool toggle)
        {
            Function.Call(Hash.CLEAR_HELP, toggle);
        }
        public static void ClearGPSMultiRoute()
        {
            Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
        }
        public static void RadarAndHud(bool Radar, bool Hud)
        {
            Function.Call(Hash.DISPLAY_RADAR, Radar);
            Function.Call(Hash.DISPLAY_HUD, Hud);
        }
        public static bool IsPauseMenuActive()
        {
            return Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
        }
        public static bool IsHudComponentActive(HudComponent id)
        {
            return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, (int)id);
        }
        public static void HideHudMarkersThisFrame()
        {
            Function.Call(Hash.HIDE_HUDMARKERS_THIS_FRAME);
        }
        public static void HideMinimapExteriorMapThisFrame()
        {
            Function.Call(Hash.HIDE_MINIMAP_EXTERIOR_MAP_THIS_FRAME);
        }
        public static void HideMinimapInteriorMapThisFrame()
        {
            Function.Call(Hash.HIDE_MINIMAP_INTERIOR_MAP_THIS_FRAME);
        }
        public static void HudSuppressWeaponWheelResultsThisFrame()
        {
            Function.Call(Hash.HUD_SUPPRESS_WEAPON_WHEEL_RESULTS_THIS_FRAME);
        }
        public static void SetFakePauseMapPlayerPositionThisFrame(float x, float y)
        {
            Function.Call(Hash.SET_FAKE_PAUSEMAP_PLAYER_POSITION_THIS_FRAME, x, y);
        }
        public static void SetInsideVerySmallInterior(bool toggle)
        {
            Function.Call(Hash.SET_INSIDE_VERY_SMALL_INTERIOR, toggle);
        }
        public static void DontTiltMinimapThisFrame()
        {
            Function.Call(Hash.DONT_TILT_MINIMAP_THIS_FRAME);
        }
        public static void DontZoomMinimapWhenSnipingThisFrame()
        {
            Function.Call(Hash.DONT_ZOOM_MINIMAP_WHEN_SNIPING_THIS_FRAME);
        }
        public static void DontZoomMinimapWhenRunningThisFrame()
        {
            Function.Call(Hash.DONT_ZOOM_MINIMAP_WHEN_RUNNING_THIS_FRAME);
        }
        public static void FlashMinimapDisplay()
        {
            Function.Call(Hash.FLASH_MINIMAP_DISPLAY);
        }
        public static bool IsPauseMapInInteriorMode()
        {
            return Function.Call<bool>(Hash.IS_PAUSEMAP_IN_INTERIOR_MODE);
        }
        public static void ShowHudComponentThisFrame(int id)
        {
            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, id);
        }
        public static void ShowScriptedHudComponentThisFrame(int id)
        {
            Function.Call(Hash.SHOW_​SCRIPTED_​HUD_​COMPONENT_​THIS_​FRAME, id);
        }
        public static void HideHudComponentThisFrame(int id)
        {
            Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, id);
        }
        public static bool IsHudComponentActive(int id)
        {
            return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, id);
        }
    }
}
