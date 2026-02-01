using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
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
}
