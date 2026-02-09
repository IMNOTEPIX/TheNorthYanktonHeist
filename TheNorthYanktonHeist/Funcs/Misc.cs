using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fMisc
    {
        public static void SetTimeScale(float timeScale)
        {
            Function.Call(Hash.SET_​TIME_​SCALE, timeScale);
        }
        public static object pushArgs(params object[] args)
        {
            foreach (object obj in args)
            {
                if (obj.GetType() == typeof(int))
                {
                    return (int)obj;
                }
                else if (obj.GetType() == typeof(float))
                {
                    return (float)obj;
                }
                else if (obj.GetType() == typeof(double))
                {
                    return (float)((double)obj);
                }
                else if (obj.GetType() == typeof(bool))
                {
                    return (bool)obj;
                }
                else if (obj.GetType() == typeof(string))
                {
                    return (string)obj;
                }
                else if (obj.GetType() == typeof(char))
                {
                    return ((char)obj).ToString();
                }
            }
            return null;
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
        public static void ClearArea(float X, float Y, float Z, float radius, bool p4, bool ignoreCopCars, bool ignoreObjects, bool p7)
        {
            Function.Call(Hash.CLEAR_AREA, X, Y, Z, radius, p4, ignoreCopCars, ignoreObjects, p7);
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
        public static void ClearAreaOfVehicles(Vector3 xyz, float radius, bool p4 = false, bool p5 = false, bool p6 = false, bool p7 = false, bool p8 = false, bool p9 = false, int p10 = 0)
        {
            Function.Call(Hash.CLEAR_AREA_OF_VEHICLES, xyz.X, xyz.Y, xyz.Z, radius, p4, p5, p6, p7, p8, p9, p10);
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
}
