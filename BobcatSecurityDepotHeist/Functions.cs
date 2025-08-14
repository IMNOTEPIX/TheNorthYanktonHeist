using GTA;
using GTA.Graphics;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static BobcatSecurityDepotHeist.Functions;
using Hash = GTA.Native.Hash;
using Screen = GTA.UI.Screen;

namespace BobcatSecurityDepotHeist
{
    internal class Functions
    {
        public class fMisc
        {
            public static object pushArgs(params object[] args)
            {
                foreach (object obj in args)
                {
                    if (obj.GetType() == typeof(int))
                    {
                        return (int)obj;
                    }
                    else
                    {
                        if (obj.GetType() == typeof(float))
                        {
                            return (float)obj;
                        }
                        else
                        {
                            if (obj.GetType() == typeof(double))
                            {
                                return (float)((double)obj);
                            }
                            else
                            {
                                if (obj.GetType() == typeof(bool))
                                {
                                    return (bool)obj;
                                }
                                else
                                {
                                    if (obj.GetType() == typeof(string))
                                    {
                                        return (string)obj;
                                    }
                                    else
                                    {
                                        if (obj.GetType() == typeof(char))
                                        {
                                            return ((char)obj).ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return 0;
            }
            public static void SetGamePaused(bool toggle)
            {
                Function.Call(Hash.SET_GAME_PAUSED, toggle);
            }
            public static void DisableHospitalRestart(int hospitalID, bool disable)
            {
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, hospitalID, disable);
            }
            public static void DisablePoliceRestart(int policeID, bool disable)
            {
                Function.Call(Hash.DISABLE_POLICE_RESTART, policeID, disable);
            }
            public static void ClearArea(Vector3 xyz, float radius, bool ignoreVehicles, bool ignorePeds, bool ignoreProps)
            {
                Entity[] entities = World.GetAllEntities();
                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i].Position.DistanceTo(xyz) <= radius)
                    {
                        if (ignoreVehicles && entities[i].EntityType != EntityType.Vehicle)
                        {
                            entities[i].Delete();
                        }
                        if (ignorePeds && entities[i].EntityType != EntityType.Ped)
                        {
                            entities[i].Delete();
                        }
                        if (ignoreProps && entities[i].EntityType != EntityType.Prop)
                        {
                            entities[i].Delete();
                        }
                    }
                }
            }
            public static Hash GetHashKey(string str)
            {
                return Function.Call<Hash>(Hash.GET_HASH_KEY, str);
            }
            public static int GetRandomIntInRange(int startRange, int endRange)
            {
                return Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, startRange, endRange);
            }
        }

