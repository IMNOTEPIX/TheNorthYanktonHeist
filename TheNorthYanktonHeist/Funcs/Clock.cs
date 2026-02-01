using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fClock
    {
        public static int GetClockHours => Function.Call<int>(Hash.GET_CLOCK_HOURS);
        public static int GetClockMinutes => Function.Call<int>(Hash.GET_CLOCK_MINUTES);
        public static int GetClockSeconds => Function.Call<int>(Hash.GET_CLOCK_SECONDS);
        public static int GetClockDayOfWeek => Function.Call<int>(Hash.GET_​CLOCK_​DAY_​OF_​WEEK);
        public static int GetClockDayOfMonth => Function.Call<int>(Hash.GET_​CLOCK_​DAY_​OF_​MONTH);
        public static int GetClockMonth => Function.Call<int>(Hash.GET_​CLOCK_​MONTH);
        public static int GetClockYear => Function.Call<int>(Hash.GET_​CLOCK_​YEAR);
        public static int ReturnNextDayOfWeek()
        {
            int dwVar = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_WEEK);
            if (dwVar == 6)
                return 0;
            else
                return dwVar += 1;
        }
        public static void SetClockTime(int hour = 0, int minute = 0, int second = 0, bool ease = false, bool easemins = true, int timeEaseAmount = 1)
        {
            if (!ease)
                Function.Call(Hash.SET_CLOCK_TIME, hour, minute, second);
            else
            {
                if (easemins)
                {
                    for (int time = timeEaseAmount; GetClockHours != hour && GetClockMinutes != minute; AddToClockTime(0, time, 0))
                    {
                        Script.Wait(0);
                    }
                }
                else
                {
                    for (int time = timeEaseAmount; GetClockHours != hour && GetClockMinutes != minute; AddToClockTime(0, 0, time))
                    {
                        Script.Wait(0);
                    }
                }
                if (GetClockHours == hour && GetClockMinutes > (minute - 10) && GetClockMinutes < (minute + 10))
                    SetClockTime(hour, minute, second);
            }
        }
        public static void PauseClock(bool toggle)
        {
            Function.Call(Hash.PAUSE_​CLOCK, toggle);
        }
        public static void AdvanceClockTimeTo(int hour = 0, int minute = 0, int second = 0)
        {
            Function.Call(Hash.ADVANCE_​CLOCK_​TIME_​TO, hour, minute, second);
        }
        public static void AddToClockTime(int hours = 0, int minutes = 0, int seconds = 0)
        {
            Function.Call(Hash.ADD_​TO_​CLOCK_​TIME, hours, minutes, seconds);
        }
        public static void SetClockDate(int day, int month, int year)
        {
            Function.Call(Hash.SET_​CLOCK_​DATE, day, month, year);
        }
        public static int GetMillisecondsPerGameMinute()
        {
            return Function.Call<int>(Hash.GET_​MILLISECONDS_​PER_​GAME_​MINUTE);
        }
        /// <summary>
        /// Gets System Time
        /// </summary>
        public static unsafe void GetPOSIXTime(int* year, int* month, int* day, int* hour, int* minute, int* second)
        {
            Function.Call(Hash.GET_​POSIX_​TIME, year, month, day, hour, minute, second);
        }
        /// <summary>
        /// Gets current UTC time
        /// </summary>
        public static unsafe void GetUTCTime(int* year, int* month, int* day, int* hour, int* minute, int* second)
        {
            Function.Call(Hash.GET_​UTC_​TIME, year, month, day, hour, minute, second);
        }
        /// <summary>
        /// Gets Local System Time
        /// </summary>
        public static unsafe void GetLocalTime(int* year, int* month, int* day, int* hour, int* minute, int* second)
        {
            Function.Call(Hash.GET_​LOCAL_​TIME, year, month, day, hour, minute, second);
        }
    }
}
