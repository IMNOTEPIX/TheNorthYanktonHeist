using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TheNorthYanktonHeist.Extension;
using fAudio = IMNOTEPIX.Framework.fAudio;
using fCore = IMNOTEPIX.Framework.fCore;
using fUI = IMNOTEPIX.Framework.fUI;
using fWorld = IMNOTEPIX.Framework.fWorld;
using fPlayer = IMNOTEPIX.Framework.fPlayer;
using fInterior = IMNOTEPIX.Framework.fWorld.Interior;
using fBlip = IMNOTEPIX.Framework.fBlip;
using IMNOTEPIX.Framework.fWorld.Anims;
using Screen = GTA.UI.Screen;
using IMNOTEPIX.Framework.fGameplay.Combat;
using System.Net.Http;
using System.Security.Permissions;
using GTA.Input;

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
        static Prop debugProp2;

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
                            Controls.DisableAllControlActionsThisFrame(ControlType.PlayerControl);
                            warning.CallFunction("SHOW_POPUP_WARNING", -1, "WARNING", "North Yankton Heist Dlcpack Is Not Installed.", "Objective Subtitles Will Not Work Correctly.", true);
                            warningButtons.Draw();
                            if (Controls.IsDisabledControlJustPressed(ControlType.PlayerControl, ControlAction.FrontendAccept))
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
                        func_590();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
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
        int[] iLocal_1206 = new int[5];
        Vehicle[] iLocal_1050 = new Vehicle[7];
        string sLocal_559 = "ALPrologue";
        Vector3[] Local_130 = new Vector3[39];
        float[] fLocal_257 = new float[39];
        Vector3 Local_97 = new Vector3(0f, 0f, 0f);
        Ped[] iLocal_665 = new Ped[42];
        int iLocal_1224 = fWorld.Misc.joaat("WEAPON_PISTOL");
        RelationshipGroup iLocal_1230 = new RelationshipGroup(RelationshipGroupHash.HatesPlayer);
        int iLocal_1227 = fWorld.Misc.joaat("WEAPON_PUMPSHOTGUN");
        int iLocal_1225 = fWorld.Misc.joaat("WEAPON_SMG");
        int iLocal_545 = 0;
        int iLocal_546 = 0;
        int iLocal_547 = 0;
        int iLocal_548 = 0;
        int iLocal_74 = 0;
        bool bLocal_1984 = false;
        int iLocal_37 = 0;
        int iLocal_36 = 0;
        int iLocal_1985 = 0;
        int iLocal_1986 = 0;
        int iLocal_1987 = 0;
        int iLocal_1241 = -1;
        bool[] func_761 = new bool[999];
        int joaat(string str) => fWorld.Misc.joaat(str);
        Vehicle func_485(int iParam1, Vector3 Param2, float fParam5, int iParam6, float fParam7)
        {
            Vehicle iParam0 = null;

            if (!fWorld.Entity.DoesEntityExist(iParam0))
            {
                iParam0 = fWorld.Vehicle.CreateVehicle(iParam1, Param2, fParam5);
                if (iParam6 >= 0)
                {
                    fWorld.Vehicle.SetVehicleColours(iParam0, iParam6, iParam6);
                }
                fWorld.Vehicle.SetVehicleDirtLevel(iParam0, fParam7);
                fWorld.Entity.SetEntityShouldFreezeWaitingOnCollision(iParam0, true);
            }
            return iParam0;
        }
        float func_472(Vector3 Param0)
        {
            return Param0.Z;
        }
        Ped func_470(int iParam1, Vector3 Param2, float fParam5, int iParam6)//Position - 0x72D08
        {
            Ped iParam0 = null;

            if (!fWorld.Entity.DoesEntityExist(iParam0))
            {
                iParam0 = fWorld.Ped.CreatePed(iParam1, Param2, fParam5);
                if (iParam6 == 1)
                {
                    fWorld.Ped.SetPedDefaultComponentVariation(iParam0);
                }
                fWorld.Entity.SetEntityShouldFreezeWaitingOnCollision(iParam0, true);
            }
            return iParam0;
        }
        void func_715()//Position - 0x95DE2
        {
            Local_130[0 /*3*/] = new Vector3(5353.823f, -5184.7437f, 81.762f);
            fLocal_257[0] = 169.0588f;
            Local_130[1 /*3*/] = new Vector3(5355.1685f, -5182.404f, 81.762f);
            fLocal_257[1] = 61.0588f;
            Local_130[2 /*3*/] = new Vector3(5353.92f, -5190.8887f, 81.762f);
            fLocal_257[2] = 130.9412f;
            Local_130[3 /*3*/] = new Vector3(5352.0396f, -5188.7974f, 81.762f);
            fLocal_257[3] = 21.5294f;
            Local_130[4 /*3*/] = new Vector3(5353.89f, -5192.23f, 81.762f);
            fLocal_257[4] = 115.6477f;
            Local_130[5 /*3*/] = new Vector3(5343.47f, -5196.2905f, 81.762f);
            fLocal_257[5] = 358.9412f;
            Local_130[6 /*3*/] = new Vector3(5382.5513f, -5176.6104f, 80.4568f);
            fLocal_257[6] = 108.5755f;
            Local_130[7 /*3*/] = new Vector3(5381.8916f, -5175.5967f, 80.4709f);
            fLocal_257[7] = 123.564f;
            Local_130[8 /*3*/] = new Vector3(5381.189f, -5173.658f, 80.4267f);
            fLocal_257[8] = 117.5804f;
            Local_130[9 /*3*/] = new Vector3(5389.69f, -5191.0996f, 80.2098f);
            fLocal_257[9] = 143.6466f;
            Local_130[10 /*3*/] = new Vector3(5390.5415f, -5186.277f, 79.9868f);
            fLocal_257[10] = 59.6471f;
            Local_130[11 /*3*/] = new Vector3(5410.387f, -5182.0073f, 78.6563f);
            fLocal_257[11] = 135.1765f;
            Local_130[12 /*3*/] = new Vector3(5408.598f, -5180.1704f, 78.6633f);
            fLocal_257[12] = 133.0588f;
            Local_130[13 /*3*/] = new Vector3(5419.569f, -5171.7134f, 77.9652f);
            fLocal_257[13] = 201.5294f;
            Local_130[14 /*3*/] = new Vector3(5411.251f, -5179.0444f, 78.4814f);
            fLocal_257[14] = 45.5294f;
            Local_130[15 /*3*/] = new Vector3(5432.578f, -5151.1963f, 77.1536f);
            fLocal_257[15] = 104.1176f;
            Local_130[16 /*3*/] = new Vector3(5431.671f, -5148.8687f, 77.062f);
            fLocal_257[16] = 106.2353f;
            Local_130[17 /*3*/] = new Vector3(5410.832f, -5177.5654f, 79.483f);
            fLocal_257[17] = 115.8309f;
            Local_130[18 /*3*/] = new Vector3(3497.6106f, -4872.6333f, 110.5807f);
            fLocal_257[18] = 262.9412f;
            Local_130[19 /*3*/] = new Vector3(3497.8025f, -4870.141f, 110.7024f);
            fLocal_257[19] = 266.0508f;
            Local_130[20 /*3*/] = new Vector3(3494.8452f, -4869.093f, 110.7144f);
            fLocal_257[20] = 279.8827f;
            Local_130[21 /*3*/] = new Vector3(3494.4653f, -4866.5806f, 110.7753f);
            fLocal_257[21] = 280.5879f;
            Local_130[22 /*3*/] = new Vector3(3496.471f, -4865.463f, 110.7407f);
            fLocal_257[22] = 226.2353f;
            Local_130[23 /*3*/] = new Vector3(3495.5537f, -4864.558f, 110.7269f);
            fLocal_257[23] = 222f;
            Local_130[24 /*3*/] = new Vector3(3530.2092f, -4673.253f, 113.2062f);
            fLocal_257[24] = 274.9412f;
            Local_130[25 /*3*/] = new Vector3(3532.4717f, -4673.429f, 113.2055f);
            fLocal_257[25] = 189.5294f;
            Local_130[26 /*3*/] = new Vector3(3533.283f, -4671.6245f, 113.206f);
            fLocal_257[26] = 213.2454f;
            Local_130[27 /*3*/] = new Vector3(3536.7886f, -4671.368f, 113.2061f);
            fLocal_257[27] = 270.7059f;
            Local_130[28 /*3*/] = new Vector3(3533.1016f, -4666.006f, 113.1611f);
            fLocal_257[28] = 176.8235f;
            Local_130[29 /*3*/] = new Vector3(3542.2075f, -4660.9946f, 113.4237f);
            fLocal_257[29] = 131.647f;
            Local_130[30 /*3*/] = new Vector3(3546.6194f, -4659.553f, 113.1374f);
            fLocal_257[30] = 212.8229f;
            Local_130[31 /*3*/] = new Vector3(3550.46f, -4651.6206f, 113.2091f);
            fLocal_257[31] = 92.1176f;
            Local_130[32 /*3*/] = new Vector3(3552.158f, -4654.5864f, 113.2239f);
            fLocal_257[32] = 174.4004f;
            Local_130[33 /*3*/] = new Vector3(3548.897f, -4650.2505f, 113.2084f);
            fLocal_257[33] = 177.3907f;
            Local_130[34 /*3*/] = new Vector3(3541.2754f, -4662.743f, 113.3224f);
            fLocal_257[34] = 255.1765f;
            Local_130[35 /*3*/] = new Vector3(3514.9692f, -4655.0806f, 113.5005f);
            fLocal_257[35] = 226.6858f;
            Local_130[36 /*3*/] = new Vector3(3544.4329f, -4643.0356f, 113.1429f);
            fLocal_257[36] = 233.6201f;
            Local_130[37 /*3*/] = new Vector3(3560.461f, -4635.5728f, 113.8873f);
            fLocal_257[37] = 166.5792f;
            Local_130[38 /*3*/] = new Vector3(3510.5347f, -4675.332f, 113.2635f);
            fLocal_257[38] = 320.8466f;
        }
        void func_469(Ped uParam0)//Position - 0x72C67
        {
            if (!fWorld.Entity.IsEntityDead(uParam0, false))
            {
                fWorld.Ped.SetPedRandomComponentVariation(uParam0, 0);
                uParam0.RelationshipGroup = iLocal_1230;
                //fWorld.Ped.SetRelationshipBetweenGroups(fWorld.Ped.RelationshipTypes.Hate, fPlayer.Player.Character.RelationshipGroup.Hash, iLocal_1230);
                //fWorld.Ped.SetRelationshipBetweenGroups(fWorld.Ped.RelationshipTypes.Hate, iLocal_1230, fPlayer.Player.Character.RelationshipGroup.Hash);
                fWorld.Ped.SetPedAsEnemy(uParam0, true);
                fWorld.Entity.SetEntityHealth(uParam0, 200);
                fWorld.Ped.SetPedMaxHealth(uParam0, 200);
                fWorld.Ped.SetPedDiesWhenInjured(uParam0, true);
                fWorld.Ped.SetPedAccuracy(uParam0, 1);
                fWorld.Weapon.GiveWeaponToPed(uParam0, iLocal_1224, -1, true, true);
                fWorld.Ped.SetPedCombatMovement(uParam0, 2);
                fWorld.Ped.SetPedCombatAttributes(uParam0, 6, false);
                fWorld.Ped.SetPedCombatAttributes(uParam0, 3, false);
                fWorld.Ped.SetPedCombatAttributes(uParam0, 1, false);
                fWorld.Ped.SetPedConfigFlag(uParam0, 188, true);
                fWorld.Ped.SetPedConfigFlag(uParam0, 249, true);
                fWorld.Ped.SetPedConfigFlag(uParam0, 272, true);
            }
        }
        void func_467(int iParam0)//Position - 0x72B68
        {
            int iVar0;

            if (!func_471(Local_130[iParam0 /*3*/], Local_97, false))
            {
                iVar0 = joaat("S_M_M_SnowCop_01");
                iLocal_665[iParam0] = func_470(iVar0, Local_130[iParam0 /*3*/], fLocal_257[iParam0], 1);
                func_469(iLocal_665[iParam0]);
                fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[iParam0], true);
            }
        }
        bool func_471(Vector3 Param0, Vector3 Param3, bool bParam6)//Position - 0x72D43
        {
            if (bParam6)
            {
                return (Param0.X == Param3.X && Param0.Y == Param3.Y);
            }
            return ((Param0.X == Param3.X && Param0.Y == Param3.Y) && Param0.Z == Param3.Z);
        }
        bool func_575(Ped iParam0)//Position - 0x7DFE8
        {
            if (fWorld.Entity.DoesEntityExist(iParam0))
            {
                if (fWorld.Ped.IsPedInjured(iParam0))
                {
                    return true;
                }
            }
            return false;
        }
        bool func_654(Entity uParam0)//Position - 0x87A4A
        {
            if (fWorld.Entity.DoesEntityExist(uParam0))
            {
                if (fWorld.Entity.IsEntityDead(uParam0, false))
                {
                    return true;
                }
            }
            return false;
        }
        bool func_658(int iParam0, string sParam1)//Position - 0x87FFE
        {
            fWorld.VehicleRecording.RequestVehicleRecording(iParam0, sParam1);
            if (!fWorld.VehicleRecording.HasVehicleRecordingBeenLoaded(iParam0, sParam1))
                Script.Yield();
            if (fWorld.VehicleRecording.HasVehicleRecordingBeenLoaded(iParam0, sParam1))
            {
                return true;
            }
            else
                return false;
        }
        void func_590()//Position - 0x80A84
        {
            int iVar0;
            int iVar1;

            int iVar46;
            int iVar47;
            int iVar48;
            int iVar49;
            int iVar50;
            int iVar51;
            int iVar52;

            if (!func_761[0])
            {
                fCore.Timer.SetTimerA(0);
                func_658(3, sLocal_559);
                func_658(4, sLocal_559);
                func_658(5, sLocal_559);
                func_658(6, sLocal_559);
                func_658(8, sLocal_559);
                func_658(9, sLocal_559);
                func_658(11, sLocal_559);
                func_715();
                Game.Player.SetControlState(true, SetPlayerControlFlags.AmbientScript);
                if (!fAudio.Audio.IsAudioSceneActive("PROLOGUE_POLICE_SHOOTOUT"))
                {
                    fAudio.Audio.StartAudioScene("PROLOGUE_POLICE_SHOOTOUT");
                }
                fUI.Hud.RadarAndHud(true, true);
                if (!Function.Call<bool>(Hash.DOES_SCRIPTED_COVER_POINT_EXIST_AT_COORDS, 5336.8643f, -5193.5386f, 81.9129f))
                {
                    iLocal_1206[0] = Function.Call<int>(Hash.ADD_COVER_POINT, 5336.8174f, -5193.3325f, 81.911f, 270.8514f, 1, 0, 1, false);
                }
                if (!Function.Call<bool>(Hash.DOES_SCRIPTED_COVER_POINT_EXIST_AT_COORDS, 5332.273f, -5185.271f, 81.7757f))
                {
                    iLocal_1206[1] = Function.Call<int>(Hash.ADD_COVER_POINT, 5332.273f, -5185.271f, 81.7757f, 276.6267f, 0, 2, 3, false);
                }
                if (!Function.Call<bool>(Hash.DOES_SCRIPTED_COVER_POINT_EXIST_AT_COORDS, 5332.1934f, -5191.9116f, 81.7758f))
                {
                    iLocal_1206[2] = Function.Call<int>(Hash.ADD_COVER_POINT, 5332.1934f, -5191.9116f, 81.7758f, 276.6232f, 1, 2, 3, false);
                }
                if (!Function.Call<bool>(Hash.DOES_SCRIPTED_COVER_POINT_EXIST_AT_COORDS, 5326.3535f, -5185.2485f, 81.7796f))
                {
                    iLocal_1206[3] = Function.Call<int>(Hash.ADD_COVER_POINT, 5326.3535f, -5185.2485f, 81.7796f, 276.997f, 0, 2, 3, false);
                }
                if (!Function.Call<bool>(Hash.DOES_SCRIPTED_COVER_POINT_EXIST_AT_COORDS, 5326.3535f, -5191.832f, 81.7743f))
                {
                    iLocal_1206[4] = Function.Call<int>(Hash.ADD_COVER_POINT, 5326.3535f, -5191.832f, 81.7743f, 277.2627f, 1, 2, 3, false);
                }
                fWorld.Ped.SetPedUsingActionMode(fPlayer.Player.Character, true, -1, null);
                fWorld.Entity.SetEntityProofs(fPlayer.Player.Character, false, false, false, false, false, true, false, false);
                fWorld.Ped.SetPedUpperBodyDamageOnly(fPlayer.Player.Character, true);
                iLocal_1050[0] = func_485(joaat("policeold2"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(3, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(3, 0f, sLocal_559)), -1, 1097859072f);
                iLocal_1050[1] = func_485(joaat("policeold2"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(4, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(4, 0f, sLocal_559)), -1, 1097859072f);
                iLocal_1050[2] = func_485(joaat("policeold1"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(5, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(5, 0f, sLocal_559)), -1, 1097859072f);
                iLocal_1050[3] = func_485(joaat("policeold1"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(6, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(6, 0f, sLocal_559)), -1, 1097859072f);
                iLocal_1050[4] = func_485(joaat("policeold2"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(8, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(8, 0f, sLocal_559)), -1, 1097859072f);
                iLocal_1050[5] = func_485(joaat("policeold2"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(9, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(9, 0f, sLocal_559)), -1, 1097859072f);
                iLocal_1050[6] = func_485(joaat("policeold2"), new Vector3(5505.63f, -5128.1724f, 77.3763f), 87.963f, -1, 1097859072f);
                iVar0 = 0;
                while (iVar0 < iLocal_1050.Length)
                {
                    if (!fWorld.Entity.IsEntityDead(iLocal_1050[iVar0], false))
                    {
                        fWorld.Vehicle.SetVehicleStrong(iLocal_1050[iVar0], true);
                        fWorld.Vehicle.SetVehicleConsideredByPlayer(iLocal_1050[iVar0], false);
                        fWorld.Entity.SetEntityOnlyDamagedByPlayer(iLocal_1050[iVar0], true);
                        fWorld.Entity.FreezeEntityPosition(iLocal_1050[iVar0], true);
                    }
                    iVar0++;
                }
                iVar0 = 0;
                while (iVar0 < 17)
                {
                    if (iVar0 != 7)
                    {
                        func_467(iVar0);
                        if ((iVar0 == 0 || iVar0 == 3) || iVar0 == 7)
                        {
                            fWorld.Weapon.GiveWeaponToPed(iLocal_665[iVar0], iLocal_1225, -1, true, true);
                        }
                        if ((((iVar0 == 4 || iVar0 == 5) || iVar0 == 9) || iVar0 == 14) || iVar0 == 16)
                        {
                            fWorld.Weapon.GiveWeaponToPed(iLocal_665[iVar0], iLocal_1227, -1, true, true);
                        }
                        if (iVar0 >= 0 && iVar0 <= 8)
                        {
                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[iVar0], 4, false);
                            fWorld.Ped.SetCombatFloat(iLocal_665[iVar0], 5, 1f);
                        }
                    }
                    iVar0++;
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[0]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[0], iLocal_1050[0], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[1]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[1], iLocal_1050[0], 0);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[2]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[2], iLocal_1050[1], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[3]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[3], iLocal_1050[1], 0);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[4]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[4], iLocal_1050[2], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[5]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[5], iLocal_1050[2], 0);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[9]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[9], iLocal_1050[3], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[10]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[10], iLocal_1050[3], 0);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[11]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[11], iLocal_1050[4], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[12]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[12], iLocal_1050[4], 0);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[13]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[13], iLocal_1050[5], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[14]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[14], iLocal_1050[5], 0);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[15]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[15], iLocal_1050[6], -1);
                }
                if (fWorld.Entity.DoesEntityExist(iLocal_665[16]))
                {
                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[16], iLocal_1050[6], 0);
                }
                Function.Call(Hash.ADD_COVER_BLOCKING_AREA, 5370.142f, -5212.455f, 78.9143f, 5359.361f, -5191.83f, 86.8785f, true, true, true, true);
                Function.Call(Hash.ADD_COVER_BLOCKING_AREA, 5375.438f, -5180.967f, 79.4894f, 5360.391f, -5161.3555f, 88.813f, true, true, true, true);
                Function.Call(Hash.ADD_COVER_BLOCKING_AREA, 5366.0146f, -5162.681f, 81.0458f, 5274.0205f, -5152.04f, 89.9633f, true, true, true, true);
                Function.Call(Hash.ADD_COVER_BLOCKING_AREA, 5397.5415f, -5174.056f, 79.035f, 5373.813f, -5160.871f, 87.0641f, true, true, true, true);
                Function.Call(Hash.ADD_COVER_BLOCKING_AREA, 5428.415f, -5193.0215f, 76.6645f, 5411.4014f, -5185.282f, 82.3215f, true, true, true, true);
                iLocal_545 = Function.Call<int>(Hash.ADD_NAVMESH_BLOCKING_OBJECT, 5324.8066f, -5130.89f, 75.4401f, 90f, 70f, 15f, 0f, false, 7);
                iLocal_546 = Function.Call<int>(Hash.ADD_NAVMESH_BLOCKING_OBJECT, 5370.8306f, -5156.042f, 80.358f, 19.5f, 50f, 15f, 0f, false, 7);
                iLocal_547 = Function.Call<int>(Hash.ADD_NAVMESH_BLOCKING_OBJECT, 5370.3306f, -5226.042f, 80.358f, 15f, 50f, 15f, 0f, false, 7);
                iLocal_548 = Function.Call<int>(Hash.ADD_NAVMESH_BLOCKING_OBJECT, 5357.09f, -5164.394f, 82.90531f, 8f, 5f, 3f, 0f, false, 7);
                iLocal_74 = Game.GameTime + 7500;
                func_761[0] = true;
            }
            else
            {
                bLocal_1984 = true;
                iVar47 = 0;
                iVar49 = 0;
                iVar46 = 0;
                while (iVar46 < 9)
                {
                    if (!fWorld.Ped.IsPedInjured(iLocal_665[iVar46]))
                    {
                        bLocal_1984 = false;
                        if (fWorld.Entity.IsEntityAtCoord(iLocal_665[iVar46], 5323.8535f, -5194.2f, 93.5186f, 41f, 36f, 13f, false, true, 0))
                        {
                            iVar49 = 1;
                        }
                    }
                    else
                    {
                        iVar47++;
                    }
                    iVar46++;
                }
                if (iLocal_37 < (iLocal_1050.Length - 1))
                {
                    iLocal_37++;
                }
                else
                {
                    iLocal_37 = 0;
                }
                iVar46 = iLocal_37;
                if (!fWorld.Entity.IsEntityDead(iLocal_1050[iVar46], false))
                {
                    if (fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[iVar46]))
                    {
                        if (fWorld.Entity.DoesEntityExist(fWorld.Vehicle.GetPedInVehicleSeat(iLocal_1050[iVar46], -1, false)) && fWorld.Ped.IsPedInjured(fWorld.Vehicle.GetPedInVehicleSeat(iLocal_1050[iVar46], -1, false)))
                        {
                            fWorld.VehicleRecording.StopPlaybackRecordedVehicle(iLocal_1050[iVar46]);
                        }
                    }
                }
                if (iLocal_36 < (iLocal_665.Length - 1))
                {
                    iLocal_36++;
                }
                else
                {
                    iLocal_36 = 0;
                }
                iVar46 = iLocal_36;
                iVar46 = 0;
                while (iVar46 < iLocal_665.Length)
                {
                    if (!fWorld.Ped.IsPedInjured(iLocal_665[iVar46]))
                    {
                        if (fWorld.Ped.IsPedInAnyVehicle(iLocal_665[iVar46], true))
                        {
                            fWorld.Ped.SetPedResetFlag(iLocal_665[iVar46], 282, true);
                        }
                    }
                    iVar46++;
                }
                if (!func_761[16] && fCore.Timer.TimerA() > 1000)
                {
                    fPlayer.Player.FakeWantedLevel = 5;
                    fWorld.Entity.FreezeEntityPosition(iLocal_1050[1], false);
                    fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[1], 4, sLocal_559, true);
                    fWorld.Vehicle.SetVehicleSiren(iLocal_1050[1], true);
                    fWorld.VehicleRecording.ForcePlaybackRecordedVehicleUpdate(iLocal_1050[1], true);
                    fWorld.Entity.FreezeEntityPosition(iLocal_1050[0], false);
                    fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[0], 3, sLocal_559, true);
                    fWorld.VehicleRecording.PausePlaybackRecordedVehicle(iLocal_1050[0]);
                    fWorld.VehicleRecording.ForcePlaybackRecordedVehicleUpdate(iLocal_1050[0], true);
                    fWorld.Entity.FreezeEntityPosition(iLocal_1050[2], false);
                    fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[2], 5, sLocal_559, true);
                    fWorld.VehicleRecording.PausePlaybackRecordedVehicle(iLocal_1050[2]);
                    fWorld.VehicleRecording.ForcePlaybackRecordedVehicleUpdate(iLocal_1050[2], true);
                    func_761[16] = true;
                }
                if (!func_761[17])
                {
                    if (fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, 5334.1455f, -5195.717f, 80.342f, 5324.581f, -5173.136f, 87.0081f, 10f, false, true, 0) || Game.GameTime > iLocal_74)
                    {
                        fWorld.VehicleRecording.UnpausePlaybackRecordedVehicle(iLocal_1050[0]);
                        fWorld.Vehicle.SetVehicleSiren(iLocal_1050[0], true);
                        fWorld.VehicleRecording.UnpausePlaybackRecordedVehicle(iLocal_1050[2]);
                        fWorld.Vehicle.SetVehicleSiren(iLocal_1050[2], true);
                        if (!fWorld.Ped.IsPedInjured(iLocal_665[6]))
                        {
                            fWorld.Ped.SetPedSphereDefensiveArea(iLocal_665[6], 5366.3364f, -5184.4507f, 81.5742f, 2f, true, false);
                            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[6], 100f, 0);
                        }
                        if (!fWorld.Ped.IsPedInjured(iLocal_665[8]))
                        {
                            fWorld.Ped.SetPedCombatMovement(iLocal_665[8], 2);
                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[8], 50, true);
                            fWorld.Ped.SetPedConfigFlag(iLocal_665[8], 286, true);
                            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[8], 100f, 0);
                        }
                        func_761[17] = true;
                    }
                }
                else
                {
                    if (!fWorld.Ped.IsPedInjured(iLocal_665[6]))
                    {
                        if (!func_761[1])
                        {
                            if ((fPlayer.Player.GetDistanceTo(iLocal_665[6].Position) < 10f || fWorld.Entity.GetEntityHealth(iLocal_665[6]) < fWorld.Ped.GetPedMaxHealth(iLocal_665[6])) || iVar47 >= 3)
                            {
                                fWorld.Ped.RemovePedDefensiveArea(iLocal_665[6], false);
                                fWorld.Ped.SetPedCombatMovement(iLocal_665[6], 2);
                                fWorld.Ped.SetPedCombatAttributes(iLocal_665[6], 50, true);
                                Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[6], 100f, 0);
                                func_761[1] = true;
                            }
                        }
                    }
                    if (!fWorld.Entity.IsEntityDead(iLocal_665[0], false))
                    {
                        if (!func_761[3])
                        {
                            if (fPlayer.Player.GetDistanceTo(iLocal_665[0].Position) < 8f)
                            {
                                fWorld.Ped.RemovePedDefensiveArea(iLocal_665[0], false);
                            }
                            func_761[3] = true;
                        }
                    }
                }
                switch (iLocal_1985)
                {
                    case 0:
                        if (func_761[17])
                        {
                            if (!func_761[244])
                            {
                                if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[0]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[0]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(3, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[0]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[0], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(3, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                {
                                    fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[0], true);
                                    func_761[244] = true;
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[0]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[0]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(3, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[0]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[0], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(3, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_575(iLocal_665[0]))
                                {
                                    if (!func_761[29])
                                    {
                                        fWorld.Ped.SetPedSphereDefensiveArea(iLocal_665[0], 5344.2725f, -5180.306f, 81.7773f, 2f, true, false);
                                        Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[0], 100f, 0);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[0], 1, false);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[0], 3, true);
                                        func_761[29] = true;
                                    }
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[0]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[0]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(3, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[0]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[0], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(3, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_575(iLocal_665[1]))
                                {
                                    if (!func_761[30])
                                    {
                                        fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[1], false);
                                        Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[1], 1000f, 0);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[1], 1, false);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[1], 3, true);
                                        func_761[30] = true;
                                    }
                                }
                            }
                        }
                        if (func_761[16])
                        {
                            if (!func_761[245])
                            {
                                if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[1]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[1]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(4, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[1]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[1], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(4, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                {
                                    fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[1], true);
                                    fWorld.Vehicle.SetVehicleSiren(iLocal_1050[1], false);
                                    func_761[245] = true;
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[1]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[1]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(4, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[1]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[1], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(4, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_575(iLocal_665[2]))
                                {
                                    if (!func_761[31])
                                    {
                                        Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[2], 100f, 0);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[2], 1, false);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[2], 3, true);
                                        func_761[31] = true;
                                    }
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[1]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[1]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(4, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[1]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[1], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(4, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_575(iLocal_665[3]))
                                {
                                    if (!func_761[32])
                                    {
                                        fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[3], false);
                                        Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[3], 1000f, 0);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[3], 1, false);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[3], 3, true);
                                        func_761[32] = true;
                                    }
                                }
                            }
                        }
                        if (func_761[17])
                        {
                            if (!func_761[246])
                            {
                                if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[2]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[2]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(5, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[2]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[2], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(5, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                {
                                    fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[2], true);
                                    fWorld.Vehicle.SetVehicleSiren(iLocal_1050[2], false);
                                    func_761[246] = true;
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[2]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[2]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(5, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[2]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[2], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(5, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_575(iLocal_665[4]))
                                {
                                    if (!func_761[33])
                                    {
                                        fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[4], false);
                                        Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[4], 1000f, 0);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[4], 1, false);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[4], 3, true);
                                        func_761[33] = true;
                                    }
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[2]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[2]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(5, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[2]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[2], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(5, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_575(iLocal_665[5]))
                                {
                                    if (!func_761[34])
                                    {
                                        Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[5], 100f, 0);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[5], 1, false);
                                        fWorld.Ped.SetPedCombatAttributes(iLocal_665[5], 3, true);
                                        func_761[34] = true;
                                    }
                                }
                            }
                        }
                        if ((((((func_575(iLocal_665[0]) || !fWorld.Ped.IsPedInVehicle(iLocal_665[0], iLocal_1050[0], false)) && (func_575(iLocal_665[1]) || !fWorld.Ped.IsPedInVehicle(iLocal_665[1], iLocal_1050[0], false))) && (func_575(iLocal_665[2]) || !fWorld.Ped.IsPedInVehicle(iLocal_665[2], iLocal_1050[1], false))) && (func_575(iLocal_665[3]) || !fWorld.Ped.IsPedInVehicle(iLocal_665[3], iLocal_1050[1], false))) && (func_575(iLocal_665[4]) || !fWorld.Ped.IsPedInVehicle(iLocal_665[4], iLocal_1050[2], false))) && (func_575(iLocal_665[5]) || !fWorld.Ped.IsPedInVehicle(iLocal_665[5], iLocal_1050[2], false)))
                        {
                            iLocal_1985++;
                        }
                        break;
                }
                switch (iLocal_1986)
                {
                    case 0:
                        if (bLocal_1984 == true && !fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, 5338.454f, -5212.938f, 81.762024f, 5338.228f, -5161.655f, 86.762024f, 35f, false, true, 0))
                        {
                            if (!func_654(iLocal_1050[0]))
                            {
                                if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[0]) < 500f)
                                {
                                    fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[0], 500f);
                                }
                                if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[0]) < 500f)
                                {
                                    fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[0], 500f);
                                }
                                fWorld.Entity.SetEntityProofs(iLocal_1050[0], false, false, false, false, false, false, false, false);
                            }
                            if (!func_654(iLocal_1050[1]))
                            {
                                if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[1]) < 500f)
                                {
                                    fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[1], 500f);
                                }
                                if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[1]) < 500f)
                                {
                                    fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[1], 500f);
                                }
                                fWorld.Entity.SetEntityProofs(iLocal_1050[1], false, false, false, false, false, false, false, false);
                            }
                            if (!func_654(iLocal_1050[2]))
                            {
                                if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[2]) < 500f)
                                {
                                    fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[2], 500f);
                                }
                                if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[2]) < 500f)
                                {
                                    fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[2], 500f);
                                }
                                fWorld.Entity.SetEntityProofs(iLocal_1050[2], false, false, false, false, false, false, false, false);
                            }
                            fCore.Timer.SetTimerB(0);
                            iLocal_1986++;
                        }
                        break;

                    case 1:
                        if (fWorld.Ped.IsPedInjured(iLocal_665[9]) && fWorld.Ped.IsPedInjured(iLocal_665[10]))
                        {
                            iLocal_1986++;
                        }
                        break;
                }
                if (iLocal_1987 > 0)
                {
                    if (!func_761[247])
                    {
                        if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[3]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[3]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(6, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[3]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[3], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(6, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                        {
                            fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[3], true);
                            func_761[247] = true;
                        }
                    }
                    if (!func_761[295])
                    {
                        if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[3]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[3]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(6, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[3]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[3], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(6, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                        {
                            if (!fWorld.Ped.IsPedInjured(iLocal_665[9]))
                            {
                                Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[9], 100f, 0);
                                fWorld.Ped.SetPedCombatAttributes(iLocal_665[9], 1, false);
                                fWorld.Ped.SetPedCombatAttributes(iLocal_665[9], 3, true);
                                func_761[295] = true;
                            }
                        }
                    }
                    if (!func_761[288])
                    {
                        if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[3]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[3]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(6, sLocal_559) / 100f) * 99f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[3]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[3], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(6, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                        {
                            if (!fWorld.Ped.IsPedInjured(iLocal_665[10]))
                            {
                                Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[10], 100f, 0);
                                fWorld.Ped.SetPedCombatAttributes(iLocal_665[10], 1, false);
                                fWorld.Ped.SetPedCombatAttributes(iLocal_665[10], 3, true);
                                fWorld.Entity.SetEntityOnlyDamagedByPlayer(iLocal_665[10], true);
                                fWorld.Ped.SetPedMaxHealth(iLocal_665[10], 215);
                                fWorld.Entity.SetEntityHealth(iLocal_665[10], 215);
                                func_761[288] = true;
                            }
                        }
                    }
                }
                switch (iLocal_1987)
                {
                    case 0:
                        if (bLocal_1984 == true)
                        {
                            fWorld.Entity.FreezeEntityPosition(iLocal_1050[3], false);
                            fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[3], 6, sLocal_559, true);
                            fWorld.Vehicle.SetVehicleSiren(iLocal_1050[3], true);
                            fWorld.VehicleRecording.SkipTimeInPlaybackRecordedVehicle(iLocal_1050[3], 1500f);
                            iLocal_1987++;
                        }
                        break;

                    case 1:
                        if (fWorld.Entity.DoesEntityExist(iLocal_1050[3]) && fWorld.Entity.DoesEntityExist(fPlayer.Player.Character))
                        {
                            if (fPlayer.Player.GetDistanceTo(iLocal_1050[3].Position) <= 17.5f || (fWorld.Ped.IsPedInjured(iLocal_665[9]) && fWorld.Ped.IsPedInjured(iLocal_665[10])))
                            {
                                fWorld.Entity.FreezeEntityPosition(iLocal_1050[4], false);
                                fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[4], 8, sLocal_559, true);
                                fWorld.Vehicle.SetVehicleSiren(iLocal_1050[4], true);
                                fWorld.VehicleRecording.SkipTimeInPlaybackRecordedVehicle(iLocal_1050[4], 3000f);
                                fWorld.Entity.FreezeEntityPosition(iLocal_1050[5], false);
                                fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[5], 9, sLocal_559, true);
                                fWorld.Vehicle.SetVehicleSiren(iLocal_1050[5], true);
                                fWorld.VehicleRecording.SkipTimeInPlaybackRecordedVehicle(iLocal_1050[4], 2000f);
                                iLocal_1987++;
                            }
                        }
                        break;

                    case 2:
                        if (!func_761[248])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[4]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(8, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[4], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(8, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[4], true);
                                fWorld.Vehicle.SetVehicleSiren(iLocal_1050[4], false);
                                func_761[248] = true;
                            }
                        }
                        if (!func_761[289])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[4]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(8, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[4], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(8, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!fWorld.Ped.IsPedInjured(iLocal_665[11]))
                                {
                                    fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[11], false);
                                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[11], 1000f, 0);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[11], 1, false);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[11], 3, true);
                                    func_761[289] = true;
                                }
                            }
                        }
                        if (!func_761[290])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[4]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(8, sLocal_559) / 100f) * 99f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[4], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(8, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!fWorld.Ped.IsPedInjured(iLocal_665[12]))
                                {
                                    fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[12], false);
                                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[12], 1000f, 0);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[12], 1, false);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[12], 3, true);
                                    func_761[290] = true;
                                }
                            }
                        }
                        if (!func_761[249])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[5]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(9, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[5], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(9, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[5], true);
                                func_761[249] = true;
                            }
                        }
                        if (!func_761[291])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[5]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(9, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[5], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(9, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!fWorld.Ped.IsPedInjured(iLocal_665[13]))
                                {
                                    fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[13], false);
                                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[13], 1000f, 0);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[13], 1, false);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[13], 3, true);
                                    func_761[291] = true;
                                }
                            }
                        }
                        if (!func_761[292])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[5]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(9, sLocal_559) / 100f) * 99f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[5], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(9, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!fWorld.Ped.IsPedInjured(iLocal_665[14]))
                                {
                                    fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[14], false);
                                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[14], 1000f, 0);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[14], 1, false);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[14], 3, true);
                                    func_761[292] = true;
                                }
                            }
                        }
                        if (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[4]) && !fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[5]))
                        {
                            iLocal_1987++;
                        }
                        break;

                    case 3:
                        if (fWorld.Ped.IsPedInjured(iLocal_665[9]) && fWorld.Ped.IsPedInjured(iLocal_665[10]))
                        {
                            fCore.Timer.SetTimerA(0);
                            iLocal_1987++;
                        }
                        break;

                    case 4:
                        if (!fWorld.Entity.IsEntityDead(iLocal_1050[4], false))
                        {
                            fAudio.Audio.RequestScriptAudioBank("Prologue_Explosions_Cop_Car", false, -1);
                        }
                        if (iLocal_1241 != -1)
                        {
                            if (fAudio.Audio.HasSoundFinished(iLocal_1241))
                            {
                                fAudio.Audio.StopSound(iLocal_1241);
                                fAudio.Audio.ReleaseSoundId(iLocal_1241);
                                iLocal_1241 = -1;
                                fAudio.Audio.ReleaseNamedScriptAudioBank("Prologue_Explosions_Cop_Car");
                            }
                        }
                        if ((((fWorld.Ped.IsPedInjured(iLocal_665[11]) && fWorld.Ped.IsPedInjured(iLocal_665[12])) && fWorld.Ped.IsPedInjured(iLocal_665[13])) && fWorld.Ped.IsPedInjured(iLocal_665[14])) || fWorld.Entity.IsEntityAtCoord(fPlayer.Player.Character, 5473.0723f, -5128.806f, 80.06776f, 56f, 43f, 5f, false, true, 0))
                        {
                            if (!func_654(iLocal_1050[3]))
                            {
                                fWorld.Entity.SetEntityProofs(iLocal_1050[3], false, false, false, false, false, false, false, false);
                            }
                            if (!func_654(iLocal_1050[4]))
                            {
                                fWorld.Entity.SetEntityProofs(iLocal_1050[4], false, false, false, false, false, false, false, false);
                            }
                            if (!func_654(iLocal_1050[5]))
                            {
                                fWorld.Entity.SetEntityProofs(iLocal_1050[5], false, false, false, false, false, false, false, false);
                            }
                            fCore.Timer.SetTimerA(0);
                            fWorld.Entity.FreezeEntityPosition(iLocal_1050[6], false);
                            fWorld.Vehicle.SetVehicleSiren(iLocal_1050[6], true);
                            fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[6], 11, sLocal_559, true);
                            iLocal_1987++;
                        }
                        break;

                    case 5:
                        fCore.Timer.SetTimerA(0);
                        iLocal_1987++;
                        break;

                    case 6:
                        if (!func_761[250])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[6]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[6]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(11, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[6]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[6], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(11, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[6], true);
                                fWorld.Vehicle.SetVehicleSiren(iLocal_1050[6], false);
                                func_761[250] = true;
                            }
                        }
                        if (!func_761[293])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[6]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[6]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(11, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[6]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[6], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(11, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!fWorld.Ped.IsPedInjured(iLocal_665[15]))
                                {
                                    fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[15], false);
                                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[15], 1000f, 0);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[15], 1, false);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[15], 3, true);
                                    func_761[293] = true;
                                }
                            }
                        }
                        if (!func_761[294])
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[6]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[6]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(11, sLocal_559) / 100f) * 99f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[6]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[6], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(11, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!fWorld.Ped.IsPedInjured(iLocal_665[16]))
                                {
                                    fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[16], false);
                                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[16], 1000f, 0);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[16], 1, false);
                                    fWorld.Ped.SetPedCombatAttributes(iLocal_665[16], 3, true);
                                    func_761[294] = true;
                                }
                            }
                        }
                        if (fWorld.Ped.IsPedInjured(iLocal_665[15]) && fWorld.Ped.IsPedInjured(iLocal_665[16]))
                        {
                            iLocal_1987++;
                            fCore.Timer.SetTimerA(0);
                        }
                        break;

                    case 7:
                        if (!func_654(iLocal_1050[6]))
                        {
                            if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[6]) < 500f)
                            {
                                fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[6], 500f);
                            }
                            if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[6]) < 500f)
                            {
                                fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[6], 500f);
                            }
                            fWorld.Entity.SetEntityProofs(iLocal_1050[6], false, false, false, false, false, false, false, false);
                        }
                        iLocal_1987++;
                        break;
                    case 8:
                        if ((func_761[268] || func_761[269]) || func_761[266])
                        {
                            if (!func_761[241])
                            {
                                if (((fWorld.Entity.DoesEntityExist(iLocal_665[17]) && fWorld.Ped.IsPedInjured(iLocal_665[17])) && (fWorld.Entity.DoesEntityExist(iLocal_665[18]) && fWorld.Ped.IsPedInjured(iLocal_665[18]))) && (fWorld.Entity.DoesEntityExist(iLocal_665[19]) && fWorld.Ped.IsPedInjured(iLocal_665[19])))
                                {
                                    func_761[268] = false;
                                    func_761[269] = false;
                                    func_761[266] = false;
                                    func_761[241] = true;
                                }
                            }
                        }
                        if (!func_761[267])
                        {
                            if (fWorld.Entity.IsEntityInAngledArea(fPlayer.Player.Character, 5430.2236f, -5157.384f, 86.30035f, 5437.865f, -5089.6914f, 76.0554f, 150f, false, true, 0))
                            {
                                iLocal_1050[7] = func_485(joaat("policeold2"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(701, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(701, 0f, sLocal_559)), -1, 1097859072);
                                iLocal_1050[8] = func_485(joaat("policeold2"), fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(702, 0f, sLocal_559), func_472(fWorld.VehicleRecording.GetRotationOfVehicleRecordingAtTime(702, 0f, sLocal_559)), -1, 1097859072);
                                fWorld.Vehicle.SetVehicleConsideredByPlayer(iLocal_1050[7], false);
                                fWorld.Vehicle.SetVehicleConsideredByPlayer(iLocal_1050[8], false);
                                func_467(17);
                                func_467(18);
                                func_467(19);
                                func_467(20);
                                if (fWorld.Entity.DoesEntityExist(iLocal_665[17]))
                                {
                                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[17], iLocal_1050[7], -1);
                                }
                                if (fWorld.Entity.DoesEntityExist(iLocal_665[18]))
                                {
                                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[18], iLocal_1050[7], 0);
                                }
                                if (fWorld.Entity.DoesEntityExist(iLocal_665[19]))
                                {
                                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[19], iLocal_1050[8], -1);
                                }
                                if (fWorld.Entity.DoesEntityExist(iLocal_665[20]))
                                {
                                    fWorld.Ped.SetPedIntoVehicle(iLocal_665[20], iLocal_1050[8], 0);
                                }
                                if (fWorld.VehicleRecording.HasVehicleRecordingBeenLoaded(701, sLocal_559))
                                {
                                    fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[7], 701, sLocal_559, true);
                                    fWorld.VehicleRecording.SkipTimeInPlaybackRecordedVehicle(iLocal_1050[7], 3000f);
                                    fWorld.Vehicle.SetVehicleSiren(iLocal_1050[7], true);
                                }
                                if (fWorld.VehicleRecording.HasVehicleRecordingBeenLoaded(702, sLocal_559))
                                {
                                    fWorld.VehicleRecording.StartPlaybackRecordedVehicle(iLocal_1050[8], 702, sLocal_559, true);
                                    fWorld.VehicleRecording.SkipTimeInPlaybackRecordedVehicle(iLocal_1050[8], 3000f);
                                    fWorld.Vehicle.SetVehicleSiren(iLocal_1050[8], true);
                                }
                                if (func_761[240])
                                {
                                    fWorld.VehicleRecording.SkipToEndAndStopPlaybackRecordedVehicle(iLocal_1050[7]);
                                    fWorld.VehicleRecording.SkipToEndAndStopPlaybackRecordedVehicle(iLocal_1050[8]);
                                    if (!fWorld.Ped.IsPedInjured(iLocal_665[17]))
                                    {
                                        fWorld.Ped.ApplyDamageToPed(iLocal_665[17], fWorld.Entity.GetEntityHealth(iLocal_665[17]) + 100, true, 0, 0);
                                    }
                                    if (!fWorld.Ped.IsPedInjured(iLocal_665[18]))
                                    {
                                        fWorld.Ped.ApplyDamageToPed(iLocal_665[18], fWorld.Entity.GetEntityHealth(iLocal_665[18]) + 100, true, 0, 0);
                                    }
                                    if (!fWorld.Ped.IsPedInjured(iLocal_665[19]))
                                    {
                                        fWorld.Ped.ApplyDamageToPed(iLocal_665[19], fWorld.Entity.GetEntityHealth(iLocal_665[19]) + 100, true, 0, 0);
                                    }
                                    if (!fWorld.Ped.IsPedInjured(iLocal_665[20]))
                                    {
                                        fWorld.Ped.ApplyDamageToPed(iLocal_665[20], fWorld.Entity.GetEntityHealth(iLocal_665[20]) + 100, true, 0, 0);
                                    }
                                }
                                func_761[267] = true;
                            }
                        }
                        else
                        {
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[7]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(701, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[7], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(701, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_761[251])
                                {
                                    if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[7]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(701, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[7], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(701, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                    {
                                        fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[7], true);
                                        func_761[251] = true;
                                    }
                                }
                                if (!func_761[39])
                                {
                                    if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[7]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(701, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[7], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(701, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                    {
                                        if (!fWorld.Ped.IsPedInjured(iLocal_665[17]))
                                        {
                                            fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[7], true);
                                            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[17], 100f, 0);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[17], 1, false);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[17], 3, true);
                                            func_761[39] = true;
                                        }
                                    }
                                }
                                if (!func_761[40])
                                {
                                    if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[7]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(701, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[7]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[7], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(701, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                    {
                                        if (!fWorld.Ped.IsPedInjured(iLocal_665[18]))
                                        {
                                            fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[18], false);
                                            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[18], 1000f, 0);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[18], 1, false);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[18], 3, true);
                                            func_761[40] = true;
                                        }
                                    }
                                }
                            }
                            if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[8]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(702, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[8], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(702, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                            {
                                if (!func_761[252])
                                {
                                    if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[8]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(702, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[8], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(702, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                    {
                                        fWorld.Vehicle.SetVehicleHasMutedSirens(iLocal_1050[8], true);
                                        fWorld.Vehicle.SetVehicleSiren(iLocal_1050[8], false);
                                        func_761[252] = true;
                                    }
                                }
                                if (!func_761[41])
                                {
                                    if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[8]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(702, sLocal_559) / 100f) * 90f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[8], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(702, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                    {
                                        if (!fWorld.Ped.IsPedInjured(iLocal_665[19]))
                                        {
                                            fWorld.Ped.SetBlockingOfNonTemporaryEvents(iLocal_665[19], false);
                                            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[19], 1000f, 0);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[19], 1, false);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[19], 3, true);
                                            func_761[41] = true;
                                        }
                                    }
                                }
                                if (!func_761[42])
                                {
                                    if ((fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && fWorld.VehicleRecording.GetTimePositionInRecording(iLocal_1050[8]) > ((fWorld.VehicleRecording.GetTotalDurationOfVehicleRecording(702, sLocal_559) / 100f) * 95f)) || (!fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[8]) && !fWorld.Entity.IsEntityAtCoord(iLocal_1050[8], fWorld.VehicleRecording.GetPositionOfVehicleRecordingAtTime(702, 0f, sLocal_559), new Vector3(5f, 5f, 5f), false, true, 0)))
                                    {
                                        if (!fWorld.Ped.IsPedInjured(iLocal_665[20]))
                                        {
                                            Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, iLocal_665[20], 100f, 0);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[20], 1, false);
                                            fWorld.Ped.SetPedCombatAttributes(iLocal_665[20], 3, true);
                                            func_761[42] = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (iLocal_37 < (iLocal_1050.Length - 1))
                        {
                            iLocal_37++;
                        }
                        else
                        {
                            iLocal_37 = 0;
                        }
                        iVar0 = iLocal_37;
                        if (!fWorld.Entity.IsEntityDead(iLocal_1050[iVar0], false))
                        {
                            if (fWorld.VehicleRecording.IsPlaybackGoingOnForVehicle(iLocal_1050[iVar0]))
                            {
                                if (fWorld.Entity.DoesEntityExist(fWorld.Vehicle.GetPedInVehicleSeat(iLocal_1050[iVar0], -1, false)) && fWorld.Ped.IsPedInjured(fWorld.Vehicle.GetPedInVehicleSeat(iLocal_1050[iVar0], -1, false)))
                                {
                                    fWorld.VehicleRecording.StopPlaybackRecordedVehicle(iLocal_1050[iVar0]);
                                }
                            }
                        }
                        iVar0 = 0;
                        while (iVar0 < iLocal_665.Length)
                        {
                            if (!fWorld.Ped.IsPedInjured(iLocal_665[iVar0]))
                            {
                                if (fWorld.Ped.IsPedInAnyVehicle(iLocal_665[iVar0], true))
                                {
                                    fWorld.Ped.SetPedResetFlag(iLocal_665[iVar0], 282, true);
                                }
                            }
                            iVar0++;
                        }
                        iLocal_1987++;
                        break;
                }
                if (iLocal_1987 > 5)
                {
                    if (!func_654(iLocal_1050[3]))
                    {
                        if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[3]) < -100f)
                        {
                            fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[3], -100f);
                        }
                        if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[3]) < -100f)
                        {
                            fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[3], -100f);
                        }
                        fWorld.Entity.SetEntityProofs(iLocal_1050[3], false, false, false, false, false, false, false, false);
                    }
                    if (!func_654(iLocal_1050[4]))
                    {
                        if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[4]) < -100f)
                        {
                            fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[4], -100f);
                        }
                        if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[4]) < -100f)
                        {
                            fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[4], -100f);
                        }
                        fWorld.Entity.SetEntityProofs(iLocal_1050[4], false, false, false, false, false, false, false, false);
                    }
                    if (!func_654(iLocal_1050[5]))
                    {
                        if (fWorld.Vehicle.GetVehicleEngineHealth(iLocal_1050[5]) < -100f)
                        {
                            fWorld.Vehicle.SetVehicleEngineHealth(iLocal_1050[5], -100f);
                        }
                        if (fWorld.Vehicle.GetVehiclePetrolTankHealth(iLocal_1050[5]) < -100f)
                        {
                            fWorld.Vehicle.SetVehiclePetrolTankHealth(iLocal_1050[5], -100f);
                        }
                        fWorld.Entity.SetEntityProofs(iLocal_1050[5], false, false, false, false, false, false, false, false);
                    }
                }
            }
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (debug)
            {
                if (e.KeyCode == Keys.N)
                {
                    //Screen.ShowSubtitle("~s~Get in ~y~position.~s~");
                    Debug2 = 0;
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
                    //SpawnBooth();*//*
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
                    fCore.Debug.CopyPlayerPosWithAddons();
                    //1777.488f, 3326.681f, 41.43328f
                }
                if (e.KeyCode == Keys.NumPad9)
                {
                    fPlayer.Player.PedPos(5416.67f, -5175.303f, 79.22997f);
                    //big.Load(true);
                    //Debug2 = 1;
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

        void CleanUp()
        {
            int iVar52;

            fWorld.Misc.SetGamePaused(false);
            if (iLocal_1241 != -1)
            {
                fAudio.Audio.StopSound(iLocal_1241);
                fAudio.Audio.ReleaseSoundId(iLocal_1241);
                iLocal_1241 = -1;
            }
            fAudio.Audio.ReleaseNamedScriptAudioBank("Prologue_Explosions_Cop_Car");
            if (fAudio.Audio.IsAlarmPlaying("PROLOGUE_VAULT_ALARMS"))
            {
                fAudio.Audio.StopAlarm("PROLOGUE_VAULT_ALARMS");
            }
            if (fAudio.Audio.IsAudioSceneActive("PROLOGUE_POLICE_SHOOTOUT"))
            {
                fAudio.Audio.StopAudioScene("PROLOGUE_POLICE_SHOOTOUT");
            }
            fPlayer.Player.FakeWantedLevel = 0;
            iVar52 = 0;
            while (iVar52 < iLocal_1206.Length)
            {
                Function.Call(Hash.REMOVE_COVER_POINT, iLocal_1206[iVar52]);
                iVar52++;
            }
            iVar52 = 0;
            while (iVar52 < iLocal_665.Length)
            {
                if (fWorld.Entity.DoesEntityExist(iLocal_665[iVar52]))
                {
                    iLocal_665[iVar52]?.Delete();
                    iLocal_665[iVar52].Model.MarkAsNoLongerNeeded();
                }
                iVar52++;
            }
            iVar52 = 0;
            while (iVar52 < iLocal_1050.Length)
            {
                if (fWorld.Entity.DoesEntityExist(iLocal_1050[iVar52]))
                {
                    iLocal_1050[iVar52]?.Delete();
                    iLocal_1050[iVar52].Model.MarkAsNoLongerNeeded();
                }
                iVar52++;
            }
            iVar52 = 0;
            while (iVar52 < iLocal_1050.Length)
            {
                if (!fWorld.Entity.IsEntityDead(iLocal_1050[iVar52], false))
                {
                    fWorld.Entity.SetEntityProofs(iLocal_1050[iVar52], false, false, false, false, false, false, false, false);
                }
                iVar52++;
            }
        }

        private void onShutdown(object sender, EventArgs e)
        {
            bool flag = true;
            if (true == flag)
            {
                CleanUp();
                fCore.SceneManager.StopCurrentScene();
                debugProp2?.Delete();
                debugProp2 = null;
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
                fWorld.Vehicle.DeleteVehicleList(fCore.Debug.DebugVehicles);
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
                fWorld.Vehicle.CreateVehicleForList(depotVehicles, new Model("stockade3"), new Vector3((5341.3525f + 1.365f), -5177.149f, 81.762f), 0.3367f);
                Function.Call(Hash.SET_VEHICLE_IS_CONSIDERED_BY_PLAYER, depotVehicles[0], false);
                Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, depotVehicles[0], true);
                fWorld.Vehicle.CreateVehicleForList(depotVehicles, new Model("stockade3"), new Vector3((5337.0996f + 1.365f), -5177.0317f, 81.762f), 2.5903f);
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
