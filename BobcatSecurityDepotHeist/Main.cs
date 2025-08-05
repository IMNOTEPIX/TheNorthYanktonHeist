using BillsyLiamGTA.UI.Elements;
using BillsyLiamGTA.UI.Scaleform;
using BillsyLiamGTA.UI.Timerbars;
using BobcatSecurityDepotHeist;
using Global;
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
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static BillsyLiamGTA.UI.Elements.VariableTimer;
using static BobcatSecurityDepotHeist.Functions;
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

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (debug)
            {
                if (e.KeyCode == Keys.N)
                {
                    missionSwitch = 2;
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
                    fDebugStuff.CopyPlayerPosWithAddons();
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
                //Function.Call(Hash.ON_ENTER_MP);
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

        private void onTick(object sender, EventArgs e)
        {
            if (!ScriptSetup)
            {
                if (!Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE) && !Screen.IsFadingIn)
                {
                    //fInterior.MPMaps.LoadMpMaps();
                    GTA.Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
                    ScriptSetup = true;
                }
            }
            else
            {
                switch (Debug2)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
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

namespace BobcatSecurityDepotHeist
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
        bool WasDestroyed = false;
        bool SpawnedIn = false;
        bool fixedPosition = false;
        int num;

        private void OnTick(object sender, EventArgs e)
        {
            Screen.ShowSubtitle($"{Globals.missionSwitch}");
            if (Globals.missionSwitch == 0)
            {
                if (HeliBlip == null && fPlayer.GetDistanceTo(HeliPos) > 500f && !WasDestroyed && !SpawnedIn)
                {
                    HeliBlip = fBlip.CreateBlipForCoordWithParams(HeliPos, (BlipSprite)64, (BlipColor)5, 1f, "The Ludendorff Heist");
                }
                if (HeliBlip != null)
                {
                    if (StartHeli == null)
                    {
                        if (fPlayer.GetDistanceTo(HeliPos) > 200f)
                        {
                            if (fPlayer.GetDistanceTo(HeliPos) < 300f)
                            {
                                num = fMisc.GetRandomIntInRange(1, 7);
                                StartHeli = fVehicle.CreateVehicle("buzzard", HeliPos, HeliHeading);
                                SpawnedIn = true;
                            }
                        }

                    }
                    if (!SpawnedIn)
                    {
                        HeliBlip.DisplayType = BlipDisplayType.NoDisplay;
                    }
                    if (!SpawnedIn && fPlayer.GetDistanceTo(HeliPos) > 200f)
                    {
                        HeliBlip.DisplayType = BlipDisplayType.Default;
                    }
                    if (WasDestroyed && fPlayer.GetDistanceTo(HeliPos) > 200f)
                    {
                        WasDestroyed = false;
                        SpawnedIn = false;
                    }
                    if (StartHeli != null && StartHeli.IsConsideredDestroyed)
                    {
                        if (StartHeli.AttachedBlip != null)
                        {
                            StartHeli.AttachedBlip.Delete();
                        }
                        HeliBlip.DisplayType = BlipDisplayType.NoDisplay;
                        WasDestroyed = true;
                    }
                }
                if (StartHeli != null)
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
                    StartHeli.ToggleExtra(8, false);
                    StartHeli.ToggleExtra(9, false);
                    fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLAYER_BUZZARD"), StartHeli, fPlayer.ped);
                    fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLANE_ROCKET"), StartHeli, fPlayer.ped);
                    switch (num)
                    {
                        case 1:
                            fVehicle.SetVehicleColours(StartHeli, 111, 111);
                            break;
                        case 2:
                            fVehicle.SetVehicleColours(StartHeli, 8, 8);
                            break;
                        case 3:
                            fVehicle.SetVehicleColours(StartHeli, 99, 99);
                            break;
                        case 4:
                            fVehicle.SetVehicleColours(StartHeli, 69, 69);
                            break;
                        case 5:
                            fVehicle.SetVehicleColours(StartHeli, 90, 90);
                            break;
                        case 6:
                            fVehicle.SetVehicleColours(StartHeli, 33, 33);
                            break;
                        case 7:
                            fVehicle.SetVehicleColours(StartHeli, 147, 147);
                            break;
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
                        StartHeli.AttachedBlip.Name = "The Ludendorff Heist";
                        if (HeliBlip != null)
                            HeliBlip.DisplayType = BlipDisplayType.NoDisplay;
                        if (fPlayer.GetDistanceTo(StartHeli.Position) < 6f)
                        {
                            fHud.ClearAllHelpMessages();
                            fHud.ClearHelp(true);
                            fHud.DisplayHelpText("Press ~INPUT_ENTER~ to start The Ludendorff Heist.");
                            if (Game.IsControlJustPressed(GTA.Control.Enter))
                            {
                                fPlayer.ped.Task.ClearAll();
                                fPlayer.ped.Task.EnterVehicle(StartHeli, VehicleSeat.Driver);
                                if (fPlayer.ped.CurrentVehicle == StartHeli)
                                {
                                    Globals.missionSwitch = 1;
                                    if (HeliBlip != null)
                                    {
                                        HeliBlip.Delete();
                                        HeliBlip = null;
                                    }
                                    StartHeli.AttachedBlip.Alpha = 0;
                                }
                                else
                                {
                                    StartHeli.AttachedBlip.Alpha = 255;
                                }
                            }
                        }
                    }
                    if (fPlayer.GetDistanceTo(StartHeli.Position) > 440f)
                    {
                        if (StartHeli.AttachedBlip != null)
                        {
                            StartHeli.AttachedBlip.Delete();
                        }
                        StartHeli.Delete();
                        StartHeli = null;
                        SpawnedIn = false;
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
            if (!PlayerInArea)
                MissionFailCleanUp();
        }

        Vehicle StartHeli;
        Blip LudendorffNorthYankton;
        bool FailCountdownVisible = false;
        bool PlayerInArea = false;
        bool WarningMessageShown = false;
        VariableTimer failTimer = new VariableTimer(30000);
        CountdownTimerbar failCountdown;


        public event TimerExpired OnTimerExpired;

        private void onTick(object sender, EventArgs e)
        {
            switch (Globals.missionSwitch)
            {
                case 1:
                    fHud.ClearAllPrints();
                    fHud.ClearBrief();
                    fHud.ClearAllHelpMessages();
                    fHud.ClearGPSMultiRoute();
                    fHud.ClearHelp(true);
                    Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
                    Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.IdleStart);
                    fAudio.TriggerMusicEvent("MP_MC_START_VACUUM_8");
                    fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.Idle);
                    fMissionShard missionShard = new fMissionShard();
                    missionShard.Shard_In("The Ludendorff Heist", "Break into the Bobcat Security Depot in Ludendorff, and clean out the vault.");
                    missionShard = null;
                    Globals.missionSwitch = 2;
                    break;
                case 2:
                    new GlobalsController(null);
                    Globals.globalBlips = 2;
                    Globals.scriptTerminator = 2;
                    Screen.ShowSubtitle("Fly to ~y~Ludendorff, North Yankton~w~");
                    if (Start.StartHeli != null)
                    {
                        StartHeli = Start.StartHeli;
                    }
                    if (LudendorffNorthYankton == null)
                    {
                        LudendorffNorthYankton = fBlip.CreateBlipForCoordWithParams(new Vector3(1614f, 8098f, 259f), BlipSprite.Standard, (BlipColor)5, 1f, "Destination");
                    }
                    if (StartHeli != null)
                    {
                        MidsizedMessage failWarning = new MidsizedMessage("Leaving the Mission Area", "Return to the mission area.");
                        fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLAYER_BUZZARD"), StartHeli, fPlayer.ped);
                        fVehicle.DisableVehicleWeapon(true, fMisc.GetHashKey("VEHICLE_WEAPON_PLANE_ROCKET"), StartHeli, fPlayer.ped);
                        if (!fEntity.IsEntityInArea(fPlayer.ped, new Vector3(2000f, 6205f, -7f), new Vector3(1084f, 8888f, 525f)))
                        {
                            PlayerInArea = false;
                            if (!FailCountdownVisible)
                            {
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
                        if (fEntity.IsEntityInArea(fPlayer.ped, new Vector3(2000f, 6205f, -7f), new Vector3(1084f, 8888f, 525f)) && failTimer.Counter > 0 && FailCountdownVisible && !PlayerInArea)
                        {
                            Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
                            failCountdown.VariableTimer.RemoveTime(30001);
                            OnTimerExpired?.Invoke(this);
                            WarningMessageShown = false;
                            PlayerInArea = true;
                            FailCountdownVisible = false;
                        }
                        if (StartHeli.AttachedBlip != null)
                        {
                            if (fPlayer.ped.CurrentVehicle == StartHeli)
                            {
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
            }
            if (Start.StartHeli != null)
            {
                StartHeli = Start.StartHeli;
            }
        }

        void MissionFailCleanUp()
        {
            fHud.ClearAllPrints();
            fHud.ClearBrief();
            fHud.ClearAllHelpMessages();
            fHud.ClearGPSMultiRoute();
            fHud.ClearHelp(true);
            Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, false);
            Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, false);
            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.MusicStop);
            new GlobalsController(null);
            Globals.globalBlips = 3;
            Globals.scriptTerminator = 3;
            if (StartHeli != null)
            {
                if (StartHeli.AttachedBlip != null)
                {
                    StartHeli.AttachedBlip.Delete();
                }
                StartHeli.MarkAsNoLongerNeeded();
            }
            if (LudendorffNorthYankton != null)
            {
                LudendorffNorthYankton.Delete();
                LudendorffNorthYankton = null;
            }
            Globals.missionSwitch = 0;
            fMissionShard failShard = new fMissionShard();
            failShard.Shard_In("HEIST FAILED", "You left the mission area.", 6);
            failShard = null;
        }

        void CleanUp()
        {
            fAudio.ChangeMusicEventIntensity(fAudio.MusicEventIntensity.MusicStop);
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
        }

        private void onShutdown(object sender, EventArgs e)
        {
            CleanUp();
        }
    }
}
