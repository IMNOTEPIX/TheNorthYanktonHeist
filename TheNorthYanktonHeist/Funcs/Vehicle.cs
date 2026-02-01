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
    public class fVehicle
    {
        /// <summary>
        /// p1 = timers???
        /// </summary>
        public static bool IsVehicleStuckTimerUp(Vehicle vehicle, int p1, int ms)
        {
            return Function.Call<bool>(Hash.IS_​VEHICLE_​STUCK_​TIMER_​UP, vehicle, p1, ms);
        }
        public static void SetVehicleIsConsideredByPlayer(Vehicle vehicle, bool toggle)
        {
            Function.Call(Hash.SET_​VEHICLE_​IS_​CONSIDERED_​BY_​PLAYER, vehicle, toggle);
        }
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
                        vehList[i].Delete();
                }
                vehList.Clear();
            }
        }
        public static void DeleteVehiclesInArray(Vehicle[] vehArray)
        {
            if (vehArray.Length > 0)
            {
                for (int i = 0; i < vehArray.Length; i++)
                {
                    if (vehArray[i] != null)
                        vehArray[i].Delete();
                }
            }
        }

        public static Vehicle CreateVehicleForList(List<Vehicle> vehList, Model model, Vector3 pos, float heading = 0)
        {
            Vehicle veh = World.CreateVehicle(model, pos, heading);
            while (veh != null && !veh.Exists())
            {
                Script.Wait(0);
            }
            if (!vehList.Contains(veh))
                vehList.Add(veh);
            return veh;
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

        /// <summary>
        /// colors https://pastebin.com/pwHci0xK
        /// </summary>
        public static void SetVehicleColours(Vehicle vehicle, int colorPrimary, int colorSecondary)
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
}
