using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TheNorthYanktonHeist.Extension;
using fCore = IMNOTEPIX.Framework.fCore;
using fUI = IMNOTEPIX.Framework.fUI;
using fWorld = IMNOTEPIX.Framework.fWorld;
using fPlayer = IMNOTEPIX.Framework.fPlayer;
using fInterior = IMNOTEPIX.Framework.fWorld.Interior;
using fBlip = IMNOTEPIX.Framework.fBlip;
using Screen = GTA.UI.Screen;

public static class Globals
{
    //public static Hash NorthYanktonHeistDLC = fMisc.GetHashKey("northyanktonheist");
    public const string NYHeistDLCgxtString = "NTH_TOSTART";

    public static int missionSwitch = 0;

    //public static int globalBlips = 3;
    //public static int globalScripts = 3;
    public static int Debug2 = -1;
    public static int Debug = -1;
    public static bool LogScriptManager = false;
    public static bool debug = true;
    public static int integer1 = 0;
    public static int switch1 = 0;
    public static int sceneID1 = 0;
    public static bool boolean1 = false;
    public static int[] intStorage = new int[100];
}

namespace Global
{
    using static Globals;
    using static System.Windows.Forms.AxHost;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
    using static TheNorthYanktonHeist.Heist;

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
                fInterior.Interior.RemoveIpl(value);
            }
            foreach (string value2 in LoadAllIPLS)
            {
                fInterior.Interior.RemoveIpl(value2);
                fInterior.Interior.RequestIpl(value2);
            }
        }

        List<Blip> excludeBlips = new List<Blip>();

        fUI.Scaleforms.InstructionalButton warningbutton = new fUI.Scaleforms.InstructionalButton(GTA.Control.FrontendAccept, "OK");
        fUI.Scaleforms.InstructionalButtons warningButtons = new fUI.Scaleforms.InstructionalButtons();
        static fWorld.Cutscene.CutsceneCreation debugCutscene = null;
        static Vehicle debugHeli;
        static Ped cutscenePed1;
        static Ped cutscenePed2;
        static Ped cutscenePed3;
        static Blip debugBlip;
        static Camera debugCam;
        static Prop debugProp;
        int scaleID = 0;


        private void onTick(object sender, EventArgs e)
        {
            // Driveway to the depot: 
            // Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, fPlayer.ped, 5356.7324f, -5201.1553f, 80.83122f, 5356.5454f, -5179.6f, 96.83691f, 20f, false, true, 0) || Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, fPlayer.ped, 5417.894f, -5108.7925f, 75.56319f, 5412.488f, -5240.66f, 95.59789f, 100f, false, true, 0)
            // Depot:
            // fEntity.IsEntityInArea(fPlayer.ped, new Vector3(5364.804f, -5158.941f, 79f), new Vector3(5283.942f, -5229.462f, 89f))
            if (!ScriptSetup)
            {
                if (!Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE) && !Screen.IsFadingIn)
                {
                    Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                    ScriptSetup = true;
                }
                if (Screen.IsFadingIn)
                {
                    if (!Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, NYHeistDLCgxtString) && !dlcPackWarningShown)
                    {
                        if (!instructionalButtonsSetUp)
                        {
                            warningButtons.Buttons.Add(warningbutton);
                            instructionalButtonsSetUp = true;
                        }
                        else
                        {
                            warning.Render2D();
                            Game.DisableAllControlsThisFrame();
                            warning.CallFunction("SHOW_POPUP_WARNING", -1, "WARNING", "North Yankton Heist Dlcpack Is Not Installed.", "Objective Subtitles Will Not Work Correctly.", true);
                            warningButtons.Draw();
                            if (Game.IsControlJustPressed(GTA.Control.FrontendAccept))
                            {
                                warningButtons.Buttons.Clear();
                                warning.Dispose();
                                warningbutton = null;
                                warningButtons.Update();
                                dlcPackWarningShown = true;
                            }
                        }
                    }
                }
            }
            else
            {
                switch (Debug2)
                {
                    case 0:
                        Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
                        RayfireScenes.PrologueDoorRayfire.Scene();
                        break;
                    case 1:
                        big.Update();
                        break;
                }
                /*
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
                switch (globalScripts)
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
                        globalScripts = 0;
                        break;
                }*/
            }
        }
        fUI.TimerBars.HudCountdownBar timerBar;
        fUI.Scaleforms.BigMessageHandler big = new fUI.Scaleforms.BigMessageHandler();

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (debug)
            {
                if (e.KeyCode == Keys.N)
                {
                    //Screen.ShowSubtitle("~s~Get in ~y~position.~s~");
                    //Debug2++;
                    //fHud.DisplayHelpText("~s~Leave the security room before it ~r~explodes.~s~");
                    //Screen.ShowSubtitle("~s~Torch~s~ the ~y~security room~s~ to ~r~erase the footage.~s~");
                    //Screen.ShowSubtitle("~s~Torch the ~r~security room.~s~");
                    //fHud.DisplayHelpText("~s~Shoot the area around the door lock to shoot open the door.");
                    //fDebug.CopyToClipboard(fInterior.GetRoomKeyFromEntity(fPlayer.ped).ToString());
                    /*
                    // "~s~Security ~r~cameras caught~s~ the mayhem you caused. ", "~y~Head to the security room~s~ to eradicate the footage."
                    fHud.DisplayHelpText_Duration(0, false, true, 10000, "~s~Security cameras caught your robbery on camera.", " ~y~Head to the security room~s~ to ~r~erase the footage.~s~");
                    /*
                    test = World.GetClosestProp(new Vector3(5305.461f, -5177.75f, 83.66856f), 0.1f, -311575617);
                    Debug2 = 0;
                    /*
                    var takeBar = new HudValueBar("TAKE", isMoney: true) { Value = 0, ValueColor = HudColors.Get(HudColor.Green) };
                    HudBarController.Register(takeBar);/*
                    Debug2 = 0;
                    new CartGrab(CartType.Gold_a, new Vector3(100, 200, 30)).CreateWithAutoBlip().ConnectToValueBar(takeBar);
                    new CartGrab(CartType.Standard, fPlayer.ped.Position + new Vector3(0f, -2f, -1f)).CreateWithAutoBlip().ConnectToValueBar(takeBar);
                    new CartGrab(CartType.Gold_c, fPlayer.ped.Position + new Vector3(-2f, -2f, -1f)).CreateWithAutoBlip().ConnectToValueBar(takeBar);
                    new CartGrab(CartType.Diamonds_c, fPlayer.ped.Position + new Vector3(-4f, -2f, -1f)).CreateWithAutoBlip().ConnectToValueBar(takeBar);
                    //cart.CreateWithBlip((BlipSprite)514, BlipColor.Grey, "Cocaine", 0.9f);
                    //cart2.CreateWithBlip((BlipSprite)617, (BlipColor)26, "Diamonds", 0.9f);
                    //cart3.CreateWithBlip((BlipSprite)618, (BlipColor)28, "Gold", 0.9f);
                    //cart4.CreateWithBlip((BlipSprite)272, BlipColor.Green, "Cash", 0.9f);
                    //cart.UseRemoteCounterSound = true;
                    //cart2.UseRemoteCounterSound = true;
                    //Function.Call(Hash.LOAD_STREAM, "PROLOGUE_BLOW_THE_VAULT_MASTER", 0);
                    //fAudio.PlayStreamFrontend();
                    //SpawnBooth();*/
                    ScriptManager.ScriptManager.KillScripts();
                    TheNorthYanktonHeist.Heist.checkpoint = 3;
                    Globals.missionSwitch = 1000;
                    TheNorthYanktonHeist.Heist.justFailed = true;
                    SetFailVariation(FailVariations.PlaneDestroyed);
                    TheNorthYanktonHeist.Heist.MissionFailCleanUpRequired = true;
                    //fPlayer.PedPos(3263.05f, -4704.67f, 104.67f, 0f);
                    //fInterior.PrologueMap.LoadYankton();
                    //fInterior.PrologueMap.EnableYanktonTraffic = true;
                    //missionSwitch = 3;
                    //Debug2 = 0;
                    /*
                    Blip[] allBlips = World.GetAllBlips();
                    if (allBlips.Length > 0)
                    {
                        foreach (Blip blip in allBlips)
                        {
                            if (blip != null && blip.Sprite == (BlipSprite)272)
                            {
                                blip.Delete();
                            }
                        }
                    }*/
                }
                if (e.KeyCode == Keys.NumPad7)
                {
                    //fHud.DisplayHeistHelpText("NTH_GOTODEPOT", true);
                    //Screen.FadeIn(0);
                    //Framework.Core.Debug.CopyPlayerPosWithAddons();
                    //1777.488f, 3326.681f, 41.43328f
                }
                if (e.KeyCode == Keys.NumPad9)
                {
                    big.Load(true);
                    Debug2 = 1;
                    //TheNorthYanktonHeist.Funcs.fDebug.CopyToClipboard(fPlayer.ped.Heading.ToString() + "f");
                }
                GTA.Entity[] anyEntity = World.GetAllEntities();
                foreach (GTA.Entity entity in anyEntity)
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
                        fCore.Debug.CopyToClipboard(Final);
                    }

                    if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, entity) && e.KeyCode == Keys.O)
                    {
                    }

                    if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, entity) && e.KeyCode == Keys.L)
                    {
                        float heading = entity.Heading;
                        string heading2 = heading + "f";
                        string Final = heading2;
                        fCore.Debug.CopyToClipboard(Final);
                    }

                    if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, entity) && e.KeyCode == Keys.K)
                    {
                        if (entity.Model.IsInCdImage)
                        {
                            int modelHash = entity.Model.Hash;
                            fCore.Debug.CopyToClipboard(modelHash.ToString());
                        }
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
                        fCore.Debug.CopyToClipboard(Final);
                    }
                }
            }
        }

        private void onShutdown(object sender, EventArgs e)
        {
            bool flag = true;
            if (true == flag)
            {
                fCore.SceneManager.StopCurrentScene();
                debugProp?.Delete();
                debugProp = null;
                iLocal_1176?.Delete();
                iLocal_1176 = null;
                plane?.Delete();
                plane = null;
                debugBlip?.Delete();
                debugBlip = null;
                debugHeli?.Delete();
                debugHeli = null;
                cutscenePed1?.Delete();
                cutscenePed1 = null;
                cutscenePed2?.Delete();
                cutscenePed2 = null;
                cutscenePed3?.Delete();
                cutscenePed3 = null;
                debugCam?.Delete();
                debugCam = null;
                fWorld.Vehicle.DeleteList(fCore.Debug.DebugVehicles);
                fCore.Streaming.RemoveAnimDicts(fCore.Debug.DebugAnimDicts);
                fWorld.Object.DeleteObjectsInList(fCore.Debug.DebugProps);
                fWorld.Object.DeleteObjectsInList(boothProps);
                fBlip.Blip.ToggleShortRangeForLongRangeBlips(fBlip.Blip.GetLongRangeBlips(), false);
                if (Screen.IsFadedOut || Screen.IsFadingOut)
                    Screen.FadeIn(0);
                fUI.Hud.ClearAllHelpMessages();
                fUI.Hud.ClearBrief();
                fUI.Hud.ClearSubtitles();
                fUI.Hud.ClearHelp(true);
                fUI.Hud.ClearGPSMultiRoute();
                fPlayer.Player.SetWantedLevelTo0();
                fPlayer.Player.FakeWantedLevel = 0;
                fPlayer.Player.SetMaxWantedLevelToNormal();
                fUI.Hud.RadarAndHud(true, true);
                fUI.Graphics.AnimpostFXStopAll();
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
        static List<Prop> boothProps = new List<Prop>();
        public static void SpawnBooth()
        {
            if (boothProps.Count == 0)
            {
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.593f, -5181.164f, 84.52729f), new Vector3(0f, 180f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.589f, -5181.263f, 82.06731f), new Vector3(0f, 0f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.7f, -5181.164f, 84.52729f), new Vector3(0f, 180f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("prop_fncwood_02b"), new Vector3(5361.696f, -5181.263f, 82.06731f), new Vector3(0f, 0f, 90f), false, false);
                fWorld.Object.CreateObjectForList(boothProps, new Model("apa_mp_h_acc_rugwools_03"), new Vector3(5362.6f, -5180.766f, 82.17793f), new Vector3(0f, 0f, 0f), false, false);
            }

        }
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

        Vehicle plane;
        bool dlcPackWarningShown = false;
        /*
        InstructionalButtons warningButtons = new InstructionalButtons();
        InstructionalButtonContainer Continue = new InstructionalButtonContainer(InputControl.FrontendAccept, "OK");*/
        Scaleform warning = Scaleform.RequestMovie("POPUP_WARNING");
        bool instructionalButtonsSetUp = false;
        Ped[] allPeds = World.GetAllPeds();
        public static Prop iLocal_1176;
        //SynchronizedScene test = new SynchronizedScene(new Vector3(5297.9f, -5188.12f, 83.51837f), -68f);

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
