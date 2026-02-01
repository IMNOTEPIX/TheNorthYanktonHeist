using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fWeather
    {
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

        private static string WeatherTypeToString(WeatherTypes weatherType)
        {
            int idx = (int)weatherType;
            if (idx >= 0 && idx < WeatherStr.Length)
                return WeatherStr[idx];
            return null;
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
            uint weatherHash = (uint)Function.Call<Hash>(Hash.GET_PREV_WEATHER_TYPE_HASH_NAME);
            for (int i = 0; i < WeatherHashes.Length; i++)
            {
                if (weatherHash == WeatherHashes[i])
                    return (WeatherTypes)i;
            }
            return WeatherTypes.EXTRASUNNY;
        }
        public static WeatherTypes GetNextWeatherEnum()
        {
            uint weatherHash = (uint)Function.Call<Hash>(Hash.GET_​NEXT_​WEATHER_​TYPE_​HASH_​NAME);
            for (int i = 0; i < WeatherHashes.Length; i++)
            {
                if (weatherHash == WeatherHashes[i])
                    return (WeatherTypes)i;
            }
            return WeatherTypes.EXTRASUNNY;
        }
        public static bool IsCurrentWeatherType(WeatherTypes weatherType)
        {
            return Function.Call<bool>(Hash.IS_​PREV_​WEATHER_​TYPE, WeatherTypeToString(weatherType));
        }
        public static bool IsNextWeatherType(WeatherTypes weatherType)
        {
            return Function.Call<bool>(Hash.IS_​NEXT_​WEATHER_​TYPE, WeatherTypeToString(weatherType));
        }
        public static void SetWeatherTypePersist(WeatherTypes weatherType)
        {
            Function.Call(Hash.SET_​WEATHER_​TYPE_​PERSIST, WeatherTypeToString(weatherType));
        }
        public static void SetWeatherTypeNowPersist(WeatherTypes weatherType)
        {
            Function.Call(Hash.SET_WEATHER_TYPE_NOW_PERSIST, WeatherTypeToString(weatherType));
        }
        public static void SetWeatherTypeNow(WeatherTypes weatherType)
        {
            Function.Call(Hash.SET_WEATHER_TYPE_NOW, WeatherTypeToString(weatherType));
        }
        public static void SetWeatherTypeOvertimePersist(WeatherTypes weatherType, float time)
        {
            Function.Call(Hash.SET_​WEATHER_​TYPE_​OVERTIME_​PERSIST, WeatherTypeToString(weatherType), time);
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
            Function.Call(Hash.SET_​OVERRIDE_​WEATHER, WeatherTypeToString(weatherType));
        }
        /// <summary>
        /// Identical to SET_OVERRIDE_WEATHER but has an additional BOOL param that sets some weather var to 0 if true
        /// </summary>
        public static void SetOverrideWeatherEx(WeatherTypes weatherType, bool p1)
        {
            Function.Call(Hash.SET_​OVERRIDE_​WEATHEREX, WeatherTypeToString(weatherType), p1);
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
        public static Hash GetCurrentWeatherTypeHash()
        {
            return Function.Call<Hash>(Hash.GET_​PREV_​WEATHER_​TYPE_​HASH_​NAME);
        }
        public static Hash GetNextWeatherTypeHash()
        {
            return Function.Call<Hash>(Hash.GET_​NEXT_​WEATHER_​TYPE_​HASH_​NAME);
        }
    }
}
