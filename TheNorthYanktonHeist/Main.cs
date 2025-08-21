using BillsyLiamGTA.UI.Elements;
using BillsyLiamGTA.UI.Scaleform;
using BillsyLiamGTA.UI.Timerbars;
using TheNorthYanktonHeist;
using Global;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using static BillsyLiamGTA.UI.Elements.VariableTimer;
using static TheNorthYanktonHeist.Functions;
using Hash = GTA.Native.Hash;
using Screen = GTA.UI.Screen;

public static class Globals
{
    public static int missionSwitch = 0;

    public static int globalBlips = 3;
    public static int scriptTerminator = 3;
    public static int Debug2 = 0;
    public static int Debug = -1;
    public static bool debug = true;
    public static int integer1 = 0;
    public static int switch1 = 0;
    public static int sceneID1 = 0;
    public static bool boolean1 = false;
    public static int[] intStorage = new int[100];
}

namespace Global
{
    using static Functions;
    using static Globals;

    public class GlobalsController : Script
    {
        public GlobalsController(params Blip[] blipsToExclude)
        {
            if (blipsToExclude != null && blipsToExclude.Length > 0)
            {
                for (int i = 0; i < blipsToExclude.Length; i++)
                {
                    if (blipsToExclude[i] != null)
                    {
                        excludeBlips.Add(blipsToExclude[i]);
                    }
                }
            }
            else
            {
                return;
            }
        }
        public GlobalsController()
        {
            Tick += onTick;
            Aborted += onShutdown;
            KeyDown += onKeyDown;
            foreach (string value in RemoveOnlyIPLS)
            {
                fInterior.RemoveIpl(value);
            }
            foreach (string value2 in LoadAllIPLS)
            {
                fInterior.RemoveIpl(value2);
                fInterior.RequestIpl(value2);
            }
        }

        List<Blip> excludeBlips = new List<Blip>();

        static fCutsceneCreation debugCutscene = null;
        static Vehicle debugHeli;
        static Ped cutscenePed1;
        static Ped cutscenePed2;
        static Ped cutscenePed3;
        static Blip debugBlip;
        static Camera debugCam;
        int scaleID = 0;

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (debug)
            {
                if (e.KeyCode == Keys.N)
                {
                    //fInterior.PrologueMap.EnableYanktonTraffic = true;
                    missionSwitch = 3;
                    //Debug2 = 20;
                    //Function.Call(Hash.ACTIVATE_​FRONTEND_​MENU, "DISPLAY_CORONA_BUTTONS", false, -1);
                    //HUD::PAUSE_MENU_ACTIVATE_CONTEXT(joaat("DISPLAY_CORONA_BUTTONS"));
                    //Function.Call(Hash.PAUSE_MENU_ACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_SHOW_PLANNING"));
                    /*
                    Function.Call(Hash.PAUSE_MENU_DEACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_SELECT"));
                    Function.Call(Hash.PAUSE_MENU_DEACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_ZOOM"));
                    Function.Call(Hash.PAUSE_MENU_DEACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_MAP_AVAIL"));
                    Function.Call(Hash.PAUSE_MENU_DEACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_MAP_CLOSE"));
                    Function.Call(Hash.PAUSE_MENU_DEACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_VIEW_VSTAT"));
                    Function.Call(Hash.PAUSE_MENU_DEACTIVATE_CONTEXT, fMisc.GetHashKey("CORONA_VIEW_VMODS"));
                    
                    //Debug2++;
                    //missionSwitch = 2;
                    /*
                    Blip[] allBlips = World.GetAllBlips();
                    if (allBlips.Length > 0)
                    {
                        foreach (Blip blip in allBlips)
                        {
                            if (blip != null && blip.Sprite == BlipSprite.Standard)
                            {
                                blip.Delete();
                            }
                        }
                    }*/
                }
                if (e.KeyCode == Keys.NumPad7)
                {
                    Screen.FadeIn(0);
                    //fDebugStuff.CopyPlayerPosWithAddons();
                }
                if (e.KeyCode == Keys.NumPad9)
                {
                    fDebugStuff.CopyToClipboard(fPlayer.ped.Heading.ToString() + "f");
                }
                Entity[] anyEntity = World.GetAllEntities();
                foreach (Entity entity in anyEntity)
                {
                    if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, entity) && e.KeyCode == Keys.B)
                    {
                        Vector3 vector = entity.Position;
                        string vectorX = vector.X.ToString();
                        string vectorY = vector.Y.ToString();
                        string vectorZ = vector.Z.ToString();
                        string XYZ = vector.ToString().Replace("X:", string.Empty).Replace("Y:", string.Empty).Replace("Z:", string.Empty);
                        string Text2 = XYZ.Replace(vectorX, vectorX + "f,");
                        string Text3 = Text2.Replace(vectorY, vectorY + "f,");
                        string Final = Text3.Replace(vectorZ, vectorZ + "f");
                        fDebugStuff.CopyToClipboard(Final);
                    }

