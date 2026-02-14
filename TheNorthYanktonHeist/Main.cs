using BillsyLiamGTA.UI.Elements;
using BillsyLiamGTA.UI.Scaleform;
using BillsyLiamGTA.UI.Timerbars;
using Global;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using TheNorthYanktonHeist;
using TheNorthYanktonHeist.Funcs;
using TheNorthYanktonHeist.Scenes;
using static Globals;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Hash = GTA.Native.Hash;
using Screen = GTA.UI.Screen;

namespace TheNorthYanktonHeist
{
    
    public class Start : Script
    {
        public Start()
        {
            Tick += OnTick;
            Aborted += OnShutdown;
        }
        Vector3 PlanePos = new Vector3(1731.8021f, 3308.911f, 40.2237f);
        float PlaneH = 194.8553f;
        public static Vehicle Plane;
        public static Blip PlaneBlip;
        bool WasDestroyed 
        { 
            get 
            {
                if (Plane != null)
                    return IsPlaneDestroyed;
                else
                    return false;
            }
        }
        bool SpawnedIn => Plane != null;
        public static bool fixedPosition = false;
        bool IsPlaneDestroyed
        {
            get
            {
                if (Plane != null && Plane.IsConsideredDestroyed && fEntity.GetEntityHealth(Plane) <= 0)
                    return true;
                if (Plane != null && !Plane.IsConsideredDestroyed && fEntity.GetEntityHealth(Plane) > 0)
                    return false;
                return true;
            }
        }
        public static int startSwitch = 0;

