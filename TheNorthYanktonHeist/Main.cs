using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using Hash = GTA.Native.Hash;
using Screen = GTA.UI.Screen;
using TheNorthYanktonHeist.Extension;
using TheNorthYanktonHeist.Scenes;
using static Globals;
using fAudio = IMNOTEPIX.Framework.fAudio;
using fCore = IMNOTEPIX.Framework.fCore;
using fUI = IMNOTEPIX.Framework.fUI;
using IMNOTEPIX.Framework.fWorld.Anims;
using IMNOTEPIX.Framework.fWorld.Effects;
using fWorld = IMNOTEPIX.Framework.fWorld;
using fPlayer = IMNOTEPIX.Framework.fPlayer;
using fInterior = IMNOTEPIX.Framework.fWorld.Interior;
using fBlip = IMNOTEPIX.Framework.fBlip;
using IMNOTEPIX.Framework.fInterfaces;
using fMinigames = IMNOTEPIX.Framework.fGameplay.Minigames;
using fCombat = IMNOTEPIX.Framework.fGameplay.Combat;
using IMNOTEPIX.Framework.fGameplay.Minigames;

namespace TheNorthYanktonHeist
{
    public class Start : GTA.Script
    {
        public Start()
        {
            Tick += OnTick;
            Aborted += OnShutdown;
        }
        Vector3 PlanePos = new Vector3(1731.8021f, 3308.911f, 40.2237f);
        float PlaneH = 194.8553f;
        public static GTA.Vehicle Plane;
        public static GTA.Blip PlaneBlip;
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
                if (Plane != null && Plane.IsConsideredDestroyed && fWorld.Entity.GetEntityHealth(Plane) <= 0)
                    return true;
                if (Plane != null && !Plane.IsConsideredDestroyed && fWorld.Entity.GetEntityHealth(Plane) > 0)
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
                    PlaneBlip?.Delete();
                    PlaneBlip = null;
                    if (Plane != null)
                    {
                        Plane.AttachedBlip?.Delete();
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
                            if (PlaneBlip == null && fPlayer.Player.GetDistanceTo(PlanePos) > 500f && !WasDestroyed && !SpawnedIn)
                            {
                                PlaneBlip = fBlip.Blip.CreateBlipForCoordWithParams(PlanePos, (BlipSprite)916, (BlipColor)5, 1f, "North Yankton Heist");
                                PlaneBlip.RotationFloat = PlaneH;
                            }
                            if (PlaneBlip != null)
                            {
                                if (fPlayer.Player.IsWanted)
                                    PlaneBlip.Alpha = 0;
                                else
                                    PlaneBlip.Alpha = 255;
                                if (Plane == null)
                                {
                                    if (fPlayer.Player.GetDistanceTo(PlanePos) > 200f)
                                    {
                                        if (fPlayer.Player.GetDistanceTo(PlanePos) < 300f)
                                        {
                                            fWorld.Misc.ClearAreaOfVehicles(PlanePos, 10f);
                                            Plane = fWorld.Vehicle.Create("cuban800", PlanePos, PlaneH);
                                        }
                                    }

                                }
                            }
                            if (Plane != null)
                            {
                                if (!IsPlaneDestroyed)
                                {
                                    if (!fWorld.Entity.HasCollisionLoadedAroundEntity(Plane))
                                    {
                                        fWorld.Entity.SetEntityLoadCollisionFlag(Plane, true);
                                    }
                                    if (!fixedPosition)
                                    {
                                        fWorld.Vehicle.PlaceOnGround(Plane);
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
                                            PlaneBlip.DisplayType = GTA.BlipDisplayType.NoDisplay;
                                        if (PlaneBlip.DisplayType == GTA.BlipDisplayType.NoDisplay)
                                            startSwitch = 1;
                                    }
                                }
                            }
                            break;
                        case 1:
                            if (Game.Player.CanStartMission)
                            {
                                fWorld.Misc.ClearAreaEntities(new Vector3(1735.602f, 3294.539f, 41.80106f), 10f, false, true, true);
                                if (fPlayer.Player.IsWanted)
                                {
                                    fWorld.Vehicle.SetConsideredByPlayer(Plane, false);
                                    if (Plane.AttachedBlip != null)
                                        Plane.AttachedBlip.Alpha = 0;
                                    if (fPlayer.Player.GetDistanceTo(Plane.Position) < 6f)
                                    {
                                        if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                            fUI.Hud.DisplayHelpTextThisFrameGXT("NTH_STARTWANTED", true);
                                        else
                                            fUI.Hud.DisplayHelpText("~s~Lose the cops to begin the heist.");
                                    }
                                    else
                                        fUI.Hud.ClearHelp(true);
                                }
                                else
                                {
                                    fWorld.Vehicle.SetConsideredByPlayer(Plane, true);
                                    if (Plane.AttachedBlip != null)
                                        Plane.AttachedBlip.Alpha = 255;
                                    if (fPlayer.Player.GetDistanceTo(Plane.Position) < 6f)
                                    {
                                        fUI.Hud.ClearBrief();
                                        if (!fPlayer.Player.Character.IsEnteringVehicle || !fPlayer.Player.Character.IsSittingInVehicle(Plane))
                                        {
                                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                                fUI.Hud.DisplayHelpTextThisFrameGXT("NTH_TOSTART");
                                            else
                                                fUI.Hud.DisplayHelpText("~s~Press ~INPUT_ENTER~ to start the North Yankton Heist.");
                                        }
                                        if (fPlayer.Player.Character.IsEnteringVehicle || fPlayer.Player.Character.IsSittingInVehicle(Plane))
                                        {
                                            fUI.Hud.ClearAllSubtitles();
                                            fUI.Hud.ClearBrief();
                                            fUI.Hud.ClearAllHelpMessages();
                                            fUI.Hud.ClearGPSMultiRoute();
                                            fUI.Hud.ClearHelp(true);
                                            startSwitch = 2;
                                        }
                                        if (Game.IsControlJustPressed(GTA.Control.Enter))
                                        {
                                            fCore.Timer.SetTimerA(0);
                                            fUI.Hud.ClearAllSubtitles();
                                            fUI.Hud.ClearBrief();
                                            fUI.Hud.ClearAllHelpMessages();
                                            fUI.Hud.ClearGPSMultiRoute();
                                            fUI.Hud.ClearHelp(true);
                                            tastEnterplane();

                                            startSwitch = 2;
                                        }
                                    }
                                    else
                                        fUI.Hud.ClearHelp(true);
                                }
                            }
                            break;
                        case 2:
                            if (fPlayer.Player.Character.CurrentVehicle == Plane)
                            {
                                PlaneBlip?.Delete();
                                PlaneBlip = null;
                                Plane.AttachedBlip.Name = "Plane";
                                Plane.AttachedBlip.Color = BlipColor.Blue;
                                Plane.AttachedBlip.Alpha = 0;
                                startSwitch = 0;
                                fixedPosition = false;
                                Globals.missionSwitch = 1;
                            }
                            else
                            {
                                if (fCore.Timer.TimerA() > 15000)
                                {
                                    fPlayer.Player.Character.SetIntoVehicle(Plane, GTA.VehicleSeat.Driver);
                                    PlaneBlip?.Delete();
                                    PlaneBlip = null;
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
                        if (!SpawnedIn && fPlayer.Player.GetDistanceTo(PlanePos) > 200f)
                        {
                            PlaneBlip.DisplayType = BlipDisplayType.Default;
                        }
                        if (Plane != null && Plane.IsConsideredDestroyed)
                        {
                            Plane.AttachedBlip?.Delete();
                            PlaneBlip.DisplayType = BlipDisplayType.NoDisplay;
                            startSwitch = 0;
                        }
                    }
                    if (Plane != null)
                    {
                        if (fPlayer.Player.GetDistanceTo(Plane.Position) > 440f)
                        {
                            Plane.AttachedBlip?.Delete();
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
            PlaneBlip?.Delete();
            PlaneBlip = null;
            if (Plane != null)
            {
                Plane.AttachedBlip?.Delete();
                Plane.MarkAsNoLongerNeeded();
                Plane = null;
            }
        }

        unsafe void tastEnterplane()
        {
            int iVar32 = 0;
            Function.Call(Hash.CLEAR_SEQUENCE_TASK, &iVar32);
            Function.Call(Hash.OPEN_SEQUENCE_TASK, &iVar32);
            Function.Call(Hash.TASK_ENTER_VEHICLE, fPlayer.Player.Character, Plane, 20000, -1, 2.0, 1, 0);
            Function.Call(Hash.CLOSE_SEQUENCE_TASK, iVar32);
            Function.Call(Hash.TASK_PERFORM_SEQUENCE, fPlayer.Player.Character, iVar32);
            Function.Call(Hash.CLEAR_SEQUENCE_TASK, &iVar32);
        }
        private void OnShutdown(object sender, EventArgs e)
        {
            CleanUp();
        }
    }

    public class Heist : GTA.Script
    {
        public Heist()
        {
            fCore.SceneManager.Register("BombPlant", () => new BombPlantScene());
            Tick += onTick;
            Aborted += onShutdown;
            Aborted += (_, _) => fMinigames.CartGrab.DisposeAll();
            Tick += (_, _) => fMinigames.CartGrab.UpdateAll();
            Tick += (_, _) => fCombat.CombatRegistry.Check();
        }


        GTA.Vehicle Plane;
        GTA.Blip LudendorffNorthYankton;
        public static bool PlayerTeleportedToPrologue = false;
        bool IsPlaneDestroyed
        {
            get
            {
                if (Plane != null && Plane.IsConsideredDestroyed && fWorld.Entity.GetEntityHealth(Plane) <= 0)
                    return true;
                if (Plane != null && !Plane.IsConsideredDestroyed && fWorld.Entity.GetEntityHealth(Plane) > 0)
                    return false;
                return true;
            }
        }
        bool IsPrologueVehicleDestroyed
        {
            get
            {
                if (PrologueVehicle != null && PrologueVehicle.IsConsideredDestroyed || fWorld.Vehicle.GetEngineHealth(PrologueVehicle) <= 0)
                    return true;
                if (PrologueVehicle != null && !PrologueVehicle.IsConsideredDestroyed || fWorld.Vehicle.GetEngineHealth(PrologueVehicle) > 0)
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
        GTA.Vehicle PrologueVehicle;
        string PrologueVehicleName;
        GTA.Camera PrologueIntroCam;
        GTA.Camera PrologueIntroCam2;
        int num;
        GTA.Blip DepotBlip;
        public static bool justFailed = false;
        fWorld.Weather.WeatherTypes weatherTypeBeforeMission;
        bool weatherTypeSaved = false;
        bool outfitSaved = false;
        bool timeAdvanced1 = false;
        Vector2 local;
        bool scriptsKilled = false;

        private void onTick(object sender, EventArgs e)
        {
            //Screen.ShowHelpText($"{Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString)}");
            //fPathfind.GetClosestVehicleNode(fPlayer.Player.ped.CurrentVehicle.Position, out local);
            //Screen.ShowSubtitle($"{local}");
            if (Globals.missionSwitch > 3)
            {
                //fPathfind.GetClosestVehicleNode(fPlayer.Player.ped.Position, out local);
                //local = new Vector2(fPlayer.Player.ped.Position.X, fPlayer.Player.ped.Position.Y);
            }
            //Screen.ShowSubtitle($"{fWorld.Entity.IsEntityInArea(fPlayer.Player.ped, new Vector3(3873.535f, -5083.751f, 150f), new Vector3(3462.987f, -4794.556f, 65f))|| fWorld.Entity.IsEntityInArea(fPlayer.Player.ped, new Vector3(3896.537f, -5161.008f, 150f), new Vector3(4500.178f, -4978.984f, 65f)) || fWorld.Entity.IsEntityInArea(fPlayer.Player.ped, new Vector3(4500.178f, -4978.984f, 150f), new Vector3(5545.2f, -5260.716f, 65f))}");
            switch (Globals.missionSwitch)
            {
                case -1:
                    restarting = false;
                    restartPending = false;
                    fWorld.Interior.PrologueMap.YankTonRemove = true;
                    fWorld.Weather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                    fWorld.Clock.SetClockTime(12, 0, 0);
                    fWorld.Interior.PrologueMap.UnloadYankton();
                    fUI.Hud.ClearAllSubtitles();
                    fUI.Hud.ClearBrief();
                    fUI.Hud.ClearAllHelpMessages();
                    fUI.Hud.ClearGPSMultiRoute();
                    fUI.Hud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                    fAudio.Audio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Idle);
                    Globals.missionSwitch = 2;
                    break;
                case 1:
                    fAudio.Audio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.Audio.PrepareMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Idle);
                    checkpoint = 1;
                    if (!scriptsKilled)
                    {
                        ScriptManager.ScriptManager.KillScripts();
                        scriptsKilled = true;
                    }
                    fUI.Hud.ClearBrief();
                    fUI.Hud.ClearAllHelpMessages();
                    fUI.Hud.ClearGPSMultiRoute();
                    fUI.Hud.ClearHelp(true);
                    fUI.Scaleforms.MissionShard missionShard = new fUI.Scaleforms.MissionShard();
                    missionShard.ShardIn("~s~North Yankton Heist", "~s~Break into the ~y~Bobcat Security Depot~s~ in North Yankton and clean out the ~g~vault.~s~", 2, 0.375f);
                    missionShard = null;
                    Wait(1000);
                    if (!outfitSaved)
                    {
                        pedProps = fWorld.Ped.GetPedPropData(fPlayer.Player.Character);
                        pedVariation = fWorld.Ped.GetPedVariationData(fPlayer.Player.Character);
                        outfitSaved = true;
                    }
                    if (!weatherTypeSaved)
                    {
                        weatherTypeBeforeMission = fWorld.Weather.GetCurrentWeatherEnum();
                        weatherTypeSaved = true;
                    }
                    fUI.Hud.ClearAllSubtitles();
                    fUI.Hud.ClearBrief();
                    fUI.Hud.ClearAllHelpMessages();
                    fUI.Hud.ClearGPSMultiRoute();
                    fUI.Hud.ClearHelp(true);
                    GTA.Audio.SetAudioFlag(GTA.AudioFlags.LoadMPData, true);
                    GTA.Audio.SetAudioFlag(GTA.AudioFlags.DisableFlightMusic, true);
                    GTA.Audio.SetAudioFlag(GTA.AudioFlags.WantedMusicDisabled, true);
                    fAudio.Audio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Idle);
                    if (weatherTypeSaved && outfitSaved)
                    {
                        Globals.missionSwitch = 2;
                    }
                    break;
                case 2:
                    fAudio.Audio.StartAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                    if (fPlayer.Player.IsWanted)
                    {
                        if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                            fUI.Hud.ShowGXTSubtitle("NTH_WANTED");
                        else
                            Screen.ShowSubtitle("~s~Lose the Cops.");
                    }
                    if (LudendorffNorthYankton == null)
                    {
                        LudendorffNorthYankton = fBlip.Blip.CreateBlipForCoordWithParams(new Vector3(-3841.2847f, 5020.6064f, 174.8651f), BlipSprite.Standard, (BlipColor)5, 1f, "Destination");
                    }
                    if (Plane != null)
                    {
                        if (fPlayer.Player.Character.IsInVehicle(Plane))
                        {
                            if (!fPlayer.Player.IsWanted)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fUI.Hud.ShowGXTSubtitle("NTH_FLYTONY");
                                else
                                    Screen.ShowSubtitle("~s~Fly to ~y~North Yankton.~s~");
                            }
                        }
                        if (fPlayer.Player.Character.IsInVehicle(Plane))
                        {
                            if (Plane.AttachedBlip != null)
                            {
                                if (LudendorffNorthYankton != null)
                                {
                                    if (fPlayer.Player.IsWanted)
                                        LudendorffNorthYankton.Alpha = 0;
                                    else
                                        LudendorffNorthYankton.Alpha = 255;
                                    if (fPlayer.Player.GetVehicleDistanceTo(new Vector2(-3841.2847f, 5020.6064f)) < 500f && !fPlayer.Player.IsWanted)
                                    {
                                        Globals.missionSwitch = 3;
                                    }
                                }
                                Start.PlaneBlip?.Delete();
                                Start.PlaneBlip = null;
                                Plane.AttachedBlip.Alpha = 0;
                            }
                        }
                        else
                        {
                            if (!fPlayer.Player.IsWanted)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fUI.Hud.ShowGXTSubtitle("NTH_GETBKINTOPLANE");
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
                            fAudio.Audio.PrepareMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Suspense);
                            checkpoint = 2;
                            LudendorffNorthYankton?.Delete();
                            LudendorffNorthYankton = null;
                            fPlayer.Player.Character.Task.DriveTo(Plane, new Vector3((-3841.2847f - 200f), 5020.6064f, (fPlayer.Player.Character.Position.Z + 10f)), 10f, VehicleDrivingFlags.None, 5f);
                            Screen.FadeOut(2500);
                            while (!Screen.IsFadedOut)
                            {
                                Wait(0);
                            }
                            GTA.UI.LoadingPrompt.Show("NorthYanktonHeist");
                            SetOutfit();
                            fWorld.Weather.SetWeatherTypeNowPersist(fWorld.Weather.WeatherTypes.Snow);
                            Wait(1000);
                            Plane.IsPositionFrozen = true;
                            Wait(7500);
                            if (Plane != null)
                            {
                                Plane.AttachedBlip?.Delete();
                                Plane.Delete();
                                Plane = null;
                            }
                            fPlayer.Player.Character.IsPositionFrozen = true;
                            fPlayer.Player.Character.IsInvincible = true;
                            Game.Player.Character.Task.ClearAll();
                            case3Switch = -1;
                            break;
                        case -1:
                            SetOutfit();
                            fWorld.Misc.EnableDispatchService(fWorld.Misc.DispatchType.DT_FireDepartment, false);
                            fWorld.Misc.EnableDispatchService(fWorld.Misc.DispatchType.DT_AmbulanceDepartment, false);
                            fPlayer.Player.SetMaxWantedLevelTo0();
                            fAudio.Audio.StopAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                            fAudio.Audio.StartAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                            Vector3 camPos = new Vector3(3575.468f, -4871.25f, 117.1896f);
                            Vector3 camRot = new Vector3(20f, 0f, 26.39456f);
                            Vector3 camPos2 = new Vector3(3576.268f, -4872.851f, 117.1896f);
                            Vector3 camRot2 = new Vector3(25f, 0f, 26.39456f);
                            Vector3 driveDestination = new Vector3(3612.947f, -4910.009f, 111.2528f);
                            Vector3 carsPos = new Vector3(3528.305f, -4878.405f, 111.2986f);
                            float carsHeading = 250.5463f;
                            fWorld.Interior.PrologueMap.LoadYankton();
                            if (fWorld.Interior.PrologueMap.YankTon)
                            {
                                fWorld.Clock.SetClockTime(4, 35, 30);
                                fWorld.Clock.PauseClock(true);
                                fPlayer.Player.Character.DryOff();
                                fPlayer.Player.Character.ClearBloodDamage();
                                fPlayer.Player.Character.ClearVisibleDamage();
                                if (fPlayer.Player.IsTrevor)
                                    fWorld.Ped.SetPedPropIndex(fPlayer.Player.Character, 1, 4, 0, true);
                                if (Screen.IsFadedOut)
                                {
                                    fPlayer.Player.Character.IsPositionFrozen = false;
                                    fUI.Hud.RadarAndHud(false, false);
                                    fPlayer.Player.PedPos(carsPos.X, carsPos.Y, carsPos.Z, carsHeading);
                                    PlayerTeleportedToPrologue = true;
                                    num = fWorld.Misc.GetRandomIntInRange(1, 4);
                                    if (PrologueVehicle == null)
                                    {
                                        switch (num)
                                        {
                                            case 1:
                                                PrologueVehicleName = "Mesa";
                                                PrologueVehicle = fWorld.Vehicle.Create(new Model("Mesa2"), carsPos, carsHeading);
                                                break;
                                            case 2:
                                                PrologueVehicleName = "Rancher";
                                                PrologueVehicle = fWorld.Vehicle.Create(new Model("RancherXL2"), carsPos, carsHeading);
                                                break;
                                            case 3:
                                                PrologueVehicleName = "Asea";
                                                PrologueVehicle = fWorld.Vehicle.Create(new Model("Asea2"), carsPos, carsHeading);
                                                break;
                                        }
                                    }
                                    if (PrologueIntroCam == null)
                                        PrologueIntroCam = fWorld.Camera.CreateScriptedCam();
                                    while (PrologueIntroCam == null)
                                        Wait(0);
                                    if (PrologueIntroCam != null)
                                        fWorld.Camera.SetupMovingCam(PrologueIntroCam, camPos, camRot, 55f, CameraShake.Hand, 0.5f);
                                    if (PrologueIntroCam2 == null)
                                        PrologueIntroCam2 = fWorld.Camera.CreateScriptedCam();
                                    while (PrologueIntroCam2 == null)
                                        Wait(0);
                                    if (PrologueIntroCam2 != null)
                                        fWorld.Camera.SetupMovingCam(PrologueIntroCam2, camPos2, camRot2, 60f, CameraShake.Hand, 0.5f);
                                    if (PrologueVehicle != null && PrologueIntroCam != null && PrologueIntroCam2 != null)
                                    {
                                        fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Suspense);
                                        fWorld.Camera.SetCamActiveWithInterp(PrologueIntroCam2, PrologueIntroCam, 5000, fWorld.Camera.CamGraphType.SlowInOut, fWorld.Camera.CamGraphType.SlowInOut);
                                        fWorld.Camera.RenderScriptCams(true, false, 3000, true, false, fWorld.Camera.RenderingOptionFlag.NoOptions);
                                        GTA.UI.LoadingPrompt.Hide();
                                        Screen.FadeIn(3000);
                                        fPlayer.Player.Character.IsInvincible = false;
                                        if (fPlayer.Player.Character.CurrentVehicle != PrologueVehicle)
                                        {
                                            fAudio.Audio.SetVehicleRadioStation(PrologueVehicle, "OFF");
                                            fAudio.Audio.SetVehicleRadioEnabled(PrologueVehicle, false);
                                            fPlayer.Player.Character.SetIntoVehicle(PrologueVehicle, VehicleSeat.Driver);
                                        }
                                        else
                                        {
                                            PrologueVehicle.LockStatus = VehicleLockStatus.PlayerCannotLeaveCanBeBrokenIntoPersist;
                                        }
                                        fPlayer.Player.Character.Task.DriveTo(PrologueVehicle, driveDestination, 55f, VehicleDrivingFlags.None, 5f);
                                        PrologueVehicle.Speed = 10f;
                                        while (fPlayer.Player.GetVehicleDistanceTo(new Vector2(3572.151f, -4890.441f)) > 5f)
                                            Wait(0);
                                        fWorld.Camera.RenderScriptCams(false, true, 1700);
                                        PrologueIntroCam?.Delete();
                                        PrologueIntroCam = null;
                                        PrologueIntroCam2?.Delete();
                                        PrologueIntroCam2 = null;
                                        fUI.Hud.RadarAndHud(true, true);
                                        Globals.missionSwitch = 4;
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case 4:
                    fAudio.Audio.PrepareMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Gunfight);
                    fWorld.Pathfind.Nodes.GetClosestVehicleNode(fPlayer.Player.Character.Position, out local);
                    enableFailCheckingForNY = true;
                    fWorld.Weather.SetWeatherTypeNowPersist(fWorld.Weather.WeatherTypes.Snow);
                    if (DepotBlip == null)
                    {
                        DepotBlip = fBlip.Blip.CreateGPSBlip(new Vector3(5308.504f, -5221.282f, 83.51885f), 1, BlipColor.Yellow2);
                    }
                    if (DepotBlip != null)
                    {
                        DepotBlip.Name = "Depot";
                        if (PrologueVehicle != null)
                        {
                            if (fPlayer.Player.Character.CurrentVehicle == PrologueVehicle)
                            {
                                if (!fWorld.Entity.IsEntityInArea(fPlayer.Player.Character, new Vector3(3543.267f, -4875.534f, 61f), new Vector3(3649.131f, -4929.502f, 125f)) || fPlayer.Player.GetVehicleDistanceTo(local) > 27f)
                                {
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fUI.Hud.ShowGXTSubtitle("NTH_GETBKONROAD");
                                    else
                                        Screen.ShowSubtitle("~s~Get back en route to the ~y~Depot.~s~");
                                }
                                if (fWorld.Entity.IsEntityInArea(fPlayer.Player.Character, new Vector3(3543.267f, -4875.534f, 61f), new Vector3(3649.131f, -4929.502f, 125f)) || fPlayer.Player.GetVehicleDistanceTo(local) < 27f)
                                {
                                    if (PrologueVehicle.AttachedBlip != null)
                                        PrologueVehicle.AttachedBlip.DisplayType = BlipDisplayType.NoDisplay;
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fUI.Hud.ShowGXTSubtitle("NTH_GOTODEPOT");
                                    else
                                        Screen.ShowSubtitle("~s~Go to the ~y~Depot.~s~");
                                    if (fPlayer.Player.GetDistanceTo(new Vector2(DepotBlip.Position.X, DepotBlip.Position.Y)) < 400f)
                                    {
                                        Spawn();
                                    }
                                    PrologueVehicle.LockStatus = VehicleLockStatus.None;
                                    if (!timeAdvanced1)
                                    {
                                        if (fWorld.Clock.Hours == 4)
                                        {
                                            fWorld.Clock.PauseClock(false);
                                        }
                                    }
                                    if (fWorld.Clock.Hours != 5 && (fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, new Vector3(5356.7324f, -5201.1553f, 80.83122f), new Vector3(5356.5454f, -5179.6f, 96.83691f), 20f, false, true, 0) || fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, new Vector3(5417.894f, -5108.7925f, 75.56319f), new Vector3(5412.488f, -5240.66f, 95.59789f), 100f, false, true, 0)))
                                    {
                                        fWorld.Clock.EaseClockTimeTo(5, 0, 0, true, 4);
                                        fWorld.Clock.PauseClock(true);
                                        timeAdvanced1 = true;
                                    }
                                    if (fWorld.Clock.Hours == 5)
                                    {
                                        fWorld.Clock.SetClockTime(5, 0, 0);
                                        fWorld.Clock.PauseClock(true);
                                        timeAdvanced1 = true;
                                        if (fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, new Vector3(5356.7324f, -5201.1553f, 80.83122f), new Vector3(5356.5454f, -5179.6f, 96.83691f), 20f, false, true, 0) || fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, new Vector3(5417.894f, -5108.7925f, 75.56319f), new Vector3(5412.488f, -5240.66f, 95.59789f), 100f, false, true, 0))
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
                                    fUI.Hud.ShowGXTSubtitle("NTH_GETBKINTOVEH");
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
                            BoothGuard.AttachedBlip?.Delete();
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
                            fWorld.Ped.SetPedCombatAttributes(BoothGuard, fWorld.Ped.CombatAttributes.CA_IS_A_GUARD, true);
                            fWorld.Ped.SetPedCombatAttributes(BoothGuard, fWorld.Ped.CombatAttributes.CA_PLAY_REACTION_ANIMS, false);
                            fWorld.Ped.SetPedConfigFlag(BoothGuard, fWorld.Ped.PedConfigFlags.PCF_DontBlipCop, true);
                            fWorld.Ped.SetPedAsCop(BoothGuard, false);
                            fWorld.Ped.SetPedCombatAbility(BoothGuard, fWorld.Ped.CombatAbilityLevel.Professional);
                            fWorld.Ped.SetPedCombatMovement(BoothGuard, (fWorld.Ped.CombatMovement)fWorld.Misc.GetRandomIntInRange(1, 4));
                            BoothGuard.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
                            BoothGuard.RelationshipGroup = AiTeam;
                            fWorld.Ped.SetRelationshipBetweenGroups(fWorld.Ped.RelationshipTypes.Hate, AiTeam, playersTeam);
                            fWorld.Ped.SetRelationshipBetweenGroups(fWorld.Ped.RelationshipTypes.Hate, playersTeam, AiTeam);
                        }
                    }
                    if (BoothGuard != null)
                    {
                        if (DepotBlip != null)
                        {
                            fBlip.Blip.ClearGPSMultiRoute();
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
                            fPlayer.Player.SetMaxWantedLevelTo0();
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_KILLGUARD");
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
                                            fUI.Hud.ShowGXTSubtitle("NTH_ENTRDEPOT");
                                        else
                                            Screen.ShowSubtitle("~s~Enter the ~y~Depot.~s~");
                                        if (fPlayer.Player.GetDistanceTo(new Vector2(DepotBlip.Position.X, DepotBlip.Position.Y)) < 1.3f)
                                        {
                                            PrevWeapon = fPlayer.Player.Character.Weapons.Current;
                                            Screen.FadeOut(1000);
                                            while (!Screen.IsFadedOut)
                                                Wait(0);
                                            LoadingPrompt.Show("NorthYanktonHeist");
                                            fWorld.Interior.PrologueMap.LoadDepot();
                                            fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Gunfight);
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
                            fCore.Streaming.RequestAnimDict("anim@apt_trans@hinge_l_action");
                            fUI.Hud.ClearSubtitles();
                            fPlayer.Player.SetMaxWantedLevelTo0();
                            fPlayer.Player.Character.IsPositionFrozen = false;
                            fUI.Hud.RadarAndHud(false, false);
                            fWorld.Clock.SetClockTime(5, 0, 0);
                            fWorld.Weather.SetWeatherTypeNowPersist(fWorld.Weather.WeatherTypes.Snow);
                            fWorld.Clock.PauseClock(true);
                            fPlayer.Player.Character.DryOff();
                            fPlayer.Player.Character.ClearBloodDamage();
                            fPlayer.Player.Character.ClearVisibleDamage();
                            if (fPlayer.Player.IsTrevor)
                                fWorld.Ped.SetPedPropIndex(fPlayer.Player.Character, 1, 4, 0, true);
                            fAudio.Audio.StopAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                            new fCombat.CombatAgent(fCombat.CombatConfig.NTH_VaultGuard(new Vector3(5300.397f, -5197.885f, 83.502f), 285.0927f));
                            new fCombat.CombatAgent(fCombat.CombatConfig.NTH_VaultGuard(new Vector3(5296.698f, -5194.016f, 83.5018f), 188.9838f));
                            new fCombat.CombatAgent(fCombat.CombatConfig.NTH_VaultGuard(new Vector3(5297.962f, -5181.915f, 83.51785f), 359.7572f));
                            new fCombat.CombatAgent(fCombat.CombatConfig.NTH_VaultGuard(new Vector3(5303.865f, -5178.598f, 83.50168f), 258.0538f));
                            new fCombat.CombatAgent(fCombat.CombatConfig.NTH_VaultGuard(new Vector3(5304.78f, -5178.218f, 83.51274f), 12.30173f));
                            fCombat.CombatRegistry.CreateAll();
                            Function.Call(Hash.ADD_NAVMESH_REQUIRED_REGION, 5297.568f, -5188.236f, 200f);
                            while (!Function.Call<bool>(Hash.IS_NAVMESH_LOADED_IN_AREA, 5297.568f - 100f, -5188.236f - 100f, 5297.568f + 100f, -5188.236f + 100f))
                                Wait(0);
                            if (AnimGuard == null)
                            {
                                AnimGuard = fWorld.Ped.CreatePed(new Model("IG_ProlSec_02"), new Vector3(5318.503f, -5206.221f, (85.7187f - 3.2f)), 139.6356f);//  Function.Call<Ped>(Hash.CREATE_PED, 26, Misc.GetHashKey("IG_ProlSec_02"), 5310.6543f, -5207.032f, (85.7187f - 3.2f), 139.6356f, true, true);
                                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, AnimGuard);
                                Function.Call(Hash.SET_ENTITY_SHOULD_FREEZE_WAITING_ON_COLLISION, AnimGuard, true);
                                Wait(50);
                            }
                            while (AnimGuard == null)
                                Wait(0);
                            if (AnimCam == null)
                            {
                                AnimCam = fWorld.Camera.CreateScriptedCam();
                                fWorld.Camera.SetupMovingCam(AnimCam, new Vector3(5308.226f, -5208.727f, 83.959f), new Vector3(-1.000001f, -3.000001f, -92.49069f), 55f, CameraShake.Hand, 0.5f);
                            }
                            while (AnimCam == null)
                            {
                                Wait(0);
                            }
                            if (AnimCam != null)
                            {
                                fWorld.Camera.SetupMovingCam(AnimCam, new Vector3(5308.226f, -5208.727f, 83.959f), new Vector3(0f, 0f, -92.49069f), 42f, CameraShake.Hand, 1f);
                                GTA.ScriptCameraDirector.StartRendering();
                            }
                            if (AnimDoor == null)
                            {
                                AnimDoor = Function.Call<Prop>(Hash.CREATE_OBJECT_NO_OFFSET, fWorld.Misc.joaat("v_ilev_cd_door2"), 5308.8574f, -5208.156f, ((86.9186f - 3.2f) - 0.05f), true, true, false, 0);
                                Function.Call(Hash.FREEZE_ENTITY_POSITION, AnimDoor, true);
                            }
                            while (AnimDoor == null)
                                Wait(0);
                            if (GTA.World.GetClosestProp(new Vector3(5305.461f, -5177.75f, 83.66856f), 0.4f, -311575617) != null)
                            {
                                secDoor = GTA.World.GetClosestProp(new Vector3(5305.461f, -5177.75f, 83.66856f), 0.4f, -311575617);
                            }
                            if (secDoor != null)
                            {
                                secDoor.IsPositionFrozen = true;
                            }
                            case5switch = 1;
                            break;
                        case 1:
                            Screen.FadeIn(1700);
                            LoadingPrompt.Hide();
                            if (!player.IsRunning)
                            {
                                fPlayer.Player.Character.Weapons.Select(GTA.WeaponHash.Unarmed);
                                player.Create();
                                door.Create();
                                player.Rate = 0f;
                                door.Rate = 0f;
                                player.PlayPed(fPlayer.Player.Character, "anim@apt_trans@hinge_l_action", "player_exit");
                                door.PlayEntity(AnimDoor, "anim@apt_trans@hinge_l_action", "door_exit");
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
                            if (PrevWeapon == WeaponHash.Unarmed)
                            {
                                fPlayer.Player.Character.Weapons.Give(WeaponHash.PumpShotgun, 260, true, true);
                                fPlayer.Player.Character.Weapons.Select(WeaponHash.PumpShotgun);
                            }
                            else
                            {
                                fPlayer.Player.Character.Weapons.Select(PrevWeapon);
                            }
                            fPlayer.Player.Character.Task.ClearAllImmediately();
                            player.Dispose();
                            door.Dispose();
                            GameplayCamera.SetCamViewModeForContext(CamViewModeContext.OnFoot, CamViewMode.FirstPerson);
                            fUI.Graphics.AnimpostFXPlay("CamPushInNeutral", 1700, false);
                            fAudio.Audio.PlaySoundFrontend("1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET");
                            AnimGuard.Weapons.Give(WeaponHash.Pistol, 8, true, true);
                            AnimGuard.Weapons.Select(WeaponHash.Pistol);
                            AnimGuard.Task.RunTo(new Vector3(5314.563f, -5206.421f, 83.51863f), false, 20000);
                            Wait(700);
                            ScriptCameraDirector.StopRendering();
                            fUI.Hud.RadarAndHud(true, true);
                            Wait(500);
                            AnimGuard.Task.AimGunAtEntity(fPlayer.Player.Character, 10000);
                            case5switch = 4;
                            break;
                        case 4:
                            AnimCam?.Delete();
                            AnimCam = null;
                            fPlayer.Player.FakeWantedLevel = 3;
                            fCombat.CombatRegistry.ActivateAll();
                            case5switch = 5;
                            break;
                        case 5:
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_PUSHTOVAULT");
                            else
                                Screen.ShowSubtitle("~s~Push through to the ~y~vault.~s~");
                            if (fPlayer.Player.GetDistanceTo(new Vector3(5297.854f, -5188.762f, 83.51839f)) < 3.7f)
                            {
                                case5switch = 6;
                            }
                            break;
                        case 6:
                            Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, fCore.Streaming.RequestAnimDict("anim@scripted@player@mission@tun_bomb_plant@male@"), "enter", 5298.2f, -5187.9f, 83.21848f, 0f, 0f, -85f, 0f, 2);
                            float z = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, fCore.Streaming.RequestAnimDict("anim@scripted@player@mission@tun_bomb_plant@male@"), "enter", 5298.2f, -5187.9f, 83.21848f, 0f, 0f, -85f, 0f, 2).Z;
                            if (InsideBlip == null)
                            {
                                InsideBlip = fBlip.Blip.CreateBlipForCoordWithParams(new Vector3(5298.512f, -5188.697f, 81.51868f), (BlipSprite)814, BlipColor.Yellow, 1f, "Vault");
                            }
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_PLANTBOMB");
                            else
                                Screen.ShowSubtitle("~s~Plant an ~r~explosive device~s~ on the ~y~vault door.~s~");
                            if (fPlayer.Player.GetDistanceTo(new Vector3(5298.2f, -5187.9f, 84.21848f)) < 2.5f)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fUI.Hud.DisplayHelpTextThisFrameGXT("NTH_TOPLANTBOMB");
                                else
                                    fUI.Hud.DisplayHelpText("~s~Press ~INPUT_CONTEXT~ to plant the ~r~explosive device.~s~");
                                if (Game.IsControlJustPressed(GTA.Control.Context))
                                {
                                    Function.Call(Hash.SET_MOVEMENT_MODE_OVERRIDE, Game.Player.Character, "DEFAULT_ACTION");
                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, vector.X, vector.Y, vector.Z, 1f, 10000, -85f, 1f);
                                    Function.Call(Hash.SET_PED_KEEP_TASK, Game.Player.Character, true);
                                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Idle);
                                    case5switch = 7;
                                }
                            }
                            break;
                        case 7:
                            InsideBlip?.Delete();
                            InsideBlip = null;
                            if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                            {
                                GameplayCamera.SetCamViewModeForContext(CamViewModeContext.InVehicle, CamViewMode.ThirdPersonFar);
                                fCore.SceneManager.StartScene("BombPlant");
                                case5switch = 8;
                            }
                            break;
                        case 8:
                            fCore.SceneManager.Update();
                            if (BombPlantScene._permBomb != null)
                            {
                                BombPlantScene._permBomb.IsPositionFrozen = true;
                                fWorld.Entity.SetEntityCanBeDamaged(BombPlantScene._permBomb, false);
                            }
                            if (!fCore.SceneManager.IsSceneRunning)
                            {
                                if (fWorld.Interior.Interior.GetRoomKeyFromEntity(fPlayer.Player.Character) != 592960010)
                                {
                                    if (InsideBlip == null)
                                    {
                                        InsideBlip = fBlip.Blip.CreateBlipForCoordWithParams(new Vector3(5308.768f, -5204.557f, 83.51859f), BlipSprite.Standard, BlipColor.Yellow, 1f, "Lobby");
                                    }
                                    InsideBlip?.Alpha = 255;
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fUI.Hud.ShowGXTSubtitle("NTH_BACKTOLOBBY");
                                    else
                                        Screen.ShowSubtitle("~s~Return to the ~y~lobby.~s~");
                                }
                                else
                                {
                                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Delivering);
                                    InsideBlip?.Alpha = 0;
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fUI.Hud.ShowGXTSubtitle("NTH_DETONATE");
                                    else
                                        Screen.ShowSubtitle("~s~Detonate the explosive.");
                                    if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                        fUI.Hud.DisplayHelpTextThisFrameGXT("NTH_TODETONATE");
                                    else
                                        fUI.Hud.DisplayHelpText("~s~Press ~INPUT_DETONATE~ to detonate the explosive.");
                                    if (Game.IsControlJustPressed(GTA.Control.Detonate))
                                    {
                                        InsideBlip?.Delete();
                                        InsideBlip = null;
                                        var trolly1 = GTA.World.GetClosestProp(new Vector3(5308.04f, -5191.028f, 82.99158f), 0.3f, 929864185);
                                        var trolly2 = GTA.World.GetClosestProp(new Vector3(5302.142f, -5191.521f, 82.99158f), 0.3f, 929864185);
                                        if (trolly1 is not null &&
                                            trolly2 is not null)
                                        {
                                            trolly1.Delete();
                                            trolly2.Delete();
                                        }
                                        c1.Create();
                                        c2.Create();
                                        c3.Create();
                                        GTA.Audio.SetAudioFlag(GTA.AudioFlags.LoadMPData, false);
                                        Wait(1500);
                                        case5switch = 9;
                                    }
                                }
                            }
                            break;
                        case 9:
                            RayfireScenes.PrologueVaultRayfire.Scene();
                            fPlayer.Player.FakeWantedLevel = 5;
                            if (RayfireScenes.PrologueVaultRayfire.vaultScene > 8)
                            {
                                fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                                fCore.Streaming.UseParticleFXAsset("scr_prologue");
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
                                c1.ConnectToValueBar(takeBar);
                                c2.ConnectToValueBar(takeBar);
                                c3.ConnectToValueBar(takeBar);
                                fUI.TimerBars.HudBarController.Register(takeBar);
                                c1.ApplyBlipPreset(fBlip.BlipPresets.VaultCart);
                                c2.ApplyBlipPreset(fBlip.BlipPresets.VaultCart);
                                c3.ApplyBlipPreset(fBlip.BlipPresets.VaultCart);
                                if (c1.GetBlip() is not null)
                                {
                                    c1.GetBlip().Alpha = 0;
                                }
                                if (c2.GetBlip() != null)
                                {
                                    c2.GetBlip().Alpha = 0;
                                }
                                if (c3.GetBlip() != null)
                                {
                                    c3.GetBlip().Alpha = 0;
                                }
                                if (VaultBlip == null)
                                {
                                    VaultBlip = fBlip.Blip.CreateBlipForCoordWithParams(new Vector3(5303.874f, -5189.139f, 82.54237f), BlipSprite.Standard, BlipColor.Yellow, 1f, "Vault");
                                }
                                fAudio.Audio.PrepareMusicEvent("MP_MC_START_VACUUM_8");
                                fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                Globals.missionSwitch = 6;
                            }
                            break;
                    }
                    break;
                case 6:
                    if (fWorld.Interior.Interior.GetRoomKeyFromEntity(fPlayer.Player.Character) == 2308199534)
                    {
                        if (!c1.IsMinigameActive && !c2.IsMinigameActive && !c3.IsMinigameActive)
                        {
                            if (c1.GetBlip() is not null)
                            {
                                c1.GetBlip().Alpha = 255;
                            }
                            if (c2.GetBlip() != null)
                            {
                                c2.GetBlip().Alpha = 255;
                            }
                            if (c3.GetBlip() != null)
                            {
                                c3.GetBlip().Alpha = 255;
                            }
                            VaultBlip?.Delete();
                            VaultBlip = null;
                        }
                    }
                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.MedIntensity);
                    if (takeBar.Value >= 450000)
                    {
                        if (!c1.IsLooted && !c2.IsLooted && !c3.IsLooted)
                        {
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_LOOTORLEAVE");
                            else
                                Screen.ShowSubtitle("~s~Continue ~g~collecting the cash~s~ or ~y~leave the vault.~s~");
                        }
                        if (c1.IsLooted && c2.IsLooted && c3.IsLooted)
                        {
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_LEAVETHEVAULT");
                            else
                                Screen.ShowSubtitle("~s~Leave the ~y~vault.~s~");
                        }
                        if (fWorld.Interior.Interior.GetRoomKeyFromEntity(fPlayer.Player.Character) == 1848825415)
                        {
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.DisplayGXTHelpText("NTH_TOERASEFOOT");
                            else
                                fUI.Hud.DisplayHelpText("~s~Security cameras caught your robbery on camera.", " ~y~Head to the security room~s~to ~r~erase the footage.~s~");
                            Globals.missionSwitch = 7;
                        }
                    }
                    else
                    {
                        if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                            fUI.Hud.ShowGXTSubtitle("NTH_COLLECTCASH");
                        else
                            Screen.ShowSubtitle("~s~Collect the ~g~cash.~s~");
                    }
                    break;
                case 7:
                    switch (AfterVaultSwitch)
                    {
                        case 0:
                            if (InsideBlip == null)
                                InsideBlip = fBlip.Blip.CreateBlipPreset(new Vector3(5305.461f, -5177.75f, 83.66856f), fBlip.BlipPresets.Destination);
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_HEADTOSEC");
                            else
                                Screen.ShowSubtitle("~s~Head to the ~y~security room.~s~");
                            if (fPlayer.Player.GetDistanceTo(new Vector3(5305.461f, -5177.75f, 83.66856f)) < 5f)
                                AfterVaultSwitch = 1;
                                break;
                        case 1:
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_SHOOTLOCK");
                            else
                                Screen.ShowSubtitle("~s~Shoot open ~y~the security room door.~s~");
                            if (fPlayer.Player.Character.IsAiming)
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fUI.Hud.DisplayGXTHelpText("NTH_TOSHOOTLOCK");
                                else
                                    fUI.Hud.DisplayHelpText("~s~~r~Shoot~s~ around ~y~the latch~s~ to force the door open.");
                                AfterVaultSwitch = 2;
                            }
                            break;
                        case 2:
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_SHOOTLOCK");
                            else
                                Screen.ShowSubtitle("~s~Shoot open ~y~the security room door.~s~");
                            if (!StungunEquiped && fWorld.Misc.HasBulletImpactedInArea(5304.271f, -5177.798f, 83.62451f, 0.12f))
                            {
                                InsideBlip?.Alpha = 0;
                                AfterVaultSwitch = 3;
                            }
                            break;
                        case 3:
                            if (secDoor != null)
                            {
                                secDoor.IsPositionFrozen = false;

                                if (!secDoor.IsPositionFrozen)
                                {
                                    Function.Call(Hash.SET_ENTITY_ANGULAR_VELOCITY, secDoor, 0f, 0f, -12f);
                                }
                                fAudio.Audio.PlaySoundFrontend("lock_break", "DLC_BTL_Collector_Sounds");
                                AfterVaultSwitch = 4;
                            }
                            break;
                        case 4:
                            if (fWorld.Effects.Fire.GetCountInRange(new Vector3(5300.542f, -5175.415f, 82.97834f), 2f) > 2)
                            {
                                InsideBlip?.Delete();
                                InsideBlip = null;
                                foreach (var blip in _torchBlips)
                                {
                                    blip?.Delete();
                                }
                                _torchBlips.Clear();
                                timerBar = new fUI.TimerBars.HudCountdownBar("TIME LEFT:", 12999)
                                {
                                    TimeColor = Color.FromArgb(255, 224, 50, 50),
                                    OnFinished = () => AfterVaultSwitch = 5
                                };
                                fUI.TimerBars.HudBarController.Register(timerBar);
                                AfterVaultSwitch = 25;
                            }
                            else
                            {
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fUI.Hud.ShowGXTSubtitle("NTH_TORCHSECROOM");
                                else
                                    Screen.ShowSubtitle("~s~Torch~s~ the ~y~security room~s~ to ~r~erase the footage.~s~");
                                if (fInterior.Interior.GetRoomKeyFromEntity(fPlayer.Player.Character) == 3256296829)
                                {
                                    InsideBlip?.Alpha = 0;
                                    if (!_torchBlipsCreated)
                                    {
                                        foreach (var point in torchPoints)
                                        {
                                            _torchBlips.Add(fBlip.Blip.CreateBlipPreset(point, fBlip.BlipPresets.Torchable));
                                        }
                                        fPlayer.Player.Character.Weapons.Give(WeaponHash.Molotov, 5, true, true);
                                        fPlayer.Player.Character.Weapons.Select(WeaponHash.Molotov);
                                        _torchBlipsCreated = true;
                                    }
                                    else
                                    {
                                        foreach (var blip in _torchBlips)
                                        {
                                            blip?.Alpha = 255;
                                        }
                                    }
                                }
                                else
                                {
                                    InsideBlip?.Alpha = 255;
                                    if (_torchBlipsCreated)
                                    {
                                        foreach (var blip in _torchBlips)
                                        {
                                            blip?.Alpha = 0;
                                        }
                                    }
                                }
                            }
                            break;
                        case 25:
                            if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                fUI.Hud.ShowGXTSubtitle("NTH_GETOUTSECROOM");
                            else
                                Screen.ShowSubtitle("~s~Leave the ~r~security room~s~ before it ~r~explodes.~s~");
                            if (secDoor != null)
                            {
                                if (timerBar.RemainingTime < 700)
                                {
                                    Function.Call(Hash.SET_ENTITY_ANGULAR_VELOCITY, secDoor, 0f, 0f, 18f);
                                    Wait(700);
                                    secDoor.Rotation = new Vector3(0f, 0f, -179.8076f);
                                    secDoor.IsPositionFrozen = true;
                                }
                            }
                            
                            break;
                        case 5:
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5300.112f, -5174.039f, 83.77322f), ExplosionType.BombCluster, 2.5f, true, false, 1f);
                            Wait(500);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5298.342f, -5175.844f, 83.72742f), ExplosionType.BombIncendiary, 0.5f, true, false, 1f);
                            Wait(500);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5298.339f, -5176.726f, 83.72742f), ExplosionType.BombIncendiary, 0.5f, true, false, 1f);
                            Wait(250);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5301.916f, -5174.015f, 83.85043f), ExplosionType.BombIncendiary, 0.2f, true, false, 0.4f);
                            Wait(100);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5298.761f, -5174.673f, 85.71891f), ExplosionType.BombIncendiary, 0.5f, true, false, 0.4f);
                            Wait(90);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5303.894f, -5173.873f, 82.52132f), ExplosionType.BombIncendiary, 0.5f, true, false, 0.4f);
                            Wait(50);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5300.247f, -5174.295f, 83.45172f), ExplosionType.BombIncendiary, 0.5f, true, false, 0.4f);
                            Wait(50);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5298.665f, -5174.837f, 83.37376f), ExplosionType.BombIncendiary, 0.5f, true, false, 0.4f);
                            Wait(50);
                            fWorld.Effects.Explosion.CreatePedOwnedExplosion(fPlayer.Player.Character, new Vector3(5304.854f, -5173.918f, 83.80856f), ExplosionType.BombIncendiary, 0.5f, true, false, 1f);
                            AfterVaultSwitch = 6;
                            break;
                        case 6:
                            fWorld.Ped.SetPedConfigFlag(fPlayer.Player.Character, fWorld.Ped.PedConfigFlags.PCF_ForcePedToFaceLeftInCover, true);
                            if (fPlayer.Player.GetDistanceTo(new Vector3(5315.436f, -5177.626f, 83.51868f)) < 0.1f && fPlayer.Player.Character.IsInCover)
                            {
                                if (fPlayer.Player.Character.IsInCoverFacingLeft)
                                {
                                    InsideBlip?.Delete();
                                    InsideBlip = null;
                                    fUI.Hud.ClearSubtitles();
                                    Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
                                    Wait(1500);
                                    fUI.Hud.RadarAndHud(false, false);
                                    AfterVaultSwitch = 7;
                                    break;
                                }
                            }
                            else
                            {
                                if (InsideBlip == null)
                                    InsideBlip = fBlip.Blip.CreateBlipPreset(new Vector3(5315.436f, -5177.626f, 83.51868f), fBlip.BlipPresets.Destination);
                                if (Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString))
                                    fUI.Hud.ShowGXTSubtitle("NTH_GETINPOSITION");
                                else
                                Screen.ShowSubtitle("~s~Get in ~y~position.~s~");
                            }
                            break;
                        case 7:
                            RayfireScenes.PrologueDoorRayfire.Scene();
                            if (RayfireScenes.PrologueDoorRayfire.doorScene >= 4)
                                AfterVaultSwitch = 8;
                            break;
                        case 8:
                            break;
                    }
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
        private WeaponHash PrevWeapon = WeaponHash.Unarmed;
        private bool _torchBlipsCreated;
        private readonly List<Blip> _torchBlips = new();
        Vector3[] torchPoints =
{
    new Vector3(5298.38f, -5176.717f, 83.72629f),
    new Vector3(5298.35f, -5175.852f, 83.72736f),
    new Vector3(5298.267f, -5174.002f, 83.68849f),
    new Vector3(5300.108f, -5174.042f, 83.77282f),
    new Vector3(5301.917f, -5174.024f, 83.85127f)
};

        fUI.TimerBars.HudCountdownBar timerBar;
        private bool StungunEquiped => fPlayer.Player.Character.Weapons.Current == WeaponHash.StunGun || fPlayer.Player.Character.Weapons.Current == WeaponHash.StunGunMultiplayer;
        Prop secDoor;
        private int AfterVaultSwitch = 0;
        fUI.TimerBars.HudValueBar takeBar = new fUI.TimerBars.HudValueBar("TAKE", isMoney: true) { Value = 0, ValueColor = HudColors.GetColor(HudColor.Green) };
        fMinigames.CartGrab c1 = new fMinigames.CartGrab(fMinigames.CartType.Standard, new Vector3(5302.142f, -5191.521f, 82.99158f), false, new Vector3(0f, 0f, 75f));
        fMinigames.CartGrab c2 = new fMinigames.CartGrab(fMinigames.CartType.Standard, new Vector3(5308.04f, -5191.028f, 82.99158f), false, new Vector3(0f, 0f, 20f));
        fMinigames.CartGrab c3 = new fMinigames.CartGrab(fMinigames.CartType.Standard, new Vector3(5306.773f, -5187.983f, 82.99162f), false, new Vector3(0f, 0f, 155f));
        Blip VaultBlip;
        Blip InsideBlip;
        SynchronizedScene player = new SynchronizedScene(new Vector3(5309.55f, -5210.1f, (86.9186f - 3.41f)));
        SynchronizedScene door = new SynchronizedScene(new Vector3((5309.55f - 0.7f), (-5210.1f + 1.902f), ((86.9186f - 3.2f) - 0.05f)));
        private Prop AnimDoor;
        private Camera AnimCam;
        private Ped AnimGuard;
        private int case5switch = -1;
        private static fWorld.Ped.CombatAttributes[] EnableAttrbutes =
            {
            fWorld.Ped.CombatAttributes.CA_BLIND_FIRE_IN_COVER,
            fWorld.Ped.CombatAttributes.CA_CAN_CHARGE,
            fWorld.Ped.CombatAttributes.CA_CAN_USE_PEEKING_VARIATIONS,
            fWorld.Ped.CombatAttributes.CA_DISABLE_BULLET_REACTIONS,
            fWorld.Ped.CombatAttributes.CA_DISABLE_FLEE_FROM_COMBAT,
            fWorld.Ped.CombatAttributes.CA_ENABLE_TACTICAL_POINTS_WHEN_DEFENSIVE,
            fWorld.Ped.CombatAttributes.CA_IS_A_GUARD,
            fWorld.Ped.CombatAttributes.CA_USE_COVER,
            fWorld.Ped.CombatAttributes.CA_USE_PROXIMITY_FIRING_RATE,
            fWorld.Ped.CombatAttributes.CA_DISABLE_ENTRY_REACTIONS,
            };
        private static fWorld.Ped.CombatAttributes[] DisableAttrbutes =
            {
            fWorld.Ped.CombatAttributes.CA_DISABLE_REACT_TO_BUDDY_SHOT,
            fWorld.Ped.CombatAttributes.CA_PLAY_REACTION_ANIMS,
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
            if (!fPlayer.Player.IsMichael)
            {
                if (!fPlayer.Player.IsFranklin)
                {
                    if (!fPlayer.Player.IsTrevor)
                    {
                        if (!fPlayer.Player.IsFreemodeMale)
                        {
                            if (!fPlayer.Player.IsFreemodeFemale)
                            {
                                return;
                            }
                            else
                            {
                                fWorld.Ped.SetPedDefaultComponentVariation(fPlayer.Player.Character);
                                fWorld.Ped.ClearAllPedProps(fPlayer.Player.Character, 1);
                                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 1, 58, 4, 0);
                                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 3, 40, 1, 0);
                                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 4, 32, 0, 0);
                                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 5, 82, 0, 0);
                                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 6, 25, 0, 0);
                                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 11, 215, 18, 0);
                            }
                        }
                        else
                        {
                            fWorld.Ped.SetPedDefaultComponentVariation(fPlayer.Player.Character);
                            fWorld.Ped.ClearAllPedProps(fPlayer.Player.Character, 1);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 1, 37, 0, 0);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 3, 33, 1, 0);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 4, 175, 0, 0);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 5, 82, 0, 0);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 6, 154, 0, 0);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 8, 74, 7, 0);
                            fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 11, 545, 0, 0);
                        }
                    }
                    else
                    {
                        fWorld.Ped.SetPedDefaultComponentVariation(fPlayer.Player.Character);
                        fWorld.Ped.ClearAllPedProps(fPlayer.Player.Character, 1);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 0, 0, 5, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 1, 0, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 2, 1, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 8, 13, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 3, 9, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 4, 9, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 6, 12, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 5, 4, 0, 0);
                        fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 9, 1, 0, 0);
                    }
                }
                else
                {
                    fWorld.Ped.SetPedDefaultComponentVariation(fPlayer.Player.Character);
                    fWorld.Ped.ClearAllPedProps(fPlayer.Player.Character, 1);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 3, 14, 4, 0);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 4, 16, 0, 0);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 5, 4, 0, 0);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 6, 9, 0, 0);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 8, 4, 0, 0);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 9, 6, 0, 0);
                    fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 1, 0, 0, 0);
                }
            }
            else
            {
                fWorld.Ped.SetPedDefaultComponentVariation(fPlayer.Player.Character);
                fWorld.Ped.ClearAllPedProps(fPlayer.Player.Character, 1);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 0, 0, 3, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 2, 1, 0, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 8, 7, 0, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 5, 5, 0, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 3, 31, 0, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 4, 26, 0, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 6, 14, 0, 0);
                fWorld.Ped.SetPedComponentVariation(fPlayer.Player.Character, 9, 1, 0, 0);
            }
        }

        public static void SpawnBooth()
        {
            if (BoothGuard == null)
            {
                BoothGuard = fWorld.Ped.CreatePed(new Model(PedHash.Security01SMM), new Vector3(5362.298f, -5181.831f, 83.21764f), 165f);
            }
            if (boothProps.Count == 0)
            {
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.593f, -5181.164f, 84.52729f), new Vector3(0f, 180f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.589f, -5181.263f, 82.06731f), new Vector3(0f, 0f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.7f, -5181.164f, 84.52729f), new Vector3(0f, 180f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.696f, -5181.263f, 82.06731f), new Vector3(0f, 0f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("apa_mp_h_acc_rugwools_03"), new Vector3(5362.6f, -5180.766f, 82.17793f), new Vector3(0f, 0f, 0f), false, false);
            }

        }
        static List<Vehicle> depotVehicles = new List<Vehicle>();
        public static void SpawnTrucks()
        {
            if (depotVehicles.Count == 0)
            {
                fWorld.Vehicle.CreateForList(depotVehicles, new Model("stockade3"), new Vector3((5341.3525f + 1.365f), -5177.149f, 81.762f), 0.3367f);
                Function.Call(Hash.SET_VEHICLE_IS_CONSIDERED_BY_PLAYER, depotVehicles[0], false);
                Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, depotVehicles[0], true);
                fWorld.Vehicle.CreateForList(depotVehicles, new Model("stockade3"), new Vector3((5337.0996f + 1.365f), -5177.0317f, 81.762f), 2.5903f);
                Function.Call(Hash.SET_VEHICLE_IS_CONSIDERED_BY_PLAYER, depotVehicles[1], false);
                Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, depotVehicles[1], true);
                Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, fWorld.Misc.joaat("stockade3"));
            }
        }
        // security booth guard pos: 5362.298f, -5181.831f, 83.21764f, 165f
        void Spawn()
        {
            if (Garagecolobject == null)
            {
                Garagecolobject = World.CreateProp(new Model("p_gdoor1colobject_s"), new Vector3(5320.59f, -5188.49f, 82.52f), new Vector3(0f, 0f, 90f), false, false);
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
                    fWorld.Entity.SetEntityCanBeDamaged(prop, false);
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

        fUI.Scaleforms.InstructionalButtons resetButtons = new fUI.Scaleforms.InstructionalButtons();
        fUI.Scaleforms.InstructionalButton ResetButtonContainer = new fUI.Scaleforms.InstructionalButton(GTA.Control.FrontendAccept, "Restart Mission");
        fUI.Scaleforms.InstructionalButton GoBackToFreeroamContainer = new fUI.Scaleforms.InstructionalButton(GTA.Control.FrontendCancel, "Leave Mission");
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
                        fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                        fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Fail);
                        fAudio.Audio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                        justFailed = true;
                        SetFailVariation(FailVariations.PlaneDestroyed);
                        MissionFailCleanUpRequired = true;
                    }
                }
                if (Globals.missionSwitch > 3 && Globals.missionSwitch < 5)
                {
                    check1 = fPlayer.Player.GetVehicleDistanceTo(local) > 60f;
                    if (enableFailCheckingForNY)
                    {
                        if (IsPrologueVehicleDestroyed)
                        {
                            fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                            fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Fail);
                            fAudio.Audio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                            justFailed = true;
                            SetFailVariation(FailVariations.PrologueVehicleDestroyed);
                            MissionFailCleanUpRequired = true;
                        }
                        else
                        {
                            if (!fWorld.Entity.IsEntityInArea(fPlayer.Player.Character, new Vector3(3543.267f, -4875.534f, 61f), new Vector3(3649.131f, -4929.502f, 125f)))
                            {
                                if (fPlayer.Player.Character.CurrentVehicle == PrologueVehicle && check1)
                                {
                                    fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Fail);
                                    fAudio.Audio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                                    justFailed = true;
                                    SetFailVariation(FailVariations.PlayerFailedToReachTheDepot);
                                    MissionFailCleanUpRequired = true;
                                }
                            }
                            if (fPlayer.Player.Character.CurrentVehicle != PrologueVehicle)
                            {
                                if (fPlayer.Player.GetDistanceTo(PrologueVehicle.Position) > 40f || fPlayer.Player.GetVehicleDistanceTo(PrologueVehicle.Position) > 40f)
                                {
                                    fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Fail);
                                    fAudio.Audio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
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
                fPlayer.Player.SetMaxWantedLevelTo0();
                if (justFailed && !restartPending)
                {
                    weatherTypeSaved = false;
                    if (fPlayer.Player.IsFranklin)
                    {
                        PlayerNameStr = "Franklin";
                    }
                    if (fPlayer.Player.IsTrevor)
                    {
                        PlayerNameStr = "Trevor";
                    }
                    if (fPlayer.Player.IsMichael)
                    {
                        PlayerNameStr = "Michael";
                    }
                    if (fPlayer.Player.IsFreemodePed)
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
                        Start.Plane.AttachedBlip?.Delete();
                        Start.Plane.MarkAsNoLongerNeeded();
                        Start.Plane = null;
                    }
                    fCombat.CombatRegistry.DisposeAll();
                    InsideBlip?.Delete();
                    InsideBlip = null;
                    AnimGuard?.Delete();
                    AnimGuard = null;
                    AnimDoor?.Delete();
                    AnimDoor = null;
                    AnimCam?.Delete();
                    AnimCam = null;
                    VaultBlip?.Delete();
                    VaultBlip = null;
                    LudendorffNorthYankton?.Delete();
                    LudendorffNorthYankton = null;
                    PrologueIntroCam?.Delete();
                    PrologueIntroCam = null;
                    PrologueIntroCam2?.Delete();
                    PrologueIntroCam2 = null;
                    if (PrologueVehicle != null)
                    {
                        PrologueVehicle.AttachedBlip?.Delete();
                        PrologueVehicle.MarkAsNoLongerNeeded();
                        PrologueVehicle = null;
                    }
                    DepotBlip?.Delete();
                    DepotBlip = null;
                    fAudio.Audio.StopAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                    fAudio.Audio.StopAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                    fUI.Hud.ClearAllSubtitles();
                    fUI.Hud.ClearBrief();
                    fUI.Hud.ClearAllHelpMessages();
                    fUI.Hud.ClearGPSMultiRoute();
                    fUI.Hud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, false);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, false);
                    fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Fail);
                    if (GetFailVariation() == FailVariations.PlaneDestroyed)
                    {
                        fUI.Scaleforms.MissionShard failShard = new fUI.Scaleforms.MissionShard();
                        failShard.ShardIn("HEIST FAILED", "The Plane Was Destroyed.", 6, 0.3f, 6, true);
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
                            fUI.Scaleforms.MissionShard failShard = new fUI.Scaleforms.MissionShard();
                            failShard.ShardIn("HEIST FAILED", $"{PrologueVehicleName} Was Destroyed.", 6, 0.3f, 6, true);
                            failShard = null;
                            failShardJustShown = true;
                            Screen.FadeOut(1000);
                            justFailed = false;
                            SetFailVariation(FailVariations.None);
                        }
                        if (GetFailVariation() == FailVariations.PlayerFailedToReachTheDepot)
                        {
                            fUI.Scaleforms.MissionShard failShard = new fUI.Scaleforms.MissionShard();
                            failShard.ShardIn("HEIST FAILED", $"{PlayerNameStr} Failed To Reach The Depot", 6, 0.3f, 6, true);
                            failShard = null;
                            failShardJustShown = true;
                            Screen.FadeOut(1000);
                            justFailed = false;
                            SetFailVariation(FailVariations.None);
                        }
                        if (GetFailVariation() == FailVariations.PlayerAbandonedThePrologueVehicle)
                        {
                            fUI.Scaleforms.MissionShard failShard = new fUI.Scaleforms.MissionShard();
                            failShard.ShardIn("HEIST FAILED", $"{PlayerNameStr} Abandoned The Vehicle", 6, 0.3f, 6, true);
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
                            resetButtons.Buttons.Clear();
                            resetButtons.Update();
                            restartPending = true;
                            if (Start.Plane == null)
                                Screen.FadeOut(2500);
                            if (Screen.IsFadedOut)
                            {
                                LoadingPrompt.Show("NorthYanktonHeist");
                                restarting = true;
                                fWorld.Vehicle.DeleteList(depotVehicles);
                                fWorld.Camera.RenderScriptCams(false, false, 0);
                                resetCam?.Delete();
                                resetCam = null;
                                if (checkpoint == 1)
                                {
                                    fWorld.Weather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                                    fWorld.Clock.SetClockTime(15, 0, 0);
                                    fWorld.Interior.PrologueMap.UnloadYankton();
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
                                            if (!fPlayer.Player.Character.IsInVehicle(Start.Plane))
                                            {
                                                Start.Plane.IsPositionFrozen = true;
                                                Function.Call(Hash.SET_VEHICLE_ENGINE_ON, Start.Plane, true, true, true);
                                                fPlayer.Player.Character.SetIntoVehicle(Start.Plane, VehicleSeat.Driver);
                                                Wait(5000);
                                            }
                                            if (fPlayer.Player.Character.IsInVehicle(Start.Plane) && Start.Plane.IsEngineRunning && !Start.Plane.IsEngineStarting)
                                            {
                                                Start.Plane.IsPositionFrozen = false;
                                                Start.Plane.Speed = 40f;
                                                Start.Plane.LandingGearState = VehicleLandingGearState.Retracted;
                                                Start.Plane.Velocity = Start.Plane.ForwardVector * 40f;
                                                fPlayer.Player.Character.DryOff();
                                                fPlayer.Player.Character.ClearBloodDamage();
                                                fPlayer.Player.Character.ClearVisibleDamage();
                                                fPlayer.Player.Character.IsVisible = true;
                                                fPlayer.Player.Character.IsPositionFrozen = false;
                                                fPlayer.Player.Character.IsInvincible = false;
                                                fPlayer.Player.SetMaxWantedLevelToNormal();
                                                fUI.Hud.RadarAndHud(true, true);
                                                LoadingPrompt.Hide();
                                                Screen.FadeIn(1700);
                                            }
                                        }
                                        else
                                        {
                                            fPlayer.Player.PedPos(828.5366f, 3302.112f, 180f);
                                            Start.Plane = fWorld.Vehicle.Create(new Model("cuban800"), new Vector3(828.5366f, 3302.112f, 180f), 70f);
                                        }
                                        break;
                                    case 2:
                                        Wait(7500);
                                        //Globals.globalBlips = 2;
                                        //Globals.globalScripts = 2;
                                        fUI.Hud.ClearAllSubtitles();
                                        fUI.Hud.ClearBrief();
                                        fUI.Hud.ClearAllHelpMessages();
                                        fUI.Hud.ClearGPSMultiRoute();
                                        fUI.Hud.ClearHelp(true);
                                        fPlayer.Player.Character.DryOff();
                                        fPlayer.Player.Character.DryOff();
                                        fPlayer.Player.Character.ClearBloodDamage();
                                        fPlayer.Player.Character.ClearVisibleDamage();
                                        fPlayer.Player.Character.IsVisible = true;
                                        fPlayer.Player.Character.IsPositionFrozen = false;
                                        fPlayer.Player.Character.IsInvincible = false;
                                        fPlayer.Player.SetMaxWantedLevelToNormal();
                                        Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                                        Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                                        fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                        fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Idle);
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
                                        fUI.Hud.ClearAllSubtitles();
                                        fUI.Hud.ClearBrief();
                                        fUI.Hud.ClearAllHelpMessages();
                                        fUI.Hud.ClearGPSMultiRoute();
                                        fUI.Hud.ClearHelp(true);
                                        fPlayer.Player.Character.DryOff();
                                        fPlayer.Player.Character.ClearBloodDamage();
                                        fPlayer.Player.Character.ClearVisibleDamage();
                                        fPlayer.Player.Character.IsVisible = true;
                                        fPlayer.Player.Character.IsPositionFrozen = false;
                                        fPlayer.Player.Character.IsInvincible = false;
                                        fPlayer.Player.SetMaxWantedLevelToNormal();
                                        Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                                        Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                                        fAudio.Audio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                                        fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Suspense);
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
                            if (checkpoint == 1 && Start.Plane != null && fPlayer.Player.Character.IsInVehicle(Start.Plane) && Screen.IsFadedIn)
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
                            resetButtons.Buttons.Clear();
                            resetButtons.Update();
                            if (Screen.IsFadedOut)
                            {
                                LoadingPrompt.Show("NorthYanktonHeist");
                                restarting = true;
                                fWorld.Vehicle.DeleteList(depotVehicles);
                                fWorld.Camera.RenderScriptCams(false, false, 0);
                                resetCam?.Delete();
                                resetCam = null;
                                fPlayer.Player.PedPos(1746.312f, 3273.837f, 40.15277f, 30.6f);
                                instructionalButtonsSetUp = false;
                                fWorld.Ped.ApplyPedPropData(fPlayer.Player.Character, pedProps);
                                fWorld.Ped.ApplyPedVariationData(fPlayer.Player.Character, pedVariation);
                                fWorld.Weather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                                fWorld.Clock.SetClockTime(13, 30, 0);
                                fPlayer.Player.Character.DryOff();
                                fPlayer.Player.Character.ClearBloodDamage();
                                fPlayer.Player.Character.ClearVisibleDamage();
                                fPlayer.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                                fPlayer.Player.Character.IsVisible = true;
                                fPlayer.Player.Character.IsPositionFrozen = false;
                                fPlayer.Player.Character.IsInvincible = false;
                                fPlayer.Player.Character.Task.ClearAllImmediately();
                                Wait(4000);
                                Screen.FadeIn(5000);
                                Wait(2500);
                                fPlayer.Player.SetMaxWantedLevelToNormal();
                                ScriptManager.ScriptManager.RestartScripts();
                                //Globals.globalBlips = 3;
                                //Globals.globalScripts = 3;
                                fUI.Hud.RadarAndHud(true, true);
                                LoadingPrompt.Hide();
                                fWorld.Interior.PrologueMap.UnloadYankton();
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
                        resetSwitch = fWorld.Misc.GetRandomIntInRange(1, 3);
                    if (!PlayerTeleportedToPrologue && resetSwitch == 0)
                        resetSwitch = 1;
                    if (resetSwitch > 0)
                    {
                        if (!PlayerTeleportedToPrologue && !restarting)
                        {
                            if (!fWorld.Interior.PrologueMap.YankTon)
                            {
                                fWorld.Interior.PrologueMap.LoadYankton();
                            }
                            else
                            {
                                if (Screen.IsFadedOut)
                                {
                                    fPlayer.Player.PedPos(5304.088f, -5189.521f, 83.51835f, 0f);
                                    fWorld.Clock.SetClockTime(12, 0, 0);
                                    fPlayer.Player.Character.IsVisible = false;
                                    fPlayer.Player.Character.IsPositionFrozen = true;
                                    fPlayer.Player.Character.IsInvincible = true;
                                    fWorld.Weather.SetWeatherTypeNowPersist(fWorld.Weather.WeatherTypes.SnowLight);
                                    fWorld.Clock.PauseClock(true);
                                }
                            }
                        }
                        if (PlayerTeleportedToPrologue && !restarting)
                        {
                            if (!fWorld.Interior.PrologueMap.YankTon)
                            {
                                fWorld.Interior.PrologueMap.LoadYankton();
                            }
                            else
                            {
                                if (Screen.IsFadedOut)
                                {
                                    fPlayer.Player.PedPos(5304.088f, -5189.521f, 83.51835f, 0f);
                                    fPlayer.Player.Character.IsVisible = false;
                                    fPlayer.Player.Character.IsPositionFrozen = true;
                                    fPlayer.Player.Character.IsInvincible = true;
                                    fWorld.Clock.SetClockTime(5, 0, 0);
                                    fWorld.Weather.SetWeatherTypeNowPersist(fWorld.Weather.WeatherTypes.Snow);
                                    fWorld.Clock.PauseClock(true);
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
                                    resetCam = fWorld.Camera.CreateScriptedCam();
                                }
                                while (resetCam == null)
                                {
                                    Wait(0);
                                }
                                if (resetCam != null)
                                {
                                    fUI.Hud.RadarAndHud(false, false);
                                    resetCam.Position = resetCamCoord7;
                                    resetCam.Rotation = resetCamRot7;
                                    fWorld.Camera.SetCamFov(resetCam, 50f);
                                    resetCam.IsActive = true;
                                    fWorld.Camera.RenderScriptCams(true, false, 0);
                                    resetCam.Position = resetCamCoord7;
                                    resetCam.Rotation = resetCamRot7;
                                    if (!resetCam.IsShaking)
                                        fWorld.Camera.ShakeCam(resetCam, "HAND_SHAKE", 0.5f);
                                }
                                break;
                            case 2:
                                if (resetCam == null)
                                {
                                    resetCam = fWorld.Camera.CreateScriptedCam();
                                }
                                while (resetCam == null)
                                {
                                    Wait(0);
                                }
                                if (resetCam != null)
                                {
                                    fUI.Hud.RadarAndHud(false, false);
                                    resetCam.Position = resetCamCoord3;
                                    resetCam.Rotation = resetCamRot3;
                                    fWorld.Camera.SetCamFov(resetCam, 60f);
                                    resetCam.IsActive = true;
                                    fWorld.Camera.RenderScriptCams(true, false, 0);
                                    resetCam.Position = resetCamCoord3;
                                    resetCam.Rotation = resetCamRot3;
                                    if (!resetCam.IsShaking)
                                        fWorld.Camera.ShakeCam(resetCam, "HAND_SHAKE", 0.5f);
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
                                resetButtons.Buttons.Add(GoBackToFreeroamContainer);
                                resetButtons.Buttons.Add(ResetButtonContainer);
                                instructionalButtonsSetUp = true;
                            }
                            else
                            {
                                resetButtons.Draw();
                                Game.DisableAllControlsThisFrame();
                            }
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

        void CleanUp()
        {
            bool flag = true;
            if (true == flag)
            {
                fAudio.Audio.StopAudioScene("MI_1_TREV_FLY_TO_LUDENDORFF");
                fAudio.Audio.StopAudioScene("MI_1_MIC_DRIVE_TO_GRAVEYARD");
                if (outfitSaved)
                {
                    fWorld.Ped.ApplyPedPropData(fPlayer.Player.Character, pedProps);
                    fWorld.Ped.ApplyPedVariationData(fPlayer.Player.Character, pedVariation);
                }
                if (weatherTypeSaved)
                    fWorld.Weather.SetWeatherTypeNowPersist(weatherTypeBeforeMission);
                fWorld.Clock.PauseClock(false);
                fWorld.Interior.PrologueMap.EnableNorthYanktonTrainTracks(false);
                fCore.Streaming.SetMapDataCullboxEnabled("prologue", false);
                fCore.Streaming.SetMapDataCullboxEnabled("Prologue_Main", false);
                fWorld.Zone.SetZoneEnabled(fWorld.Zone.GetZoneFromNameID("Prol"), false);
                fAudio.Audio.ChangeMusicEventIntensity(fAudio.Audio.MusicEventIntensity.Fail);
                fWorld.Object.DeleteObjectsInList(boothProps);
                fWorld.Vehicle.DeleteList(depotVehicles);
                fCombat.CombatRegistry.DisposeAll();
                VaultBlip?.Delete();
                VaultBlip = null;
                InsideBlip?.Delete();
                InsideBlip = null;
                Garagecolobject?.Delete();
                Garagecolobject = null;
                BoothGuard?.Delete();
                BoothGuard = null;
                PrologueIntroCam?.Delete();
                PrologueIntroCam = null;
                PrologueIntroCam2?.Delete();
                PrologueIntroCam2 = null;
                DepotBlip?.Delete();
                DepotBlip = null;
                if (PrologueVehicle != null)
                {
                    PrologueVehicle.AttachedBlip?.Delete();
                    PrologueVehicle.MarkAsNoLongerNeeded();
                    PrologueVehicle = null;
                }
                LudendorffNorthYankton?.Delete();
                LudendorffNorthYankton = null;
                if (Plane != null)
                {
                    Plane.AttachedBlip?.Delete();
                    Plane.MarkAsNoLongerNeeded();
                    Plane = null;
                }
                AnimGuard?.Delete();
                AnimGuard = null;
                AnimDoor?.Delete();
                AnimDoor = null;
                AnimCam?.Delete();
                AnimCam = null;

            }
        }

        private void onShutdown(object sender, EventArgs e)
        {
            CleanUp();
        }
    }
}