        public class fWeather
        {
            public static Hash GetCurrentWeatherTypeHashName() // Returns current weather name hash
            {
                return Function.Call<Hash>(Hash.GET_​PREV_​WEATHER_​TYPE_​HASH_​NAME);
            }
            public static Hash GetNextWeatherTypeHashName()
            {
                return Function.Call<Hash>(Hash.GET_​NEXT_​WEATHER_​TYPE_​HASH_​NAME);
            }
            public static string GetCurrentWeatherStringName()
            {
                uint weatherHash = (uint)Function.Call<Hash>(Hash.GET_​PREV_​WEATHER_​TYPE_​HASH_​NAME);
                for (int i = 0; i < WeatherHashes.Length; i++)
                {
                    if (weatherHash == WeatherHashes[i])
                    {
                        return WeatherStr[i];
                    }
                }
                return null;
            }
            public static string GetNextWeatherStringName()
            {
                uint weatherHash = (uint)Function.Call<Hash>(Hash.GET_​NEXT_​WEATHER_​TYPE_​HASH_​NAME);
                for (int i = 0; i < WeatherHashes.Length; i++)
                {
                    if (weatherHash == WeatherHashes[i])
                    {
                        return WeatherStr[i];
                    }
                }
                return null;
            }
            public static WeatherTypes GetCurrentWeatherEnum()
            {
                uint weatherHash = (uint)Function.Call<Hash>(Hash.GET_​PREV_​WEATHER_​TYPE_​HASH_​NAME);
                for (int i = 0; i < WeatherHashes.Length; i++)
                {
                    if (weatherHash == WeatherHashes[i])
                    {
                        return (WeatherTypes)i;
                    }
                }
                return WeatherTypes.EXTRASUNNY;
            }
            public static WeatherTypes GetNextWeatherEnum()
            {
                uint weatherHash = (uint)Function.Call<Hash>(Hash.GET_​NEXT_​WEATHER_​TYPE_​HASH_​NAME);
                for (int i = 0; i < WeatherHashes.Length; i++)
                {
                    if (weatherHash == WeatherHashes[i])
                    {
                        return (WeatherTypes)i;
                    }
                }
                return WeatherTypes.EXTRASUNNY;
            }
            public static bool IsCurrentWeatherType(WeatherTypes weatherType)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                return Function.Call<bool>(Hash.IS_​PREV_​WEATHER_​TYPE, WeatherType);
            }
            public static bool IsNextWeatherType(WeatherTypes weatherType)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                return Function.Call<bool>(Hash.IS_​NEXT_​WEATHER_​TYPE, WeatherType);
            }
            public static void SetWeatherTypePersist(WeatherTypes weatherType)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                Function.Call(Hash.SET_​WEATHER_​TYPE_​PERSIST, WeatherType);
            }
            public static void SetWeatherTypeNowPersist(WeatherTypes weatherType)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                Function.Call(Hash.SET_​WEATHER_​TYPE_​NOW_​PERSIST, WeatherType);
            }
            public static void SetWeatherTypeNow(WeatherTypes weatherType)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                Function.Call(Hash.SET_​WEATHER_​TYPE_​NOW, WeatherType);
            }
            public static void SetWeatherTypeOvertimePersist(WeatherTypes weatherType, float time)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                Function.Call(Hash.SET_​WEATHER_​TYPE_​OVERTIME_​PERSIST, WeatherType, time);
            }
            public static void SetRandomWeatherType()
            {
                Function.Call(Hash.SET_​RANDOM_​WEATHER_​TYPE);
            }
            public static void ClearWeatherTypePersist()
            {
                Function.Call(Hash.CLEAR_​WEATHER_​TYPE_​PERSIST);
            }
            public static unsafe void GetCurrWeatherState(Hash* weatherType1, Hash* weatherType2, float* percentWeather2)
            {
                Function.Call(Hash.GET_​CURR_​WEATHER_​STATE, weatherType1, weatherType2, percentWeather2);
            }
            /// <summary>
            /// https://nativedb.dotindustries.dev/gta5/natives/0x578C752848ECFA0C
            /// </summary>
            public static void SetCurrWeatherState(Hash weatherType1, Hash weatherType2, float percentWeather2)
            {
                Function.Call(Hash.SET_​CURR_​WEATHER_​STATE, weatherType1, weatherType2, percentWeather2);
            }
            public static void SetOverrideWeather(WeatherTypes weatherType)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                Function.Call(Hash.SET_​OVERRIDE_​WEATHER, WeatherType);
            }
            /// <summary>
            /// Identical to SET_OVERRIDE_WEATHER but has an additional BOOL param that sets some weather var to 0 if true
            /// </summary>
            public static void SetOverrideWeatherEx(WeatherTypes weatherType, bool p1)
            {
                string WeatherType = null;
                if (WeatherType == null)
                {
                    for (int i = 0; i < WeatherStr.Length; i++)
                    {
                        if ((int)weatherType == i)
                        {
                            WeatherType = WeatherStr[i];
                            break;
                        }
                    }
                }
                Function.Call(Hash.SET_​OVERRIDE_​WEATHEREX, WeatherType, p1);
            }
            public static void ClearOverrideWeather()
            {
                Function.Call(Hash.CLEAR_​OVERRIDE_​WEATHER);
            }
            /// <summary>
            /// https://nativedb.dotindustries.dev/gta5/natives/0x643E26EA6E024D92
            /// </summary>
            public static void SetRain(float intensity)
            {
                Function.Call(Hash.SET_​RAIN, intensity);
            }

        }
        public static uint[] WeatherHashes = new uint[]
{
            2544503417,
            916995460,
            821931868,
            282916021,
            2926802500,
            3146353965,
            1420204096,
            3061285535,
            1840358669,
            2764706598,
            4021743606,
            669657108,
            603685163,
            3373937154,
            2144126041,
            385726482
};
        public static string[] WeatherStr = new string[]
        {
            "EXTRASUNNY",
            "CLEAR",
            "CLOUDS",
            "smog",
            "foggy",
            "OVERCAST",
            "Rain",
            "THUNDER",
            "Clearing",
            "NEUTRAL",
            "Snow",
            "BLIZZARD",
            "SNOWLIGHT",
            "Halloween",
            "SNOW_HALLOWEEN",
            "RAIN_HALLOWEEN"
        };
        public enum WeatherTypes
        {
            EXTRASUNNY,
            CLEAR,
            CLOUDS,
            smog,
            foggy,
            OVERCAST,
            Rain,
            THUNDER,
            Clearing,
            NEUTRAL,
            Snow,
            BLIZZARD,
            SNOWLIGHT,
            Halloween,
            SNOW_HALLOWEEN,
            RAIN_HALLOWEEN
        }

        public class fZone
        {
            public static int GetZoneAtCoords(Vector3 xyz)
            {
                return Function.Call<int>(Hash.GET_​ZONE_​AT_​COORDS, xyz.X, xyz.Y, xyz.Z);
            }
            public static int GetZoneFromNameID(string zoneName)
            {
                return Function.Call<int>(Hash.GET_​ZONE_​FROM_​NAME_​ID, zoneName);
            }
            public static int GetZonePopschedule(int zoneId)
            {
                return Function.Call<int>(Hash.GET_​ZONE_​POPSCHEDULE, zoneId);
            }
            public static string GetNameOfZone(Vector3 xyz)
            {
                return Function.Call<string>(Hash.GET_​NAME_​OF_​ZONE, xyz.X, xyz.Y, xyz.Z);
            }
            public static void SetZoneEnabled(int zoneId, bool toggle)
            {
                Function.Call(Hash.SET_​ZONE_​ENABLED, zoneId, toggle);
            }
            /// <summary>
            /// cellphone range 1- 5 used for signal bar in iFruit phone
            /// </summary>
            public static int GetZoneScumminess(int zoneId)
            {
                return Function.Call<int>(Hash.GET_​ZONE_​SCUMMINESS, zoneId);
            }
            public static void OverridePopscheduleVehicleModel(int scheduleId, Hash vehicleHash)
            {
                Function.Call(Hash.OVERRIDE_​POPSCHEDULE_​VEHICLE_​MODEL, scheduleId, vehicleHash);
            }
            public static void ClearPopscheduleOverrideVehicleModel(int scheduleId)
            {
                Function.Call(Hash.CLEAR_​POPSCHEDULE_​OVERRIDE_​VEHICLE_​MODEL, scheduleId);
            }
            public static Hash GetHashOfMapAreaAtCoords(Vector3 xyz)
            {
                return Function.Call<Hash>(Hash.GET_​HASH_​OF_​MAP_​AREA_​AT_​COORDS, xyz.X, xyz.Y, xyz.Z);
            } 
        }

        public class fClock
        {
            public static int ReturnNextDayOfWeek()
            {
                int dwVar = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_WEEK);
                if (dwVar == 0)
                {
                    return 1;
                }
                if (dwVar == 1)
                {
                    return 2;
                }
                if (dwVar == 2)
                {
                    return 3;
                }
                if (dwVar == 3)
                {
                    return 4;
                }
                if (dwVar == 4)
                {
                    return 5;
                }
                if (dwVar == 5)
                {
                    return 6;
                }
                if (dwVar == 6)
                {
                    return 0;
                }
                return -1;
            }

            public static void SetClockTime(int hour =0, int minute=0, int second=0, bool ease = false, int timeEaseAmountMin = 1)
            {
                if (!ease)
                    Function.Call(Hash.SET_CLOCK_TIME, hour, minute, second);
                else
                {
                    for (int time = timeEaseAmountMin; GetClockHours() != hour && GetClockMinutes() != minute; AddToClockTime(0, time, 0))
                    {
                        Script.Wait(0);
                    }
                    if (GetClockHours() == hour && GetClockMinutes() > (minute - 10) && GetClockMinutes() < (minute + 10))
                    {
                        SetClockTime(hour, minute, second);
                    }
                }
            }
            public static void PauseClock(bool toggle)
            {
                Function.Call(Hash.PAUSE_​CLOCK, toggle);
            }
            public static void AdvanceClockTimeTo(int hour=0, int minute=0, int second = 0)
            {
                Function.Call(Hash.ADVANCE_​CLOCK_​TIME_​TO, hour, minute, second);
            }
            public static void AddToClockTime(int hours =0, int minutes =0, int seconds = 0)
            {
                Function.Call(Hash.ADD_​TO_​CLOCK_​TIME, hours, minutes, seconds);
            }
            public static int GetClockHours()
            {
                return Function.Call<int>(Hash.GET_CLOCK_HOURS);
            }
            public static int GetClockMinutes()
            {
                return Function.Call<int>(Hash.GET_CLOCK_MINUTES);
            }
            public static int GetClockSeconds()
            {
                return Function.Call<int>(Hash.GET_CLOCK_SECONDS);
            }
            public static void SetClockDate(int day, int month, int year)
            {
                Function.Call(Hash.SET_​CLOCK_​DATE, day, month, year);
            }
            public static int GetClockDayOfWeek()
            {
                return Function.Call<int>(Hash.GET_​CLOCK_​DAY_​OF_​WEEK);
            }
            public static int GetClockDayOfMonth()
            {
                return Function.Call<int>(Hash.GET_​CLOCK_​DAY_​OF_​MONTH);
            }
            public static int GetClockMonth()
            {
                return Function.Call<int>(Hash.GET_​CLOCK_​MONTH);
            }
            public static int GetClockYear()
            {
                return Function.Call<int>(Hash.GET_​CLOCK_​YEAR);
            }
            public static int GetMillisecondsPerGameMinute()
            {
                return Function.Call<int>(Hash.GET_​MILLISECONDS_​PER_​GAME_​MINUTE);
            }
            public static unsafe void GetPOSIXTime(int* year, int* month, int* day, int* hour, int* minute, int* second) // Gets System Time
            {
                Function.Call(Hash.GET_​POSIX_​TIME, year, month, day, hour, minute, second);
            }
            public static unsafe void GetUTCTime(int* year, int* month, int* day, int* hour, int* minute, int* second) // Gets current UTC time
            {
                Function.Call(Hash.GET_​UTC_​TIME, year, month, day, hour, minute, second);
            }
            public static unsafe void GetLocalTime(int* year, int* month, int* day, int* hour, int* minute, int* second) // Gets Local System Time
            {
                Function.Call(Hash.GET_​LOCAL_​TIME, year, month, day, hour, minute, second);
            }
        }

        public class fRespawn : Script
        {
            static Wanted PlayerWantedLevel;
            public class Vector4
            {
                public Vector4(float x, float y, float z, float h)
                {
                    X = x;
                    Y = y;
                    Z = z;
                    H = h;
                }

                public static Vector4 Zero
                {
                    get
                    {
                        return new Vector4(0f, 0f, 0f, 0f);
                    }
                }

                public float X;

                public float Y;

                public float Z;

                public float H;
            }

            public static int ReturnWantedLevel = 0;

            public static int ReturnHour = fClock.GetClockHours();

            public static int ReturnMinute = fClock.GetClockMinutes();

            public static int ReturnSecond = fClock.GetClockSeconds();

            public static Vector4 playerSpawn = null;

            public enum Spawnpointflags
            {
                SPAWNPOINTS_FLAG_DEFAULT,
                SPAWNPOINTS_FLAG_MAY_SPAWN_IN_INTERIOR,
                SPAWNPOINTS_FLAG_MAY_SPAWN_IN_EXTERIOR,
                SPAWNPOINTS_FLAG_ALLOW_NOT_NETWORK_SPAWN_CANDIDATE_POLYS = 4,
                SPAWNPOINTS_FLAG_ALLOW_ISOLATED_POLYS = 8,
                SPAWNPOINTS_FLAG_ALLOW_ROAD_POLYS = 16,
                SPAWNPOINTS_FLAG_ONLY_POINTS_AGAINST_EDGES = 32
            }
            public fRespawn()
            {
                Tick += OnTick;
            }

            private void OnTick(object sender, EventArgs e)
            {
                if (GlobalVariable.Get(5).Read<int>() == 1)
                {
                    RespawnControl();
                }
            }
            public unsafe static void RespawnControl()
            {
                int num = Game.GameTime + 2000;
                bool flag = false;
                if (Game.Player.Character.IsDead)
                {
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                    Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED");
                    Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED", false, -1);
                    Function.Call(Hash.START_AUDIO_SCENE, "DEATH_SCENE");
                    Script.Wait(50);
                    Scaleform scaleform = Scaleform.RequestMovie("MP_BIG_MESSAGE_FREEMODE");
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds", true);
                    GameplayCamera.Shake(CameraShake.DeathFail, 1.5f);
                    fGraphics.AnimpostFXPlay("DeathFailMPIn", 0, false);
                    int red = 255;
                    int num2 = 0;
                    int num3 = 0;
                    while (!Screen.IsFadedOut)
                    {
                        if (Game.GameTime > num)
                        {
                            scaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", new object[]
                            {
                    fGraphics.ToColorHexString(Color.FromArgb(255, red, num2, num3), "WASTED"),
                    "",
                    0,
                    true,
                    true
                            });
                            scaleform.Render2D();
                            if (!flag)
                            {
                                fGraphics.SetTransitionTimecycleModifier("NG_deathfail_BW_base", 10f);
                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TextHit", "WastedSounds", true);
                                flag = true;
                            }
                            else
                            {
                                if (num2 < 255)
                                {
                                    num2 += 5;
                                }
                                if (num3 < 255)
                                {
                                    num3 += 5;
                                }
                            }
                        }
                        Script.Yield();
                    }
                    while (!Screen.IsFadingIn)
                    {
                        scaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", new object[]
                        {
                fGraphics.ToColorHexString(Color.FromArgb(255, red, num2, num3), "WASTED"),
                "",
                0,
                true,
                true
                        });
                        scaleform.Render2D();
                        Script.Yield();
                    }
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                    Function.Call(Hash.STOP_AUDIO_SCENE, "DEATH_SCENE");
                    Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED");
                    scaleform.Dispose();
                    fClock.SetClockTime(ReturnHour, ReturnMinute, ReturnSecond);
                    Screen.StopEffects();
                    fGraphics.AnimpostFXStopAll();
                    fGraphics.ClearTimecycleModifier();
                    GameplayCamera.StopShaking();
                    fHud.RadarAndHud(true, true);
                    if (playerSpawn == null)
                    {
                        Vector3 position = Game.Player.Character.Position;
                        int num4 = 2;
                        float value = 150f;
                        if (fInterior.GetInteriorFromEntity(Game.Player.Character) > 0U)
                        {
                            value = 150f;
                            num4 = 1;
                        }
                        if (Function.Call<bool>(Hash.SPAWNPOINTS_IS_SEARCH_ACTIVE))
                        {
                            Function.Call(Hash.SPAWNPOINTS_CANCEL_SEARCH);
                        }
                        Function.Call(Hash.SPAWNPOINTS_START_SEARCH, position.X, position.Y, position.Z, value, 5f, 24 | num4 | 32, -1f, 20000);
                        while (!Function.Call<bool>(Hash.SPAWNPOINTS_IS_SEARCH_COMPLETE))
                        {
                            Script.Wait(0);
                        }
                        float GroundZ;
                        float GroundZ2;
                        int num5 = Function.Call<int>(Hash.SPAWNPOINTS_GET_NUM_SEARCH_RESULTS);
                        int value2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, num5);
                        Function.Call(Hash.SPAWNPOINTS_GET_SEARCH_RESULT, value2, &position.X, &position.Y, &position.Z);
                        World.GetGroundHeight(new Vector3(position.X, position.Y, position.Z), out GroundZ, GetGroundHeightMode.Normal);
                        playerSpawn = new Vector4(position.X, position.Y, GroundZ, 0f);
                        World.GetGroundHeight(new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z), out GroundZ2, GetGroundHeightMode.Normal);
                        if (num5 == 0)
                        {
                            playerSpawn = new Vector4(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, GroundZ2, 0f);
                        }
                    }
                    fPlayer.PedPos(playerSpawn.X, playerSpawn.Y, playerSpawn.Z, playerSpawn.H);
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                    playerSpawn = null;
                    PlayerWantedLevel.SetWantedLevel(ReturnWantedLevel, false);
                    Script.Wait(2000);
                    Screen.FadeIn(500);
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                }
            }

            public static void SetRespawnStat(bool toggle)
            {
                if (toggle)
                {
                    GlobalVariable.Get(5).Write<int>(1);
                    Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                    fMisc.DisableHospitalRestart(0, true);
                    fMisc.DisableHospitalRestart(1, true);
                    fMisc.DisableHospitalRestart(2, true);
                    fMisc.DisableHospitalRestart(3, true);
                    fMisc.DisableHospitalRestart(4, true);
                    fMisc.DisableHospitalRestart(5, true);
                    fMisc.DisablePoliceRestart(0, true);
                    fMisc.DisablePoliceRestart(1, true);
                    fMisc.DisablePoliceRestart(2, true);
                    fMisc.DisablePoliceRestart(3, true);
                    fMisc.DisablePoliceRestart(4, true);
                    fMisc.DisablePoliceRestart(5, true);
                    fMisc.DisablePoliceRestart(6, true);
                }
                else
                {
                    GlobalVariable.Get(5).Write<int>(0);
                    Function.Call(Hash.REQUEST_SCRIPT, "respawn_controller");
                    while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, "respawn_controller"))
                        Script.Wait(0);
                    Function.Call<int>(Hash.START_NEW_SCRIPT, "respawn_controller", 128);
                    Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, "respawn_controller");
                    fMisc.DisableHospitalRestart(0, false);
                    fMisc.DisableHospitalRestart(1, false);
                    fMisc.DisableHospitalRestart(2, false);
                    fMisc.DisableHospitalRestart(3, false);
                    fMisc.DisableHospitalRestart(4, false);
                    fMisc.DisableHospitalRestart(5, false);
                    fMisc.DisablePoliceRestart(0, false);
                    fMisc.DisablePoliceRestart(1, false);
                    fMisc.DisablePoliceRestart(2, false);
                    fMisc.DisablePoliceRestart(3, false);
                    fMisc.DisablePoliceRestart(4, false);
                    fMisc.DisablePoliceRestart(5, false);
                    fMisc.DisablePoliceRestart(6, false);
                }
            }
        }

        public class fPlayer
        {
            public static bool IsFreemodePed
            {
                get
                {
                    uint hash = (uint)Game.Player.Character.Model.Hash;
                    if (hash != 1885233650U/*M*/) // Freemode Peds
                    {
                        if (hash != 2627665880U/*F*/)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public static bool IsFreemodeMale;

            public static bool IsFreemodeFemale;

            public static PedHash PlayerPreviousModel;

            public static bool IsMainCharacterPeds
            {
                get
                {
                    uint hash = (uint)Game.Player.Character.Model.Hash;
                    if (hash != 225514697U) // michael
                    {
                        if (hash != 2602752943U) // franklin
                        {
                            if (hash != 2608926626U) // trevor
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public static Ped ped
            {
                get
                {
                    return Game.Player.Character;
                }
            }

            public static int GetPlayerWantedLevel(Player player)
            {
                return Function.Call<int>(Hash.GET_​PLAYER_​WANTED_​LEVEL, player);
            }
            public static void SetPlayerWantedLevel(Player player, int wantedLevel, bool disableNoMission = false)
            {
                Function.Call(Hash.SET_​PLAYER_​WANTED_​LEVEL, player, wantedLevel, disableNoMission);
            }

            public static bool IsWanted
            {
                get
                {
                    return GetPlayerWantedLevel(Game.Player) < 0;
                }
            }

            public static int WantedLevel
            {
                get
                {
                    return GetPlayerWantedLevel(Game.Player);
                }
                set
                {
                    SetPlayerWantedLevel(Game.Player, value, false);
                }
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

            public static void SetWantedLevelTo0()
            {
                WantedLevel = 0;
            }
            public static void SetMaxWantedLevelTo0()
            {
                MaxWantedLevel = 0;
            }
            public static void SetMaxWantedLevelToNormal()
            {
                MaxWantedLevel = 5;
            }

            public static float GetDistanceTo(Vector3 position)
            {
                return Game.Player.Character.Position.DistanceTo(position);
            }
            public static float GetCarDistanceTo(Vector3 position)
            {
                if (Game.Player.Character.CurrentVehicle != null)
                {
                    return Game.Player.Character.CurrentVehicle.Position.DistanceTo(position);
                }
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

            public static uint GetPlayerModelHash()
            {
                return (uint)Game.Player.Character.Model.Hash;
            }
            public unsafe static void PlayerModelSet(Ped ped)
            {
                bool flag = ped.Model == PedHash.FreemodeMale01;
                if (flag)
                {
                    IsFreemodeMale = true;
                    IsFreemodeFemale = false;
                }
                else
                {
                    bool flag2 = ped.Model == (PedHash)2627665880U;
                    if (flag2)
                    {
                        IsFreemodeMale = false;
                        IsFreemodeFemale = true;
                    }
                    else
                    {
                        IsFreemodeMale = false;
                        IsFreemodeFemale = false;
                        PlayerPreviousModel = ped.Model;
                    }
                }
                ulong num = (ulong)((long)ped.MemoryAddress);
                ulong num2 = num + 32UL;
                *(long*)(num2 + 24UL) = -1080659212L; // Corrected to use a pointer cast
            }

            public unsafe static void PlayerModelSetBack(Ped ped)
            {
                bool isFreemodeMale = IsFreemodeMale;
                if (isFreemodeMale)
                {
                    ulong num = (ulong)((long)ped.MemoryAddress);
                    ulong num2 = num + 32UL;
                    *(long*)(num2 + 24UL) = 1885233650L; // Corrected to use a pointer cast
                }
                else
                {
                    bool isFreemodeFemale = IsFreemodeFemale;
                    if (isFreemodeFemale)
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

            public static void PedPos(float X, float Y, float Z, float heading)
            {
                Game.Player.Character.Position = new Vector3(X, Y, Z);
                Game.Player.Character.Heading = heading;
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

        public class fAnimations
        {
            public static bool IsSynchronizedSceneRunning(int sceneID)
            {
                return Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, sceneID);
            }
            public static bool IsEntityPlayingAnim(Entity entity, string animDict, string animName, int taskFlag = 3)
            {
                return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, entity, animDict, animName, taskFlag);
            }
            public static int CreateSynchronizedScene(Vector3 xyz, float roll = 0, float pitch = 0, float yaw = 0, int p6 = 2)
            {
                return Function.Call<int>(Hash.CREATE_​SYNCHRONIZED_​SCENE, xyz.X, xyz.Y, xyz.Z, roll, pitch, yaw, p6);
            }
            public static void SetSynchronizedScenePhase(int sceneID, float phase)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, sceneID, phase);
            }
            public static float GetSynchronizedScenePhase(int sceneID)
            {
                return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, sceneID);
            }
            public static void SetSynchronizedSceneRate(int sceneID, float rate)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, sceneID, rate);
            }
            public static float GetSynchronizedSceneRate(int sceneID)
            {
                return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, sceneID);
            }
            public static void SetSynchronizedSceneLooped(int sceneID, bool toggle)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sceneID, toggle);
            }
            public static bool IsSynchronizedSceneLooped(int sceneID)
            {
                return Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_LOOPED, sceneID);
            }
            public static void SetSynchronizedSceneHoldLastFrame(int sceneID, bool toggle)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, sceneID, toggle);
            }
            public static bool IsSynchronizedSceneHoldLastFrame(int sceneID)
            {
                return Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, sceneID);
            }
            public static void ForceEntityAiAndAnimationUpdate(Entity entity)
            {
                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, entity);
            }
            public static void ForcePedAiAndAnimationUpdate(Ped ped, bool p1, bool p2)
            {
                Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, ped, p1, p2);
            }
            public static void PlaySynchronizedEntityAnim(Entity entity, int syncedScene, string animation, string AnimDict, float p5, int p6, float p4 = 1000.0f, float p7 = 1000.0f)
            {
                Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, entity, syncedScene, animation, AnimDict, p4, p5, p6, p7);
            }
            public static void TaskPlayAnim(Ped ped, string AnimDictName, string AnimName, float BlendInDelta = 8f, float BlendOutDelta = 8f, int timeToPlay = -1,
                AnimFlags AnimFlags = AnimFlags.AF_DEFAULT, float startPhase = 0f, bool phaseControlled = false, IkControlFlags ikFlags = IkControlFlags.AIK_NONE, bool allowOverrideCloneUpdate = false)
            {
                Function.Call(Hash.TASK_PLAY_ANIM, ped, AnimDictName, AnimName, BlendInDelta, BlendOutDelta, timeToPlay, AnimFlags, startPhase, phaseControlled, ikFlags, allowOverrideCloneUpdate);
            }
            public static void TaskPlayAnimAdvanced(Ped ped, string AnimDictName, string AnimName, Vector3 pos, Vector3 rot, float BlendInDelta = 8f, float BlendOutDelta = 8f,
                int TimeToPlay = -1, AnimFlags AnimFlags = AnimFlags.AF_DEFAULT, float startPhase = 0f, int RotOrder = 2, IkControlFlags ikFlags = IkControlFlags.AIK_NONE)
            {
                Function.Call(Hash.TASK_PLAY_ANIM_ADVANCED, ped, AnimDictName, AnimName, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, BlendInDelta, BlendOutDelta, TimeToPlay, AnimFlags, startPhase, RotOrder, ikFlags);
            }
            public static float GetAnimDuration(string animDict, string animName)
            {
                return Function.Call<float>(Hash.GET_ANIM_DURATION, animDict, animName);
            }
            public static float GetEntityAnimCurrentTime(Entity entity, string animDict, string animName)
            {
                return Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, entity, animDict, animName);
            }
            public static float GetEntityAnimTotalTime(Entity entity, string animDict, string animName)
            {
                return Function.Call<float>(Hash.GET_ENTITY_ANIM_TOTAL_TIME, entity, animDict, animName);
            }
            public static void TaskSynchronizedScene(Ped ped, int sceneID, string animDict, string animName, float blendIn, float blendOut, SyncScenePlaybackFlags flags, int ragdollBlockFlags, float moverBlendDelta, int ikFlags)
            {
                Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ped, sceneID, animDict, animName, blendIn, blendOut, flags, ragdollBlockFlags, moverBlendDelta, ikFlags);
            }
            public static void TakeOwnershipOfSynchronizedScene(int sceneID)
            {
                Function.Call(Hash.TAKE_OWNERSHIP_OF_SYNCHRONIZED_SCENE, sceneID);
            }

            public enum SyncScenePlaybackFlags
            {
                SYNCED_SCENE_NONE,
                SYNCED_SCENE_USE_PHYSICS,
                SYNCED_SCENE_TAG_SYNC_OUT,
                SYNCED_SCENE_DONT_INTERRUPT = 4,
                SYNCED_SCENE_ON_ABORT_STOP_SCENE = 8,
                SYNCED_SCENE_ABORT_ON_WEAPON_DAMAGE = 16,
                SYNCED_SCENE_BLOCK_MOVER_UPDATE = 32,
                SYNCED_SCENE_LOOP_WITHIN_SCENE = 64,
                SYNCED_SCENE_PRESERVE_VELOCITY = 128,
                SYNCED_SCENE_EXPAND_PED_CAPSULE_FROM_SKELETON = 256,
                SYNCED_SCENE_ACTIVATE_RAGDOLL_ON_COLLISION = 512,
                SYNCED_SCENE_HIDE_WEAPON = 1024,
                SYNCED_SCENE_ABORT_ON_DEATH = 2048,
                SYNCED_SCENE_VEHICLE_ABORT_ON_LARGE_IMPACT = 4096,
                SYNCED_SCENE_VEHICLE_ALLOW_PLAYER_ENTRY = 8192,
                SYNCED_SCENE_PROCESS_ATTACHMENTS_ON_START = 16384,
                SYNCED_SCENE_NET_ON_EARLY_NON_PED_STOP_RETURN_TO_START = 32768,
                SYNCED_SCENE_SET_PED_OUT_OF_VEHICLE_AT_START = 65536,
                SYNCED_SCENE_NET_DISREGARD_ATTACHMENT_CHECKS = 131072
            }
            public enum AnimFlags
            {
                AF_DEFAULT,
                AF_LOOPING,
                AF_HOLD_LAST_FRAME,
                AF_REPOSITION_WHEN_FINISHED = 4,
                AF_NOT_INTERRUPTABLE = 8,
                AF_UPPERBODY = 16,
                AF_SECONDARY = 32,
                AF_REORIENT_WHEN_FINISHED = 64,
                AF_ABORT_ON_PED_MOVEMENT = 128,
                AF_ADDITIVE = 256,
                AF_TURN_OFF_COLLISION = 512,
                AF_OVERRIDE_PHYSICS = 1024,
                AF_IGNORE_GRAVITY = 2048,
                AF_EXTRACT_INITIAL_OFFSET = 4096,
                AF_EXIT_AFTER_INTERRUPTED = 8192,
                AF_TAG_SYNC_IN = 16384,
                AF_TAG_SYNC_OUT = 32768,
                AF_TAG_SYNC_CONTINUOUS = 65536,
                AF_FORCE_START = 131072,
                AF_USE_KINEMATIC_PHYSICS = 262144,
                AF_USE_MOVER_EXTRACTION = 524288,
                AF_HIDE_WEAPON = 1048576,
                AF_ENDS_IN_DEAD_POSE = 2097152,
                AF_ACTIVATE_RAGDOLL_ON_COLLISION = 4194304,
                AF_DONT_EXIT_ON_DEATH = 8388608,
                AF_ABORT_ON_WEAPON_DAMAGE = 16777216,
                AF_DISABLE_FORCED_PHYSICS_UPDATE = 33554432,
                AF_PROCESS_ATTACHMENTS_ON_START = 67108864,
                AF_EXPAND_PED_CAPSULE_FROM_SKELETON = 134217728,
                AF_USE_ALTERNATIVE_FP_ANIM = 268435456,
                AF_BLENDOUT_WRT_LAST_FRAME = 536870912,
                AF_USE_FULL_BLENDING = 1073741824
            }
            public enum IkControlFlags
            {
                AIK_NONE,
                AIK_DISABLE_LEG_IK,
                AIK_DISABLE_ARM_IK,
                AIK_DISABLE_HEAD_IK = 4,
                AIK_DISABLE_TORSO_IK = 8,
                AIK_DISABLE_TORSO_REACT_IK = 16,
                AIK_USE_LEG_ALLOW_TAGS = 32,
                AIK_USE_LEG_BLOCK_TAGS = 64,
                AIK_USE_ARM_ALLOW_TAGS = 128,
                AIK_USE_ARM_BLOCK_TAGS = 256,
                AIK_PROCESS_WEAPON_HAND_GRIP = 512,
                AIK_USE_FP_ARM_LEFT = 1024,
                AIK_USE_FP_ARM_RIGHT = 2048,
                AIK_DISABLE_TORSO_VEHICLE_IK = 4096,
                AIK_LINKED_FACIAL = 8192
            }
        }

        public class fDebugStuff
        {
            public static int DebugSceneID = 0;
            public static int DebugSceneID2 = 0;
            public static List<Vehicle> DebugVehicles = new List<Vehicle>();
            public static List<string> DebugAnimDicts = new List<string>();
            public static List<Prop> DebugProps = new List<Prop>();

            [STAThread]
            public static void CopyPlayerPosWithAddons()
            {
                void CopyToClipboard(string text)
                {
                    GTA.UI.Screen.ShowSubtitle("~p~Copied~s~: to clipboard!");
                    Thread thread = new Thread((ThreadStart)(() => Clipboard.SetText(text)));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                Vector3 GetPlayerPosition()
                {
                    Vector3 PlayerPos = new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);
                    return PlayerPos;
                }
                Vector3 vector = GetPlayerPosition();
                string vectorX = vector.X.ToString();
                string vectorY = vector.Y.ToString();
                string vectorZ = vector.Z.ToString();
                string XYZ = vector.ToString().Replace("X:", string.Empty).Replace("Y:", string.Empty).Replace("Z:", string.Empty);
                string Text2 = XYZ.Replace(vectorX, vectorX + "f,");
                string Text3 = Text2.Replace(vectorY, vectorY + "f,");
                string Final = Text3.Replace(vectorZ, vectorZ + "f");
                CopyToClipboard(Final);
            }

            [STAThread]
            public static void CopyToClipboard(string text)
            {
                GTA.UI.Screen.ShowSubtitle("~p~Copied~s~: to clipboard!");
                Thread thread = new Thread((ThreadStart)(() => Clipboard.SetText(text)));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

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

        public class fMath
        {
            public static int CEIL(float value)
            {
                return Function.Call<int>(Hash.CEIL, value);
            }
        }

        public class fMissionShard : Script
        {
            public unsafe void DeleteScaleformID()
            {
                int num = scaleID;
                Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
                scaleID = 0;
            }

            public void RequestScaleformID()
            {
                scaleID = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MIDSIZED_MESSAGE");
                while (!fScaleforms.HasScaleformMovieLoaded(scaleID))
                {
                    Script.Wait(0);
                }
            }
            public void DrawShard()
            {
                if (!Game.Player.Character.IsDead)
                {
                    Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleID, 255, 255, 255, 255, 0);
                }
            }

            public void Shard_In(string ShardName, string ShardDescription, int color = 2, float speed = 0.5f, int colorout = 0, bool failShard = false)
            {
                DeleteScaleformID();
                RequestScaleformID();
                Script.Wait(500);
                fScaleforms.CallFunction(scaleID, "SHOW_SHARD_MIDSIZED_MESSAGE", ShardName, ShardDescription, color, false, true);
                if (failShard)
                {
                    Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MP2023_1/DLC_MP2023_1_Bicycle_Race", true, -1);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Fail", "Bike_Time_Trials_Soundset", true);
                }
                else
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, shardInSoundName, shardInSoundSet, true);
                int num = Game.GameTime + 7000;
                while (Game.GameTime < num)
                {
                    DrawShard();
                    Script.Wait(0);
                }
                Shard_Out(colorout, speed);
                num = Game.GameTime + 2000;
                while (Game.GameTime < num)
                {
                    DrawShard();
                    Script.Wait(0);
                }
                DeleteScaleformID();
                Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_MP2023_1/DLC_MP2023_1_Bicycle_Race");
            }

            public void Shard_Out(int color, float speed)
            {
                fScaleforms.CallFunction(scaleID, "SHARD_ANIM_OUT", color, speed);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, shardOutSoundName, shardOutSoundSet, true);
            }

            public void Cleanup()
            {
                if (scaleID != 0)
                {
                    DeleteScaleformID();
                }
            }

            public int scaleID = 0;

            public string shardInSoundName = "Shard_Appear";

            public string shardInSoundSet = "GTAO_FM_Events_Soundset";

            public string shardOutSoundName = "Shard_Disappear";

            public string shardOutSoundSet = "GTAO_FM_Events_Soundset";

        }

        public class fAudio
        {
            public static bool TriggerMusicEvent(string eventName)
            {
                return Function.Call<bool>(Hash.TRIGGER_MUSIC_EVENT, eventName);
            }
            public static bool PrepareMusicEvent(string eventName)
            {
                return Function.Call<bool>(Hash.PREPARE_​MUSIC_​EVENT, eventName);
            }
            public static bool AudioIsMusicPlaying()
            {
                return Function.Call<bool>(Hash.AUDIO_​IS_​MUSIC_​PLAYING);
            }
            public static void ChangeMusicEventIntensity(MusicEventIntensity intensity)
            {
                if (intensity == MusicEventIntensity.StaffProblemsCroupierChaseStart)
                    TriggerMusicEvent("CH_STAFF_PROBLEMS_CROUPIER_CHASE_START");
                if (intensity == MusicEventIntensity.IdleStart)
                    TriggerMusicEvent("CH_IDLE_START");
                if (intensity == MusicEventIntensity.MedIntensityStart)
                    TriggerMusicEvent("CH_MED_INTENSITY_START");
                if (intensity == MusicEventIntensity.GunfightStart)
                    TriggerMusicEvent("CH_GUNFIGHT_START");
                if (intensity == MusicEventIntensity.DeliveringStart)
                    TriggerMusicEvent("HEI4_DELIVERING_START");
                if (intensity == MusicEventIntensity.SuspenseStart)
                    TriggerMusicEvent("HEI4_SUSPENSE_START");
                if (intensity == MusicEventIntensity.Suspense)
                    TriggerMusicEvent("CH_SUSPENSE");
                if (intensity == MusicEventIntensity.MedIntensity)
                    TriggerMusicEvent("CH_MED_INTENSITY");
                if (intensity == MusicEventIntensity.Delivering)
                    TriggerMusicEvent("CH_DELIVERING");
                if (intensity == MusicEventIntensity.Gunfight)
                    TriggerMusicEvent("CH_GUNFIGHT");
                if (intensity == MusicEventIntensity.VehicleAction)
                    TriggerMusicEvent("CH_VEHICLE_ACTION");
                if (intensity == MusicEventIntensity.Idle)
                    TriggerMusicEvent("CH_IDLE");
                if (intensity == MusicEventIntensity.Silent)
                    TriggerMusicEvent("CH_SILENT");
                if (intensity == MusicEventIntensity.Fail)
                    TriggerMusicEvent("CH_FAIL");
                if (intensity == MusicEventIntensity.MusicStop)
                    TriggerMusicEvent("CH_MUSIC_STOP");
            }
            public static void PrepareMusicEventIntensity(MusicEventIntensity intensity)
            {
                if (intensity == MusicEventIntensity.StaffProblemsCroupierChaseStart)
                    PrepareMusicEvent("CH_STAFF_PROBLEMS_CROUPIER_CHASE_START");
                if (intensity == MusicEventIntensity.IdleStart)
                    PrepareMusicEvent("CH_IDLE_START");
                if (intensity == MusicEventIntensity.MedIntensityStart)
                    PrepareMusicEvent("CH_MED_INTENSITY_START");
                if (intensity == MusicEventIntensity.GunfightStart)
                    PrepareMusicEvent("CH_GUNFIGHT_START");
                if (intensity == MusicEventIntensity.DeliveringStart)
                    PrepareMusicEvent("HEI4_DELIVERING_START");
                if (intensity == MusicEventIntensity.SuspenseStart)
                    PrepareMusicEvent("HEI4_SUSPENSE_START");
                if (intensity == MusicEventIntensity.Suspense)
                    PrepareMusicEvent("CH_SUSPENSE");
                if (intensity == MusicEventIntensity.MedIntensity)
                    PrepareMusicEvent("CH_MED_INTENSITY");
                if (intensity == MusicEventIntensity.Delivering)
                    PrepareMusicEvent("CH_DELIVERING");
                if (intensity == MusicEventIntensity.Gunfight)
                    PrepareMusicEvent("CH_GUNFIGHT");
                if (intensity == MusicEventIntensity.VehicleAction)
                    PrepareMusicEvent("CH_VEHICLE_ACTION");
                if (intensity == MusicEventIntensity.Idle)
                    PrepareMusicEvent("CH_IDLE");
                if (intensity == MusicEventIntensity.Silent)
                    PrepareMusicEvent("CH_SILENT");
                if (intensity == MusicEventIntensity.Fail)
                    PrepareMusicEvent("CH_FAIL");
                if (intensity == MusicEventIntensity.MusicStop)
                    PrepareMusicEvent("CH_MUSIC_STOP");
            }
            public static void PlayPedAmbientSpeechNative(Ped ped, string speechName, string speechParam, int p3 = 1)
            {
                Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, ped, speechName, speechParam, p3);
            }
            public static void SetUserRadioControlEnabled(bool toggle)
            {
                Function.Call(Hash.SET_USER_RADIO_CONTROL_ENABLED, toggle);
            }

            public enum MusicEventIntensity
            {
                StaffProblemsCroupierChaseStart,
                IdleStart,
                MedIntensityStart,
                GunfightStart,
                DeliveringStart,
                SuspenseStart,
                Suspense,
                MedIntensity,
                Delivering,
                Gunfight,
                VehicleAction,
                Idle,
                Silent,
                Fail,
                MusicStop
            }
        }

        public class fStreaming
        {
            public static string RequestAnimDict(string dict, List<string> animDictList = null)
            {
                if (dict != string.Empty)
                {
                    if (animDictList != null)
                    {
                        animDictList.Add(dict);
                    }
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
                    while (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
                    {
                        Function.Call(Hash.REMOVE_ANIM_DICT, dict);
                        Script.Yield();
                    }
                }
            }
            public static void RemovePTFXAsset()
            {
                Function.Call(Hash.REMOVE_PTFX_ASSET);
            }


        }

        public class fCam
        {
            public static void ShakeCam(Camera cam, string type, float amplitude)
            {
                Function.Call(Hash.SHAKE_CAM, cam, type, amplitude);
            }
            public static void SetCamFov(Camera cam, float fov)
            {
                Function.Call(Hash.SET_CAM_FOV, cam, fov);
            }
            public static Camera CreateCam(string camName, bool createCamera = true)
            {
                return Function.Call<Camera>(Hash.CREATE_CAM, camName, createCamera);
            }
            public static Camera CreateScriptedCam()
            {
                Camera camera = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
                while (camera != null && !camera.Exists())
                {
                    Script.Wait(0);
                }
                return camera;
            }
            public static void SetupMovingCam(Camera cam, Vector3 pos, Vector3 rot, float fov, CameraShake camShakeType, float camShakeIntensity)
            {
                cam.Detach();
                cam.StopPointing();
                cam.Position = pos;
                cam.Rotation = rot;
                Function.Call(Hash.SET_CAM_FOV, cam, fov);
                cam.Shake(camShakeType, camShakeIntensity);
            }
            public static void SetCamActiveWithInterp(Camera DestinationCam, Camera OriginCam, int Duration, CamGraphType GraphTypePos = CamGraphType.GRAPH_TYPE_SIN_ACCEL_DECEL, CamGraphType GraphTypeRot = CamGraphType.GRAPH_TYPE_SIN_ACCEL_DECEL)
            {
                Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, DestinationCam, OriginCam, Duration, GraphTypePos, GraphTypeRot);
            }
            public static void RenderScriptCams(bool render, bool ease, int easeTime, bool bShouldLockInterpolationSourceFrame = true, bool bShouldApplyAcrossAllThreads = false, RenderingOptionFlag RenderingOptions = RenderingOptionFlag.RO_NO_OPTIONS)
            {
                Function.Call(Hash.RENDER_SCRIPT_CAMS, render, ease, easeTime, bShouldLockInterpolationSourceFrame, bShouldApplyAcrossAllThreads, RenderingOptions);
            }
            public static bool PlayCamAnim(Camera cam, string animName, string animDict, Vector3 pos, Vector3 rot, CamAnimationFlags animFlags, int rotOrder = 2)
            {
                return Function.Call<bool>(Hash.PLAY_CAM_ANIM, cam, animName, animDict, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, animFlags, rotOrder);
            }
            public static bool PlaySynchronizedCamAnim(Camera cam, int sceneID, string animName, string animDictionary)
            {
                return Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, cam, sceneID, animName, animDictionary);
            }
            public static bool IsCamPlayingAnim(Camera cam, string animName, string animDictionary)
            {
                return Function.Call<bool>(Hash.IS_CAM_PLAYING_ANIM, cam, animName, animDictionary);
            }

            public enum CamAnimationFlags
            {
                CAF_DEFAULT,
                CAF_LOOPING
            }
            public enum CamGraphType
            {
                GRAPH_TYPE_LINEAR,
                GRAPH_TYPE_SIN_ACCEL_DECEL,
                GRAPH_TYPE_ACCEL,
                GRAPH_TYPE_DECEL,
                GRAPH_TYPE_SLOW_IN,
                GRAPH_TYPE_SLOW_OUT,
                GRAPH_TYPE_SLOW_IN_OUT,
                GRAPH_TYPE_VERY_SLOW_IN,
                GRAPH_TYPE_VERY_SLOW_OUT,
                GRAPH_TYPE_VERY_SLOW_IN_SLOW_OUT,
                GRAPH_TYPE_SLOW_IN_VERY_SLOW_OUT,
                GRAPH_TYPE_VERY_SLOW_IN_VERY_SLOW_OUT,
                GRAPH_TYPE_EASE_IN,
                GRAPH_TYPE_EASE_OUT,
                GRAPH_TYPE_QUADRATIC_EASE_IN,
                GRAPH_TYPE_QUADRATIC_EASE_OUT,
                GRAPH_TYPE_QUADRATIC_EASE_IN_OUT,
                GRAPH_TYPE_CUBIC_EASE_IN,
                GRAPH_TYPE_CUBIC_EASE_OUT,
                GRAPH_TYPE_CUBIC_EASE_IN_OUT,
                GRAPH_TYPE_QUARTIC_EASE_IN,
                GRAPH_TYPE_QUARTIC_EASE_OUT,
                GRAPH_TYPE_QUARTIC_EASE_IN_OUT,
                GRAPH_TYPE_QUINTIC_EASE_IN,
                GRAPH_TYPE_QUINTIC_EASE_OUT,
                GRAPH_TYPE_QUINTIC_EASE_IN_OUT,
                GRAPH_TYPE_CIRCULAR_EASE_IN,
                GRAPH_TYPE_CIRCULAR_EASE_OUT,
                GRAPH_TYPE_CIRCULAR_EASE_IN_OUT,
                GRAPH_TYPE_MAX
            }
            public enum RenderingOptionFlag
            {
                RO_NO_OPTIONS,
                RO_STOP_RENDERING_OPTION_WHEN_PLAYER_EXITS_INTO_COVER
            }
        }

        public class fHud
        {
            public static void ToggleNorthYanktonMap(bool toggle)
            {
                Function.Call(Hash.SET_​MINIMAP_​IN_​PROLOGUE, toggle);
            }

            public static void ShowNotification(string message, bool isImportant, bool cacheMessage = true)
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
            public static IntPtr CellEmailBcon
            {
                get
                {
                    return StringToCoTaskMemUTF8("CELL_EMAIL_BCON");
                }
            }
            private static byte[] _strBufferForStringToCoTaskMemUTF8 = new byte[100];
            public unsafe static IntPtr StringToCoTaskMemUTF8(string s)
            {
                bool flag = s == null;
                IntPtr result;
                if (flag)
                {
                    result = IntPtr.Zero;
                }
                else
                {
                    int byteCount = Encoding.UTF8.GetByteCount(s);
                    bool flag2 = byteCount > _strBufferForStringToCoTaskMemUTF8.Length;
                    if (flag2)
                    {
                        _strBufferForStringToCoTaskMemUTF8 = new byte[byteCount * 2];
                    }
                    Encoding.UTF8.GetBytes(s, 0, s.Length, _strBufferForStringToCoTaskMemUTF8, 0);
                    IntPtr intPtr = Marshal.AllocCoTaskMem(byteCount + 1);
                    bool flag3 = intPtr == IntPtr.Zero;
                    if (flag3)
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

            public static void RadarAndHud(bool Hud, bool Radar)
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

        public class fPathfind
        {
            public static void SetAllowStreamPrologueNodes(bool toggle)
            {
                Function.Call(Hash.SET_ALLOW_STREAM_PROLOGUE_NODES, toggle);
            }
        }

        public class fCutscene
        {
            public static int GetCutsceneTotalDuration()
            {
                return Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION);
            }

            public static string LoadCutsceneWithFlag(string cutscene, int playbackflag)
            {
                while (!Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED, cutscene))
                {
                    Function.Call(Hash.REQUEST_CUTSCENE_WITH_PLAYBACK_LIST, cutscene, playbackflag, 8);
                    Script.Yield();
                }
                return cutscene;
            }

            public static string LoadCutscene(string cutscene)
            {
                while (!Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED, cutscene))
                {
                    Function.Call(Hash.REQUEST_CUTSCENE, cutscene, 8);
                    Script.Yield();
                }
                return cutscene;
            }

            public static string LoadCutfile(string cutscene)
            {
                while (!Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutscene))
                {
                    Function.Call(Hash.REQUEST_CUT_FILE, cutscene);
                    Script.Yield();
                }
                return cutscene;
            }

            public static string RemoveCutfile(string cutscene)
            {
                while (Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutscene))
                {
                    Function.Call(Hash.REMOVE_CUT_FILE, cutscene);
                    Script.Yield();
                }
                return cutscene;
            }

            public static int GetNumOfConcats(string cutsceneName)
            {
                Function.Call(Hash.REQUEST_CUT_FILE, cutsceneName);
                bool flag = Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutsceneName);
                int result;
                if (flag)
                {
                    int num = Function.Call<int>(Hash.GET_CUT_FILE_CONCAT_COUNT, cutsceneName);
                    Function.Call(Hash.REMOVE_CUT_FILE, cutsceneName);
                    result = num;
                }
                else
                {
                    result = 0;
                }
                return result;
            }

            public static int GetCutsceneTime()
            {
                return Function.Call<int>(Hash.GET_CUTSCENE_TIME);
            }

            public static bool HasCutsceneFinished()
            {
                return Function.Call<bool>(Hash.HAS_CUTSCENE_FINISHED);
            }

            public static bool HasCutsceneLoaded()
            {
                return Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED);
            }

            public static bool IsCutscenePlaying()
            {
                return Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING);
            }

            public static void StopCutsceneImmediately()
            {
                Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
            }

            public static void SetCutsceneCanBeSkipped(bool toggle)
            {
                Function.Call(Hash.SET_CUTSCENE_CAN_BE_SKIPPED, toggle);
            }

            public static void RegisterEntityForCutscene(Entity entity, string cutsceneEntityName, int cutsceneUsage = 2, int modelname = 0, int cutsceneEntityOptionFlag = 0)
            {
                int entityHandle = entity.Exists() ? entity.Handle : 0;
                Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, entityHandle, cutsceneEntityName, cutsceneUsage, modelname, cutsceneEntityOptionFlag);
            }

            public static void RegisterEntityForCutscene(int entity, string cutsceneEntityName, int cutsceneUsage = 2, int modelname = 0, int cutsceneEntityOptionFlag = 0)
            {
                Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, entity, cutsceneEntityName, cutsceneUsage, modelname, cutsceneEntityOptionFlag);
            }

            public static void SetCutsceneOrigin(Vector3 cutscenePos, float cutsceneHeading)
            {
                Function.Call(Hash.SET_CUTSCENE_ORIGIN, cutscenePos.X, cutscenePos.Y, cutscenePos.Z, cutsceneHeading, 0);
            }

            public static void SetCutsceneOriginAndRotation(string cutsceneName, Vector3 position, Vector3 rotation)
            {
                int numOfConcats = GetNumOfConcats(cutsceneName);
                for (int i = 0; i < numOfConcats; i++)
                {
                    Function.Call(Hash.SET_CUTSCENE_ORIGIN_AND_ORIENTATION, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, i);
                }
            }

            public static void StartCutscene(int flag = 0)
            {
                Function.Call(Hash.START_CUTSCENE, flag);
            }

            public static void StartCutscene(CutscenePlaybackFlags flag = (CutscenePlaybackFlags)0)
            {
                Function.Call(Hash.START_CUTSCENE, flag);
            }

            public static void RemoveCutscene()
            {
                Function.Call(Hash.REMOVE_CUTSCENE);
            }

            public static void StopCutsceneAudio()
            {
                Function.Call(Hash.STOP_CUTSCENE_AUDIO, 0);
            }

            public static void SetCutsceneFadeValues(bool fadeOutAtStart = false, bool fadeInAtStart = false, bool fadeOutAtEnd = false, bool fadeInAtEnd = false)
            {
                Function.Call(Hash.SET_CUTSCENE_FADE_VALUES, fadeOutAtStart, fadeInAtStart, fadeOutAtEnd, fadeInAtEnd);
            }

            public static void SetPedOutfitCutscene(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_1");
                if (flag)
                {
                    CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                    CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                    CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                    CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                    CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                    CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                    CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                    CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                    CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                    CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                    CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                    CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                }
            }

            public static void GetPedOutfitCutscene(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_1");
                if (flag)
                {
                    string[] array = CutscenePed1Comp[0].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[1].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[2].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[3].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[4].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[5].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[6].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[7].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[8].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[9].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[10].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed1Comp[11].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                }
            }

            public static void SetPedOutfitCutscene_MP2(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_2");
                if (flag)
                {
                    CutscenePed2Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                    CutscenePed2Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                    CutscenePed2Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                    CutscenePed2Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                    CutscenePed2Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                    CutscenePed2Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                    CutscenePed2Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                    CutscenePed2Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                    CutscenePed2Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                    CutscenePed2Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                    CutscenePed2Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                    CutscenePed2Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                }
            }

            public static void GetPedOutfitCutscene_MP2(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_2");
                if (flag)
                {
                    string[] array = CutscenePed2Comp[0].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[1].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[2].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[3].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[4].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[5].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[6].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[7].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[8].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[9].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[10].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed2Comp[11].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                }
            }

            public static void SetPedOutfitCutscene_MP3(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_3");
                if (flag)
                {
                    CutscenePed3Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                    CutscenePed3Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                    CutscenePed3Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                    CutscenePed3Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                    CutscenePed3Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                    CutscenePed3Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                    CutscenePed3Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                    CutscenePed3Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                    CutscenePed3Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                    CutscenePed3Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                    CutscenePed3Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                    CutscenePed3Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                }
            }

            public static void GetPedOutfitCutscene_MP3(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_3");
                if (flag)
                {
                    string[] array = CutscenePed3Comp[0].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[1].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[2].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[3].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[4].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[5].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[6].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[7].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[8].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[9].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[10].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed3Comp[11].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                }
            }

            public static void SetPedOutfitCutscene_MP4(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_4");
                if (flag)
                {
                    CutscenePed4Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                    CutscenePed4Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                    CutscenePed4Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                    CutscenePed4Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                    CutscenePed4Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                    CutscenePed4Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                    CutscenePed4Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                    CutscenePed4Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                    CutscenePed4Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                    CutscenePed4Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                    CutscenePed4Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                    CutscenePed4Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                }
            }

            public static void GetPedOutfitCutscene_MP4(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_4");
                if (flag)
                {
                    string[] array = CutscenePed4Comp[0].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[1].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[2].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[3].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[4].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[5].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[6].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[7].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[8].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[9].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[10].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed4Comp[11].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                }
            }

            public static void SetPedOutfitCutscene_MP5(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_5");
                if (flag)
                {
                    CutscenePed5Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                    CutscenePed5Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                    CutscenePed5Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                    CutscenePed5Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                    CutscenePed5Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                    CutscenePed5Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                    CutscenePed5Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                    CutscenePed5Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                    CutscenePed5Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                    CutscenePed5Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                    CutscenePed5Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                    CutscenePed5Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                }
            }

            public static void GetPedOutfitCutscene_MP5(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_5");
                if (flag)
                {
                    string[] array = CutscenePed5Comp[0].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[1].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[2].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[3].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[4].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[5].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[6].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[7].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[8].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[9].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[10].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed5Comp[11].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                }
            }

            public static void SetPedOutfitCutscene_MP6(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_6");
                if (flag)
                {
                    CutscenePed6Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                    CutscenePed6Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                    CutscenePed6Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                    CutscenePed6Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                    CutscenePed6Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                    CutscenePed6Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                    CutscenePed6Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                    CutscenePed6Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                    CutscenePed6Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                    CutscenePed6Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                    CutscenePed6Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                    CutscenePed6Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                }
            }

            public static void GetPedOutfitCutscene_MP6(string MP, Ped NonCutscene)
            {
                bool flag = MP.Equals("MP_6");
                if (flag)
                {
                    string[] array = CutscenePed6Comp[0].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[1].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[2].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[3].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[4].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[5].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[6].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[7].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[8].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[9].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[10].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                    array = CutscenePed6Comp[11].Split(new char[]
                    {
                    '_'
                    });
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                }
            }

            public static List<string> CutscenePed1Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public static List<string> CutscenePed2Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public static List<string> CutscenePed3Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public static List<string> CutscenePed4Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public static List<string> CutscenePed5Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public static List<string> CutscenePed6Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public enum CutsceneUsage
            {
                CU_ANIMATE_EXISTING_SCRIPT_ENTITY,
                CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY,
                CU_CREATE_AND_ANIMATE_NEW_SCRIPT_ENTITY,
                CU_DONT_ANIMATE_ENTITY
            }

            public enum CutsceneEntityOptionFlag
            {
                CEO_NONE,
                CEO_PRESERVE_FACE_BLOOD_DAMAGE,
                CEO_PRESERVE_BODY_BLOOD_DAMAGE,
                CEO_REMOVE_BODY_BLOOD_DAMAGE = 4,
                CEO_CLONE_DAMAGE_TO_CS_MODEL = 8,
                CEO_RESET_CAPSULE_AT_END = 16,
                CEO_IS_CASCADE_SHADOW_FOCUS_ENTITY_DURING_EXIT = 32,
                CEO_IGNORE_MODEL_NAME = 64,
                CEO_PRESERVE_HAIR_SCALE = 128,
                CEO_INSTANT_HAIR_SCALE_SETUP = 256,
                CEO_DONT_RESET_PED_CAPSULE = 512,
                CEO_UPDATE_AS_REAL_DOOR = 1024
            }

            public enum CutsceneSection
            {
                CS_SECTION_1 = 1,
                CS_SECTION_2,
                CS_SECTION_3 = 4,
                CS_SECTION_4 = 8,
                CS_SECTION_5 = 16,
                CS_SECTION_6 = 32,
                CS_SECTION_7 = 64,
                CS_SECTION_8 = 128,
                CS_SECTION_9 = 256,
                CS_SECTION_10 = 512,
                CS_SECTION_11 = 1024,
                CS_SECTION_12 = 2048,
                CS_SECTION_13 = 4096,
                CS_SECTION_14 = 8192,
                CS_SECTION_15 = 16384,
                CS_SECTION_16 = 32768,
                CS_SECTION_17 = 65536,
                CS_SECTION_18 = 131072,
                CS_SECTION_19 = 262144,
                CS_SECTION_20 = 524288,
                CS_SECTION_21 = 1048576,
                CS_SECTION_22 = 2097152,
                CS_SECTION_23 = 4194304,
                CS_SECTION_24 = 8388608,
                CS_SECTION_25 = 16777216,
                CS_SECTION_26 = 33554432,
                CS_SECTION_27 = 67108864,
                CS_SECTION_28 = 134217728,
                CS_SECTION_29 = 268435456,
                CS_SECTION_30 = 536870912,
                CS_SECTION_31 = 1073741824
            }

            public enum CutscenePlaybackFlags
            {
                CUTSCENE_REQUESTED_FROM_WIDGET = 1,
                CUTSCENE_REQUESTED_DIRECTLY_FROM_SKIP,
                CUTSCENE_REQUESTED_FROM_Z_SKIP = 4,
                CUTSCENE_REQUESTED_IN_MISSION = 8,
                CUTSCENE_PLAYBACK_FORCE_LOAD_AUDIO_EVENT = 16
            }
        }

        public class fCutsceneCreation
        {
            public void AddRegisterEntityToList(int ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
            {
                RegisterEntityChunk item = new RegisterEntityChunk(ent, cHandle, usage, modelNames, entityOptionsFlag);
                this.theseEntities.Add(item);
            }

            public void AddRegisterEntityToList(Entity ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
            {
                RegisterEntityChunk item = new RegisterEntityChunk(ent, cHandle, usage, modelNames, entityOptionsFlag);
                this.theseEntities.Add(item);
            }

            public fCutsceneCreation(string cutsceneName, Vector3 pos, Vector3 rot, bool setPlayerModel)
            {
                CutsceneName = cutsceneName;
                Pos = pos;
                Rot = rot;
                SetPlayerModel = setPlayerModel;
            }

            public fCutsceneCreation(string cutsceneName, Vector3 pos, Vector3 rot, bool setPlayerModel, bool fadeOutAtStart = false, bool fadeInAtStart = false, bool fadeOutAtEnd = false, bool fadeInAtEnd = false)
            {
                CutsceneName = cutsceneName;
                Pos = pos;
                Rot = rot;
                SetPlayerModel = setPlayerModel;
                FadeOutAtStart = fadeOutAtStart;
                FadeInAtStart = fadeInAtStart;
                FadeOutAtEnd = fadeOutAtEnd;
                FadeInAtEnd = fadeInAtEnd;
            }

            public void StartCutscene()
            {
                for (int i = 0; i < this.theseEntities.Count; i++)
                {
                    bool flag = this.theseEntities[i].Ent != null && this.theseEntities[i].Ent.EntityType == EntityType.Ped;
                    if (flag)
                    {
                        this.theseEntities[i].SetPedOutfitCutscene((Ped)this.theseEntities[i].Ent);
                    }
                }
                fCutscene.LoadCutscene(this.CutsceneName);
                bool setPlayerModel = this.SetPlayerModel;
                if (setPlayerModel)
                {
                    fPlayer.PlayerModelSet(Game.Player.Character);
                }
                for (int j = 0; j < this.theseEntities.Count; j++)
                {
                    bool flag2 = this.theseEntities[j].EntInt != -1;
                    if (flag2)
                    {
                        fCutscene.RegisterEntityForCutscene(0, this.theseEntities[j].CHandle, (int)this.theseEntities[j].Usage, this.theseEntities[j].ModelNames, (int)this.theseEntities[j].EntityOptionsFlag);
                    }
                    else
                    {
                        fCutscene.RegisterEntityForCutscene(this.theseEntities[j].Ent, this.theseEntities[j].CHandle, (int)this.theseEntities[j].Usage, this.theseEntities[j].ModelNames, (int)this.theseEntities[j].EntityOptionsFlag);
                    }
                }
                fCutscene.StartCutscene(fCutscene.CutscenePlaybackFlags.CUTSCENE_REQUESTED_IN_MISSION);
                bool flag3 = this.FadeOutAtStart || this.FadeInAtStart || this.FadeOutAtEnd || this.FadeInAtEnd;
                if (flag3)
                {
                    fCutscene.SetCutsceneFadeValues(this.FadeOutAtStart, this.FadeInAtStart, this.FadeOutAtEnd, this.FadeInAtEnd);
                }
                while (!fCutscene.IsCutscenePlaying())
                {
                    Script.Wait(0);
                }
                for (int k = 0; k < this.theseEntities.Count; k++)
                {
                    bool flag4 = this.theseEntities[k].Ent != null && this.theseEntities[k].Ent.EntityType == EntityType.Ped;
                    if (flag4)
                    {
                        this.theseEntities[k].GetPedOutfitCutscene((Ped)this.theseEntities[k].Ent);
                    }
                }
                bool setPlayerModel2 = this.SetPlayerModel;
                if (setPlayerModel2)
                {
                    fPlayer.PlayerModelSetBack(Game.Player.Character);
                }
            }

            public void StartCutsceneTillEnd()
            {
                fCutscene.LoadCutscene(this.CutsceneName);
                bool setPlayerModel = this.SetPlayerModel;
                if (setPlayerModel)
                {
                    fPlayer.PlayerModelSet(Game.Player.Character);
                }
                for (int i = 0; i < this.theseEntities.Count; i++)
                {
                    fCutscene.RegisterEntityForCutscene(this.theseEntities[i].Ent, this.theseEntities[i].CHandle, (int)this.theseEntities[i].Usage, this.theseEntities[i].ModelNames, (int)this.theseEntities[i].EntityOptionsFlag);
                }
                fCutscene.StartCutscene(fCutscene.CutscenePlaybackFlags.CUTSCENE_REQUESTED_IN_MISSION);
                Script.Wait(50);
                bool setPlayerModel2 = this.SetPlayerModel;
                if (setPlayerModel2)
                {
                    fPlayer.PlayerModelSetBack(Game.Player.Character);
                }
                while (!fCutscene.HasCutsceneFinished())
                {
                    Script.Wait(0);
                }
            }

            public void Cleanup()
            {
                bool flag = this.theseEntities.Count > 0;
                if (flag)
                {
                    this.theseEntities.Clear();
                }
                fCutscene.RemoveCutscene();
            }

            public string CutsceneName = "";

            public Vector3 Pos;

            public Vector3 Rot;

            public bool SetPlayerModel = true;

            public List<RegisterEntityChunk> theseEntities = new List<RegisterEntityChunk>();

            public bool FadeOutAtStart = false;

            public bool FadeInAtStart = false;

            public bool FadeOutAtEnd = false;

            public bool FadeInAtEnd = false;
        }

        public class RegisterEntityChunk
        {
            public RegisterEntityChunk(Entity ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
            {
                this.Ent = ent;
                this.CHandle = cHandle;
                this.Usage = usage;
                this.ModelNames = modelNames;
                this.EntityOptionsFlag = entityOptionsFlag;
            }

            public RegisterEntityChunk(int ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
            {
                this.EntInt = ent;
                this.CHandle = cHandle;
                this.Usage = usage;
                this.ModelNames = modelNames;
                this.EntityOptionsFlag = entityOptionsFlag;
            }

            public void SetPedOutfitCutscene(Ped NonCutscene)
            {
                this.CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                this.CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                this.CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                this.CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                this.CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                this.CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                this.CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                this.CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                this.CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                this.CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                this.CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                this.CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
                this.CutscenePedPropComp[0] = "0_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 0).ToString();
                this.CutscenePedPropComp[1] = "1_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 1).ToString();
                this.CutscenePedPropComp[2] = "2_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 2).ToString();
                this.CutscenePedPropComp[3] = "6_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 6).ToString();
                this.CutscenePedPropComp[4] = "7_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 7).ToString();
            }

            public void GetPedOutfitCutscene(Ped NonCutscene)
            {
                string[] array = this.CutscenePed1Comp[0].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[1].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[2].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[3].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[4].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[5].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[6].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[7].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[8].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[9].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[10].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = this.CutscenePed1Comp[11].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
                string[] array2 = this.CutscenePedPropComp[0].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 0, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
                array2 = this.CutscenePedPropComp[1].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 1, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
                array2 = this.CutscenePedPropComp[2].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 2, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
                array2 = this.CutscenePedPropComp[3].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 6, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
                array2 = this.CutscenePedPropComp[4].Split(new char[]
                {
                '_'
                });
                Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 7, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
            }

            public Entity Ent;

            public int EntInt = -1;

            public string CHandle;

            public fCutscene.CutsceneUsage Usage;

            public int ModelNames;

            public fCutscene.CutsceneEntityOptionFlag EntityOptionsFlag;

            public List<string> CutscenePed1Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

            public List<string> CutscenePedPropComp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "6_0_0",
            "7_0_0"
        };
        }

        public class fGraphics
        {
            public static void DrawRect(float x, float y, float width, float height, int r, int g, int b, int a, bool stereo)
            {
                Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a, stereo);
            }

            public static void DrawMarker(MarkerTypes type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, int red, int green, int blue, int alpha,
        bool bobUpAndDown = false, bool faceCamera = false, int p19 = 2, bool rotateY = false, string textureDict = null, string textureName = null, bool drawOnEnts = false)
            {
                Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, alpha, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
            }
            private static bool toggle1 = true;
            public static void DrawMarker(bool FadeOutFadeIn, MarkerTypes type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, int red, int green, int blue, int alpha,
                float distanceTillFadeInActive = 20f,
        bool bobUpAndDown = false, bool faceCamera = false, int p19 = 2, bool rotateY = false, string textureDict = null, string textureName = null, bool drawOnEnts = false)
            {
                if (FadeOutFadeIn)
                {
                    if (fPlayer.GetDistanceTo(pos) < distanceTillFadeInActive)
                    {
                        if (toggle1)
                        {
                            Globals.integer1 = 1;
                        }
                        if (Globals.integer1 == 1)
                        {
                            for (int i = 0; i < alpha - 1; i += 2)
                            {
                                if (fHud.IsHudComponentActive(HudComponent.WeaponWheel))
                                {
                                    break;
                                }
                                if (i == alpha - 1)
                                {
                                    break;
                                }
                                else
                                {
                                    Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, i, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
                                    Script.Yield();
                                }
                            }
                            Globals.integer1 = default;
                            toggle1 = false;
                        }
                        Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, alpha, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
                    }
                    else
                    {
                        if (!toggle1)
                        {
                            Globals.integer1 = 1;
                        }
                        if (Globals.integer1 == 1)
                        {
                            for (int i = alpha; i > 0; i -= 2)
                            {
                                if (fHud.IsHudComponentActive(HudComponent.WeaponWheel))
                                {
                                    break;
                                }
                                if (i == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, i, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
                                    Script.Yield();
                                }
                            }
                            Globals.integer1 = default;
                            toggle1 = true;
                        }
                    }
                }
                else
                    Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, alpha, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
            }

            public enum MarkerTypes
            {
                UpsideDownCone = 0,
                VerticalCylinder = 1,
                ThickChevronUp = 2,
                ThinChevronUp = 3,
                CheckeredFlagRect = 4,
                CheckeredFlagCircle = 5,
                VerticleCircle = 6,
                PlaneModel = 7,
                LostMCDark = 8,
                LostMCLight = 9,
                Number0 = 10,
                Number1 = 11,
                Number2 = 12,
                Number3 = 13,
                Number4 = 14,
                Number5 = 15,
                Number6 = 16,
                Number7 = 17,
                Number8 = 18,
                Number9 = 19,
                ChevronUpx1 = 20,
                ChevronUpx2 = 21,
                ChevronUpx3 = 22,
                HorizontalCircleFat = 23,
                ReplayIcon = 24,
                HorizontalCircleSkinny = 25,
                HorizontalCircleSkinny_Arrow = 26,
                HorizontalSplitArrowCircle = 27,
                DebugSphere = 28,
                DallorSign = 29,
                HorizontalBars = 30,
                WolfHead = 31,
                MarkerTypeQuestionMark = 32,
                MarkerTypePlaneSymbol = 33,
                MarkerTypeHelicopterSymbol = 34,
                MarkerTypeBoatSymbol = 35,
                MarkerTypeCarSymbol = 36,
                MarkerTypeMotorcycleSymbol = 37,
                MarkerTypeBikeSymbol = 38,
                TruckSymbol = 39,
                ParachuteSymbol = 40,
                Thruster_Jetpack = 41,
                SawbladeSymbol = 42,
                Box = 43
            };

            public static void AnimpostFXStopAll()
            {
                Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
            }
            public static void AnimpostFXPlay(string effectName, int duration, bool looped)
            {
                Function.Call(Hash.ANIMPOSTFX_PLAY, effectName, duration, looped);
            }
            public static string ColorToHex(Color color)
            {
                return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            }
            public static string ToColorHexString(Color thisColor, string text)
            {
                return string.Concat(new string[]
                {
                "<FONT COLOR='#",
                fGraphics.ColorToHex(thisColor),
                "'>",
                text,
                "</FONT>"
                });
            }
            public static void SetTransitionTimecycleModifier(string modName, float transitionTime)
            {
                Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, modName, transitionTime);
            }
            public static void ClearTimecycleModifier()
            {
                Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
            }
        }

        public class fProp
        {
            public static void DeletePropsInList(List<Prop> propList)
            {
                if (propList.Count > 0)
                {
                    for (int i = 0; i < propList.Count; i++)
                    {
                        if (propList[i] != null)
                        {
                            propList[i].Delete();
                        }
                    }
                    propList.Clear();
                }
            }
            public static void CreatePropForList(bool noOffset, List<Prop> propList, Model model, Vector3 pos, Vector3 rot, bool dynamic, bool placeOnGround = true)
            {
                if (noOffset)
                {
                    Prop prop = World.CreatePropNoOffset(model, pos, rot, dynamic);
                    while (prop != null && !prop.Exists())
                    {
                        Script.Wait(0);
                    }
                    if (!propList.Contains(prop))
                        propList.Add(prop);
                }
                else
                {
                    Prop prop = World.CreateProp(model, pos, rot, dynamic, placeOnGround);
                    while (prop != null && !prop.Exists())
                    {
                        Script.Wait(0);
                    }
                    if (!propList.Contains(prop))
                        propList.Add(prop);
                }
            }
            public static Prop CreatePropNoOffset(Model model, Vector3 pos, Vector3 rot, bool dynamic)
            {
                return World.CreatePropNoOffset(model, pos, rot, dynamic);
            }
            public static Prop CreateProp(Model model, Vector3 pos, Vector3 rot, bool dynamic, bool placeOnGround)
            {
                return World.CreateProp(model, pos, rot, dynamic, placeOnGround);
            }
        }

        public class fEntity
        {
            public static void SetCanClimbOnEntity(Entity entity, bool toggle)
            {
                Function.Call(Hash.SET_CAN_CLIMB_ON_ENTITY, entity, toggle);
            }
            public static bool IsEntityInArea(Entity entity, Vector3 xyz1, Vector3 xyz2, bool p7 = false, bool p8 = true, int p9 = 0)
            {
                return Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, entity, xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, p7, p8, p9);
            }
            /// <summary>
            /// By "Angled Area" they mean a rectangle.
            /// </summary>
            public static bool IsEntityInAngledArea(Entity entity, Vector3 xyz1, Vector3 xyz2, float width, bool debug = false, bool includeZ = true, int p10 = 0)
            {
                return Function.Call<bool>(Hash.IS_​ENTITY_​IN_​ANGLED_​AREA, entity, xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, width, debug, includeZ, p10);
            }
            public static bool HasCollisionLoadedAroundEntity(Entity entity)
            {
                return Function.Call<bool>(Hash.HAS_​COLLISION_​LOADED_​AROUND_​ENTITY, entity);
            }
            public static void SetEntityLoadCollisionFlag(Entity entity, bool toggle, int p2 = 1)
            {
                Function.Call(Hash.SET_​ENTITY_​LOAD_​COLLISION_​FLAG, entity, toggle, p2);
            }
        }

        public class fVehicle
        {
            public static void SwitchTrainTrack(TrainTracks trainTrack, bool toggleTrainSpawn)
            {
                Function.Call(Hash.SWITCH_​TRAIN_​TRACK, (int)trainTrack, toggleTrainSpawn);
            }
            public enum TrainTracks
            {
                MainTrackAroundSanAndreas = 0, // Vector3 Pos: 1084.48f, 3231.45f, 39.2565f
                DavisQuartzQuarryBranch, // Vector3 Pos: 2787.94f, 2837.48f, 35.3899f
                SecondTrackAlongsideLiveTrackAlongRoyLewensteinBlv, // Vector3 Pos: 290.534f, -1845.34f, 25.776f
                MetroTrackCircuit, // Vector3 Pos: 193.196f, -603.836f, 16.7565f
                BranchInMirrorParkRailyard, // Vector3 Pos: 536.946f, -452.129f, 23.7995f
                BranchInMirrorParkRailyard2, // Vector3 Pos: 561.516f, -432.01f, 23.8422f
                LosSantosBranchToMirrorParkRailyard, // Vector3 Pos: 499.604f, -1571.59f, 28.3367f
                OvergroundPartOfMetroTrackAlongForumDr, // Vector3 Pos: -213.538f, -1019.98f, 28.1413f
                BranchToMirrorParkRailyard, // Vector3 Pos: 532.34f, -952.224f, 26.1233f
                YanktonTrain, // Vector3 Pos: 4318.79f, -4714.68f, 112.313f 
                PartOfMetroTrackNearMissionRow, // Vector3 Pos: 519.823f, -1187.75f, 28.3182f
                YanktonPrologueMissionTrain // Vector3 Pos: 4243.14f, -6506.32f, 109.381f 
            }

            public static void DeleteVehiclesInList(List<Vehicle> vehList)
            {
                if (vehList.Count > 0)
                {
                    for (int i = 0; i < vehList.Count; i++)
                    {
                        if (vehList[i] != null)
                        {
                            vehList[i].Delete();
                        }
                    }
                    vehList.Clear();
                }
            }

            public static void CreateVehicleForList(List<Vehicle> vehList, Model model, Vector3 pos, float heading = 0)
            {
                Vehicle veh = World.CreateVehicle(model, pos, heading);
                while (veh != null && !veh.Exists())
                {
                    Script.Wait(0);
                }
                if (!vehList.Contains(veh))
                    vehList.Add(veh);
            }

            public static Vehicle CreateVehicle(Model model, Vector3 pos, float heading = 0)
            {
                return World.CreateVehicle(model, pos, heading);
            }

            public static void DisableVehicleWeapon(bool disabled, Hash weaponHash, Vehicle vehicle, Ped owner)
            {
                Function.Call(Hash.DISABLE_​VEHICLE_​WEAPON, disabled, weaponHash, vehicle, owner);
            }

            public static bool IsVehicleWeaponDisabled(Hash weaponHash, Vehicle vehicle, Ped owner)
            {
                return Function.Call<bool>(Hash.IS_​VEHICLE_​WEAPON_​DISABLED, weaponHash, vehicle, owner);
            }

            public static void SetVehicleColours(Vehicle vehicle, int colorPrimary, int colorSecondary) // colors https://pastebin.com/pwHci0xK
            {
                Function.Call(Hash.SET_​VEHICLE_​COLOURS, vehicle, colorPrimary, colorSecondary);
            }

            public static float GetVehicleEngineHealth(Vehicle vehicle)
            {
                return Function.Call<float>(Hash.GET_​VEHICLE_​ENGINE_​HEALTH, vehicle);
            }

            public static bool SetVehicleOnGroundProperly(Vehicle vehicle, float p1 = 5.0f)
            {
                return Function.Call<bool>(Hash.SET_​VEHICLE_​ON_​GROUND_​PROPERLY, vehicle, p1);
            }
        }

        public class fPed
        {
            public static void CheckPedsInList(List<Ped> pedList)
            {
                if (pedList.Count > 0)
                {
                    for (int i = 0; i < pedList.Count; i++)
                    {
                        if (pedList[i] != null)
                        {
                            if (fInterior.GetInteriorFromEntity(pedList[i]) == fInterior.GetInteriorFromEntity(Game.Player.Character))
                            {
                                if (fInterior.GetRoomKeyFromEntity(pedList[i]) == fInterior.GetRoomKeyFromEntity(Game.Player.Character))
                                {
                                    if (pedList[i].AttachedBlip != null)
                                    {
                                        pedList[i].AttachedBlip.Alpha = 255;
                                    }
                                }
                                else
                                {
                                    if (pedList[i].AttachedBlip != null)
                                    {
                                        pedList[i].AttachedBlip.Alpha = 0;
                                    }
                                }
                            }
                            else
                            {
                                if (pedList[i].AttachedBlip != null)
                                {
                                    pedList[i].AttachedBlip.Alpha = 0;
                                }
                            }
                            if (pedList[i].IsDead)
                            {
                                if (pedList[i].AttachedBlip != null)
                                {
                                    pedList[i].AttachedBlip.Delete();
                                }
                                pedList[i].MarkAsNoLongerNeeded();
                                if (pedList.Contains(pedList[i]))
                                {
                                    pedList.Remove(pedList[i]);
                                }
                            }
                        }
                    }
                }
            }

            public static void DeletePedsInList(List<Ped> pedList)
            {
                if (pedList.Count > 0)
                {
                    for (int i = 0; i < pedList.Count; i++)
                    {
                        if (pedList[i] != null)
                        {
                            pedList[i].Delete();
                        }
                    }
                    pedList.Clear();
                }
            }

            public static void SetListPedBlip(List<Ped> pedList, int SpriteID, BlipColor color, string blipName, float scale)
            {
                if (pedList.Count > 0)
                {
                    for (int i = 0; i < pedList.Count; i++)
                    {
                        if (pedList[i] != null)
                        {
                            if (pedList[i].AttachedBlip == null)
                            {
                                pedList[i].AddBlip();
                                for (; ; )
                                {
                                    Blip attachedBlip = pedList[i].AttachedBlip;
                                    if (attachedBlip == null || attachedBlip.Exists())
                                    {
                                        break;
                                    }
                                    Script.Wait(0);
                                }
                                pedList[i].AttachedBlip.Sprite = (BlipSprite)SpriteID;
                                pedList[i].AttachedBlip.Color = color;
                                pedList[i].AttachedBlip.Name = blipName;
                                pedList[i].AttachedBlip.Scale = scale;
                                pedList[i].AttachedBlip.IsShortRange = true;
                                pedList[i].AttachedBlip.DisplayType = BlipDisplayType.MiniMapOnly;
                            }
                        }
                    }
                }
            }

            public static void CreatePedForList(List<Ped> pedList, Model model, Vector3 pos, float heading = 0)
            {
                Ped ped = World.CreatePed(model, pos, heading);
                while (ped != null && !ped.Exists())
                {
                    Script.Wait(0);
                }
                if (!pedList.Contains(ped))
                    pedList.Add(ped);
            }

            public static Ped CreatePed(Model model, Vector3 pos, float heading = 0)
            {
                return World.CreatePed(model, pos, heading);
            }

            public static bool ForcePedMotionState(Ped PedIndex, MotionStates state, bool shouldRestart = false, ExitStates exitstate = ExitStates.FAUS_DEFAULT, bool ForceAIPreCameraUpdate = false)
            {
                return Function.Call<bool>(Hash.FORCE_PED_MOTION_STATE, PedIndex, state, shouldRestart, exitstate, ForceAIPreCameraUpdate);
            }

            public enum MotionStates
            {
                MS_ON_FOOT_IDLE = -1871534317,
                MS_ON_FOOT_WALK = -668482597,
                MS_ON_FOOT_RUN = -530524,
                MS_ON_FOOT_SPRINT = -1115154469,
                MS_CROUCH_IDLE = 1140525470,
                MS_CROUCH_WALK = 147004056,
                MS_CROUCH_RUN = 898879241,
                MS_DO_NOTHING = 247561816,
                MS_DIVING_IDLE = 1212730861,
                MS_DIVING_SWIM = -1855028596,
                MS_PARACHUTING = -1161760501,
                MS_AIMING = 1063765679,
                MS_ACTIONMODE_IDLE = -633298724,
                MS_ACTIONMODE_WALK = -762290521,
                MS_ACTIONMODE_RUN = 834330132,
                MS_STEALTHMODE_IDLE = 1110276645,
                MS_STEALTHMODE_WALK = 69908130,
                MS_STEALTHMODE_RUN = -83133983
            }
            public enum ExitStates
            {
                FAUS_DEFAULT,
                FAUS_CUTSCENE_EXIT
            }

        }

        public class fInterior
        {
            public static uint GetInteriorFromEntity(Entity entity)
            {
                return Function.Call<uint>(Hash.GET_INTERIOR_FROM_ENTITY, entity);
            }
            public static uint GetRoomKeyFromEntity(Entity entity)
            {
                return Function.Call<uint>(Hash.GET_ROOM_KEY_FROM_ENTITY, entity);
            }
            public static uint GetInteriorFromCoords(Vector3 coords)
            {
                return Function.Call<uint>(Hash.GET_INTERIOR_AT_COORDS, coords.X, coords.Y, coords.Z);
            }

            public class MPMaps
            {
                public static void LoadMpMaps()
                {
                    if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, "xm_hatch_closed"))
                    {
                        Function.Call(Hash.ON_ENTER_MP);
                        LoadingPrompt.Hide();
                        Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
                    }
                    foreach (string str in RemoveOnlyIPLS)
                    {
                        Function.Call(Hash.REMOVE_IPL, str);
                    }
                    foreach (string str2 in LoadAllIPLS)
                    {
                        Function.Call(Hash.REMOVE_IPL, str2);
                        Function.Call(Hash.REQUEST_IPL, str2);
                    }
                }

                public static List<string> LoadAllIPLS = new List<string>
        {
            "xm_hatch_01_cutscene",
            "xm_hatch_02_cutscene",
            "xm_hatch_03_cutscene",
            "xm_hatch_04_cutscene",
            "xm_hatch_05_cutscene",
            "xm_hatch_06_cutscene",
            "xm_hatch_07_cutscene",
            "xm_hatch_08_cutscene",
            "xm_hatch_09_cutscene",
            "xm_hatch_10_cutscene",
            "xm_hatch_closed",
            "xm_hatches_terrain",
            "xm_hatches_terrain_lod",
            "sm_smugdlc_interior_placement",
            "xm_mpchristmasadditions",
            "xm_siloentranceclosed_x17",
            "id2_14_during1",
            "shr_int",
            "xm_x17dlc_int_placement_interior_8_x17dlc_int_sub_milo_",
            "bkr_bi_hw1_13_int",
            "bkr_bi_id1_23_door",
            "vw_dlc_casino_door",
            "xm_x17dlc_int_placement_interior_4_x17dlc_int_facility_milo_",
            "xm_x17dlc_int_placement_interior_5_x17dlc_int_facility2_milo_",
            "xm_x17dlc_int_placement_interior_0_x17dlc_int_base_ent_milo_",
            "xm_x17dlc_int_placement_interior_1_x17dlc_int_base_loop_milo_",
            "xm_x17dlc_int_placement_interior_2_x17dlc_int_bse_tun_milo_",
            "xm_x17dlc_int_placement_interior_3_x17dlc_int_base_milo_",
            "xm_x17dlc_int_placement_interior_6_x17dlc_int_silo_01_milo_",
            "xm_x17dlc_int_placement_interior_7_x17dlc_int_silo_02_milo_",
            "xm_x17dlc_int_placement_interior_10_x17dlc_int_tun_straight_milo_",
            "xm_x17dlc_int_placement_interior_11_x17dlc_int_tun_slope_flat_milo_",
            "xm_x17dlc_int_placement_interior_12_x17dlc_int_tun_flat_slope_milo_",
            "xm_x17dlc_int_placement_interior_13_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_14_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_15_x17dlc_int_tun_straight_milo_",
            "xm_x17dlc_int_placement_interior_16_x17dlc_int_tun_straight_milo_",
            "xm_x17dlc_int_placement_interior_17_x17dlc_int_tun_slope_flat_milo_",
            "xm_x17dlc_int_placement_interior_18_x17dlc_int_tun_slope_flat_milo_",
            "xm_x17dlc_int_placement_interior_19_x17dlc_int_tun_flat_slope_milo_",
            "xm_x17dlc_int_placement_interior_20_x17dlc_int_tun_flat_slope_milo_",
            "xm_x17dlc_int_placement_interior_21_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_22_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_23_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_24_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_25_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_26_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_27_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_28_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_29_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_34_x17dlc_int_lab_milo_",
            "xm_x17dlc_int_placement_interior_35_x17dlc_int_tun_entry_milo_",
            "xm_x17dlc_int_placement_strm_0",
            "xm_x17dlc_int_placement_interior_33_x17dlc_int_02_milo_",
            "xm_prop_x17_tem_control_01",
            "SP1_10_real_interior",
            "post_hiest_unload",
            "facelobby",
            "FIBlobby",
            "Coroner_Int_on",
            "h4_ch2_mansion_final",
            "hei_ch1_06e_strm_1",
            "ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo",
            "FINBANK",
            "h4_islandairstrip_doorsopen",
            "v_tunnel_hole",
            "plane_crash_trench",
            "hei_dlc_casino_door",
            "vw_casino_main",
            "vw_casino_carpark",
            "vw_casino_garage",
            "vw_casino_penthouse",
            "hei_dlc_casino_aircon",
            "hei_dlc_casino_aircon_lod",
            "hei_dlc_casino_door",
            "hei_dlc_casino_door_lod",
            "hei_dlc_vw_roofdoors_locked",
            "hei_dlc_windows_casino",
            "hei_dlc_windows_casino_lod",
            "ch_chint09_closed",
            "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo",
            "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo",
            "h4_island_padlock_props",
            "h4_BoatBlockers",
            "h4_Mansion_Gate_Closed",
            "xm3_collision_fixes",
            "xm3_cutscene_doors",
            "xm3_doc_sign",
            "xm3_doc_sign_lod",
            "xm3_garage_fix",
            "xm3_garage_fix_lod",
            "xm3_security_fix",
            "xm3_stash_cams",
            "xm3_sum2_fix",
            "xm3_sum2_fix_lod",
            "xm3_warehouse",
            "xm3_warehouse_grnd",
            "xm3_warehouse_lod",
            "m23_2_cargoship",
            "m23_2_cargoship_bridge"
        };

                public static List<string> RemoveOnlyIPLS = new List<string>
        {
            "xm_bunkerentrance_door",
            "chemgrill_grp1",
            "id2_14_pre_no_int",
            "id2_14_post_no_int",
            "id2_14_on_fire",
            "id2_14_during_door",
            "id2_14_during2",
            "burnt_switch_off",
            "id2_14_during1",
            "fakeint",
            "fakeint_boards",
            "shr_int",
            "carshowroom_boarded",
            "carshowroom_broken",
            "SP1_10_fake_interior",
            "bh1_16_refurb",
            "jewel2fake",
            "FIBlobbyfake",
            "h4_islandairstrip_doorsclosed",
            "hei_po1_07_strm_2",
            "v_tunnel_hole_swap",
            "dt1_03_shutter",
            "dt1_03_gr_closed",
            "atriumglcut",
            "atriumglstatic",
            "atriumglmission",
            "FBI_repair",
            "FBI_colPLUG",
            "DT1_05_rubble",
            "DT1_05_HC_REMOVE",
            "DT1_05_HC_REQ",
            "dt1_05_slod",
            "dt1_05_damage_slod",
            "dt1_05_build1_damage_lod",
            "dt1_05_build1_damage",
            "dt1_05_build1_h",
            "DT1_05_REQUEST",
            "FBI_repair_lod",
            "dt1_05_build1_h",
            "dt1_05_build1_damage",
            "dt1_05_build1_damage_lod",
            "h4_island_padlock_props",
            "h4_islandxdock_water_hatch",
            "h4_islandx_barrack_hatch",
            "h4_BoatBlockers",
            "h4_underwater_gate_closed",
            "h4_Mansion_Gate_Broken",
            "h4_Mansion_Gate_Closed",
            "hei_ch1_06e_strm_2",
            "hei_ch1_06e_strm_1"
        };
            }

            public class PrologueMap : Script
            {
                public PrologueMap()
                {
                    Tick += OnTick;
                    UnloadPrologueMap();
                }

                private void OnTick(object sender, EventArgs e)
                {
                    for (int i = 0; i < LoadPrologueIPLS.Count; i++)
                    {
                        if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, LoadPrologueIPLS[i]))
                        {
                            if (PrologueIPLSLoaded < LoadPrologueIPLS.Count)
                                PrologueIPLSLoaded++;
                            else
                                PrologueIPLSLoaded = LoadPrologueIPLS.Count;
                        }
                    }
                    for (int i = 0; i < UnloadPrologueIPLS.Count; i++)
                    {
                        if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, UnloadPrologueIPLS[i]))
                        {
                            if (PrologueIPLSUnloaded < UnloadPrologueIPLS.Count)
                                PrologueIPLSUnloaded++;
                            else
                                PrologueIPLSUnloaded = UnloadPrologueIPLS.Count;
                        }
                    }
                }

                public static void EnableNorthYanktonTrains(bool enabled)
                {
                    if (enabled)
                    {
                        fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonPrologueMissionTrain, true);
                        fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonTrain, true);
                        return;
                    }
                    fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonPrologueMissionTrain, false);
                    fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonTrain, false);
                }
                public static bool IsPrologueMapLoading;
                public static bool IsPrologueMapUnloading;
                public static bool IsPrologueMapLoaded
                {
                    get
                    {
                        if (PrologueIPLSLoaded == 30)
                        {
                            PrologueIPLSUnloaded = 0;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                public static bool IsPrologueMapUnloaded
                {
                    get
                    {
                        if (PrologueIPLSUnloaded == 30)
                        {
                            PrologueIPLSLoaded = 0;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                public static void LoadPrologueMap()
                {
                    IsPrologueMapLoading = true;
                    IsPrologueMapUnloading = false;
                    foreach (string ipl in LoadPrologueIPLS)
                    {
                        Function.Call(Hash.REMOVE_IPL, ipl);
                        Function.Call(Hash.REQUEST_IPL, ipl);
                    }
                }
                public static void UnloadPrologueMap()
                {
                    IsPrologueMapUnloading = true;
                    IsPrologueMapLoading = false;
                    foreach (string ipl in UnloadPrologueIPLS)
                    {
                        Function.Call(Hash.REMOVE_IPL, ipl);
                    }
                }
                public static int PrologueIPLSLoaded = 0;
                public static int PrologueIPLSUnloaded = 0;
                public static List<string> LoadPrologueIPLS = new List<string>
                {
                    "prologue01",
                    "prologue02",
                    "prologue03",
                    "prologue04",
                    "prologue05",
                    "prologue06",
                    "prologuerd",
                    "prologue01c",
                    "prologue01d",
                    "prologue01e",
                    "prologue01f",
                    "prologue01g",
                    "prologue01h",
                    "prologue01i",
                    "prologue01j",
                    "prologue01k",
                    "prologue01z",
                    "prologue03b",
                    "prologue04b",
                    "prologue05b",
                    "prologue06b",
                    "prologue_occl",
                    "prologue06_int",
                    "prologue03_grv_dug",
                    "prologue_grv_torch",
                    "des_protree_end",
                    "des_protree_start",
                    "prologue06_pannel",
                    "plg_occl_00",
                    "prologuerdb"
                };
                public static List<string> UnloadPrologueIPLS = new List<string>
                {
                    "prologue01",
                    "prologue02",
                    "prologue03",
                    "prologue04",
                    "prologue05",
                    "prologue06",
                    "prologuerd",
                    "prologue01c",
                    "prologue01d",
                    "prologue01e",
                    "prologue01f",
                    "prologue01g",
                    "prologue01h",
                    "prologue01i",
                    "prologue01j",
                    "prologue01k",
                    "prologue01z",
                    "prologue03b",
                    "prologue04b",
                    "prologue05b",
                    "prologue06b",
                    "prologue_occl",
                    "prologue06_int",
                    "prologue03_grv_dug",
                    "prologue_grv_torch",
                    "des_protree_end",
                    "des_protree_start",
                    "prologue06_pannel",
                    "plg_occl_00",
                    "prologuerdb"
                };
            }

            public static void RequestIpl(string iplName)
            {
                Function.Call(Hash.REQUEST_IPL, iplName);
            }

            public static void RemoveIpl(string iplName)
            {
                Function.Call(Hash.REMOVE_IPL, iplName);
            }
        }

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
                if (num != 225514697U) // michael
                {
                    if (num != 2602752943U) // franklin
                    {
                        if (num != 2608926626U) // trevor
                        {
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Default", true);
                        }
                        else
                        {
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Trevor", true);
                        }
                    }
                    else
                    {
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Franklin", true);
                    }
                }
                else
                {
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Michael", true);
                }
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
        }

        public class fInGameScripts
        {
            public static void StartAllNeededToStartScripts()
            {
                StartScriptMissionTriggerers();
                StartScriptSideJobs();
            }

            static void StartScriptMissionTriggerers()
            {
                StartScript("mission_triggerer_a", 5050);
                StartScript("mission_triggerer_b", 5050);
                StartScript("mission_triggerer_c", 5050);
                StartScript("mission_triggerer_d", 5050);
            }
            static void StartScriptSideJobs()
            {
                StartScript("controller_taxi", 2050);
                StartScript("controller_towing", 2050);
                StartScript("controller_trafficking", 1424);
            }

            public static void TerminateScriptsExeptMissions()
            {
                TerminateLaunchers();
                TerminateAllPropertyManagement();
                TerminateAllRandomEvents();
                TerminateRaces();
                TerminateSideJobs();
                TerminateAmbientAreas();
                TerminateBaseJumps();
                TerminateMichaelEvents();
                TerminateRampages();
            }
            public static void TerminateScriptsWithMissions()
            {
                TerminateScriptsExeptMissions();
                TerminateMissionTriggerers();
            }


            static void TerminateMissionTriggerers()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_a");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_b");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_c");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_d");
            }
            static void TerminateGolf()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_golf");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "golf");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "golf_ai_foursome");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "golf_ai_foursome_putting");
            }
            static void TerminateSideJobs()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_taxi");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_towing");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_trafficking");
            }
            static void TerminateAmbientAreas()
            {
                TerminateShops();
                TerminateStripClub();
                TerminateCinemas();
                TerminateGolf();
                TerminateBlimp();
                TerminateAmusementPark();
                TerminateCableCars();
                TerminateBootyCalls();
            }
            static void TerminateShops()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "shop_controller");
            }
            static void TerminateRaces()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_racing");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_offroadracing");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_races");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "country_race_controller");
            }
            static void TerminateRampages()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_rampage");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "rampage_controller");
            }
            static void TerminateStripClub()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "stripclub");
            }
            static void TerminateCinemas()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "act_cinema");
            }
            static void TerminateBlimp()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "ambientblimp");
            }
            static void TerminateAmusementPark()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "fairgroundhub");
            }
            static void TerminateCableCars()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "cablecar");
            }
            static void TerminateBootyCalls()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "bootycallhandler");
            }
            static void TerminateBaseJumps()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "bj");
            }
            static void TerminateMichaelEvents()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "me_jimmy1");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "me_amanda1");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "me_tracey1");
            }
            static void TerminateLaunchers()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_abigail");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_barry");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_basejumpheli");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_basejumppack");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_carwash");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_darts");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_dreyfuss");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_epsilon");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_extreme");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_fanatic");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_hao");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_hunting");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_hunting_ambient");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_josh");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_maude");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_minute");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_mrsphilips");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_nigel");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_omega");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_paparazzo");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_pilotschool");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_range");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_stunts");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_tennis");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_thelastone");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_tonya");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_triathlon");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_yoga");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "bailbond_launcher");

                // Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "");
            }
            static void TerminateAllPropertyManagement()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_defend");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_delivery");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_gang_attack");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_plane_promotion");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_recover_stolen");

                //Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "");
            }
            static void TerminateAllRandomEvents()
            {
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_yetarian");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_stag_do");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_snatched");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_shoprobbery");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_securityvan");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_seaplane");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_rescuehostage");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prisonvanbreak");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prisonerlift");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_paparazzi");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_muggings");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_mountdance");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_monkey");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_lured");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_lossantosintl");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_homeland_security");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_hitch_lift");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_getaway_driver");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_gangfight");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_gang_intimidation");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_duel");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_drunkdriver");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_domestic");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_dealgonewrong");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_cultshootout");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_crashrescue");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_chasethieves");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_cartheft");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_bus_tours");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_burials");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_border");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_bikethief");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_atmrobbery");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_arrests");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_armybase");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_accident");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_abandonedcar");

                //Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "");
            }

            public static void StartScript(string scriptName, int buffer)
            {
                Function.Call(Hash.REQUEST_SCRIPT, scriptName);
                while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, scriptName))
                {
                    Function.Call(Hash.REQUEST_SCRIPT, scriptName);
                    Script.Yield();
                }
                Function.Call(Hash.START_NEW_SCRIPT, scriptName, buffer);
                Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, scriptName);
            }

            void RequestScript(string scriptName)
            {
                Function.Call(Hash.REQUEST_SCRIPT, scriptName);
                while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, scriptName))
                {
                    Function.Call(Hash.REQUEST_SCRIPT, scriptName);
                    Script.Yield();
                }
                Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, scriptName);
            }
        }

        public class fBlip
        {
            public static void ShowTickOnBlip(Blip blip, bool toggle)
            {
                Function.Call(Hash.SHOW_TICK_ON_BLIP, blip, toggle);
            }

            static bool IsBlipGameplayBlip(Blip blip)
            {
                if (blip.Sprite == (BlipSprite)0 || blip.Sprite == (BlipSprite)1 || blip.Sprite == (BlipSprite)2 || blip.Sprite == (BlipSprite)3 || blip.Sprite == (BlipSprite)4
                    || blip.Sprite == (BlipSprite)5 || blip.Sprite == (BlipSprite)6 || blip.Sprite == (BlipSprite)7 || blip.Sprite == (BlipSprite)8 || blip.Sprite == (BlipSprite)9
                    || blip.Sprite == (BlipSprite)10 || blip.Sprite == (BlipSprite)11 || blip.Sprite == (BlipSprite)12 || blip.Sprite == (BlipSprite)13 || blip.Sprite == (BlipSprite)14
                    || blip.Sprite == (BlipSprite)15 || blip.Sprite == (BlipSprite)41 || blip.Sprite == (BlipSprite)42 || blip.Sprite == (BlipSprite)185 || blip.Sprite == (BlipSprite)162 /*(Point of interest)*/)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            static bool IsSpecificBlip_0(Blip blip)
            {
                if (blip.Sprite == (BlipSprite)71 || blip.Sprite == (BlipSprite)72 || blip.Sprite == (BlipSprite)73 || blip.Sprite == (BlipSprite)110
                    || blip.Sprite == (BlipSprite)162 || blip.Sprite == (BlipSprite)313)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            static Blip[] allBlips = World.GetAllBlips();
            public static void SetMostBlipsInvisible(List<Blip> excludeListBlips)
            {
                for (int i = 0; i < allBlips.Length; i++)
                {
                    if (allBlips[i] != null)
                    {
                        if (!IsBlipGameplayBlip(allBlips[i]) && !IsSpecificBlip_0(allBlips[i]))
                        {
                            if (excludeListBlips.Count > 0 && excludeListBlips != null)
                            {
                                for (int ii = 0; ii < excludeListBlips.Count; ii++)
                                {
                                    if (excludeListBlips[ii] != allBlips[i])
                                    {
                                        allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                                    }
                                }
                            }
                            else
                            {
                                allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                            }
                        }
                    }
                }
            }
            public static void SetAllBlipsInvisible(List<Blip> excludeListBlips)
            {
                for (int i = 0; i < allBlips.Length; i++)
                {
                    if (allBlips[i] != null)
                    {
                        if (!IsBlipGameplayBlip(allBlips[i]))
                        {
                            if (excludeListBlips.Count > 0 && excludeListBlips != null)
                            {
                                for (int ii = 0; ii < excludeListBlips.Count; ii++)
                                {
                                    if (excludeListBlips[ii] != allBlips[i])
                                        allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                                }
                            }
                            else
                            {
                                allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                            }
                        }
                    }
                }
            }
            public static void SetAllBlipsVisible(List<Blip> excludeListBlips)
            {
                for (int i = 0; i < allBlips.Length; i++)
                {
                    if (allBlips[i] != null)
                    {
                        if (excludeListBlips.Count > 0 && excludeListBlips != null)
                        {
                            for (int ii = 0; ii < excludeListBlips.Count; ii++)
                            {
                                if (excludeListBlips[ii] != allBlips[i])
                                    allBlips[i].DisplayType = BlipDisplayType.Default;
                            }
                        }
                        else
                        {
                            allBlips[i].DisplayType = BlipDisplayType.Default;
                        }
                    }
                }
            }
            public static void SetAllBlipsVisible()
            {
                for (int i = 0; i < allBlips.Length; i++)
                {
                    if (allBlips[i] != null)
                    {
                        allBlips[i].DisplayType = BlipDisplayType.Default;
                    }
                }
            }

            public static void DeleteListBlips(List<Blip> blipList)
            {
                if (blipList.Count > 0)
                {
                    for (int i = 0; i < blipList.Count; i++)
                    {
                        if (blipList[i] != null)
                        {
                            blipList[i].Delete();
                        }
                    }
                    blipList.Clear();
                }
            }

            public static Blip CreateBlipForRadiusWithParams(Vector3 pos, float radius, BlipColor Color)
            {
                Blip blip = AddBlipForRadius(pos, radius);
                if (blip != null)
                {
                    blip.Color = Color;
                    return blip;
                }
                return null;
            }
            public static Blip AddBlipForRadius(Vector3 pos, float radius)
            {
                return Function.Call<Blip>(Hash.ADD_BLIP_FOR_RADIUS, pos.X, pos.Y, pos.Z, radius);
            }

            public static Blip CreateBlipForCoordWithParams(Vector3 pos, BlipSprite Sprite, BlipColor Color, float Scale, string Name, int Alpha = 255)
            {
                Blip blip = AddBlipForCoord(pos);
                if (blip != null)
                {
                    blip.Alpha = Alpha;
                    blip.Sprite = Sprite;
                    blip.Color = Color;
                    blip.Scale = Scale;
                    blip.Name = Name;
                    return blip;
                }
                return null;
            }
            public static Blip CreateBlipForCoordWithParams(Vector3 pos, BlipSprite Sprite, BlipColor Color, float ScaleX, float ScaleY, string Name, int Alpha = 255)
            {
                Blip blip = AddBlipForCoord(pos);
                if (blip != null)
                {
                    blip.Alpha = Alpha;
                    blip.Sprite = Sprite;
                    blip.Color = Color;
                    blip.ScaleX = ScaleX;
                    blip.ScaleY = ScaleY;
                    blip.Name = Name;
                    return blip;
                }
                return null;
            }
            public static Blip AddBlipForCoord(Vector3 xyz)
            {
                return Function.Call<Blip>(Hash.ADD_​BLIP_​FOR_​COORD, xyz.X, xyz.Y, xyz.Z);
            }

            public static Blip CreateGPSBlip(Vector3 pos, int spriteID, BlipColor color) // aka Mission Blip.
            {
                Blip blip = World.CreateBlip(pos);
                while (blip != null && !blip.Exists())
                {
                    Script.Wait(0);
                }
                blip.Sprite = (BlipSprite)spriteID;
                blip.Color = color;
                SetBlipGPS(blip, 156, true, false);
                return blip;
            }
            public static void SetBlipGPS(Blip blip, int color, bool drawFromPlayer, bool displayonfoot)
            {
                if (blip != null)
                {
                    Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
                    Function.Call(Hash.START_GPS_MULTI_ROUTE, color, drawFromPlayer, displayonfoot);
                    Function.Call(Hash.SET_GPS_MULTI_ROUTE_RENDER, true);
                    Function.Call(Hash.ADD_POINT_TO_GPS_MULTI_ROUTE, blip.Position.X, blip.Position.Y, blip.Position.Z);
                }
            }
            public static void SetGPSMultiRouteRender(bool toggle)
            {
                Function.Call(Hash.SET_GPS_MULTI_ROUTE_RENDER, toggle);
            }
            public static void ClearGPSMultiRoute()
            {
                Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
            }

            static List<Blip> LongRangeBlips = GetLongRangeBlips();
            public static List<Blip> GetLongRangeBlips()
            {
                List<Blip> list = new List<Blip>();
                for (int i = 0; i < allBlips.Length; i++)
                {
                    if (!allBlips[i].IsShortRange)
                    {
                        list.Add(allBlips[i]);
                    }
                }
                return list;
            }
            public static void ToggleShortRangeForLongRangeBlips(bool IsShortRange)
            {
                for (int i = 0; i < LongRangeBlips.Count; i++)
                {
                    LongRangeBlips[i].IsShortRange = IsShortRange;
                }
            }
            public static Blip AddBlipForArea(Vector3 areaCenter, float width, float height)
            {
                return Function.Call<Blip>(Hash.ADD_​BLIP_​FOR_​AREA, areaCenter.X, areaCenter.Y, areaCenter.Z, width, height);
            }
            public static void SetBlipRotation(Blip blip, int rotation)
            {
                Function.Call(Hash.SET_​BLIP_​ROTATION, blip, rotation);
            }

        }
    }
    
}
// mph_pri_fin_ext