        private void OnTick(object sender, EventArgs e)
        {
            if (!Game.IsMissionActive)
            {
                if (Plane != null && Plane.AttachedBlip != null)
                {
                    Plane.AttachedBlip.RotationFloat = Plane.Heading;
                }
            }
            if (Globals.missionSwitch == 0)
            {
                if (Game.IsMissionActive)
                {
                    if (PlaneBlip != null)
                    {
                        PlaneBlip.Delete();
                        PlaneBlip = null;
                    }
                    if (Plane != null)
                    {
                        if (Plane.AttachedBlip != null)
                        {
                            Plane.AttachedBlip.Delete();
                        }
                        Plane.Delete();
                        Plane = null;
                    }
                }
                else
                {
                    switch (startSwitch)
                    {
                        case 0:
                            Heist.restarting = false;
                            if (PlaneBlip == null && fPlayer.GetDistanceTo(PlanePos) > 500f && !WasDestroyed && !SpawnedIn)
                            {
                                PlaneBlip = fBlip.CreateBlipForCoordWithParams(PlanePos, (BlipSprite)916, (BlipColor)5, 1f, "North Yankton Heist");
                                PlaneBlip.RotationFloat = PlaneH;
                            }
                            if (PlaneBlip != null)
                            {
                                if (fPlayer.IsWanted)
                                    PlaneBlip.Alpha = 0;
                                else
                                    PlaneBlip.Alpha = 255;
                                if (Plane == null)
                                {
                                    if (fPlayer.GetDistanceTo(PlanePos) > 200f)
                                    {
                                        if (fPlayer.GetDistanceTo(PlanePos) < 300f)
                                        {
                                            fMisc.ClearAreaOfVehicles(PlanePos, 10f);
                                            Plane = fVehicle.CreateVehicle("cuban800", PlanePos, PlaneH);
                                        }
                                    }

                                }
                            }
                            if (Plane != null)
                            {
                                if (!IsPlaneDestroyed)
                                {
                                    if (!fEntity.HasCollisionLoadedAroundEntity(Plane))
                                    {
                                        fEntity.SetEntityLoadCollisionFlag(Plane, true);
                                    }
                                    if (!fixedPosition)
                                    {
                                        fVehicle.SetVehicleOnGroundProperly(Plane);
                                        fixedPosition = true;
                                    }
                                    if (PlaneBlip != null && Plane.AttachedBlip == null)
                                    {
                                        Plane.AddBlip();
                                    }
                                    if (Plane.AttachedBlip != null)
                                    {
                                        Plane.AttachedBlip.Alpha = 255;
                                        Plane.AttachedBlip.RotationFloat = Plane.Heading;
                                        Plane.AttachedBlip.Sprite = (BlipSprite)916;
                                        Plane.AttachedBlip.Color = (BlipColor)5;
                                        Plane.AttachedBlip.Name = "North Yankton Heist";
                                        if (PlaneBlip != null)
                                            PlaneBlip.DisplayType = BlipDisplayType.NoDisplay;
                                        if (PlaneBlip.DisplayType == BlipDisplayType.NoDisplay)
                                            startSwitch = 1;
                                    }
                                }
                            }
                            break;
                        case 1:
                            if (Game.Player.CanStartMission)
                            {
                                fMisc.ClearArea(new Vector3(1735.602f, 3294.539f, 41.80106f), 10f, false, true, true);
                                if (fPlayer.IsWanted)
                                {
                                    fVehicle.SetVehicleIsConsideredByPlayer(Plane, false);
                                    if (Plane.AttachedBlip != null)
                                        Plane.AttachedBlip.Alpha = 0;
                                    if (fPlayer.GetDistanceTo(Plane.Position) < 6f)
                                    {
                                        if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                            fHud.DisplayGXTHelpText("NTH_STARTWANTED");
                                        else
                                            fHud.DisplayHelpText("~s~Lose the cops to begin the heist.");
                                        
                                    }
                                }
                                else
                                {
                                    fVehicle.SetVehicleIsConsideredByPlayer(Plane, true);
                                    if (Plane.AttachedBlip != null)
                                        Plane.AttachedBlip.Alpha = 255;
                                    if (fPlayer.GetDistanceTo(Plane.Position) < 6f)
                                    {
                                        fHud.ClearBrief();
                                        if (!fPlayer.ped.IsEnteringVehicle || !fPlayer.ped.IsSittingInVehicle(Plane))
                                        {
                                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                                fHud.DisplayGXTHelpText("NTH_TOSTART");
                                            else
                                                fHud.DisplayHelpText("~s~Press ~INPUT_ENTER~ to start the North Yankton Heist.");
                                        }
                                        if (fPlayer.ped.IsEnteringVehicle || fPlayer.ped.IsSittingInVehicle(Plane))
                                        {
                                            fHud.ClearAllPrints();
                                            fHud.ClearBrief();
                                            fHud.ClearAllHelpMessages();
                                            fHud.ClearGPSMultiRoute();
                                            fHud.ClearHelp(true);
                                            startSwitch = 2;
                                        }
                                        if (Game.IsControlJustPressed(GTA.Control.Enter))
                                        {
                                            fTimer.SetTimerA(0);
                                            fHud.ClearAllPrints();
                                            fHud.ClearBrief();
                                            fHud.ClearAllHelpMessages();
                                            fHud.ClearGPSMultiRoute();
                                            fHud.ClearHelp(true);
                                            tastEnterplane();
                                            
                                            startSwitch = 2;
                                        }
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (fPlayer.ped.CurrentVehicle == Plane)
                            {
                                if (PlaneBlip != null)
                                {
                                    PlaneBlip.Delete();
                                    PlaneBlip = null;
                                }
                                Plane.AttachedBlip.Name = "Plane";
                                Plane.AttachedBlip.Color = BlipColor.Blue;
                                Plane.AttachedBlip.Alpha = 0;
                                startSwitch = 0;
                                fixedPosition = false;
                                Globals.missionSwitch = 1;
                            }
                            else
                            {
                                if (fTimer.TimerA() > 15000)
                                {
                                    fPlayer.ped.SetIntoVehicle(Plane, VehicleSeat.Driver);
                                    if (PlaneBlip != null)
                                    {
                                        PlaneBlip.Delete();
                                        PlaneBlip = null;
                                    }
                                    Plane.AttachedBlip.Color = BlipColor.Blue;
                                    Plane.AttachedBlip.Alpha = 0;
                                    startSwitch = 0;
                                    fixedPosition = false;
                                    Globals.missionSwitch = 1;
                                }
                                else
                                    Plane.AttachedBlip.Alpha = 255;
                            }
                            break;
                    }
                    if (PlaneBlip != null)
                    {
                        if (!SpawnedIn)
                        {
                            PlaneBlip.DisplayType = BlipDisplayType.NoDisplay;
                        }
                        if (!SpawnedIn && fPlayer.GetDistanceTo(PlanePos) > 200f)
                        {
                            PlaneBlip.DisplayType = BlipDisplayType.Default;
                        }
                        if (Plane != null && Plane.IsConsideredDestroyed)
                        {
                            if (Plane.AttachedBlip != null)
                            {
                                Plane.AttachedBlip.Delete();
                            }
                            PlaneBlip.DisplayType = BlipDisplayType.NoDisplay;
                            startSwitch = 0;
                        }
                    }
                    if (Plane != null)
                    {
                        if (fPlayer.GetDistanceTo(Plane.Position) > 440f)
                        {
                            if (Plane.AttachedBlip != null)
                            {
                                Plane.AttachedBlip.Delete();
                            }
                            Plane.Delete();
                            Plane = null;
                            startSwitch = 0;
                        }
                    }
                }
            }
        }

        void CleanUp()
        {
            if (PlaneBlip != null)
            {
                PlaneBlip.Delete();
                PlaneBlip = null;
            }
            if (Plane != null)
            {
                if (Plane.AttachedBlip != null)
                {
                    Plane.AttachedBlip.Delete();
                }
                Plane.MarkAsNoLongerNeeded();
                Plane = null;
            }
        }

        unsafe void tastEnterplane()
        {
            int iVar32 = 0;
            Function.Call(Hash.CLEAR_SEQUENCE_TASK, &iVar32);
            Function.Call(Hash.OPEN_SEQUENCE_TASK, &iVar32);
            Function.Call(Hash.TASK_ENTER_VEHICLE, fPlayer.ped, Plane, 20000, -1, 2.0, 1, 0);
            Function.Call(Hash.CLOSE_SEQUENCE_TASK, iVar32);
            Function.Call(Hash.TASK_PERFORM_SEQUENCE, fPlayer.ped, iVar32);
            Function.Call(Hash.CLEAR_SEQUENCE_TASK, &iVar32);
        }
        private void OnShutdown(object sender, EventArgs e)
        {
            CleanUp();
        }
    }

    public class Heist : Script
    {
        public Heist()
        {
            Tick += onTick;
            Aborted += onShutdown;
        }


        Vehicle Plane;
        Blip LudendorffNorthYankton;
        public static bool PlayerTeleportedToPrologue = false;
        bool IsPlaneDestroyed
        {
            get
            {
                if (Plane != null && Plane.IsConsideredDestroyed && fEntity.GetEntityHealth(Plane) <= 0)
                    return true;
                if (Plane != null && !Plane.IsConsideredDestroyed && fEntity.GetEntityHealth(Plane) > 0)
                    return false;
                return true;
            }
        }
        bool IsPrologueVehicleDestroyed
        {
            get
            {
                if (PrologueVehicle != null && PrologueVehicle.IsConsideredDestroyed || fVehicle.GetVehicleEngineHealth(PrologueVehicle) <= 0)
                    return true;
                if (PrologueVehicle != null && !PrologueVehicle.IsConsideredDestroyed || fVehicle.GetVehicleEngineHealth(PrologueVehicle) > 0)
                    return false;
                return true;
            }
        }
        private static int FailVariation { get; set; }
        public static void SetFailVariation(FailVariations failVariation) => FailVariation = (int)failVariation;
        public static FailVariations GetFailVariation() => (FailVariations)FailVariation;
        public enum FailVariations
        {
            None,
            PlaneDestroyed,
            PrologueVehicleDestroyed,
            PlayerAbandonedThePrologueVehicle,
            PlayerFailedToReachTheDepot,
        }
        Vehicle PrologueVehicle;
        string PrologueVehicleName;
        Camera PrologueIntroCam;
        Camera PrologueIntroCam2;
        int num;
        Blip DepotBlip;
        public static bool justFailed = false;
        fWeather.WeatherTypes weatherTypeBeforeMission;
        bool weatherTypeSaved = false;
        bool outfitSaved = false;
        bool timeAdvanced1 = false;
        Vector2 local;
        bool scriptsKilled = false;

        private void onTick(object sender, EventArgs e)
        {
            //Screen.ShowHelpText($"{Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString)}");
            //fPathfind.GetClosestVehicleNode(fPlayer.ped.CurrentVehicle.Position, out local);
            //Screen.ShowSubtitle($"{local}");
            if (Globals.missionSwitch > 3)
            {
                //fPathfind.GetClosestVehicleNode(fPlayer.ped.Position, out local);
                //local = new Vector2(fPlayer.ped.Position.X, fPlayer.ped.Position.Y);
            }
            //Screen.ShowSubtitle($"{fEntity.IsEntityInArea(fPlayer.ped, new Vector3(3873.535f, -5083.751f, 150f), new Vector3(3462.987f, -4794.556f, 65f))|| fEntity.IsEntityInArea(fPlayer.ped, new Vector3(3896.537f, -5161.008f, 150f), new Vector3(4500.178f, -4978.984f, 65f)) || fEntity.IsEntityInArea(fPlayer.ped, new Vector3(4500.178f, -4978.984f, 150f), new Vector3(5545.2f, -5260.716f, 65f))}");
            switch (Globals.missionSwitch)
            {
                case -1:
                    restarting = false;
                    restartPending = false;
                    fWeather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                    fClock.SetClockTime(12, 0, 0);
                    fInterior.RemoveIpl("prologue06_int");
                    fInterior.RemoveIpl("prologue01");
                    fInterior.RemoveIpl("prologue02");
                    fInterior.RemoveIpl("prologue03");
                    fInterior.RemoveIpl("prologue04");
                    fInterior.RemoveIpl("prologue05");
                    fInterior.RemoveIpl("prologue06");
                    fInterior.RemoveIpl("prologuerd");
                    fInterior.RemoveIpl("Prologue01c");
                    fInterior.RemoveIpl("Prologue01d");
                    fInterior.RemoveIpl("Prologue01e");
                    fInterior.RemoveIpl("Prologue01f");
                    fInterior.RemoveIpl("Prologue01g");
                    fInterior.RemoveIpl("prologue01h");
                    fInterior.RemoveIpl("prologue01i");
                    fInterior.RemoveIpl("prologue01j");
                    fInterior.RemoveIpl("prologue01k");
                    fInterior.RemoveIpl("prologue01z");
                    fInterior.RemoveIpl("prologue03b");
                    fInterior.RemoveIpl("prologue04b");
                    fInterior.RemoveIpl("prologue05b");
                    fInterior.RemoveIpl("prologue06b");
                    fInterior.RemoveIpl("prologuerdb");
                    fInterior.RemoveIpl("DES_ProTree_start");
                    fInterior.RemoveIpl("prologue_grv_torch");
                    fInterior.RemoveIpl("prologue03_grv_dug");
                    fInterior.RemoveIpl("DES_ProTree_start_lod");
                    fInterior.RemoveIpl("prologue04_cover");
                    fInterior.RemoveIpl("prologue03_grv_fun");
                    fInterior.RemoveIpl("prologue03_grv_cov");
                    fInterior.RemoveIpl("prologue_LODLights");
                    fInterior.RemoveIpl("prologue_DistantLights");
                    int zone = fZone.GetZoneFromNameID("PrLog");
                    fZone.SetZoneEnabled(zone, false);
                    fHud.ToggleNorthYanktonMap(false);
                    fPathfind.SetAllowStreamPrologueNodes(false);
                    fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, false, true);
                    fInterior.PrologueMap.YankTon = false;
                    fInterior.PrologueMap.EnableNorthYanktonTrainTracks(false);
                    fStreaming.SetMapDataCullboxEnabled("prologue", false);
                    fStreaming.SetMapDataCullboxEnabled("Prologue_Main", false);
                    fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), false);
                    //Globals.globalBlips = 1;
                    //Globals.globalScripts = 2;
                    fHud.ClearAllPrints();
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                    fAudio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                    Globals.missionSwitch = 2;
                    break;
                case 1:
                    fAudio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                    checkpoint = 1;
                    if (!scriptsKilled)
                    {
                        ScriptManager.ScriptManager.KillScripts();
                        scriptsKilled = true;
                    }
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    MissionShard missionShard = new MissionShard();
                    missionShard.Shard_In("~s~North Yankton Heist", "~s~Break into the ~y~Bobcat Security Depot~s~ in North Yankton and clean out the ~g~vault.~s~", 2, 0.375f);
                    missionShard = null;
                    Wait(1000);
                    if (!outfitSaved)
                    {
                        pedProps = fPed.GetPedPropData(fPlayer.ped);
                        pedVariation = fPed.GetPedVariationData(fPlayer.ped);
                        outfitSaved = true;
                    }
                    if (!weatherTypeSaved)
                    {
                        weatherTypeBeforeMission = fWeather.GetCurrentWeatherEnum();
                        weatherTypeSaved = true;
                    }
                    //Globals.globalBlips = 1;
                    //Globals.globalScripts = 2;
                    fHud.ClearAllPrints();
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                    fAudio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                    if (weatherTypeSaved && outfitSaved)
                    {
                        Globals.missionSwitch = 2;
                    }
                    break;
                case 2:
                    fAudio.StartAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                    //Globals.globalBlips = 2;
                    //Globals.globalScripts = 2;
                    if (fPlayer.IsWanted)
                    {
                        if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                            fHud.ShowGXTSubtitle("NTH_WANTED");
                        else
                            Screen.ShowSubtitle("~s~Lose the Cops.");
                    }
                    if (LudendorffNorthYankton == null)
                    {
                        LudendorffNorthYankton = fBlip.CreateBlipForCoordWithParams(new Vector3(-3841.2847f, 5020.6064f, 174.8651f), BlipSprite.Standard, (BlipColor)5, 1f, "Destination");
                    }
                    if (Plane != null)
                    {
                        if (fPlayer.ped.IsInVehicle(Plane))
                        {
                            if (!fPlayer.IsWanted)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fHud.ShowGXTSubtitle("NTH_FLYTONY");
                                else
                                    Screen.ShowSubtitle("~s~Fly to ~y~North Yankton.~s~");
                            }
                        }
                        if (fPlayer.ped.IsInVehicle(Plane))
                        {
                            if (Plane.AttachedBlip != null)
                            {
                                if (LudendorffNorthYankton != null)
                                {
                                    if (fPlayer.IsWanted)
                                        LudendorffNorthYankton.Alpha = 0;
                                    else
                                        LudendorffNorthYankton.Alpha = 255;
                                    if (fPlayer.GetCarDistanceTo(new Vector2(-3841.2847f, 5020.6064f)) < 500f && !fPlayer.IsWanted)
                                    {
                                        Globals.missionSwitch = 3;
                                    }
                                }
                                if (Start.PlaneBlip != null)
                                {
                                    Start.PlaneBlip.Delete();
                                    Start.PlaneBlip = null;
                                }
                                Plane.AttachedBlip.Alpha = 0;
                            }
                        }
                        else
                        {
                            if (!fPlayer.IsWanted)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fHud.ShowGXTSubtitle("NTH_GETBKINTOPLANE");
                                else
                                    Screen.ShowSubtitle("~s~Get back in the ~b~Plane.~s~");
                            }
                            if (LudendorffNorthYankton != null)
                                LudendorffNorthYankton.Alpha = 0;
                            Plane.AttachedBlip.Alpha = 255;
                            Plane.AttachedBlip.DisplayType = BlipDisplayType.Default;
                        }
                    }
                    break;
                case 3:
                    if (restarting)
                        restarting = false;
                    switch (case3Switch)
                    {
                        case 1:
                            fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.Suspense);
                            checkpoint = 2;
                            if (LudendorffNorthYankton != null)
                            {
                                LudendorffNorthYankton.Delete();
                                LudendorffNorthYankton = null;
                            }
                            fPlayer.ped.Task.DriveTo(Plane, new Vector3((-3841.2847f - 200f), 5020.6064f, (fPlayer.ped.Position.Z + 10f)), 10f, VehicleDrivingFlags.None, 5f);
                            Screen.FadeOut(2500);
                            while (!Screen.IsFadedOut)
                            {
                                Wait(0);
                            }
                            LoadingPrompt.Show("NorthYanktonHeist");
                            SetOutfit();
                            Wait(1000);
                            Plane.IsPositionFrozen = true;
                            Wait(7500);
                            if (Plane != null)
                            {
                                if (Plane.AttachedBlip != null)
                                {
                                    Plane.AttachedBlip.Delete();
                                }
                                Plane.Delete();
                                Plane = null;
                            }
                            fPlayer.ped.IsPositionFrozen = true;
                            fPlayer.ped.IsInvincible = true;
                            Game.Player.Character.Task.ClearAll();
                            case3Switch = -1;
                            break;
                        case -1:
                            SetOutfit();
                            fPlayer.SetMaxWantedLevelTo0();
                            fAudio.StopAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                            fAudio.StartAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                            Vector3 camPos = new Vector3(3575.468f, -4871.25f, 117.1896f);
                            Vector3 camRot = new Vector3(20f, 0f, 26.39456f);
                            Vector3 camPos2 = new Vector3(3576.268f, -4872.851f, 117.1896f);
                            Vector3 camRot2 = new Vector3(25f, 0f, 26.39456f);
                            Vector3 driveDestination = new Vector3(3612.947f, -4910.009f, 111.2528f);
                            Vector3 carsPos = new Vector3(3528.305f, -4878.405f, 111.2986f);
                            float carsHeading = 250.5463f;
                            if (!fInterior.PrologueMap.YankTon)
                            {
                                fInterior.PrologueMap.LoadYankton();
                            }
                            if (fInterior.PrologueMap.YankTon)
                            {
                                fClock.SetClockTime(4, 30, 0);
                                fWeather.SetWeatherTypeNowPersist(fWeather.WeatherTypes.Snow);
                                fClock.PauseClock(true);
                                fInterior.PrologueMap.EnableNorthYanktonTrainTracks(true);
                                fStreaming.SetMapDataCullboxEnabled("prologue", true);
                                fStreaming.SetMapDataCullboxEnabled("Prologue_Main", true);
                                fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                                Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                                fPlayer.ped.ClearBloodDamage();
                                fPlayer.ped.ClearVisibleDamage();
                                if (fPlayer.IsTrevor)
                                    fPed.SetPedPropIndex(fPlayer.ped, 1, 4, 0, true);
                                if (Screen.IsFadedOut)
                                {
                                    fPlayer.ped.IsPositionFrozen = false;
                                    fHud.RadarAndHud(false, false);
                                    fPlayer.PedPos(carsPos.X, carsPos.Y, carsPos.Z, carsHeading);
                                    PlayerTeleportedToPrologue = true;
                                    num = fMisc.GetRandomIntInRange(1, 4);
                                    if (PrologueVehicle == null)
                                    {
                                        switch (num)
                                        {
                                            case 1:
                                                PrologueVehicleName = "Mesa";
                                                PrologueVehicle = fVehicle.CreateVehicle(new Model("Mesa2"), carsPos, carsHeading);
                                                break;
                                            case 2:
                                                PrologueVehicleName = "Rancher";
                                                PrologueVehicle = fVehicle.CreateVehicle(new Model("RancherXL2"), carsPos, carsHeading);
                                                break;
                                            case 3:
                                                PrologueVehicleName = "Asea";
                                                PrologueVehicle = fVehicle.CreateVehicle(new Model("Asea2"), carsPos, carsHeading);
                                                break;
                                        }
                                    }
                                    if (PrologueIntroCam == null)
                                        PrologueIntroCam = fCam.CreateScriptedCam();
                                    while (PrologueIntroCam == null)
                                        Wait(0);
                                    if (PrologueIntroCam != null)
                                        fCam.SetupMovingCam(PrologueIntroCam, camPos, camRot, 55f, CameraShake.Hand, 0.5f);
                                    if (PrologueIntroCam2 == null)
                                        PrologueIntroCam2 = fCam.CreateScriptedCam();
                                    while (PrologueIntroCam2 == null)
                                        Wait(0);
                                    if (PrologueIntroCam2 != null)
                                        fCam.SetupMovingCam(PrologueIntroCam2, camPos2, camRot2, 60f, CameraShake.Hand, 0.5f);
                                    if (PrologueVehicle != null && PrologueIntroCam != null && PrologueIntroCam2 != null)
                                    {
                                        fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Suspense);
                                        fCam.SetCamActiveWithInterp(PrologueIntroCam2, PrologueIntroCam, 5000, fCam.CamGraphType.GRAPH_TYPE_SLOW_IN_OUT, fCam.CamGraphType.GRAPH_TYPE_SLOW_IN_OUT);
                                        fCam.RenderScriptCams(true, false, 3000, true, false, fCam.RenderingOptionFlag.RO_NO_OPTIONS);
                                        LoadingPrompt.Hide();
                                        Screen.FadeIn(3000);
                                        fPlayer.ped.IsInvincible = false;
                                        if (fPlayer.ped.CurrentVehicle != PrologueVehicle)
                                        {
                                            fAudio.SetVehicleRadioStation(PrologueVehicle, "OFF");
                                            fAudio.SetVehicleRadioEnabled(PrologueVehicle, false);
                                            fPlayer.ped.SetIntoVehicle(PrologueVehicle, VehicleSeat.Driver);
                                        }
                                        else
                                        {
                                            PrologueVehicle.LockStatus = VehicleLockStatus.PlayerCannotLeaveCanBeBrokenIntoPersist;
                                        }
                                        fPlayer.ped.Task.DriveTo(PrologueVehicle, driveDestination, 55f, VehicleDrivingFlags.None, 5f);
                                        PrologueVehicle.Speed = 10f;
                                        while (fPlayer.GetCarDistanceTo(new Vector2(3572.151f, -4890.441f)) > 5f)
                                            Wait(0);
                                        fCam.RenderScriptCams(false, true, 1700);
                                        if (PrologueIntroCam != null)
                                        {
                                            PrologueIntroCam.Delete();
                                            PrologueIntroCam = null;
                                        }
                                        if (PrologueIntroCam2 != null)
                                        {
                                            PrologueIntroCam2.Delete();
                                            PrologueIntroCam2 = null;
                                        }
                                        fHud.RadarAndHud(true, true);
                                        Globals.missionSwitch = 4;
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case 4:
                    fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.Gunfight);
                    fPathfind.GetClosestVehicleNode(fPlayer.ped.Position, out local);
                    enableFailCheckingForNY = true;
                    if (!fInterior.PrologueMap.YankTon)
                    {
                        fInterior.PrologueMap.LoadYankton();
                        fInterior.PrologueMap.EnableNorthYanktonTrainTracks(true);
                        fStreaming.SetMapDataCullboxEnabled("prologue", true);
                        fStreaming.SetMapDataCullboxEnabled("Prologue_Main", true);
                        fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                        fWeather.SetWeatherTypeNowPersist(fWeather.WeatherTypes.Snow);
                    }
                    if (DepotBlip == null)
                    {
                        DepotBlip = fBlip.CreateGPSBlip(new Vector3(5308.504f, -5221.282f, 83.51885f), 1, BlipColor.Yellow2);
                    }
                    if (DepotBlip != null)
                    {
                        DepotBlip.Name = "Depot";
                        if (PrologueVehicle != null)
                        {
                            if (fPlayer.ped.CurrentVehicle == PrologueVehicle)
                            {
                                if (!fEntity.IsEntityInArea(fPlayer.ped, new Vector3(3543.267f, -4875.534f, 61f), new Vector3(3649.131f, -4929.502f, 125f)) || fPlayer.GetCarDistanceTo(local) > 27f)
                                {
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fHud.ShowGXTSubtitle("NTH_GETBKONROAD");
                                    else
                                        Screen.ShowSubtitle("~s~Get back en route to the ~y~Depot.~s~");
                                }
                                if (fEntity.IsEntityInArea(fPlayer.ped, new Vector3(3543.267f, -4875.534f, 61f), new Vector3(3649.131f, -4929.502f, 125f)) || fPlayer.GetCarDistanceTo(local) < 27f)
                                {
                                    if (PrologueVehicle.AttachedBlip != null)
                                        PrologueVehicle.AttachedBlip.DisplayType = BlipDisplayType.NoDisplay;
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fHud.ShowGXTSubtitle("NTH_GOTODEPOT");
                                    else
                                        Screen.ShowSubtitle("~s~Go to the ~y~Depot.~s~");
                                    if (fPlayer.GetDistanceTo(new Vector2(DepotBlip.Position.X, DepotBlip.Position.Y)) < 400f)
                                    {
                                        Spawn();
                                    }
                                    PrologueVehicle.LockStatus = VehicleLockStatus.None;
                                    if (!timeAdvanced1)
                                    {
                                        if (fClock.GetClockHours == 4)
                                        {
                                            fClock.PauseClock(false);
                                        }
                                    }
                                    if (fClock.GetClockHours != 5 && (fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(5356.7324f, -5201.1553f, 80.83122f), new Vector3(5356.5454f, -5179.6f, 96.83691f), 20f, false, true, 0) || fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(5417.894f, -5108.7925f, 75.56319f), new Vector3(5412.488f, -5240.66f, 95.59789f), 100f, false, true, 0)))
                                    {
                                        fClock.SetClockTime(5, 0, 0, true, false, 4);
                                        fClock.PauseClock(true);
                                        timeAdvanced1 = true;
                                    }
                                    if (fClock.GetClockHours == 5)
                                    {
                                        fClock.SetClockTime(5, 0, 0);
                                        fClock.PauseClock(true);
                                        timeAdvanced1 = true;
                                        if (fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(5356.7324f, -5201.1553f, 80.83122f), new Vector3(5356.5454f, -5179.6f, 96.83691f), 20f, false, true, 0) || fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(5417.894f, -5108.7925f, 75.56319f), new Vector3(5412.488f, -5240.66f, 95.59789f), 100f, false, true, 0))
                                        {
                                            Globals.missionSwitch = 5;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (PrologueVehicle.AttachedBlip == null)
                                {
                                    PrologueVehicle.AddBlip();
                                    PrologueVehicle.AttachedBlip.Color = BlipColor.Blue;
                                    PrologueVehicle.AttachedBlip.Sprite = BlipSprite.Standard;
                                    PrologueVehicle.AttachedBlip.Name = "Car";
                                    PrologueVehicle.AttachedBlip.IsFriendly = true;
                                    PrologueVehicle.AttachedBlip.IsShortRange = false;
                                }
                                else
                                {
                                    PrologueVehicle.AttachedBlip.DisplayType = BlipDisplayType.Default;
                                }
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fHud.ShowGXTSubtitle("NTH_GETBKINTOVEH");
                                else
                                    Screen.ShowSubtitle("~s~Get back in the ~b~car.~s~");
                            }
                        }
                    }
                    break;
                case 5:
                    if (BoothGuard != null)
                    {
                        if (BoothGuard.IsDead)
                        {
                            if (BoothGuard.AttachedBlip != null)
                            {
                                BoothGuard.AttachedBlip.Delete();
                            }
                            BoothGuard.MarkAsNoLongerNeeded();
                        }
                        else
                        {
                            if (BoothGuard.AttachedBlip == null)
                            {
                                BoothGuard.AddBlip();
                                for (; ; )
                                {
                                    Blip attachedBlip = BoothGuard.AttachedBlip;
                                    if (attachedBlip != null && attachedBlip.Exists())
                                    {
                                        break;
                                    }
                                    Script.Wait(0);
                                }
                                BoothGuard.AttachedBlip.Sprite = (BlipSprite)270;
                                BoothGuard.AttachedBlip.Color = BlipColor.Red;
                                BoothGuard.AttachedBlip.Name = "Guard";
                                BoothGuard.AttachedBlip.Scale = 0.7f;
                                BoothGuard.AttachedBlip.DisplayType = BlipDisplayType.MiniMapOnly;
                            }
                            fPed.SetPedCombatAttributes(BoothGuard, fPed.CombatAttributes.CA_IS_A_GUARD, true);
                            fPed.SetPedCombatAttributes(BoothGuard, fPed.CombatAttributes.CA_PLAY_REACTION_ANIMS, false);
                            fPed.SetPedConfigFlag(BoothGuard, fPed.PedConfigFlags.PCF_DontBlipCop, true);
                            fPed.SetPedAsCop(BoothGuard, false);
                            fPed.SetPedCombatAbility(BoothGuard, fPed.CombatAbilityLevel.CAL_PROFESSIONAL);
                            fPed.SetPedCombatMovement(BoothGuard, fMisc.GetRandomIntInRange(1, 4));
                            BoothGuard.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
                            BoothGuard.RelationshipGroup = AiTeam;
                            fPed.SetRelationshipBetweenGroups(fPed.RelationshipTypes.ACQUAINTANCE_TYPE_PED_HATE, AiTeam, playersTeam);
                            fPed.SetRelationshipBetweenGroups(fPed.RelationshipTypes.ACQUAINTANCE_TYPE_PED_HATE, playersTeam, AiTeam);
                        }
                    }
                    if (BoothGuard != null)
                    {
                        if (DepotBlip != null)
                        {
                            fBlip.ClearGPSMultiRoute();
                            if (!BoothGuard.IsDead)
                            {
                                DepotBlip.Alpha = 0;
                            }
                            else
                            {
                                DepotBlip.Alpha = 255;
                            }
                        }
                        if (!BoothGuard.IsDead)
                        {
                            fPlayer.SetMaxWantedLevelTo0();
                            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Suspense);
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fHud.ShowGXTSubtitle("NTH_KILLGUARD");
                            else
                                Screen.ShowSubtitle("~s~Take out the ~r~Guard.~s~");
                        }
                        else
                        {
                            if (DepotBlip != null)
                            {
                                if (DepotBlip.Alpha == 255)
                                {
                                    if (case5switch == -1)
                                    {
                                        if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                            fHud.ShowGXTSubtitle("NTH_ENTRDEPOT");
                                        else
                                            Screen.ShowSubtitle("~s~Enter the ~y~Depot.~s~");
                                        if (fPlayer.GetDistanceTo(new Vector2(DepotBlip.Position.X, DepotBlip.Position.Y)) < 1.3f)
                                        {
                                            Screen.FadeOut(1000);
                                            while (!Screen.IsFadedOut)
                                                Wait(0);
                                            LoadingPrompt.Show("NorthYanktonHeist");
                                            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Gunfight);
                                            DepotBlip.Delete();
                                            DepotBlip = null;
                                            case5switch = 0;
                                        }
                                    }
                                }
                            }
                        }
                       
                    }
                    switch (case5switch)
                    {
                        case 0:
                            if (World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 929864185) != null &&
                                World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 929864185) != null)
                            {
                                Function.Call(Hash.CREATE_​MODEL_​SWAP, 5308.04f, -5191.028f, 82.99158f, 0.3f, 929864185, 269934519, true);
                                Function.Call(Hash.CREATE_​MODEL_​SWAP, 5302.142f, -5191.521f, 82.99158f, 0.3f, 929864185, 269934519, true);

                            }
                            if (World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 269934519) != null &&
                                World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 269934519) != null)
                            {
                                trolly1 = World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 269934519);
                                trolly2 = World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 269934519);
                            }
                            fHud.ClearPrints();
                            fPlayer.SetMaxWantedLevelTo0();
                            fPlayer.ped.IsPositionFrozen = false;
                            PlayerTeleportedToPrologue = true;
                            fHud.RadarAndHud(false, false);
                            fClock.SetClockTime(5, 0, 0);
                            fWeather.SetWeatherTypeNowPersist(fWeather.WeatherTypes.Snow);
                            fClock.PauseClock(true);
                            fInterior.PrologueMap.EnableNorthYanktonTrainTracks(true);
                            fStreaming.SetMapDataCullboxEnabled("prologue", true);
                            fStreaming.SetMapDataCullboxEnabled("Prologue_Main", true);
                            fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                            Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                            fPlayer.ped.ClearBloodDamage();
                            fPlayer.ped.ClearVisibleDamage();
                            if (fPlayer.IsTrevor)
                                fPed.SetPedPropIndex(fPlayer.ped, 1, 4, 0, true);
                            fAudio.StopAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                            new Combats(-681004504, new Vector3(5300.397f, -5197.885f, 83.502f), 285.0927f, fPed.CombatMovement.CM_WillAdvance, fPed.CombatAbilityLevel.CAL_AVERAGE, WeaponHash.Pistol, DisableAttrbutes, EnableAttrbutes);
                            new Combats(-681004504, new Vector3(5296.698f, -5194.016f, 83.5018f), 188.9838f, fPed.CombatMovement.CM_Defensive, fPed.CombatAbilityLevel.CAL_PROFESSIONAL, WeaponHash.CarbineRifle, DisableAttrbutes, EnableAttrbutes);
                            new Combats(-681004504, new Vector3(5297.962f, -5181.915f, 83.51785f), 359.7572f, fPed.CombatMovement.CM_Defensive, fPed.CombatAbilityLevel.CAL_PROFESSIONAL, WeaponHash.CarbineRifle, DisableAttrbutes, EnableAttrbutes);
                            new Combats(-681004504, new Vector3(5303.865f, -5178.598f, 83.50168f), 258.0538f, fPed.CombatMovement.CM_WillAdvance, fPed.CombatAbilityLevel.CAL_PROFESSIONAL, WeaponHash.PumpShotgun, DisableAttrbutes, EnableAttrbutes);
                            new Combats(-681004504, new Vector3(5300.957f, -5175.863f, 83.50175f), 12.30173f, fPed.CombatMovement.CM_Defensive, fPed.CombatAbilityLevel.CAL_PROFESSIONAL, WeaponHash.SMG, DisableAttrbutes, EnableAttrbutes);
                            CombatsList.Combats_Create();
                            Function.Call(Hash.ADD_NAVMESH_REQUIRED_REGION, 5297.568f, -5188.236f, 100f);
                            while (!Function.Call<bool>(Hash.IS_NAVMESH_LOADED_IN_AREA, 5297.568f - 50f, -5188.236f - 50f, 5297.568f + 50f, -5188.236f + 50f))
                                Wait(0);
                            if (AnimGuard == null)
                            {
                                AnimGuard = fPed.CreatePed(new Model("IG_ProlSec_02"), new Vector3(5318.503f, -5206.221f, (85.7187f - 3.2f)), 139.6356f);//  Function.Call<Ped>(Hash.CREATE_PED, 26, fMisc.GetHashKey("IG_ProlSec_02"), 5310.6543f, -5207.032f, (85.7187f - 3.2f), 139.6356f, true, true);
                                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, AnimGuard);
                                Function.Call(Hash.SET_ENTITY_SHOULD_FREEZE_WAITING_ON_COLLISION, AnimGuard, true);
                                Wait(50);
                            }
                            while (AnimGuard == null)
                                Wait(0);
                            if (AnimCam == null)
                            {
                                AnimCam = fCam.CreateScriptedCam();
                                fCam.SetupMovingCam(AnimCam, new Vector3(5308.226f, -5208.727f, 83.959f), new Vector3(-1.000001f, -3.000001f, -92.49069f), 55f, CameraShake.Hand, 0.5f);
                            }
                            while (AnimCam == null)
                            {
                                Wait(0);
                            }
                            if (AnimCam != null)
                            {
                                fCam.SetupMovingCam(AnimCam, new Vector3(5308.226f, -5208.727f, 83.959f), new Vector3(0f, 0f, -92.49069f), 42f, CameraShake.Hand, 1f);
                                ScriptCameraDirector.StartRendering();
                            }
                            if (AnimDoor == null)
                            {
                                AnimDoor = Function.Call<Prop>(Hash.CREATE_OBJECT_NO_OFFSET, fMisc.GetHashKey("v_ilev_cd_door2"), 5308.8574f, -5208.156f, ((86.9186f - 3.2f) - 0.05f), true, true, false, 0);
                                Function.Call(Hash.FREEZE_ENTITY_POSITION, AnimDoor, true);
                            }
                            while (AnimDoor == null)
                                Wait(0);
                            case5switch = 1;
                            break;
                        case 1:
                            Screen.FadeIn(1700);
                            LoadingPrompt.Hide();
                            if (!player.IsRunning)
                            {
                                fPlayer.ped.Weapons.Select(WeaponHash.Unarmed);
                                player.Create();
                                door.Create();
                                player.Rate = 0f;
                                door.Rate = 0f;
                                player.PlayPed(fPlayer.ped, fStreaming.RequestAnimDict("anim@apt_trans@hinge_l_action"), "player_exit");
                                door.PlayEntity(AnimDoor, fStreaming.RequestAnimDict("anim@apt_trans@hinge_l_action"), "door_exit");
                                door.PlayAudioEvent();
                            }
                            else
                            {
                                Wait(1000);
                                player.Rate = 1f;
                                door.Rate = 1f;
                                case5switch = 2;
                            }
                            break;
                        case 2:
                            if (player.IsRunning)
                            {
                                if (player.Phase >= 0.18f)
                                {
                                    AnimCam.Shake(CameraShake.Jolt, 2.5f);
                                    while (player.Phase < 0.75f)
                                    {
                                        Wait(0);
                                    }
                                    case5switch = 3;
                                }
                            }
                            break;
                        case 3:
                            fPlayer.ped.Weapons.Give(WeaponHash.PumpShotgun, 260, true, true);
                            fPlayer.ped.Weapons.Select(WeaponHash.PumpShotgun);
                            fPlayer.ped.Task.ClearAllImmediately();
                            player.Dispose();
                            door.Dispose();
                            GameplayCamera.SetCamViewModeForContext(CamViewModeContext.OnFoot, CamViewMode.FirstPerson);
                            fGraphics.AnimpostFXPlay("CamPushInNeutral", 1700, false);
                            fAudio.PlaySoundFrontend("1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET");
                            AnimGuard.Weapons.Give(WeaponHash.Pistol, 8, true, true);
                            AnimGuard.Weapons.Select(WeaponHash.Pistol);
                            AnimGuard.Task.RunTo(new Vector3(5314.563f, -5206.421f, 83.51863f), false, 20000);
                            Wait(700);
                            ScriptCameraDirector.StopRendering();
                            fHud.RadarAndHud(true, true);
                            Wait(500);
                            AnimGuard.Task.AimGunAtEntity(fPlayer.ped, 10000);
                            case5switch = 4;
                            break;
                        case 4:
                            CombatsList.Combats_Activate();
                            if (AnimCam != null)
                            {
                                AnimCam.Delete();
                                AnimCam = null;
                            }
                            fPlayer.FakeWantedLevel = 3;
                            case5switch = 5;
                            break;
                        case 5:
                            CombatsList.Combats_Check();
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fHud.ShowGXTSubtitle("NTH_PUSHTOVAULT");
                            else
                                Screen.ShowSubtitle("~s~Push through to the ~y~vault.~s~");
                            if (fPlayer.GetDistanceTo(new Vector3(5297.854f, -5188.762f, 83.51839f)) < 3.7f)
                            {
                                case5switch = 6;
                            }
                            break;
                        case 6:
                            if (InsideBlip == null)
                            {
                                InsideBlip = fBlip.CreateBlipForCoordWithParams(new Vector3(5298.512f, -5188.697f, 81.51868f), (BlipSprite)814, BlipColor.Yellow, 1f, "Vault");
                            }
                            CombatsList.Combats_Check();
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fHud.ShowGXTSubtitle("NTH_PLANTBOMB");
                            else
                                Screen.ShowSubtitle("~s~Plant an ~g~explosive device~s~ on the ~y~vault door.~s~");
                            if (fPlayer.GetDistanceTo(new Vector3(5298.2f, -5187.9f, 84.21848f)) < 2.5f)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fHud.DisplayGXTHelpText("NTH_TOPLANTBOMB");
                                else
                                    fHud.DisplayHelpText("~s~Press ~INPUT_CONTEXT~ to plant the ~g~explosive device.~s~");
                                Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, fStreaming.RequestAnimDict("anim@scripted@player@mission@tun_bomb_plant@male@"), "enter", 5298.2f, -5187.9f, 83.21848f, 0f, 0f, -85f, 0f, 2);
                                float z = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, fStreaming.RequestAnimDict("anim@scripted@player@mission@tun_bomb_plant@male@"), "enter", 5298.2f, -5187.9f, 83.21848f, 0f, 0f, -85f, 0f, 2).Z;
                                if (Game.IsControlJustPressed(GTA.Control.Context))
                                {
                                    Function.Call(Hash.SET_MOVEMENT_MODE_OVERRIDE, Game.Player.Character, "DEFAULT_ACTION");
                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, vector.X, vector.Y, vector.Z, 1f, 10000, -85f, 1f);
                                    Function.Call(Hash.SET_PED_KEEP_TASK, Game.Player.Character, true);
                                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                                    case5switch = 7;
                                }
                            }
                            break;
                        case 7:
                            if (InsideBlip != null)
                            {
                                InsideBlip.Delete();
                                InsideBlip = null;
                            }
                            if (World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 929864185) != null &&
                                World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 929864185) != null)
                            {
                                Function.Call(Hash.CREATE_​MODEL_​SWAP, 5308.04f, -5191.028f, 82.99158f, 0.3f, 929864185, 269934519, true);
                                Function.Call(Hash.CREATE_​MODEL_​SWAP, 5302.142f, -5191.521f, 82.99158f, 0.3f, 929864185, 269934519, true);

                            }
                            if (World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 269934519) != null &&
                                World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 269934519) != null)
                            {
                                trolly1 = World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 269934519);
                                trolly2 = World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 269934519);
                            }
                            if (trolly3 == null)
                            {
                                trolly3 = fProp.CreatePropNoOffset(269934519, new Vector3(5306.773f, -5187.983f, 82.99162f), new Vector3(0f, 0f, 155f), false);
                            }
                            else
                            {
                                trolly3.IsPositionFrozen = false;
                            }
                            if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                            {
                                GameplayCamera.SetCamViewModeForContext(CamViewModeContext.InVehicle, CamViewMode.ThirdPersonFar);
                                SceneManager.StartScene("BombPlant");
                                case5switch = 8;
                            }
                            break;
                        case 8:
                            SceneManager.Update();
                            if (BombPlantScene._permBomb != null)
                            {
                                BombPlantScene._permBomb.IsPositionFrozen = true;
                                fEntity.SetEntityCanBeDamaged(BombPlantScene._permBomb, false);
                            }
                            if (!SceneManager.IsSceneRunning)
                            {
                                if (InsideBlip == null)
                                {
                                    InsideBlip = fBlip.CreateBlipForCoordWithParams(new Vector3(5308.768f, -5204.557f, 83.51859f), BlipSprite.Standard, BlipColor.Yellow, 1f, "Lobby");
                                }
                                GameplayCamera.SetCamViewModeForContext(CamViewModeContext.OnFoot, CamViewMode.ThirdPersonMedium);
                                if (fInterior.GetRoomKeyFromEntity(fPlayer.ped) != 592960010)
                                {
                                    InsideBlip.Alpha = 255;
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fHud.ShowGXTSubtitle("NTH_BACKTOLOBBY");
                                    else
                                        Screen.ShowSubtitle("~s~Return to the ~y~lobby.~s~");
                                }
                                else
                                {
                                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Delivering);
                                    InsideBlip.Alpha = 0;
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fHud.ShowGXTSubtitle("NTH_DETONATE");
                                    else
                                        Screen.ShowSubtitle("~r~Detonate the explosives.~s~");
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fHud.DisplayGXTHelpText("NTH_TODETONATE");
                                    else
                                        fHud.DisplayHelpText("~s~Press ~INPUT_DETONATE~ to ~r~detonate~s~ the explosives.");
                                    if (Game.IsControlJustPressed(GTA.Control.Detonate))
                                    {
                                        Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
                                        Wait(700);
                                        case5switch = 9;
                                    }
                                }
                            }
                            break;
                        case 9:
                            RayfireScenes.PrologueVaultRayfire.Scene();
                            if (RayfireScenes.PrologueVaultRayfire.vaultScene > 8)
                            {
                                fStreaming.RequestNamedPTFXAsset("scr_prologue");
                                fStreaming.UseParticleFXAsset("scr_prologue");
                                if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                {
                                    if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, RayfireScenes.PrologueVaultRayfire.PTFX1))
                                    {
                                        RayfireScenes.PrologueVaultRayfire.PTFX1 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_haze", 5299f, -5189f, 82.6f, 0f, 0f, 0f, 1f, false, false, false, false);
                                    }
                                    if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, RayfireScenes.PrologueVaultRayfire.PTFX2))
                                    {
                                        RayfireScenes.PrologueVaultRayfire.PTFX2 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_fog", 5299f, -5189f, 82.6f, 0f, 0f, 0f, 1f, false, false, false, false);
                                    }
                                }
                            }
                            if (RayfireScenes.PrologueVaultRayfire.vaultScene >= 10)
                            {
                                Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                            }
                            if (RayfireScenes.PrologueVaultRayfire.vaultScene >= 12)
                            {
                                fAudio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                                fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                Globals.missionSwitch = 6;
                            }
                            break;
                    }
                    break;
                case 6:
                    if (World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 929864185) != null ||
                                World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 929864185) != null)
                    {
                        Function.Call(Hash.CREATE_​MODEL_​SWAP, 5308.04f, -5191.028f, 82.99158f, 0.3f, 929864185, 269934519, true);
                        Function.Call(Hash.CREATE_​MODEL_​SWAP, 5302.142f, -5191.521f, 82.99158f, 0.3f, 929864185, 269934519, true);

                    }
                    if ((trolly1 != null || trolly2 != null) && (World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 269934519) != null ||
                        World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 269934519) != null))
                    {
                        trolly1 = World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 269934519);
                        trolly2 = World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 269934519);
                    }
                    if (trolly3 == null)
                    {
                        trolly3 = fProp.CreatePropNoOffset(269934519, new Vector3(5306.773f, -5187.983f, 82.99162f), new Vector3(0f, 0f, 155f), false);
                    }
                    else
                    {
                        trolly3.IsPositionFrozen = false;
                    }
                    if (trolly1 != null)
                    {
                        if (trolly1.AttachedBlip == null)
                            trolly1.AddBlip();
                        else
                        {
                            trolly1.AttachedBlip.Color = BlipColor.Green;
                            trolly1.AttachedBlip.Sprite = BlipSprite.Standard;
                            trolly1.AttachedBlip.Name = "Cash";
                            trolly1.AttachedBlip.Scale = 0.75f;
                        }
                    }
                    if (trolly2 != null)
                    {
                        if (trolly2.AttachedBlip == null)
                            trolly2.AddBlip();
                        else
                        {
                            trolly2.AttachedBlip.Color = BlipColor.Green;
                            trolly2.AttachedBlip.Sprite = BlipSprite.Standard;
                            trolly2.AttachedBlip.Name = "Cash";
                            trolly2.AttachedBlip.Scale = 0.75f;
                        }
                    }
                    if (trolly2 != null)
                    {
                        if (trolly2.AttachedBlip == null)
                            trolly2.AddBlip();
                        else
                        {
                            trolly2.AttachedBlip.Color = BlipColor.Green;
                            trolly2.AttachedBlip.Sprite = BlipSprite.Standard;
                            trolly2.AttachedBlip.Name = "Cash";
                            trolly2.AttachedBlip.Scale = 0.75f;
                        }
                    }
                    if (VaultBlip == null)
                    {
                        VaultBlip = fBlip.CreateBlipForCoordWithParams(new Vector3(5303.874f, -5189.139f, 82.54237f), BlipSprite.Standard, BlipColor.Yellow, 1f, "Vault");
                    }
                    if (fInterior.GetRoomKeyFromEntity(fPlayer.ped) == 2308199534)
                    {
                        if (trolly1.AttachedBlip != null)
                        {
                            trolly1.AttachedBlip.Alpha = 255;
                        }
                        if (trolly2.AttachedBlip != null)
                        {
                            trolly2.AttachedBlip.Alpha = 255;
                        }
                        if (trolly3.AttachedBlip != null)
                        {
                            trolly3.AttachedBlip.Alpha = 255;
                        }
                        if (VaultBlip != null)
                        {
                            VaultBlip.Alpha = 0;
                        }
                    }
                    else
                    {
                        if (trolly1.AttachedBlip != null)
                        {
                            trolly1.AttachedBlip.Alpha = 0;
                        }
                        if (trolly2.AttachedBlip != null)
                        {
                            trolly2.AttachedBlip.Alpha = 0;
                        }
                        if (trolly3.AttachedBlip != null)
                        {
                            trolly3.AttachedBlip.Alpha = 0;
                        }
                        if (VaultBlip != null)
                        {
                            VaultBlip.Alpha = 255;
                        }
                    }
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.MedIntensity);
                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                        fHud.ShowGXTSubtitle("NTH_LOOTVAULT");
                    else
                        Screen.ShowSubtitle("~s~Loot the ~y~vault.~s~");
                    break;
            }
            if (Globals.missionSwitch != 0)
            {
                MissionFailHandler();
            }
            if (Start.Plane != null)
            {
                Plane = Start.Plane;
            }
        }
        Prop trolly1;
        Prop trolly2;
        Prop trolly3;
        Blip VaultBlip;
        Blip InsideBlip;
        SynchronizedScene player = new SynchronizedScene(new Vector3(5309.55f, -5210.1f, (86.9186f - 3.41f)));
        SynchronizedScene door = new SynchronizedScene(new Vector3((5309.55f - 0.7f), (-5210.1f + 1.902f), ((86.9186f - 3.2f) - 0.05f)));
        private Prop AnimDoor;
        private Camera AnimCam;
        private Ped AnimGuard;
        private int case5switch = -1;
        private static fPed.CombatAttributes[] EnableAttrbutes =
            {
            fPed.CombatAttributes.CA_BLIND_FIRE_IN_COVER,
            fPed.CombatAttributes.CA_CAN_CHARGE,
            fPed.CombatAttributes.CA_CAN_USE_PEEKING_VARIATIONS,
            fPed.CombatAttributes.CA_DISABLE_BULLET_REACTIONS,
            fPed.CombatAttributes.CA_DISABLE_FLEE_FROM_COMBAT,
            fPed.CombatAttributes.CA_ENABLE_TACTICAL_POINTS_WHEN_DEFENSIVE,
            fPed.CombatAttributes.CA_IS_A_GUARD,
            fPed.CombatAttributes.CA_USE_COVER,
            fPed.CombatAttributes.CA_USE_PROXIMITY_FIRING_RATE,
            fPed.CombatAttributes.CA_DISABLE_ENTRY_REACTIONS,
            };
        private static fPed.CombatAttributes[] DisableAttrbutes =
            {
            fPed.CombatAttributes.CA_DISABLE_REACT_TO_BUDDY_SHOT,
            fPed.CombatAttributes.CA_PLAY_REACTION_ANIMS,
            };

        public int AiTeam = World.AddRelationshipGroup("aiteam").Hash;
        public int playersTeam = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");

        static Prop Garagecolobject;
        static Ped BoothGuard;
        static List<Prop> boothProps = new List<Prop>();
        Dictionary<int, Tuple<int, int>> pedProps;
        Dictionary<int, Tuple<int, int>> pedVariation;

        public static void SetOutfit()
        {
            if (!fPlayer.IsMichael)
            {
                if (!fPlayer.IsFranklin)
                {
                    if (!fPlayer.IsTrevor)
                    {
                        if (!fPlayer.IsFreemodeMale)
                        {
                            if (!fPlayer.IsFreemodeFemale)
                            {
                                return;
                            }
                            else
                            {
                                fPed.SetPedDefaultComponentVariation(fPlayer.ped);
                                fPed.ClearAllPedProps(fPlayer.ped, 1);
                                fPed.SetPedComponentVariation(fPlayer.ped, 1, 58, 4, 0);
                                fPed.SetPedComponentVariation(fPlayer.ped, 3, 40, 1, 0);
                                fPed.SetPedComponentVariation(fPlayer.ped, 4, 32, 0, 0);
                                fPed.SetPedComponentVariation(fPlayer.ped, 5, 82, 0, 0);
                                fPed.SetPedComponentVariation(fPlayer.ped, 6, 25, 0, 0);
                                fPed.SetPedComponentVariation(fPlayer.ped, 11, 215, 18, 0);
                            }
                        }
                        else
                        {
                            fPed.SetPedDefaultComponentVariation(fPlayer.ped);
                            fPed.ClearAllPedProps(fPlayer.ped, 1);
                            fPed.SetPedComponentVariation(fPlayer.ped, 1, 37, 0, 0);
                            fPed.SetPedComponentVariation(fPlayer.ped, 3, 33, 1, 0);
                            fPed.SetPedComponentVariation(fPlayer.ped, 4, 175, 0, 0);
                            fPed.SetPedComponentVariation(fPlayer.ped, 5, 82, 0, 0);
                            fPed.SetPedComponentVariation(fPlayer.ped, 6, 154, 0, 0);
                            fPed.SetPedComponentVariation(fPlayer.ped, 8, 74, 7, 0);
                            fPed.SetPedComponentVariation(fPlayer.ped, 11, 545, 0, 0);
                        }
                    }
                    else
                    {
                        fPed.SetPedDefaultComponentVariation(fPlayer.ped);
                        fPed.ClearAllPedProps(fPlayer.ped, 1);
                        fPed.SetPedComponentVariation(fPlayer.ped, 0, 0, 5, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 1, 0, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 2, 1, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 8, 13, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 3, 9, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 4, 9, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 6, 12, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 5, 4, 0, 0);
                        fPed.SetPedComponentVariation(fPlayer.ped, 9, 1, 0, 0);
                    }
                }
                else
                {
                    fPed.SetPedDefaultComponentVariation(fPlayer.ped);
                    fPed.ClearAllPedProps(fPlayer.ped, 1);
                    fPed.SetPedComponentVariation(fPlayer.ped, 3, 14, 4, 0);
                    fPed.SetPedComponentVariation(fPlayer.ped, 4, 16, 0, 0);
                    fPed.SetPedComponentVariation(fPlayer.ped, 5, 4, 0, 0);
                    fPed.SetPedComponentVariation(fPlayer.ped, 6, 9, 0, 0);
                    fPed.SetPedComponentVariation(fPlayer.ped, 8, 4, 0, 0);
                    fPed.SetPedComponentVariation(fPlayer.ped, 9, 6, 0, 0);
                    fPed.SetPedComponentVariation(fPlayer.ped, 1, 0, 0, 0);
                }
            }
            else
            {
                fPed.SetPedDefaultComponentVariation(fPlayer.ped);
                fPed.ClearAllPedProps(fPlayer.ped, 1);
                fPed.SetPedComponentVariation(fPlayer.ped, 0, 0, 3, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 2, 1, 0, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 8, 7, 0, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 5, 5, 0, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 3, 31, 0, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 4, 26, 0, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 6, 14, 0, 0);
                fPed.SetPedComponentVariation(fPlayer.ped, 9, 1, 0, 0);
            }
        }

        public static void SpawnBooth()
        {
            if (BoothGuard == null)
            {
                BoothGuard = fPed.CreatePed(new Model(PedHash.Security01SMM), new Vector3(5362.298f, -5181.831f, 83.21764f), 165f);
            }
            if (boothProps.Count == 0)
            {
                fProp.CreatePropForList(true, boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.593f, -5181.164f, 84.52729f), new Vector3(0f, 180f, 90f), false, false);
                fProp.CreatePropForList(true, boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.589f, -5181.263f, 82.06731f), new Vector3(0f, 0f, 90f), false, false);
                fProp.CreatePropForList(true, boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.7f, -5181.164f, 84.52729f), new Vector3(0f, 180f, 90f), false, false);
                fProp.CreatePropForList(true, boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.696f, -5181.263f, 82.06731f), new Vector3(0f, 0f, 90f), false, false);
                fProp.CreatePropForList(true, boothProps, new Model("apa_mp_h_acc_rugwools_03"), new Vector3(5362.6f, -5180.766f, 82.17793f), new Vector3(0f, 0f, 0f), false, false);
            }

        }
        static List<Vehicle> depotVehicles = new List<Vehicle>();
        public static void SpawnTrucks()
        {
            if (depotVehicles.Count == 0)
            {
                fVehicle.CreateVehicleForList(depotVehicles, new Model("stockade3"), new Vector3((5341.3525f + 1.365f), -5177.149f, 81.762f), 0.3367f);
                Function.Call(Hash.SET_VEHICLE_IS_CONSIDERED_BY_PLAYER, depotVehicles[0], false);
                Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, depotVehicles[0], true);
                fVehicle.CreateVehicleForList(depotVehicles, new Model("stockade3"), new Vector3((5337.0996f + 1.365f), -5177.0317f, 81.762f), 2.5903f);
                Function.Call(Hash.SET_VEHICLE_IS_CONSIDERED_BY_PLAYER, depotVehicles[1], false);
                Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, depotVehicles[1], true);
                Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, fMisc.GetHashKey("stockade3"));
            }
        }
        // security booth guard pos: 5362.298f, -5181.831f, 83.21764f, 165f
        void Spawn()
        {
            if (Garagecolobject == null)
            {
                Garagecolobject = fProp.CreateProp(new Model("p_gdoor1colobject_s"), new Vector3(5320.59f, -5188.49f, 82.52f), new Vector3(0f, 0f, 90f), false, false);
            }
            else
            {
                Garagecolobject.IsVisible = false;
                Garagecolobject.IsPositionFrozen = true;
            }
                SpawnBooth();
            if (boothProps.Count > 0)
            {
                foreach (Prop prop in boothProps)
                {
                    fEntity.SetEntityCanBeDamaged(prop, false);
                    prop.IsPositionFrozen = true;
                }
            }
            SpawnTrucks();
        }

        bool enableFailCheckingForNY = false;
        string PlayerNameStr;
        int case3Switch = 1;
        Camera failCam;
        int resetSwitch = 0;
        int restartSwitch = 0;
        Camera resetCam;
        Vector3 resetCamCoord7 = new Vector3(5355f, -5173f, 82.49858f);
        Vector3 resetCamRot7 = new Vector3(17f, 0f, 130.4812f);
        Vector3 resetCamCoord3 = new Vector3(5346.801f, -5178.461f, 82.4f);
        Vector3 resetCamRot3 = new Vector3(15f, 0f, 129.449f);
        InstructionalButtons resetButtons = new InstructionalButtons();
        InstructionalButtonContainer ResetButtonContainer = new InstructionalButtonContainer(InputControl.FrontendAccept, "Restart Mission");
        InstructionalButtonContainer GoBackToFreeroamContainer = new InstructionalButtonContainer(InputControl.FrontendCancel, "Leave Mission");
        bool instructionalButtonsSetUp = false;
        bool failShardJustShown = false;
        public static bool MissionFailCleanUpRequired = false;
        bool check1 = false;
        bool restartPending = false;
        public static bool restarting = false;
        /// <summary>
        /// 1 - start, 2 - in NY, 3 - depot, 4 - vault exploded
        /// </summary>
        public static int checkpoint = 0;

        void MissionFailHandler()
        {
            if (!restartPending)
            {
                if (Globals.missionSwitch > 0 && Globals.missionSwitch < 3)
                {
                    if (IsPlaneDestroyed)
                    {
                        fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                        fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Fail);
                        fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                        justFailed = true;
                        SetFailVariation(FailVariations.PlaneDestroyed);
                        MissionFailCleanUpRequired = true;
                    }
                }
                if (Globals.missionSwitch > 3 && Globals.missionSwitch < 5)
                {
                    check1 = fPlayer.GetCarDistanceTo(local) > 60f;
                    if (enableFailCheckingForNY)
                    {
                        if (IsPrologueVehicleDestroyed)
                        {
                            fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Fail);
                            fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                            justFailed = true;
                            SetFailVariation(FailVariations.PrologueVehicleDestroyed);
                            MissionFailCleanUpRequired = true;
                        }
                        else
                        {
                            if (!fEntity.IsEntityInArea(fPlayer.ped, new Vector3(3543.267f, -4875.534f, 61f), new Vector3(3649.131f, -4929.502f, 125f)))
                            {
                                if (fPlayer.ped.CurrentVehicle == PrologueVehicle && check1)
                                {
                                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Fail);
                                    fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                                    justFailed = true;
                                    SetFailVariation(FailVariations.PlayerFailedToReachTheDepot);
                                    MissionFailCleanUpRequired = true;
                                }
                            }
                            if (fPlayer.ped.CurrentVehicle != PrologueVehicle)
                            {
                                if (fPlayer.GetDistanceTo(PrologueVehicle.Position) > 40f || fPlayer.GetCarDistanceTo(PrologueVehicle.Position) > 40f)
                                {
                                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Fail);
                                    fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                                    justFailed = true;
                                    SetFailVariation(FailVariations.PlayerAbandonedThePrologueVehicle);
                                    MissionFailCleanUpRequired = true;
                                }
                            }
                        }
                    }
                }
            }
            MissionFailCleanUp();
        }
        Player currentPlayer = Game.Player;
        void MissionFailCleanUp()
        {
            if (MissionFailCleanUpRequired)
            {
                fPlayer.SetMaxWantedLevelTo0();
                if (justFailed && !restartPending)
                {
                    weatherTypeSaved = false;
                    if (fPlayer.IsFranklin)
                    {
                        PlayerNameStr = "Franklin";
                    }
                    if (fPlayer.IsTrevor)
                    {
                        PlayerNameStr = "Trevor";
                    }
                    if (fPlayer.IsMichael)
                    {
                        PlayerNameStr = "Michael";
                    }
                    if (fPlayer.IsFreemodePed)
                    {
                        PlayerNameStr = currentPlayer.Name;
                    }
                    Globals.missionSwitch = -100;
                    if (depotVehicles.Count > 0)
                    {
                        for (int i = 0; i < depotVehicles.Count; i++)
                        {
                            depotVehicles[i].MarkAsNoLongerNeeded();
                            depotVehicles.Remove(depotVehicles[i]);
                        }
                    }
                    if (Start.Plane != null)
                    {
                        if (Start.Plane.AttachedBlip != null)
                        {
                            Start.Plane.AttachedBlip.Delete();
                        }
                        Start.Plane.MarkAsNoLongerNeeded();
                        Start.Plane = null;
                    }
                    CombatsList.Combats_Dispose();
                    if (InsideBlip != null)
                    {
                        InsideBlip.Delete();
                        InsideBlip = null;
                    }
                    if (AnimGuard != null)
                    {
                        AnimGuard.Delete();
                        AnimGuard = null;
                    }
                    if (AnimDoor != null)
                    {
                        AnimDoor.Delete();
                        AnimDoor = null;
                    }
                    if (AnimCam != null)
                    {
                        AnimCam.Delete();
                        AnimCam = null;
                    }
                    if (LudendorffNorthYankton != null)
                    {
                        LudendorffNorthYankton.Delete();
                        LudendorffNorthYankton = null;
                    }
                    if (PrologueIntroCam != null)
                    {
                        PrologueIntroCam.Delete();
                        PrologueIntroCam = null;
                    }
                    if (PrologueIntroCam2 != null)
                    {
                        PrologueIntroCam2.Delete();
                        PrologueIntroCam2 = null;
                    }
                    if (PrologueVehicle != null)
                    {
                        if (PrologueVehicle.AttachedBlip != null)
                        {
                            PrologueVehicle.AttachedBlip.Delete();
                        }
                        PrologueVehicle.MarkAsNoLongerNeeded();
                        PrologueVehicle = null;
                    }
                    if (DepotBlip != null)
                    {
                        DepotBlip.Delete();
                        DepotBlip = null;
                    }
                    fAudio.StopAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                    fAudio.StopAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                    fHud.ClearAllPrints();
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, false);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, false);
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Fail);
                    if (GetFailVariation() == FailVariations.PlaneDestroyed)
                    {
                        MissionShard failShard = new MissionShard();
                        failShard.Shard_In("HEIST FAILED", "The Plane Was Destroyed.", 6, 0.3f, 6, true);
                        failShard = null;
                        failShardJustShown = true;
                        Screen.FadeOut(1000);
                        justFailed = false;
                        SetFailVariation(FailVariations.None);
                    }
                    if (PlayerTeleportedToPrologue)
                    {
                        if (GetFailVariation() == FailVariations.PrologueVehicleDestroyed)
                        {
                            MissionShard failShard = new MissionShard();
                            failShard.Shard_In("HEIST FAILED", $"{PrologueVehicleName} Was Destroyed.", 6, 0.3f, 6, true);
                            failShard = null;
                            failShardJustShown = true;
                            Screen.FadeOut(1000);
                            justFailed = false;
                            SetFailVariation(FailVariations.None);
                        }
                        if (GetFailVariation() == FailVariations.PlayerFailedToReachTheDepot)
                        {
                            MissionShard failShard = new MissionShard();
                            failShard.Shard_In("HEIST FAILED", $"{PlayerNameStr} Failed To Reach The Depot", 6, 0.3f, 6, true);
                            failShard = null;
                            failShardJustShown = true;
                            Screen.FadeOut(1000);
                            justFailed = false;
                            SetFailVariation(FailVariations.None);
                        }
                        if (GetFailVariation() == FailVariations.PlayerAbandonedThePrologueVehicle)
                        {
                            MissionShard failShard = new MissionShard();
                            failShard.Shard_In("HEIST FAILED", $"{PlayerNameStr} Abandoned The Vehicle", 6, 0.3f, 6, true);
                            failShard = null;
                            failShardJustShown = true;
                            Screen.FadeOut(1000);
                            justFailed = false;
                            SetFailVariation(FailVariations.None);
                        }
                    }
                }
                if (failShardJustShown)
                {
                    switch (restartSwitch)
                    {
                        case 1:
                            resetButtons.RemoveContainer(ResetButtonContainer);
                            resetButtons.RemoveContainer(GoBackToFreeroamContainer);
                            resetButtons.Dispose();
                            restartPending = true;
                            if (Start.Plane == null)
                                Screen.FadeOut(2500);
                            if (Screen.IsFadedOut)
                            {
                                LoadingPrompt.Show("NorthYanktonHeist");
                                restarting = true;
                                fVehicle.DeleteVehiclesInList(depotVehicles);
                                fCam.RenderScriptCams(false, false, 0);
                                if (resetCam != null)
                                {
                                    resetCam.Delete();
                                    resetCam = null;
                                }
                                if (checkpoint == 1)
                                {
                                    fWeather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                                    fClock.SetClockTime(15, 0, 0);
                                    fInterior.RemoveIpl("prologue06_int");
                                    fInterior.RemoveIpl("prologue01");
                                    fInterior.RemoveIpl("prologue02");
                                    fInterior.RemoveIpl("prologue03");
                                    fInterior.RemoveIpl("prologue04");
                                    fInterior.RemoveIpl("prologue05");
                                    fInterior.RemoveIpl("prologue06");
                                    fInterior.RemoveIpl("prologuerd");
                                    fInterior.RemoveIpl("Prologue01c");
                                    fInterior.RemoveIpl("Prologue01d");
                                    fInterior.RemoveIpl("Prologue01e");
                                    fInterior.RemoveIpl("Prologue01f");
                                    fInterior.RemoveIpl("Prologue01g");
                                    fInterior.RemoveIpl("prologue01h");
                                    fInterior.RemoveIpl("prologue01i");
                                    fInterior.RemoveIpl("prologue01j");
                                    fInterior.RemoveIpl("prologue01k");
                                    fInterior.RemoveIpl("prologue01z");
                                    fInterior.RemoveIpl("prologue03b");
                                    fInterior.RemoveIpl("prologue04b");
                                    fInterior.RemoveIpl("prologue05b");
                                    fInterior.RemoveIpl("prologue06b");
                                    fInterior.RemoveIpl("prologuerdb");
                                    fInterior.RemoveIpl("DES_ProTree_start");
                                    fInterior.RemoveIpl("prologue_grv_torch");
                                    fInterior.RemoveIpl("prologue03_grv_dug");
                                    fInterior.RemoveIpl("DES_ProTree_start_lod");
                                    fInterior.RemoveIpl("prologue04_cover");
                                    fInterior.RemoveIpl("prologue03_grv_fun");
                                    fInterior.RemoveIpl("prologue03_grv_cov");
                                    fInterior.RemoveIpl("prologue_LODLights");
                                    fInterior.RemoveIpl("prologue_DistantLights");
                                    int zone = fZone.GetZoneFromNameID("PrLog");
                                    fZone.SetZoneEnabled(zone, false);
                                    fHud.ToggleNorthYanktonMap(false);
                                    fPathfind.SetAllowStreamPrologueNodes(false);
                                    fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, false, true);
                                    fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, false, true);
                                    fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, false, true);
                                    fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, false, true);
                                    fInterior.PrologueMap.YankTon = false;
                                    fInterior.PrologueMap.EnableNorthYanktonTrainTracks(false);
                                    fStreaming.SetMapDataCullboxEnabled("prologue", false);
                                    fStreaming.SetMapDataCullboxEnabled("Prologue_Main", false);
                                    fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), false);
                                }
                                switch (checkpoint)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        if (Start.Plane != null)
                                        {
                                            if (Plane.AttachedBlip == null)
                                            {
                                                Plane.AddBlip();
                                            }
                                            if (Plane.AttachedBlip != null)
                                            {
                                                Plane.AttachedBlip.Alpha = 0;
                                                Plane.AttachedBlip.RotationFloat = Plane.Heading;
                                                Plane.AttachedBlip.Sprite = (BlipSprite)916;
                                                Plane.AttachedBlip.Color = BlipColor.Blue;
                                                Plane.AttachedBlip.Name = "Plane";
                                            }
                                            if (!fPlayer.ped.IsInVehicle(Start.Plane))
                                            {
                                                Start.Plane.IsPositionFrozen = true;
                                                Function.Call(Hash.SET_VEHICLE_ENGINE_ON, Start.Plane, true, true, true);
                                                fPlayer.ped.SetIntoVehicle(Start.Plane, VehicleSeat.Driver);
                                                Wait(5000);
                                            }
                                            if (fPlayer.ped.IsInVehicle(Start.Plane) && Start.Plane.IsEngineRunning && !Start.Plane.IsEngineStarting)
                                            {
                                                Start.Plane.IsPositionFrozen = false;
                                                Start.Plane.Speed = 40f;
                                                Start.Plane.LandingGearState = VehicleLandingGearState.Retracted;
                                                Start.Plane.Velocity = Start.Plane.ForwardVector * 40f;
                                                Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                                                fPlayer.ped.ClearBloodDamage();
                                                fPlayer.ped.ClearVisibleDamage();
                                                fPlayer.ped.IsVisible = true;
                                                fPlayer.ped.IsPositionFrozen = false;
                                                fPlayer.ped.IsInvincible = false;
                                                fPlayer.SetMaxWantedLevelToNormal();
                                                fHud.RadarAndHud(true, true);
                                                LoadingPrompt.Hide();
                                                Screen.FadeIn(1700);
                                            }
                                        }
                                        else
                                        {
                                            fPlayer.PedPos(828.5366f, 3302.112f, 180f);
                                            Start.Plane = fVehicle.CreateVehicle(new Model("cuban800"), new Vector3(828.5366f, 3302.112f, 180f), 70f);
                                        }
                                        break;
                                    case 2:
                                        Wait(7500);
                                        //Globals.globalBlips = 2;
                                        //Globals.globalScripts = 2;
                                        fHud.ClearAllPrints();
                                        fHud.ClearBrief();
                                        fHud.ClearAllHelpMessages();
                                        fHud.ClearGPSMultiRoute();
                                        fHud.ClearHelp(true);
                                        Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                                        fPlayer.ped.ClearBloodDamage();
                                        fPlayer.ped.ClearVisibleDamage();
                                        fPlayer.ped.IsVisible = true;
                                        fPlayer.ped.IsPositionFrozen = false;
                                        fPlayer.ped.IsInvincible = false;
                                        fPlayer.SetMaxWantedLevelToNormal();
                                        Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                                        Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                                        fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                        fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                                        timeAdvanced1 = false;
                                        restartPending = false;
                                        restartSwitch = 0;
                                        instructionalButtonsSetUp = false;
                                        failShardJustShown = false;
                                        PlayerTeleportedToPrologue = false;
                                        MissionFailCleanUpRequired = false;
                                        Globals.missionSwitch = 3;
                                        case3Switch = -1;
                                        break;
                                    case 3:
                                        Wait(7500);
                                        fHud.ClearAllPrints();
                                        fHud.ClearBrief();
                                        fHud.ClearAllHelpMessages();
                                        fHud.ClearGPSMultiRoute();
                                        fHud.ClearHelp(true);
                                        Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                                        fPlayer.ped.ClearBloodDamage();
                                        fPlayer.ped.ClearVisibleDamage();
                                        fPlayer.ped.IsVisible = true;
                                        fPlayer.ped.IsPositionFrozen = false;
                                        fPlayer.ped.IsInvincible = false;
                                        fPlayer.SetMaxWantedLevelToNormal();
                                        Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                                        Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                                        fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                        fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Suspense);
                                        timeAdvanced1 = false;
                                        restartPending = false;
                                        restartSwitch = 0;
                                        instructionalButtonsSetUp = false;
                                        failShardJustShown = false;
                                        PlayerTeleportedToPrologue = false;
                                        MissionFailCleanUpRequired = false;
                                        Globals.missionSwitch = 5;
                                        case5switch = 0;
                                        case3Switch = -1;
                                        break;
                                    case 4: break;
                                }
                            }
                            if (checkpoint == 1 && Start.Plane != null && fPlayer.ped.IsInVehicle(Start.Plane) && Screen.IsFadedIn)
                            {
                                restartPending = false;
                                restartSwitch = 0;
                                instructionalButtonsSetUp = false;
                                failShardJustShown = false;
                                PlayerTeleportedToPrologue = false;
                                MissionFailCleanUpRequired = false;
                                Globals.missionSwitch = -1;
                            }
                            break;
                        case 2:
                            Screen.FadeOut(2500);
                            resetButtons.RemoveContainer(ResetButtonContainer);
                            resetButtons.RemoveContainer(GoBackToFreeroamContainer);
                            resetButtons.Dispose();
                            if (Screen.IsFadedOut)
                            {
                                LoadingPrompt.Show("NorthYanktonHeist");
                                restarting = true;
                                fVehicle.DeleteVehiclesInList(depotVehicles);
                                fCam.RenderScriptCams(false, false, 0);
                                if (resetCam != null)
                                {
                                    resetCam.Delete();
                                    resetCam = null;
                                }
                                fPlayer.PedPos(1746.312f, 3273.837f, 40.15277f, 30.6f);
                                instructionalButtonsSetUp = false;
                                fPed.ApplyPedPropData(fPlayer.ped, pedProps);
                                fPed.ApplyPedVariationData(fPlayer.ped, pedVariation);
                                fWeather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                                fClock.SetClockTime(12, 0, 0);
                                fInterior.PrologueMap.EnableNorthYanktonTrainTracks(false);
                                fStreaming.SetMapDataCullboxEnabled("prologue", false);
                                fStreaming.SetMapDataCullboxEnabled("Prologue_Main", false);
                                fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), false);
                                Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                                fPlayer.ped.ClearBloodDamage();
                                fPlayer.ped.ClearVisibleDamage();
                                fPlayer.ped.Weapons.Select(WeaponHash.Unarmed);
                                fPlayer.ped.IsVisible = true;
                                fPlayer.ped.IsPositionFrozen = false;
                                fPlayer.ped.IsInvincible = false;
                                fPlayer.ped.Task.ClearAllImmediately();
                                Wait(4000);
                                Screen.FadeIn(5000);
                                Wait(2500);
                                fPlayer.SetMaxWantedLevelToNormal();
                                ScriptManager.ScriptManager.RestartScripts();
                                //Globals.globalBlips = 3;
                                //Globals.globalScripts = 3;
                                fHud.RadarAndHud(true, true);
                                LoadingPrompt.Hide();
                                fInterior.RemoveIpl("prologue06_int");
                                fInterior.RemoveIpl("prologue01");
                                fInterior.RemoveIpl("prologue02");
                                fInterior.RemoveIpl("prologue03");
                                fInterior.RemoveIpl("prologue04");
                                fInterior.RemoveIpl("prologue05");
                                fInterior.RemoveIpl("prologue06");
                                fInterior.RemoveIpl("prologuerd");
                                fInterior.RemoveIpl("Prologue01c");
                                fInterior.RemoveIpl("Prologue01d");
                                fInterior.RemoveIpl("Prologue01e");
                                fInterior.RemoveIpl("Prologue01f");
                                fInterior.RemoveIpl("Prologue01g");
                                fInterior.RemoveIpl("prologue01h");
                                fInterior.RemoveIpl("prologue01i");
                                fInterior.RemoveIpl("prologue01j");
                                fInterior.RemoveIpl("prologue01k");
                                fInterior.RemoveIpl("prologue01z");
                                fInterior.RemoveIpl("prologue03b");
                                fInterior.RemoveIpl("prologue04b");
                                fInterior.RemoveIpl("prologue05b");
                                fInterior.RemoveIpl("prologue06b");
                                fInterior.RemoveIpl("prologuerdb");
                                fInterior.RemoveIpl("DES_ProTree_start");
                                fInterior.RemoveIpl("prologue_grv_torch");
                                fInterior.RemoveIpl("prologue03_grv_dug");
                                fInterior.RemoveIpl("DES_ProTree_start_lod");
                                fInterior.RemoveIpl("prologue04_cover");
                                fInterior.RemoveIpl("prologue03_grv_fun");
                                fInterior.RemoveIpl("prologue03_grv_cov");
                                fInterior.RemoveIpl("prologue_LODLights");
                                fInterior.RemoveIpl("prologue_DistantLights");
                                int zone = fZone.GetZoneFromNameID("PrLog");
                                fZone.SetZoneEnabled(zone, false);
                                fHud.ToggleNorthYanktonMap(false);
                                fPathfind.SetAllowStreamPrologueNodes(false);
                                fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, false, true);
                                fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, false, true);
                                fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, false, true);
                                fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, false, true);
                                fInterior.PrologueMap.YankTon = false;
                                scriptsKilled = false;
                                weatherTypeSaved = false;
                                outfitSaved = false;
                                restartPending = false;
                                Globals.missionSwitch = 0;
                                restartSwitch = 0;
                                PlayerTeleportedToPrologue = false;
                                failShardJustShown = false;
                                MissionFailCleanUpRequired = false;
                            }
                            break;
                    }
                    if (PlayerTeleportedToPrologue && resetSwitch == 0)
                        resetSwitch = fMisc.GetRandomIntInRange(1, 3);
                    if (!PlayerTeleportedToPrologue && resetSwitch == 0)
                        resetSwitch = 1;
                    if (resetSwitch > 0)
                    {
                        if (!PlayerTeleportedToPrologue && !restarting)
                        {
                            if (!fInterior.PrologueMap.YankTon)
                            {
                                fInterior.PrologueMap.LoadYankton();
                            }
                            else
                            {
                                if (Screen.IsFadedOut)
                                {
                                    fPlayer.PedPos(5304.088f, -5189.521f, 83.51835f, 0f);
                                    fClock.SetClockTime(12, 0, 0);
                                    fPlayer.ped.IsVisible = false;
                                    fPlayer.ped.IsPositionFrozen = true;
                                    fPlayer.ped.IsInvincible = true;
                                    fWeather.SetWeatherTypeNowPersist(fWeather.WeatherTypes.SNOWLIGHT);
                                    fClock.PauseClock(true);
                                }
                            }
                        }
                        if (PlayerTeleportedToPrologue && !restarting)
                        {
                            if (!fInterior.PrologueMap.YankTon)
                            {
                                fInterior.PrologueMap.LoadYankton();
                            }
                            else
                            {
                                if (Screen.IsFadedOut)
                                {
                                    fPlayer.PedPos(5304.088f, -5189.521f, 83.51835f, 0f);
                                    fPlayer.ped.IsVisible = false;
                                    fPlayer.ped.IsPositionFrozen = true;
                                    fPlayer.ped.IsInvincible = true;
                                    fClock.SetClockTime(5, 0, 0);
                                    fWeather.SetWeatherTypeNowPersist(fWeather.WeatherTypes.Snow);
                                    fClock.PauseClock(true);
                                }
                            }
                        }
                    }
                    if (Screen.IsFadedOut)
                    {
                        switch (resetSwitch)
                        {
                            case 1:
                                SpawnTrucks();
                                if (resetCam == null)
                                {
                                    resetCam = fCam.CreateScriptedCam();
                                }
                                while (resetCam == null)
                                {
                                    Wait(0);
                                }
                                if (resetCam != null)
                                {
                                    fHud.RadarAndHud(false, false);
                                    resetCam.Position = resetCamCoord7;
                                    resetCam.Rotation = resetCamRot7;
                                    fCam.SetCamFov(resetCam, 50f);
                                    resetCam.IsActive = true;
                                    fCam.RenderScriptCams(true, false, 0);
                                    resetCam.Position = resetCamCoord7;
                                    resetCam.Rotation = resetCamRot7;
                                    if (!resetCam.IsShaking)
                                        fCam.ShakeCam(resetCam, "HAND_SHAKE", 0.5f);
                                }
                                break;
                            case 2:
                                if (resetCam == null)
                                {
                                    resetCam = fCam.CreateScriptedCam();
                                }
                                while (resetCam == null)
                                {
                                    Wait(0);
                                }
                                if (resetCam != null)
                                {
                                    fHud.RadarAndHud(false, false);
                                    resetCam.Position = resetCamCoord3;
                                    resetCam.Rotation = resetCamRot3;
                                    fCam.SetCamFov(resetCam, 60f);
                                    resetCam.IsActive = true;
                                    fCam.RenderScriptCams(true, false, 0);
                                    resetCam.Position = resetCamCoord3;
                                    resetCam.Rotation = resetCamRot3;
                                    if (!resetCam.IsShaking)
                                        fCam.ShakeCam(resetCam, "HAND_SHAKE", 0.5f);
                                }
                                break;
                        }
                    }
                    if (resetCam != null && restartSwitch == 0)
                    {
                        if (ScriptCameraDirector.RenderingCam == resetCam)
                        {
                            while (Screen.IsFadingIn)
                                Wait(2500);
                            if (Screen.IsFadedOut && !Screen.IsFadingOut && !Screen.IsFadingIn)
                                Screen.FadeIn(1500);
                            if (!instructionalButtonsSetUp)
                            {
                                resetButtons.Load();
                                resetButtons.AddContainer(GoBackToFreeroamContainer);
                                resetButtons.AddContainer(ResetButtonContainer);
                                instructionalButtonsSetUp = true;
                            }
                            else
                            {
                                resetButtons.UpdateScaleform();
                                resetButtons.Draw();
                                Game.DisableAllControlsThisFrame();
                            }
                            if (resetButtons.IsLoaded)
                            {
                                if (Game.IsControlJustPressed(GTA.Control.FrontendAccept))
                                {
                                    restartSwitch = 1;
                                }
                                if (Game.IsControlJustPressed(GTA.Control.FrontendCancel) || Game.IsControlJustPressed(GTA.Control.FrontendPauseAlternate))
                                {
                                    restartSwitch = 2;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CleanUp()
        {
            bool flag = true;
            if (true == flag)
            {
                fAudio.StopAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                fAudio.StopAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                if (outfitSaved)
                {
                    fPed.ApplyPedPropData(fPlayer.ped, pedProps);
                    fPed.ApplyPedVariationData(fPlayer.ped, pedVariation);
                }
                if (weatherTypeSaved)
                    fWeather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                fClock.PauseClock(false);
                fInterior.PrologueMap.EnableNorthYanktonTrainTracks(false);
                fStreaming.SetMapDataCullboxEnabled("prologue", false);
                fStreaming.SetMapDataCullboxEnabled("Prologue_Main", false);
                fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), false);
                fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Fail);
                fProp.DeletePropsInList(boothProps);
                fVehicle.DeleteVehiclesInList(depotVehicles);
                CombatsList.Combats_Dispose();
                if (trolly1 != null)
                {
                    trolly1.Delete();
                    trolly1 = null;
                }
                if (trolly2 != null)
                {
                    trolly2.Delete();
                    trolly2 = null;
                }
                if (trolly3 != null)
                {
                    trolly3.Delete();
                    trolly3 = null;
                }
                if (InsideBlip != null)
                {
                    InsideBlip.Delete();
                    InsideBlip = null;
                }
                if (Garagecolobject != null)
                {
                    Garagecolobject.Delete();
                    Garagecolobject = null;
                }
                if (BoothGuard != null)
                {
                    BoothGuard.Delete();
                    BoothGuard = null;
                }
                if (PrologueIntroCam != null)
                {
                    PrologueIntroCam.Delete();
                    PrologueIntroCam = null;
                }
                if (PrologueIntroCam2 != null)
                {
                    PrologueIntroCam2.Delete();
                    PrologueIntroCam2 = null;
                }
                if (DepotBlip != null)
                {
                    DepotBlip.Delete();
                    DepotBlip = null;
                }
                if (PrologueVehicle != null)
                {
                    if (PrologueVehicle.AttachedBlip != null)
                    {
                        PrologueVehicle.AttachedBlip.Delete();
                    }
                    PrologueVehicle.MarkAsNoLongerNeeded();
                    PrologueVehicle = null;
                }
                if (LudendorffNorthYankton != null)
                {
                    LudendorffNorthYankton.Delete();
                    LudendorffNorthYankton = null;
                }
                if (Plane != null)
                {
                    if (Plane.AttachedBlip != null)
                    {
                        Plane.AttachedBlip.Delete();
                    }
                    Plane.MarkAsNoLongerNeeded();
                    Plane = null;
                }
                if (AnimGuard != null)
                {
                    AnimGuard.Delete();
                    AnimGuard = null;
                }
                if (AnimDoor != null)
                {
                    AnimDoor.Delete();
                    AnimDoor = null;
                }
                if (AnimCam != null)
                {
                    AnimCam.Delete();
                    AnimCam = null;
                }

            }
        }

        private void onShutdown(object sender, EventArgs e)
        {
            CleanUp();
        }
    }
}