                    if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, entity) && e.KeyCode == Keys.L)
                    {
                        float heading = entity.Heading;
                        string heading2 = heading + "f";
                        string Final = heading2;
                        fDebugStuff.CopyToClipboard(Final);
                    }

                    if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, entity) && e.KeyCode == Keys.I)
                    {
                        Vector3 rotation = entity.Rotation;
                        string rotationX = rotation.X.ToString();
                        string rotationY = rotation.Y.ToString();
                        string rotationZ = rotation.Z.ToString();
                        string XYZ = rotation.ToString().Replace("X:", string.Empty).Replace("Y:", string.Empty).Replace("Z:", string.Empty);
                        string Text2 = XYZ.Replace(rotationX, rotationX + "f,");
                        string Text3 = Text2.Replace(rotationY, rotationY + "f,");
                        string Final = Text3.Replace(rotationZ, rotationZ + "f");
                        fDebugStuff.CopyToClipboard(Final);
                    }
                }
            }
        }

        private void onShutdown(object sender, EventArgs e)
        {
            bool flag = true;
            if (true == flag)
            {
                if (debugBlip != null)
                {
                    debugBlip.Delete();
                    debugBlip = null;
                }
                if (debugHeli != null)
                {
                    debugHeli.Delete();
                    debugHeli = null;
                }
                if (cutscenePed1 != null)
                {
                    cutscenePed1.Delete();
                    cutscenePed1 = null;
                }
                if (cutscenePed2 != null)
                {
                    cutscenePed2.Delete();
                    cutscenePed2 = null;
                }
                if (cutscenePed3 != null)
                {
                    cutscenePed3.Delete();
                    cutscenePed3 = null;
                }
                if (debugCam != null)
                {
                    debugCam.Delete();
                    debugCam = null;
                }
                fVehicle.DeleteVehiclesInList(fDebugStuff.DebugVehicles);
                fStreaming.RemoveAnimDict(fDebugStuff.DebugAnimDicts);
                fProp.DeletePropsInList(fDebugStuff.DebugProps);
                fBlip.ToggleShortRangeForLongRangeBlips(false);
                if (Screen.IsFadedOut || Screen.IsFadingOut)
                    Screen.FadeIn(0);
                fHud.ClearAllHelpMessages();
                fHud.ClearBrief();
                fHud.ClearPrints();
                fHud.ClearHelp(true);
                fHud.ClearGPSMultiRoute();
                fPlayer.SetWantedLevelTo0();
                fPlayer.FakeWantedLevel = 0;
                fPlayer.SetMaxWantedLevelToNormal();
                fHud.RadarAndHud(true, true);
                fGraphics.AnimpostFXStopAll();
                GlobalVariable.Get(5).Write<int>(0);
                LoadingPrompt.Hide();
                Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
                Function.Call(Hash.REMOVE_CUTSCENE);
                Game.Player.Character.IsCollisionEnabled = true;
                Game.Player.Character.IsPositionFrozen = false;
                Game.Player.Character.CanSwitchWeapons = true;
                Game.Player.Character.Task.ClearAll();
                Game.Player.Character.IsVisible = true;
                Game.Player.SetControlState(true);
                Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
                Function.Call(Hash.PAUSE_CLOCK, false);
                Function.Call(Hash.SET_TIME_SCALE, 1f);
                Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
                Function.Call(Hash.STOP_AUDIO_SCENES);
                Function.Call(Hash.STOP_PLAYER_SWITCH);
                Function.Call(Hash.SET_WIDESCREEN_BORDERS, false, 0);
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 0, false);
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 1, false);
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 2, false);
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 3, false);
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 4, false);
                Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 5, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 0, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 1, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 2, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 3, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 4, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 5, false);
                Function.Call(Hash.DISABLE_POLICE_RESTART, 6, false);
                Function.Call(Hash.TOGGLE_PAUSED_RENDERPHASES, true);
                Function.Call(Hash.TRIGGER_SCREENBLUR_FADE_OUT, 0f);
                Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
                Function.Call(Hash.SET_HIDOF_OVERRIDE, 0, 0, 0f, 0f, 0f, 0f);
                Function.Call(Hash.FORCE_CLOSE_TEXT_INPUT_BOX);
                Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 0f);
                if (Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS))
                    Function.Call(Hash.STOP_PLAYER_SWITCH);
                Function.Call<Camera>(Hash.GET_RENDERING_CAM).Delete();
                Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 3000, true, false, 0);
                Function.Call(Hash.DESTROY_ALL_CAMS, false);
            }
        }

        Vector3 resetCamCoord7 = new Vector3(5355f, -5173f, 82.49858f);
        Vector3 resetCamRot7 = new Vector3(17f, 0f, 130.4812f);
        Vector3 resetCamCoord = new Vector3(5344.149f, -5180.952f, 83.3f);
        Vector3 resetCamRot = new Vector3(16f, 0f, 130.6f);
        Vector3 resetCamCoord2 = new Vector3(5315.703f, -5175.806f, 84.61874f);
        Vector3 resetCamRot2 = new Vector3(-5f, 0f, -160f);
        Vector3 resetCamCoord3 = new Vector3(5346.801f, -5178.461f, 82.4f);
        Vector3 resetCamRot3 = new Vector3(15f, 0f, 129.449f);
        Vector3 resetCamCoord4 = new Vector3(5302.341f, -5176.98f, 84.81873f);
        Vector3 resetCamRot4 = new Vector3(-12f, 0f, 60f);
        Vector3 resetCamCoord5 = new Vector3(5292.429f, -5184.105f, 85.01872f);
        Vector3 resetCamRot5 = new Vector3(-10f, 0f, -140f);
        Vector3 resetCamCoord6 = new Vector3(5306.128f, -5213.86f, 85.11872f);
        Vector3 resetCamRot6 = new Vector3(-10f, 0f, -46.8f);
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

        private void onTick(object sender, EventArgs e)
        {
            // Driveway to the depot: 
            // Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, fPlayer.ped, 5356.7324f, -5201.1553f, 80.83122f, 5356.5454f, -5179.6f, 96.83691f, 20f, false, true, 0) || Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, fPlayer.ped, 5417.894f, -5108.7925f, 75.56319f, 5412.488f, -5240.66f, 95.59789f, 100f, false, true, 0)
            // Depot:
            // fEntity.IsEntityInArea(fPlayer.ped, new Vector3(5364.804f, -5158.941f, 79f), new Vector3(5283.942f, -5229.462f, 89f))
            if (debugBlip != null)
                fBlip.SetBlipRotation(debugBlip, fMath.CEIL(0f));
            if (!ScriptSetup)
            {
                if (!Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE) && !Screen.IsFadingIn)
                {
                    Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                    ScriptSetup = true;
                }
            }
            else
            {
                switch (Debug2)
                {
                    case 0:
                        break;
                    case 1: // arrive at depot, depot shootout
                        if (!fInterior.PrologueMap.IsPrologueMapLoaded)
                        {
                            fInterior.PrologueMap.LoadPrologueMap();
                        }
                        else
                        {
                            fPlayer.PedPos(5304.088f, -5189.521f, 83.51835f, 0f);
                            fClock.SetClockTime(12, 0, 0);
                            fPlayer.ped.IsVisible = false;
                            fPlayer.ped.IsPositionFrozen = true;
                            fPlayer.ped.IsInvincible = true;
                            fWeather.SetOverrideWeather(WeatherTypes.SNOWLIGHT);
                            fClock.PauseClock(true);
                            fInterior.PrologueMap.EnableNorthYanktonTrains(true);
                            fPathfind.SetAllowStreamPrologueNodes(true);
                            fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                            fHud.ToggleNorthYanktonMap(true);
                        }
                        SpawnTrucks();
                        if (debugCam == null)
                        {
                            debugCam = fCam.CreateScriptedCam();
                        }
                        while (debugCam == null)
                        {
                            Wait(0);
                        }
                        if (debugCam != null)
                        {
                            fHud.RadarAndHud(false, false);
                            debugCam.Position = resetCamCoord7;
                            debugCam.Rotation = resetCamRot7;
                            fCam.SetCamFov(debugCam, 50f);
                            debugCam.IsActive = true;
                            fCam.RenderScriptCams(true, false, 0);
                            debugCam.Position = resetCamCoord7;
                            debugCam.Rotation = resetCamRot7;
                            if (!debugCam.IsShaking)
                                fCam.ShakeCam(debugCam, "HAND_SHAKE", 0.5f);
                        }
                        break;
                        /*
                    case 2:
                        if (debugCam == null)
                        {
                            debugCam = fCam.CreateScriptedCam();
                        }
                        while (debugCam == null)
                        {
                            Wait(0);
                        }
                        if (debugCam != null)
                        {
                            fHud.RadarAndHud(false, false);
                            debugCam.Position = resetCamCoord2;
                            debugCam.Rotation = resetCamRot2;
                            fCam.SetCamFov(debugCam, 45f);
                            debugCam.IsActive = true;
                            fCam.RenderScriptCams(true, false, 0);
                            debugCam.Position = resetCamCoord2;
                            debugCam.Rotation = resetCamRot2;
                            if (!debugCam.IsShaking)
                                fCam.ShakeCam(debugCam, "HAND_SHAKE", 0.5f);
                        }
                        break;
                        */
                    case 3:
                        if (debugCam == null)
                        {
                            debugCam = fCam.CreateScriptedCam();
                        }
                        while (debugCam == null)
                        {
                            Wait(0);
                        }
                        if (debugCam != null)
                        {
                            fHud.RadarAndHud(false, false);
                            debugCam.Position = resetCamCoord3;
                            debugCam.Rotation = resetCamRot3;
                            fCam.SetCamFov(debugCam, 60f);
                            debugCam.IsActive = true;
                            fCam.RenderScriptCams(true, false, 0);
                            debugCam.Position = resetCamCoord3;
                            debugCam.Rotation = resetCamRot3;
                            if (!debugCam.IsShaking)
                                fCam.ShakeCam(debugCam, "HAND_SHAKE", 0.5f);
                        }
                        break;
                        /*
                    case 4:
                        if (debugCam == null)
                        {
                            debugCam = fCam.CreateScriptedCam();
                        }
                        while (debugCam == null)
                        {
                            Wait(0);
                        }
                        if (debugCam != null)
                        {
                            fHud.RadarAndHud(false, false);
                            debugCam.Position = resetCamCoord4;
                            debugCam.Rotation = resetCamRot4;
                            fCam.SetCamFov(debugCam, 55f);
                            debugCam.IsActive = true;
                            fCam.RenderScriptCams(true, false, 0);
                            debugCam.Position = resetCamCoord4;
                            debugCam.Rotation = resetCamRot4;
                            if (!debugCam.IsShaking)
                                fCam.ShakeCam(debugCam, "HAND_SHAKE", 0.5f);
                        }
                        break;
                        
                    case 5:
                        if (debugCam == null)
                        {
                            debugCam = fCam.CreateScriptedCam();
                        }
                        while (debugCam == null)
                        {
                            Wait(0);
                        }
                        if (debugCam != null)
                        {
                            fHud.RadarAndHud(false, false);
                            debugCam.Position = resetCamCoord5;
                            debugCam.Rotation = resetCamRot5;
                            fCam.SetCamFov(debugCam, 42f);
                            debugCam.IsActive = true;
                            fCam.RenderScriptCams(true, false, 0);
                            debugCam.Position = resetCamCoord5;
                            debugCam.Rotation = resetCamRot5;
                            if (!debugCam.IsShaking)
                                fCam.ShakeCam(debugCam, "HAND_SHAKE", 0.5f);
                        }
                        break;
                        
                    case 6:
                        if (debugCam == null)
                        {
                            debugCam = fCam.CreateScriptedCam();
                        }
                        while (debugCam == null)
                        {
                            Wait(0);
                        }
                        if (debugCam != null)
                        {
                            fHud.RadarAndHud(false, false);
                            debugCam.Position = resetCamCoord6;
                            debugCam.Rotation = resetCamRot6;
                            fCam.SetCamFov(debugCam, 55f);
                            debugCam.IsActive = true;
                            fCam.RenderScriptCams(true, false, 0);
                            debugCam.Position = resetCamCoord6;
                            debugCam.Rotation = resetCamRot6;
                            if (!debugCam.IsShaking)
                                fCam.ShakeCam(debugCam, "HAND_SHAKE", 0.5f);
                        }
                        break;
                        */
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                }
                switch (Debug)
                {
                    case 0:
                        if (cutscenePed1 == null)
                        {
                            cutscenePed1 = fPed.CreatePed("mp_m_freemode_01", new Vector3(1706.324f, 7991.369f, 360.9091f), 5.3f);
                        }
                        else
                        {
                            cutscenePed1.IsPositionFrozen = true;
                            cutscenePed1.IsVisible = false;
                        }
                        if (cutscenePed2 == null)
                        {
                            cutscenePed2 = fPed.CreatePed("mp_m_freemode_01", new Vector3(1706.324f, 7991.369f, 360.9091f), 5.3f);
                        }
                        else
                        {
                            cutscenePed2.IsPositionFrozen = true;
                            cutscenePed2.IsVisible = false;
                        }
                        if (cutscenePed3 == null)
                        {
                            cutscenePed3 = fPed.CreatePed("mp_m_freemode_01", new Vector3(1706.324f, 7991.369f, 360.9091f), 5.3f);
                        }
                        else
                        {
                            cutscenePed3.IsPositionFrozen = true;
                            cutscenePed3.IsVisible = false;
                        }
                        if (debugHeli == null)
                        {
                            debugHeli = fVehicle.CreateVehicle("buzzard", new Vector3(1706.324f, 7991.369f, 360.9091f), 5.3f);
                        }
                        else
                        {
                            if (!fVehicle.IsVehicleWeaponDisabled(fMisc.GetHashKey("VEHICLE_WEAPON_PLAYER_BUZZARD"), debugHeli, fPlayer.ped))
                            {
                                fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLAYER_BUZZARD"), debugHeli, fPlayer.ped);
                            }
                            debugHeli.IsPositionFrozen = true;
                        }
                        Debug++;
                        break;
                    case 1:
                        debugCutscene = new fCutsceneCreation("mph_pri_fin_ext", new Vector3(1706.324f, 7991.369f, 360.9091f), new Vector3(0f, 0f, 5.3f), false);
                        debugCutscene.AddRegisterEntityToList(cutscenePed1, "MP_1", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                        debugCutscene.AddRegisterEntityToList(Game.Player.Character, "MP_2", fCutscene.CutsceneUsage.CU_ANIMATE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                        debugCutscene.AddRegisterEntityToList(cutscenePed2, "MP_3", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                        debugCutscene.AddRegisterEntityToList(cutscenePed3, "MP_4", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                        debugCutscene.AddRegisterEntityToList(debugHeli, "MPH_Helicopter", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                        debugCutscene.StartCutscene();
                        while (!fCutscene.IsCutscenePlaying())
                        {
                            Wait(0);
                        }
                        Screen.FadeIn(1000);
                        while (!fCutscene.HasCutsceneFinished())
                        {
                            fCutscene.SetCutsceneOriginAndRotation("mph_pri_fin_ext", new Vector3(debugCutscene.Pos.X, debugCutscene.Pos.Y, debugCutscene.Pos.Z), new Vector3(debugCutscene.Rot.X, debugCutscene.Rot.Y, debugCutscene.Rot.Z));
                            debugHeli.ToggleExtra(8, false);
                            debugHeli.ToggleExtra(9, false);
                            cutscenePed1.IsVisible = false;
                            cutscenePed2.IsVisible = false;
                            cutscenePed3.IsVisible = false;
                            Wait(0);
                        }
                        debugCutscene.Cleanup();
                        debugCutscene = null;
                        cutscenePed1.Delete();
                        cutscenePed1 = null;
                        cutscenePed2.Delete();
                        cutscenePed2 = null;
                        cutscenePed3.Delete();
                        cutscenePed3 = null;
                        debugHeli.Delete();
                        debugHeli = null;
                        Game.Player.Character.Task.ClearAll();
                        Debug++;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
                switch (globalBlips)
                {
                    case 0:
                        break;
                    case 1:
                        fBlip.SetMostBlipsInvisible(excludeBlips);
                        break;
                    case 2:
                        fBlip.SetAllBlipsInvisible(excludeBlips);
                        break;
                    case 3:
                        fBlip.SetAllBlipsVisible(excludeBlips);
                        globalBlips = 0;
                        break;
                }
                switch (scriptTerminator) // Does not work on OnShutdown.
                {
                    case 0:
                        break;
                    case 1:
                        fInGameScripts.TerminateScriptsExeptMissions();
                        break;
                    case 2:
                        fInGameScripts.TerminateScriptsWithMissions();
                        break;
                    case 3:
                        fInGameScripts.StartAllNeededToStartScripts();
                        scriptTerminator = 0;
                        break;
                }
            }
        }

        public static bool ScriptSetup = false;

        public static List<string> LoadAllIPLS = new List<string>
        {
            "cs1_02_cf_onmission1",
            "cs1_02_cf_onmission2",
            "cs1_02_cf_onmission3",
            "cs1_02_cf_onmission4",
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
            "xm3_warehouse_lod"
        };

        public static List<string> RemoveOnlyIPLS = new List<string>
        {
            "cs1_02_cf_offmission",
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
}

namespace TheNorthYanktonHeist
{
    public class Start : Script
    {
        public Start()
        {
            Tick += OnTick;
            Aborted += OnShutdown;
        }
        Vector3 HeliPos = new Vector3(1600.979f, 6623.252f, 15f);
        float HeliHeading = 5.047073f;
        public static Blip HeliBlip;
        public static Vehicle StartHeli;
        bool WasDestroyed 
        { 
            get 
            {
                if (StartHeli != null)
                    return IsStartHeliDestroyed;
                else
                    return false;
            }
        }
        bool SpawnedIn 
        { 
            get 
            {
                return StartHeli != null;
            } 
        }
        public static bool fixedPosition = false;
        bool colorSet = false;
        bool IsStartHeliDestroyed
        {
            get
            {
                if (StartHeli != null && StartHeli.IsConsideredDestroyed)
                    return true;
                if (StartHeli != null && !StartHeli.IsConsideredDestroyed)
                    return false;
                return true;
            }
        }
        int startSwitch = 0;
        int num;

        private void OnTick(object sender, EventArgs e)
        {
            if (Globals.missionSwitch == 0)
            {
                switch (startSwitch)
                {
                    case 0:
                        if (HeliBlip == null && fPlayer.GetDistanceTo(HeliPos) > 500f && !WasDestroyed && !SpawnedIn)
                        {
                            HeliBlip = fBlip.CreateBlipForCoordWithParams(HeliPos, (BlipSprite)64, (BlipColor)5, 1f, "The North Yankton Heist");
                        }
                        if (HeliBlip != null)
                        {
                            if (StartHeli == null)
                            {
                                if (fPlayer.GetDistanceTo(HeliPos) > 200f)
                                {
                                    if (fPlayer.GetDistanceTo(HeliPos) < 300f)
                                    {
                                        num = fMisc.GetRandomIntInRange(1, 8);
                                        StartHeli = fVehicle.CreateVehicle("buzzard", HeliPos, HeliHeading);
                                    }
                                }

                            }
                        }
                        if (StartHeli != null)
                        {
                            if (!IsStartHeliDestroyed)
                            {
                                if (!fEntity.HasCollisionLoadedAroundEntity(StartHeli))
                                {
                                    fEntity.SetEntityLoadCollisionFlag(StartHeli, true);
                                }
                                if (!fixedPosition)
                                {
                                    fVehicle.SetVehicleOnGroundProperly(StartHeli);
                                    fixedPosition = true;
                                }
                                if (StartHeli.IsExtraOn(8) || StartHeli.IsExtraOn(9))
                                {
                                    StartHeli.ToggleExtra(8, false);
                                    StartHeli.ToggleExtra(9, false);
                                }
                                if (!colorSet)
                                {
                                    switch (num)
                                    {
                                        case 1:
                                            fVehicle.SetVehicleColours(StartHeli, 111, 111);
                                            colorSet = true;
                                            break;
                                        case 2:
                                            fVehicle.SetVehicleColours(StartHeli, 8, 8);
                                            colorSet = true;
                                            break;
                                        case 3:
                                            fVehicle.SetVehicleColours(StartHeli, 99, 99);
                                            colorSet = true;
                                            break;
                                        case 4:
                                            fVehicle.SetVehicleColours(StartHeli, 69, 69);
                                            colorSet = true;
                                            break;
                                        case 5:
                                            fVehicle.SetVehicleColours(StartHeli, 90, 90);
                                            colorSet = true;
                                            break;
                                        case 6:
                                            fVehicle.SetVehicleColours(StartHeli, 33, 33);
                                            colorSet = true;
                                            break;
                                        case 7:
                                            fVehicle.SetVehicleColours(StartHeli, 147, 147);
                                            colorSet = true;
                                            break;
                                    }
                                }
                                if (HeliBlip != null && StartHeli.AttachedBlip == null)
                                {
                                    StartHeli.AddBlip();
                                }
                                if (StartHeli.AttachedBlip != null)
                                {
                                    StartHeli.AttachedBlip.Alpha = 255;
                                    StartHeli.AttachedBlip.Sprite = (BlipSprite)64;
                                    StartHeli.AttachedBlip.Color = (BlipColor)5;
                                    StartHeli.AttachedBlip.Name = "The North Yankton Heist";
                                    if (HeliBlip != null)
                                        HeliBlip.DisplayType = BlipDisplayType.NoDisplay;
                                    if (HeliBlip.DisplayType == BlipDisplayType.NoDisplay)
                                        startSwitch = 1;
                                }
                            }
                        }
                        break;
                    case 1:
                        if (fPlayer.GetDistanceTo(StartHeli.Position) < 6f)
                        {
                            if (!fPlayer.ped.IsEnteringVehicle || !fPlayer.ped.IsSittingInVehicle(StartHeli) && !fPlayer.IsWanted)
                                fHud.DisplayHelpText("Press ~INPUT_ENTER~ to start The North Yankton Heist.");
                            if (fPlayer.IsWanted)
                                fHud.DisplayHelpText("Lose the cops before starting the heist.");
                            if (fPlayer.ped.IsEnteringVehicle || fPlayer.ped.IsSittingInVehicle(StartHeli))
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
                                fHud.ClearAllPrints();
                                fHud.ClearBrief();
                                fHud.ClearAllHelpMessages();
                                fHud.ClearGPSMultiRoute();
                                fHud.ClearHelp(true);
                                fPlayer.ped.Task.ClearAll();
                                fPlayer.ped.Task.EnterVehicle(StartHeli, VehicleSeat.Driver);
                                startSwitch = 2;
                            }
                        }
                        break;
                    case 2:
                        if (fPlayer.ped.CurrentVehicle == StartHeli)
                        {
                            fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLAYER_BUZZARD"), StartHeli, fPlayer.ped);
                            fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLANE_ROCKET"), StartHeli, fPlayer.ped);
                            if (HeliBlip != null)
                            {
                                HeliBlip.Delete();
                                HeliBlip = null;
                            }
                            StartHeli.AttachedBlip.Color = BlipColor.Blue;
                            StartHeli.AttachedBlip.Alpha = 0;
                            startSwitch = 0;
                            fixedPosition = false;
                            Globals.missionSwitch = 1;
                        }
                        else
                        {
                            StartHeli.AttachedBlip.Alpha = 255;
                        }
                        break;
                }
                if (HeliBlip != null)
                {
                    if (!SpawnedIn)
                    {
                        HeliBlip.DisplayType = BlipDisplayType.NoDisplay;
                    }
                    if (!SpawnedIn && fPlayer.GetDistanceTo(HeliPos) > 200f)
                    {
                        HeliBlip.DisplayType = BlipDisplayType.Default;
                    }
                    if (StartHeli != null && StartHeli.IsConsideredDestroyed)
                    {
                        if (StartHeli.AttachedBlip != null)
                        {
                            StartHeli.AttachedBlip.Delete();
                        }
                        HeliBlip.DisplayType = BlipDisplayType.NoDisplay;
                        startSwitch = 0;
                    }
                }
                if (StartHeli != null)
                {
                    if (fPlayer.GetDistanceTo(StartHeli.Position) > 440f)
                    {
                        if (StartHeli.AttachedBlip != null)
                        {
                            StartHeli.AttachedBlip.Delete();
                        }
                        StartHeli.Delete();
                        StartHeli = null;
                        startSwitch = 0;
                    }
                }
            }
        }

        void CleanUp()
        {
            if (HeliBlip != null)
            {
                HeliBlip.Delete();
                HeliBlip = null;
            }
            if (StartHeli != null)
            {
                if (StartHeli.AttachedBlip != null)
                {
                    StartHeli.AttachedBlip.Delete();
                }
                StartHeli.Delete();
                StartHeli = null;
            }
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
            Tick += onTick; ;
            Aborted += onShutdown;
            failTimer.OnTimerExpired += OnFailTimerExpired;
        }

        private void OnFailTimerExpired(object sender)
        {
            if (!fInterior.PrologueMap.IsPrologueMapLoaded)
            {
                if (!PlayerInArea)
                {
                    justFailed = true;
                    if (GetFailVariation() != FailVariations.LeftMissionArea)
                        SetFailVariation(FailVariations.LeftMissionArea);
                    MissionFailCleanUpRequired = true;
                }
            }
            else
            {
                if (fPlayer.ped.CurrentVehicle != PrologueVehicle)
                {
                    justFailed = true;
                    if (GetFailVariation() != FailVariations.PrologueVehicleLeft_JobBlown)
                        SetFailVariation(FailVariations.PrologueVehicleLeft_JobBlown);
                    MissionFailCleanUpRequired = true;
                }
            }
        }

        Vehicle StartHeli;
        Blip LudendorffNorthYankton;
        Blip FailArea;
        bool FailCountdownVisible = false;
        bool PlayerInArea = false;
        bool WarningMessageShown = false;
        public static bool PlayerTeleportedToPrologue = false;
        bool IsStartHeliDestroyed
        {
            get
            {
                if (StartHeli != null && StartHeli.IsConsideredDestroyed)
                    return true;
                if (StartHeli != null && !StartHeli.IsConsideredDestroyed)
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
        private static int FailVariation
        {
            get;
            set;
        }
        public static void SetFailVariation(FailVariations failVariation)
        {
            FailVariation = (int)failVariation;
        }
        public static FailVariations GetFailVariation()
        {
            return (FailVariations)FailVariation;
        }
        public enum FailVariations
        {
            None,
            LeftMissionArea,
            HelicopterDestroyed,
            PrologueVehicleDestroyed,
            PrologueVehicleLeft_JobBlown,
        }
        VariableTimer failTimer = new VariableTimer(30000);
        CountdownTimerbar failCountdown;
        Vehicle PrologueVehicle;
        string PrologueVehicleName;
        Camera PrologueIntroCam;
        Camera PrologueIntroCam2;
        int num;
        Blip DepotBlip;
        bool justFailed = false;
        WeatherTypes weatherTypeBeforeMission;
        bool weatherTypeSaved = false;
        bool timeAdvanced1 = false;
        bool timeAdvanced2 = false;
        int iVar0;

        private void onTick(object sender, EventArgs e)
        {
            switch (Globals.missionSwitch)
            {
                case 1:
                    if (!weatherTypeSaved)
                    {
                        weatherTypeBeforeMission = fWeather.GetCurrentWeatherEnum();
                        weatherTypeSaved = true;
                    }
                    fHud.ClearAllPrints();
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.IdleStart);
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                    Wait(100);
                    fMissionShard missionShard = new fMissionShard();
                    missionShard.Shard_In("~a~The North Yankton Heist~w~", "Break into the ~y~Bobcat Security Depot~w~ in Ludendorff and clean out the ~g~vault.~w~", 2, 0.375f);
                    missionShard = null;
                    Globals.missionSwitch = 2;
                    break;
                case 2:
                    fHud.HideHudComponentThisFrame((int)HudComponent.HelpText);
                    fHud.DisplayHelpText("");
                    new GlobalsController(null);
                    Globals.globalBlips = 2;
                    Globals.scriptTerminator = 2;
                    if (fEntity.IsEntityInArea(fPlayer.ped, new Vector3(2000f, 6205f, -7f), new Vector3(1084f, 8888f, 550f)) && fPlayer.ped.CurrentVehicle == StartHeli)
                    {
                        if (fPlayer.IsWanted)
                            Screen.ShowSubtitle("Lose the Cops");
                        else
                            Screen.ShowSubtitle("Fly to ~y~Ludendorff, North Yankton~w~");
                    }
                    if (!fEntity.IsEntityInArea(fPlayer.ped, new Vector3(2000f, 6205f, -7f), new Vector3(1084f, 8888f, 550f)))
                        Screen.ShowSubtitle("Return to the mission area.");
                    if (fPlayer.ped.CurrentVehicle != StartHeli && !fPlayer.IsWanted)
                        Screen.ShowSubtitle("Get back into the ~b~Buzzard~w~");
                    if (Start.StartHeli != null)
                    {
                        StartHeli = Start.StartHeli;
                    }
                    if (LudendorffNorthYankton == null)
                    {
                        LudendorffNorthYankton = fBlip.CreateBlipForCoordWithParams(new Vector3(1580f, 8745f, 259f), BlipSprite.Standard, (BlipColor)5, 1f, "Destination");
                    }
                    if (StartHeli != null)
                    {
                        if (StartHeli.AttachedBlip != null)
                        {
                            if (fPlayer.ped.CurrentVehicle == StartHeli)
                            {
                                if (LudendorffNorthYankton != null)
                                {
                                    if (fPlayer.GetCarDistanceTo(new Vector2(1580f, 8745f)) < 500f && !fPlayer.IsWanted)
                                    {
                                        Globals.missionSwitch = 3;
                                    }
                                }
                                Game.DisableControlThisFrame(GTA.Control.VehicleAttack);
                                Game.DisableControlThisFrame(GTA.Control.VehicleAttack2);
                                Game.DisableControlThisFrame(GTA.Control.VehicleFlyAttack);
                                Game.DisableControlThisFrame(GTA.Control.VehicleFlyAttack2);
                                Game.DisableControlThisFrame(GTA.Control.VehicleSelectNextWeapon);
                                Game.DisableControlThisFrame(GTA.Control.VehicleSelectPrevWeapon);
                                if (Start.HeliBlip != null)
                                {
                                    Start.HeliBlip.Delete();
                                    Start.HeliBlip = null;
                                }
                                StartHeli.AttachedBlip.Alpha = 0;
                            }
                            else
                            {
                                StartHeli.AttachedBlip.Alpha = 255;
                            }
                        }
                    }
                    break;
                case 3:
                    switch (heliCutsceneSwitch)
                    {
                        case 0:
                            if (cutscenePed1 == null)
                            {
                                cutscenePed1 = fPed.CreatePed("mp_m_freemode_01", new Vector3(0f, 0f, 0f), 0f);
                            }
                            else
                            {
                                cutscenePed1.IsPositionFrozen = true;
                                cutscenePed1.IsVisible = false;
                            }
                            if (cutscenePed2 == null)
                            {
                                cutscenePed2 = fPed.CreatePed("mp_m_freemode_01", new Vector3(0f, 0f, 0f), 0f);
                            }
                            else
                            {
                                cutscenePed2.IsPositionFrozen = true;
                                cutscenePed2.IsVisible = false;
                            }
                            if (cutscenePed3 == null)
                            {
                                cutscenePed3 = fPed.CreatePed("mp_m_freemode_01", new Vector3(0f, 0f, 0f), 0f);
                            }
                            else
                            {
                                cutscenePed3.IsPositionFrozen = true;
                                cutscenePed3.IsVisible = false;
                            }
                            heliCutsceneSwitch = 1;
                            break;
                        case 1:
                            heliCutscene = new fCutsceneCreation("mph_pri_fin_ext", new Vector3(1580f, 8745f, 259f), new Vector3(0f, 0f, 1f), false);
                            heliCutscene.AddRegisterEntityToList(cutscenePed1, "MP_1", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                            heliCutscene.AddRegisterEntityToList(Game.Player.Character, "MP_2", fCutscene.CutsceneUsage.CU_ANIMATE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                            heliCutscene.AddRegisterEntityToList(cutscenePed2, "MP_3", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                            heliCutscene.AddRegisterEntityToList(cutscenePed3, "MP_4", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                            heliCutscene.AddRegisterEntityToList(StartHeli, "MPH_Helicopter", fCutscene.CutsceneUsage.CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY, 0, fCutscene.CutsceneEntityOptionFlag.CEO_IGNORE_MODEL_NAME);
                            heliCutscene.StartCutscene();
                            while (!fCutscene.IsCutscenePlaying())
                            {
                                Wait(0);
                            }
                            while (!fCutscene.HasCutsceneFinished())
                            {
                                fCutscene.SetCutsceneCanBeSkipped(false);
                                fCutscene.SetCutsceneOriginAndRotation("mph_pri_fin_ext", new Vector3(heliCutscene.Pos.X, heliCutscene.Pos.Y, heliCutscene.Pos.Z), new Vector3(heliCutscene.Rot.X, heliCutscene.Rot.Y, heliCutscene.Rot.Z));
                                cutscenePed1.IsVisible = false;
                                cutscenePed2.IsVisible = false;
                                cutscenePed3.IsVisible = false;
                                if (fCutscene.GetCutsceneTime() > 4850 && !Screen.IsFadingOut)
                                {
                                    Screen.FadeOut(3000);
                                }
                                if (Screen.IsFadedOut)
                                {
                                    fCutscene.StopCutsceneImmediately();
                                }
                                Wait(0);
                            }
                            heliCutscene.Cleanup();
                            heliCutscene = null;
                            if (cutscenePed1 != null)
                            {
                                cutscenePed1.Delete();
                                cutscenePed1 = null;
                            }
                            if (cutscenePed2 != null)
                            {
                                cutscenePed2.Delete();
                                cutscenePed2 = null;
                            }
                            if (cutscenePed2 != null)
                            {
                                cutscenePed2.Delete();
                                cutscenePed2 = null;
                            }
                            if (LudendorffNorthYankton != null)
                            {
                                LudendorffNorthYankton.Delete();
                                LudendorffNorthYankton = null;
                            }
                            if (StartHeli != null)
                            {
                                if (StartHeli.AttachedBlip != null)
                                {
                                    StartHeli.AttachedBlip.Delete();
                                }
                                StartHeli.Delete();
                                StartHeli = null;
                            }
                            if (FailArea != null)
                            {
                                FailArea.Delete();
                                FailArea = null;
                            }
                            fPlayer.ped.IsPositionFrozen = true;
                            fPlayer.ped.IsInvincible = true;
                            Game.Player.Character.Task.ClearAll();
                            heliCutsceneSwitch = -1;
                            break;
                        case -1:
                            Vector3 camPos = new Vector3(3575.468f, -4871.25f, 117.1896f);
                            Vector3 camRot = new Vector3(20f, 0f, 26.39456f);
                            Vector3 camPos2 = new Vector3(3576.268f, -4872.851f, 117.1896f);
                            Vector3 camRot2 = new Vector3(25f, 0f, 26.39456f);
                            Vector3 driveDestination = new Vector3(3612.947f, -4910.009f, 111.2528f);
                            Vector3 carsPos = new Vector3(3528.305f, -4878.405f, 111.2986f);
                            float carsHeading = 250.5463f;
                            if (!fInterior.PrologueMap.IsPrologueMapLoading && !fInterior.PrologueMap.IsPrologueMapLoaded)
                            {
                                fInterior.PrologueMap.LoadPrologueMap();
                            }
                            if (fInterior.PrologueMap.IsPrologueMapLoaded)
                            {
                                fClock.SetClockTime(4, 30, 0);
                                fWeather.SetOverrideWeather(WeatherTypes.Snow);
                                fClock.PauseClock(true);
                                fInterior.PrologueMap.EnableNorthYanktonTrains(true);
                                fPathfind.SetAllowStreamPrologueNodes(true);
                                fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                                fHud.ToggleNorthYanktonMap(true);
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
                                        Screen.FadeIn(3000);
                                        fPlayer.ped.IsInvincible = false;
                                        if (fPlayer.ped.CurrentVehicle != PrologueVehicle)
                                        {
                                            fPlayer.ped.SetIntoVehicle(PrologueVehicle, VehicleSeat.Driver);
                                        }
                                        else
                                        {
                                            PrologueVehicle.LockStatus = VehicleLockStatus.PlayerCannotLeaveCanBeBrokenIntoPersist;
                                        }
                                        Wait(2000);
                                        fPlayer.ped.Task.DriveTo(PrologueVehicle, driveDestination, 55f, VehicleDrivingFlags.None, 5f);
                                        PrologueVehicle.Speed = 10f;
                                        while (fPlayer.GetCarDistanceTo(new Vector2(3572.151f, -4890.441f)) > 5f)
                                            Wait(0);
                                        fCam.RenderScriptCams(false, true, 1500);
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
                    if (!timeAdvanced1)
                    {
                        fClock.PauseClock(true);
                        fClock.SetClockTime(4, 30);
                    }
                    fWeather.SetOverrideWeather(WeatherTypes.Snow);
                    fHud.HideHudComponentThisFrame((int)HudComponent.HelpText);
                    fHud.DisplayHelpText("");
                    if (!fInterior.PrologueMap.IsPrologueMapLoaded)
                        fInterior.PrologueMap.LoadPrologueMap();
                    if (fInterior.PrologueMap.IsPrologueMapLoaded)
                    {
                        fInterior.PrologueMap.EnableYanktonTraffic = true;
                        fStreaming.SetMapDataCullboxEnabled("prologue", true);
                        fStreaming.SetMapDataCullboxEnabled("Prologue_Main", true);
                        fInterior.PrologueMap.EnableNorthYanktonTrains(true);
                        fPathfind.SetAllowStreamPrologueNodes(true);
                        Function.Call<bool>(Hash.LOAD_ALL_PATH_NODES, true);
                        fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                        fZone.SetZoneEnabled(fZone.GetZoneFromNameID("PrLog"), true);
                        fHud.ToggleNorthYanktonMap(true);
                    }
                    if (DepotBlip == null)
                    {
                        DepotBlip = fBlip.CreateGPSBlip(new Vector3(5360.6f, -5187.516f, 82.76017f), 1, BlipColor.Yellow2);
                    }
                    if (DepotBlip != null)
                    {
                        DepotBlip.Name = "Bobcat Security Depot";
                        if (PrologueVehicle != null)
                        {
                            if (fPlayer.ped.CurrentVehicle == PrologueVehicle)
                            {
                                if (PrologueVehicle.AttachedBlip != null)
                                    PrologueVehicle.AttachedBlip.DisplayType = BlipDisplayType.NoDisplay;
                                Screen.ShowSubtitle("Go to ~y~Bobcat Security Depot~w~");
                                PrologueVehicle.LockStatus = VehicleLockStatus.None;
                                if (fPlayer.GetCarDistanceTo(DepotBlip.Position) < 1500f)
                                {
                                    if (!timeAdvanced2)
                                    {
                                        if (fClock.GetClockHours() == 4)
                                        {
                                            fClock.PauseClock(false);
                                            timeAdvanced1 = true;
                                        }
                                    }
                                    if (fClock.GetClockHours() != 5 && (fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(5356.7324f, -5201.1553f, 80.83122f), new Vector3(5356.5454f, -5179.6f, 96.83691f), 20f, false, true, 0) || fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(5417.894f, -5108.7925f, 75.56319f), new Vector3(5412.488f, -5240.66f, 95.59789f), 100f, false, true, 0)))
                                    {
                                        fClock.SetClockTime(5, 0, 0, true, 1);
                                        fClock.PauseClock(true);
                                        timeAdvanced2 = true;
                                    }
                                    if (fClock.GetClockHours() == 5)
                                    {
                                        fClock.SetClockTime(5, 0, 0);
                                        fClock.PauseClock(true);
                                        timeAdvanced2 = true;
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
                                    PrologueVehicle.AttachedBlip.Name = "Vehicle";
                                    PrologueVehicle.AttachedBlip.IsFriendly = true;
                                    PrologueVehicle.AttachedBlip.IsShortRange = false;
                                }
                                else
                                {
                                    PrologueVehicle.AttachedBlip.DisplayType = BlipDisplayType.Default;
                                }
                                Screen.ShowSubtitle("Get back in the ~b~vehicle.~w~");
                            }
                        }
                    }
                    break;
            }
            if (Globals.missionSwitch != 0)
            {
                MissionFailHandler();
            }
            if (FailArea != null)
            {
                fBlip.SetBlipRotation(FailArea, fMath.CEIL(0f));
            }
            if (Start.StartHeli != null)
            {
                StartHeli = Start.StartHeli;
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
        void Spawn()
        {
            SpawnTrucks();
        }

        int heliCutsceneSwitch = 0;
        fCutsceneCreation heliCutscene = null;
        Ped cutscenePed1;
        Ped cutscenePed2;
        Ped cutscenePed3;
        Camera failCam;
        int failSceneID = 0;
        bool timerElapsed = false;
        bool timerStarted = false;
        int resetSwitch = 0;
        bool Restart = false;
        int getGametime = Game.GameTime;
        Camera resetCam;
        Vector3 resetCamCoord7 = new Vector3(5355f, -5173f, 82.49858f);
        Vector3 resetCamRot7 = new Vector3(17f, 0f, 130.4812f);
        Vector3 resetCamCoord3 = new Vector3(5346.801f, -5178.461f, 82.4f);
        Vector3 resetCamRot3 = new Vector3(15f, 0f, 129.449f);
        InstructionalButtons resetButtons = new InstructionalButtons();
        InstructionalButtonContainer ResetButtonContainer = new InstructionalButtonContainer(InputControl.FrontendAccept, "Restart Mission");
        InstructionalButtonContainer GoBackToFreeroamContainer = new InstructionalButtonContainer(InputControl.FrontendCancel, "Leave Mission");
        bool instructionalButtonsSetUp = false;
        bool restartCanceled = false;
        bool failShardJustShown = false;
        bool MissionFailCleanUpRequired = false;
        bool[] extraBools = new bool[100];

        void MissionFailHandler()
        {
            if (Globals.missionSwitch > 0 && Globals.missionSwitch < 3)
            {
                if (IsStartHeliDestroyed)
                {
                    fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.IdleStart);
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                    TimerbarPool.Remove(failCountdown);
                    WarningMessageShown = false;
                    PlayerInArea = true;
                    FailCountdownVisible = false;
                    justFailed = true;
                    SetFailVariation(FailVariations.HelicopterDestroyed);
                    MissionFailCleanUpRequired = true;
                }
                else
                {
                    if (FailArea == null)
                    {
                        Vector3 areaCenter = new Vector3(1542f, 7546.5f, 0f);
                        float areaWidth = 916f;
                        float areaHeight = 2683f;
                        FailArea = fBlip.AddBlipForArea(areaCenter, areaWidth, areaHeight);
                        FailArea.Alpha = 0;
                        FailArea.Color = BlipColor.Yellow;
                    }
                    else
                    {
                        MidsizedMessage failWarning = new MidsizedMessage("Leaving the Mission Area", "Return to the mission area.");
                        if (!fEntity.IsEntityInArea(fPlayer.ped, new Vector3(2000f, 6205f, -7f), new Vector3(1084f, 8888f, 550f)))
                        {
                            LudendorffNorthYankton.Alpha = 0;
                            FailArea.Alpha = 128;
                            PlayerInArea = false;
                            if (!FailCountdownVisible)
                            {
                                failTimer.Reset();
                                failCountdown = new CountdownTimerbar("TIME LEFT:", 30000);
                                failCountdown.VariableTimer = failTimer;
                                failCountdown.Draw(0.9f);
                                failTimer.Start();
                                FailCountdownVisible = true;
                            }
                            if (!WarningMessageShown)
                            {
                                failWarning.Color = 6;
                                failWarning.ScreenTime = 6500;
                                WarningMessageShown = true;
                                failWarning.Show();
                            }
                        }
                        if (fEntity.IsEntityInArea(fPlayer.ped, new Vector3(2000f, 6205f, -7f), new Vector3(1084f, 8888f, 550f)) && failTimer.Counter > 0 && FailCountdownVisible && !PlayerInArea)
                        {
                            LudendorffNorthYankton.Alpha = 255;
                            FailArea.Alpha = 0;
                            fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                            fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                            fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.IdleStart);
                            fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                            TimerbarPool.Remove(failCountdown);
                            WarningMessageShown = false;
                            PlayerInArea = true;
                            FailCountdownVisible = false;
                        }
                    }
                }
            }
            if (Globals.missionSwitch > 3)
            {
                if (IsPrologueVehicleDestroyed)
                {
                    fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.SuspenseStart);
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Suspense);
                    TimerbarPool.Remove(failCountdown);
                    failTimer.Stop();
                    failTimer.Reset();
                    WarningMessageShown = false;
                    FailCountdownVisible = false;
                    timerElapsed = false;
                    timerStarted = false;
                    justFailed = true;
                    SetFailVariation(FailVariations.PrologueVehicleDestroyed);
                    MissionFailCleanUpRequired = true;
                }
                else
                {
                    if (PrologueVehicle != null)
                    {
                        MidsizedMessage failWarning = new MidsizedMessage("Return to the vehicle", "Get back in the vehicle before the Heist is blown.");
                        if (fPlayer.ped.CurrentVehicle != PrologueVehicle)
                        {
                            DepotBlip.Alpha = 0;
                            if (!FailCountdownVisible)
                            {
                                if (!timerStarted)
                                {
                                    fTimer.SetTimerA(0);
                                    timerStarted = true;
                                }
                                if (timerStarted && fTimer.TimerA() > 10000)
                                {
                                    timerElapsed = true;
                                }
                                if (timerElapsed)
                                {
                                    failTimer.RemoveTime(9250);
                                    failCountdown = new CountdownTimerbar("TIME LEFT:", 20000);
                                    failCountdown.CountdownMusicEvent = "FM_COUNTDOWN_20S";
                                    failCountdown.VariableTimer = failTimer;
                                    failCountdown.Draw(0.9f);
                                    failTimer.Start();
                                    if (!WarningMessageShown)
                                    {
                                        failWarning.Color = 6;
                                        failWarning.ScreenTime = 6500;
                                        WarningMessageShown = true;
                                        if (fPlayer.ped.CurrentVehicle != PrologueVehicle)
                                        {
                                            failWarning.Show();
                                        }
                                    }
                                    FailCountdownVisible = true;
                                }

                            }
                        }
                        if (fPlayer.ped.CurrentVehicle == PrologueVehicle && failTimer.Counter > 0 && FailCountdownVisible)
                        {
                            DepotBlip.Alpha = 255;
                            fAudio.PrepareMusicEvent("GTA_ONLINE_STOP_SCORE");
                            fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                            fAudio.PrepareMusicEventIntensity(fAudio.MusicEventIntensity.SuspenseStart);
                            fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Suspense);
                            TimerbarPool.Remove(failCountdown);
                            failTimer.Stop();
                            failTimer.Reset();
                            WarningMessageShown = false;
                            FailCountdownVisible = false;
                            timerElapsed = false;
                            timerStarted = false;
                        }
                    }
                }
            }
            MissionFailCleanUp();
        }
        void MissionFailCleanUp()
        {
            if (MissionFailCleanUpRequired)
            {
                fPlayer.SetMaxWantedLevelTo0();
                if (justFailed)
                {
                    Globals.missionSwitch = -1;
                    if (depotVehicles.Count > 0)
                    {
                        for (int i = 0; i < depotVehicles.Count; i++)
                        {
                            depotVehicles[i].MarkAsNoLongerNeeded();
                            depotVehicles.Remove(depotVehicles[i]);
                        }
                    }
                    if (StartHeli != null)
                    {
                        if (StartHeli.AttachedBlip != null)
                        {
                            StartHeli.AttachedBlip.Delete();
                        }
                        StartHeli.MarkAsNoLongerNeeded();
                        StartHeli = null;
                    }
                    if (LudendorffNorthYankton != null)
                    {
                        LudendorffNorthYankton.Delete();
                        LudendorffNorthYankton = null;
                    }
                    if (FailArea != null)
                    {
                        FailArea.Delete();
                        FailArea = null;
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
                    if (heliCutscene != null)
                    {
                        heliCutscene.Cleanup();
                        heliCutscene = null;
                    }
                    if (cutscenePed1 != null)
                    {
                        cutscenePed1.Delete();
                        cutscenePed1 = null;
                    }
                    if (cutscenePed2 != null)
                    {
                        cutscenePed2.Delete();
                        cutscenePed2 = null;
                    }
                    if (cutscenePed2 != null)
                    {
                        cutscenePed2.Delete();
                        cutscenePed2 = null;
                    }
                    if (DepotBlip != null)
                    {
                        DepotBlip.Delete();
                        DepotBlip = null;
                    }
                    fHud.ClearAllPrints();
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, false);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, false);
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.MusicStop);
                    if (GetFailVariation() == FailVariations.LeftMissionArea)
                    {
                        fMissionShard failShard = new fMissionShard();
                        failShard.Shard_In("HEIST FAILED", "You left the mission area.", 6, 0.3f, 6, true);
                        failShard = null;
                        failShardJustShown = true;
                        Screen.FadeOut(1000);
                        justFailed = false;
                        SetFailVariation(FailVariations.None);
                    }
                    if (GetFailVariation() == FailVariations.HelicopterDestroyed)
                    {
                        fMissionShard failShard = new fMissionShard();
                        failShard.Shard_In("HEIST FAILED", "Helicopter was destroyed.", 6, 0.3f, 6, true);
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
                            fMissionShard failShard = new fMissionShard();
                            failShard.Shard_In("HEIST FAILED", $"{PrologueVehicleName} was destroyed.", 6, 0.3f, 6, true);
                            failShard = null;
                            failShardJustShown = true;
                            Screen.FadeOut(1000);
                            justFailed = false;
                            SetFailVariation(FailVariations.None);
                        }
                        if (GetFailVariation() == FailVariations.PrologueVehicleLeft_JobBlown)
                        {
                            fMissionShard failShard = new fMissionShard();
                            failShard.Shard_In("HEIST FAILED", "Job was blown.", 6, 0.3f, 6, true);
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
                    if (PlayerTeleportedToPrologue && resetSwitch == 0)
                        resetSwitch = fMisc.GetRandomIntInRange(1, 3);
                    if (!PlayerTeleportedToPrologue && resetSwitch == 0)
                    {
                        resetSwitch = 1;
                        extraBools[1] = true;
                    }
                    if (resetSwitch > 0)
                    {
                        if (extraBools[1])
                        {
                            if (!fInterior.PrologueMap.IsPrologueMapLoaded)
                            {
                                fInterior.PrologueMap.LoadPrologueMap();
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
                                    fWeather.SetOverrideWeather(WeatherTypes.SNOWLIGHT);
                                    fClock.PauseClock(true);
                                    fInterior.PrologueMap.EnableNorthYanktonTrains(true);
                                    fPathfind.SetAllowStreamPrologueNodes(true);
                                    fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                                    fHud.ToggleNorthYanktonMap(true);
                                }
                            }
                        }
                        if (PlayerTeleportedToPrologue)
                        {
                            if (!fInterior.PrologueMap.IsPrologueMapLoaded)
                            {
                                fInterior.PrologueMap.LoadPrologueMap();
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
                                    fWeather.SetOverrideWeather(WeatherTypes.Snow);
                                    fClock.PauseClock(true);
                                    fInterior.PrologueMap.EnableNorthYanktonTrains(true);
                                    fPathfind.SetAllowStreamPrologueNodes(true);
                                    fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), true);
                                    fHud.ToggleNorthYanktonMap(true);
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
                    if (resetCam != null && restartCanceled == false && Restart == false)
                    {
                        if (ScriptCameraDirector.RenderingCam == resetCam)
                        {
                            if (Screen.IsFadedOut && !Restart || !restartCanceled)
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
                                    extraBools[3] = true;
                                }
                                if (Game.IsControlJustPressed(GTA.Control.FrontendCancel) || Game.IsControlJustPressed(GTA.Control.FrontendPauseAlternate))
                                {
                                    extraBools[2] = true;
                                }
                                if (extraBools[3])
                                {
                                    Restart = true;
                                }
                                if (extraBools[2])
                                {
                                    Restart = false;
                                    restartCanceled = true;
                                }
                            }
                        }
                    }
                    if (Restart)
                    {
                        Screen.FadeOut(2500);
                        resetButtons.RemoveContainer(ResetButtonContainer);
                        resetButtons.RemoveContainer(GoBackToFreeroamContainer);
                        resetButtons.Dispose();
                        if (Screen.IsFadedOut)
                        {
                            fPlayer.ped.IsVisible = true;
                            fPlayer.ped.IsPositionFrozen = false;
                            if (resetCam != null)
                            {
                                fCam.RenderScriptCams(false, false, 0);
                                resetCam.IsActive = false;
                                resetCam.Delete();
                                resetCam = null;
                            }
                            if (Globals.missionSwitch > 0 &&  Globals.missionSwitch < 3)
                            {
                                if (StartHeli == null)
                                {
                                    num = fMisc.GetRandomIntInRange(1, 8);
                                    StartHeli = fVehicle.CreateVehicle("buzzard", new Vector3(1600.979f, 6623.252f, 15f), 5.047073f);
                                }
                            }
                        }
                    }
                    if (restartCanceled)
                    {
                        Screen.FadeOut(2500);
                        resetButtons.RemoveContainer(ResetButtonContainer);
                        resetButtons.RemoveContainer(GoBackToFreeroamContainer);
                        resetButtons.Dispose();
                        if (Screen.IsFadedOut)
                        {
                            if (resetCam != null)
                            {
                                fCam.RenderScriptCams(false, false, 0);
                                resetCam.IsActive = false;
                                resetCam.Delete();
                                resetCam = null;
                            }
                            fStreaming.RequestAnimDict("switch@michael@sitting");
                            fPlayer.PedPos(1522.371f, 6585.832f, 7.304277f, -10f);
                            instructionalButtonsSetUp = false;
                            fWeather.SetOverrideWeather(weatherTypeBeforeMission);
                            fClock.SetClockTime(12, 0, 0);
                            fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), false);
                            fClock.PauseClock(false);
                            fInterior.PrologueMap.EnableNorthYanktonTrains(false);
                            fPathfind.SetAllowStreamPrologueNodes(false);
                            fHud.ToggleNorthYanktonMap(false);
                            if (fInterior.PrologueMap.IsPrologueMapLoaded && PlayerTeleportedToPrologue)
                            {
                                fInterior.PrologueMap.UnloadPrologueMap();
                            }
                            while (failCam == null)
                            {
                                failCam = fCam.CreateScriptedCam();
                                Wait(0);
                            }
                            if (failCam != null)
                            {
                                fCam.SetCamFov(failCam, 50f);
                                fHud.RadarAndHud(false, false);
                                failCam.Position = new Vector3(1522.0f, 6584.67f, 8.301389f);
                                failCam.Rotation = new Vector3(0f, 0f, -8.4f);
                                Vector3 animpos = new Vector3(1522.371f, 6585.832f, 7.304277f);
                                Function.Call(Hash.CLEAR_PED_WETNESS, fPlayer.ped);
                                fPlayer.ped.Weapons.Select(WeaponHash.Unarmed);
                                fPlayer.ped.IsVisible = true;
                                fPlayer.ped.IsPositionFrozen = false;
                                fPlayer.ped.IsInvincible = false;
                                fPlayer.ped.Task.ClearAllImmediately();
                                fPlayer.PedPos(1522.371f, 6585.832f, 7.304277f, -10f);
                                failSceneID = fAnimations.CreateSynchronizedScene(animpos, 0f, 0f, -10f);
                                fAnimations.TaskSynchronizedScene(fPlayer.ped, failSceneID, "switch@michael@sitting", "idle", 1000f, 1000f, 0, 0, 1000f, 0);
                                failCam.IsActive = true;
                                fCam.RenderScriptCams(true, false, 0, true, false, fCam.RenderingOptionFlag.RO_NO_OPTIONS);
                                failCam.Position = new Vector3(1522.0f, 6584.67f, 8.301389f);
                                failCam.Rotation = new Vector3(0f, 0f, -8.4f);
                                Wait(4000);
                                Screen.FadeIn(5000);
                                Wait(2500);
                                fCam.RenderScriptCams(false, true, 2700, true, false, fCam.RenderingOptionFlag.RO_NO_OPTIONS);
                                failSceneID = fAnimations.CreateSynchronizedScene(animpos, 0f, 0f, -10f);
                                fAnimations.TaskSynchronizedScene(fPlayer.ped, failSceneID, "switch@michael@sitting", "exit_forward", 1f, 1f, 0, 0, 1f, 0);
                                failCam.IsActive = false;
                                while (fAnimations.GetSynchronizedScenePhase(failSceneID) < 1f)
                                    Wait(0);
                                fPlayer.ped.Task.ClearAllImmediately();
                                Wait(1000);
                                fPlayer.SetMaxWantedLevelToNormal();
                                Globals.globalBlips = 3;
                                Globals.scriptTerminator = 3;
                                fHud.RadarAndHud(true, true);
                                fStreaming.RemoveAnimDict("switch@michael@sitting");
                                extraBools[2] = false;
                                extraBools[3] = false;
                                PlayerTeleportedToPrologue = false;
                                failSceneID = 0;
                                Globals.missionSwitch = 0;
                                MissionFailCleanUpRequired = false;
                                if (failCam != null)
                                {
                                    failCam.Delete();
                                    failCam = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CleanUp()
        {
            Screen.FadeOut(0);
            if (weatherTypeSaved)
                fWeather.SetOverrideWeather(weatherTypeBeforeMission);
            fZone.SetZoneEnabled(fZone.GetZoneFromNameID("Prol"), false);
            fClock.PauseClock(false);
            fInterior.PrologueMap.EnableNorthYanktonTrains(false);
            fPathfind.SetAllowStreamPrologueNodes(false);
            fHud.ToggleNorthYanktonMap(false);
            if (fInterior.PrologueMap.IsPrologueMapLoaded && PlayerTeleportedToPrologue)
            {
                fPlayer.PedPos(1522.371f, 6585.832f, 7.304277f, -10f);
                fInterior.PrologueMap.UnloadPrologueMap();
            }
            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.MusicStop);
            fVehicle.DeleteVehiclesInList(depotVehicles);
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
                if (!fPlayer.ped.IsInVehicle(PrologueVehicle))
                {
                    PrologueVehicle.Delete();
                    PrologueVehicle = null;
                }
                else
                {
                    PrologueVehicle.MarkAsNoLongerNeeded();
                    PrologueVehicle = null;
                }
            }
            if (heliCutscene != null)
            {
                heliCutscene.Cleanup();
                heliCutscene = null;
            }
            if (cutscenePed1 != null)
            {
                cutscenePed1.Delete();
                cutscenePed1 = null;
            }
            if (cutscenePed2 != null)
            {
                cutscenePed2.Delete();
                cutscenePed2 = null;
            }
            if (cutscenePed2 != null)
            {
                cutscenePed2.Delete();
                cutscenePed2 = null;
            }
            if (LudendorffNorthYankton != null)
            {
                LudendorffNorthYankton.Delete();
                LudendorffNorthYankton = null;
            }
            if (StartHeli != null)
            {
                if (StartHeli.AttachedBlip != null)
                {
                    StartHeli.AttachedBlip.Delete();
                }
                if (!fPlayer.ped.IsInVehicle(StartHeli))
                {
                    StartHeli.Delete();
                    StartHeli = null;
                }
                else
                {
                    StartHeli.MarkAsNoLongerNeeded();
                    StartHeli = null;
                }
            }
            if (FailArea != null)
            {
                FailArea.Delete();
                FailArea = null;
            }
            Screen.FadeIn(1300);
        }

        private void onShutdown(object sender, EventArgs e)
        {
            CleanUp();
        }
    }
}
