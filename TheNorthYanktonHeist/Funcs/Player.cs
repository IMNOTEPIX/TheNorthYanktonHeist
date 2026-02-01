using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fPlayer
    {
        #region PlayerModels
        public static uint GetPlayerModelHash() => (uint)Game.Player.Character.Model.Hash;
        public static bool IsFreemodePed => IsFreemodeMale || IsFreemodeFemale;
        public static bool IsFreemodeMale => GetPlayerModelHash() == 1885233650U;
        public static bool IsFreemodeFemale => GetPlayerModelHash() == 2627665880U;
        public static PedHash PlayerPreviousModel;
        public static bool IsMainCharacterPeds => IsMichael || IsFranklin || IsTrevor;
        public static bool IsMichael => GetPlayerModelHash() == 225514697U;
        public static bool IsFranklin => GetPlayerModelHash() == 2602752943U;
        public static bool IsTrevor => GetPlayerModelHash() == 2608926626U;
        public unsafe static void PlayerModelSet(Ped ped)
        {
            if (IsFreemodeMale)
            {
            }
            else
            {
                if (IsFreemodeFemale)
                {
                }
                else
                {
                    PlayerPreviousModel = ped.Model;
                }
            }
            ulong num = (ulong)((long)ped.MemoryAddress);
            ulong num2 = num + 32UL;
            *(long*)(num2 + 24UL) = -1080659212L; // Corrected to use a pointer cast
        }
        public unsafe static void PlayerModelSetBack(Ped ped)
        {
            if (IsFreemodeMale)
            {
                ulong num = (ulong)((long)ped.MemoryAddress);
                ulong num2 = num + 32UL;
                *(long*)(num2 + 24UL) = 1885233650L; // Corrected to use a pointer cast
            }
            else
            {
                if (IsFreemodeFemale)
                {
                    ulong num3 = (ulong)((long)ped.MemoryAddress);
                    ulong num4 = num3 + 32UL;
                    // Fix for CS0221: Use 'unchecked' to allow the conversion of the constant value to 'ulong'.
                    *(long*)(num4 + 24UL) = unchecked((long)((ulong)-1667301416));
                }
                else
                {
                    ulong num5 = (ulong)((long)ped.MemoryAddress);
                    ulong num6 = num5 + 32UL;
                    *(long*)(num6 + 24UL) = (long)((ulong)PlayerPreviousModel);
                }
            }
        }
        #endregion

        public static Wanted Wanted = CreateInstance<Wanted>(Game.Player.Handle);
        public static bool IsWanted => Wanted.WantedLevel > 0;
        public static void SetWantedLevelTo0()
        {
            try
            {
                Wanted.SetWantedLevel(0, false);
                Wanted.ApplyWantedLevelChangeNow(false);
            }
            catch(Exception ex) { fHud.ShowNotification("Wanted function failed:" + ex.Message, true); }
        }
        public static int FakeWantedLevel
        {
            get
            {
                return Function.Call<int>(Hash.GET_FAKE_WANTED_LEVEL);
            }
            set
            {
                Function.Call(Hash.SET_FAKE_WANTED_LEVEL, value);
            }
        }
        public static int MaxWantedLevel
        {
            get
            {
                return Function.Call<int>(Hash.GET_MAX_WANTED_LEVEL);
            }
            set
            {
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, value);
            }
        }
        public static void SetMaxWantedLevelTo0()
        {
            MaxWantedLevel = 0;
        }
        public static void SetMaxWantedLevelToNormal()
        {
            MaxWantedLevel = 5;
        }
        private static T CreateInstance<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, args, null, null);
            return (T)instance;
        }

        public static Ped PlayerPedID() => Function.Call<Ped>(Hash.PLAYER_​PED_​ID);
        public static Ped ped => Game.Player.Character;

        public static float GetDistanceTo(Vector3 position)
        {
            return Game.Player.Character.Position.DistanceTo(position);
        }
        public static float GetCarDistanceTo(Vector3 position)
        {
            if (Game.Player.Character.CurrentVehicle != null)
                return Game.Player.Character.CurrentVehicle.Position.DistanceTo(position);
            return float.NaN;
        }
        public static float GetCarDistanceTo(Vector2 position)
        {
            if (Game.Player.Character.CurrentVehicle != null)
            {
                float x = Game.Player.Character.CurrentVehicle.Position.X;
                float y = Game.Player.Character.CurrentVehicle.Position.Y;
                Vector2 xy = new Vector2(x, y);
                return (position - xy).Length();
            }
            return float.NaN;
        }
        public static float GetDistanceTo(Vector2 position)
        {
            float x = Game.Player.Character.Position.X;
            float y = Game.Player.Character.Position.Y;
            Vector2 xy = new Vector2(x, y);
            return (position - xy).Length();
        }
        public static void PedPos(float X, float Y, float Z, float heading)
        {
            Game.Player.Character.Position = new Vector3(X, Y, Z);
            Game.Player.Character.Heading = heading;
            GameplayCamera.RelativeHeading = Game.Player.Character.Heading - Game.Player.Character.Heading;
            GameplayCamera.RelativePitch = 0f;
        }
        public static void PedPos(float X, float Y, float Z)
        {
            Game.Player.Character.Position = new Vector3(X, Y, Z);
            GameplayCamera.RelativeHeading = Game.Player.Character.Heading - Game.Player.Character.Heading;
            GameplayCamera.RelativePitch = 0f;
        }
        public static void ResetInputGait()
        {
            Function.Call(Hash.RESET_PLAYER_INPUT_GAIT, Game.Player);
        }
        public static void SimulateInputGait(float moveBlendRatio, int timer = 2000, float heading = 0f, bool useRelativeHeading = true, bool noInputInterruption = false)
        {
            Function.Call(Hash.SIMULATE_PLAYER_INPUT_GAIT, Game.Player, moveBlendRatio, timer, heading, useRelativeHeading, noInputInterruption);
        }
    }
}
