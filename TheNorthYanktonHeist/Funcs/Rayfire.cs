using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheNorthYanktonHeist.Scenes;

namespace TheNorthYanktonHeist.Funcs
{
    public class RayfireScenes : Script
    {

        public RayfireScenes()
        {
            Aborted += onShutdown;
        }

        private void onShutdown(object sender, EventArgs e)
        {
            PrologueVaultRayfire.PrologueVaultRayfireSceneCleanup();
        }

        public static class PrologueVaultRayfire
        {
            public static int vaultScene = 0;
            private static Prop rayfireVaultDoor = fRayfire.GetRayfireMapObject(new Vector3(5298.889f, -5189.087f, 82.5182f), 10f, "DES_VaultDoor001");
            private static Camera vaultCamera;
            private static Camera reactionCamera;
            private static Camera vaultZoomInCam;
            private static Prop vaultDoorScene;
            private static Prop dust;
            private static int depotInterior = fInterior.GetInteriorAtCoordsWithType(new Vector3(5311.236f, -5212.563f, (85.7187f - 3.2f)), "V_CashDepot");
            public static int PTFX1 = 0;
            public static int PTFX2 = 0;
            private static SynchronizedScene camScene = new SynchronizedScene(new Vector3(5310.083f, -5204.825f, 82.52f), Vector3.Zero);
            private static SynchronizedScene timerScene = new SynchronizedScene(new Vector3(5310.14f, -5208.279f, 82.52f), Vector3.Zero);
            private static Ped invisibleTimerPed;
            public static void Scene()
            {
                fHud.DisplayHelpText($"{vaultScene}");
                if ((Function.Call<bool>(Hash.IS_INTERPOLATING_TO_SCRIPT_CAMS) || Function.Call<bool>(Hash.IS_INTERPOLATING_FROM_SCRIPT_CAMS)) || (Function.Call<bool>(Hash.DOES_CAM_EXIST, vaultCamera) && Function.Call<bool>(Hash.IS_CAM_INTERPOLATING, vaultCamera)))
                {
                    Game.DisableControlThisFrame(Control.LookLeftRight);
                    Game.DisableControlThisFrame(Control.LookLeft);
                    Game.DisableControlThisFrame(Control.LookRight);
                    Game.DisableControlThisFrame(Control.LookUpDown);
                    Game.DisableControlThisFrame(Control.LookUp);
                    Game.DisableControlThisFrame(Control.LookDown);
                    Game.DisableControlThisFrame(Control.LookBehind);
                }
                switch (vaultScene)
                {
                    case 0:
                        Function.Call(Hash.LOAD_STREAM, "PROLOGUE_BLOW_THE_VAULT_MASTER", 0);
                        rayfireVaultDoor = fRayfire.GetRayfireMapObject(new Vector3(5298.889f, -5189.087f, 82.5182f), 10f, "DES_VaultDoor001");
                        if (invisibleTimerPed == null)
                        {
                            invisibleTimerPed = fPed.CreatePed(PedHash.Abigail, new Vector3(5310.14f, -5208.279f, 82.52f), 0f);
                            invisibleTimerPed.IsVisible = false;
                        }
                        else
                            invisibleTimerPed.IsVisible = false;
                        if (fRayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                        {
                            fRayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 4);
                        }
                        fHud.ClearPrints();
                        fAudio.StartAudioScene("PROLOGUE_DETONATE_CHARGES");
                        fAudio.StartAudioScene("PROLOGUE_MUTE_SPRINKLERS");
                        fAudio.ActivateAudioSlowmoMode("SLOWMO_PROLOGUE_VAULT");
                        Script.Wait(500);
                        fTimer.SetTimerA(0);
                        vaultScene = 1;
                        break;
                    case 1:
                        Function.Call(Hash.LOAD_STREAM, "PROLOGUE_BLOW_THE_VAULT_MASTER", 0);
                        fAudio.PrepareAlarm("PROLOGUE_VAULT_ALARMS");
                        if (vaultCamera == null)
                        {
                            vaultCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
                        }
                        fHud.RadarAndHud(false, false);
                        fHud.DisplayAmmoThisFrame(false);
                        fMisc.ClearArea(5296.97f, -5188.88f, 82.74f, 10f, true, false, false, false);
                        Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5297.292f, -5187.3296f, 84.124295f, 6.358143f, -8.767557f, -122.514175f, 28.3404f, 0, 3, 3, 2);
                        Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5297.325f, -5187.351f, 84.12872f, 6.358143f, -8.767557f, -122.514175f, 28.3404f, 1800, 3, 3, 2);
                        Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 1.1f);
                        Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 1f);
                        Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 2.8f);
                        ScriptCameraDirector.StartRendering();
                        Function.Call(Hash.CASCADE_SHADOWS_INIT_SESSION);
                        fPlayer.ped.IsInvincible = true;
                        fAudio.PlayStreamFrontend();
                        fGraphics.SetTimeCycleModifier("cashdepot");
                        Game.Player.SetControlState(false, SetPlayerControlFlags.AmbientScript);
                        Game.DisableControlThisFrame(Control.Jump);
                        Game.DisableControlThisFrame(Control.Duck);
                        Game.DisableControlThisFrame(Control.MoveUpOnly);
                        Game.DisableControlThisFrame(Control.MoveLeftOnly);
                        Game.DisableControlThisFrame(Control.MoveRightOnly);
                        Game.DisableControlThisFrame(Control.MoveDownOnly);
                        Game.DisableControlThisFrame(Control.MoveUpDown);
                        Game.DisableControlThisFrame(Control.MoveLeftRight);
                        Game.DisableControlThisFrame(Control.Cover);
                        Game.DisableControlThisFrame(Control.MeleeAttackHeavy);
                        Game.DisableControlThisFrame(Control.MeleeAttackLight);
                        Game.DisableControlThisFrame(Control.MeleeAttack1);
                        Game.DisableControlThisFrame(Control.MeleeAttack2);
                        Game.DisableControlThisFrame(Control.MeleeBlock);
                        fHud.HideHudComponentThisFrame(3);
                        fInterior.SetRoomForGameViewportByName("V_CashD_vault");
                        fPlayer.PedPos(5307.475f, -5206.6147f, 82.5187f, 269.1302f);
                        if (!fEntity.DoesEntityExist(vaultDoorScene))
                        {
                            vaultDoorScene = fProp.CreateProp("prop_vault_door_scene", new Vector3(5297.717f, -5188.909f, 81.575f), Vector3.Zero, false, false);
                            fInterior.ForceRoomForEntity(vaultDoorScene, depotInterior, fMisc.GetHashKey("V_CashD_side"));
                        }
                        fTimer.SetTimerA(0);
                        vaultScene = 2;
                        break;
                    case 2:
                        fAudio.PrepareAlarm("PROLOGUE_VAULT_ALARMS");
                        while (fTimer.TimerA() <= 0)
                            Wait(0);
                        if (fTimer.TimerA() > 1800)
                        {
                            if (BombPlantScene._permBomb != null)
                                BombPlantScene._permBomb.IsVisible = false;
                            if (fRayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                            {
                                fRayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 6);
                            }
                            if (!fAudio.IsAudioSceneActive("PROLOGUE_VAULT_RAYFIRE"))
                            {
                                fAudio.StartAudioScene("PROLOGUE_VAULT_RAYFIRE");
                            }
                            if (fAudio.IsAudioSceneActive("PROLOGUE_DETONATE_CHARGES"))
                            {
                                fAudio.StopAudioScene("PROLOGUE_DETONATE_CHARGES");
                            }
                            if (vaultCamera != null)
                            {
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 2.8f);
                                vaultCamera.Delete();
                                vaultCamera = null;
                            }
                            if (vaultCamera == null)
                            {
                                vaultCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
                            }
                            Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5292.704f, -5185.751f, 82.84772f, 9.034329f, -2.898424f, -131.66974f, 45f, 0, 0, 0, 2);
                            if (fEntity.DoesEntityExist(vaultDoorScene))
                            {
                                vaultDoorScene.Delete();
                            }
                            fTimer.SetTimerA(0);
                            vaultScene = 3;
                        }
                        break;
                    case 3:
                        fAudio.PrepareAlarm("PROLOGUE_VAULT_ALARMS");
                        while (fTimer.TimerA() <= 0)
                            Wait(0);
                        if (fTimer.TimerA() > 300)
                        {
                            fStreaming.RequestNamedPTFXAsset("scr_prologue");
                            fStreaming.UseParticleFXAsset("scr_prologue");
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "ent_ray_pro1_vault_exp_lit", 5298.2007f, -5189.052f, 83.86238f, 0f, 0f, 0f, 1f, false, false, false);
                            Function.Call(Hash.SHAKE_CAM, vaultCamera, "GRENADE_EXPLOSION_SHAKE", 3f);
                            fGamePad.SetControlShake(0 /*PLAYER_CONTROL*/, 500, 256);
                            fStreaming.RequestNamedPTFXAsset("scr_prologue");
                            fStreaming.UseParticleFXAsset("scr_prologue");
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                            {
                                if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX1))
                                {
                                   PTFX1 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_haze", 5299f, -5189f, 82.6f, 0f, 0f, 0f, 1f, false, false, false, false);
                                }
                                if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX2))
                                {
                                    PTFX2 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_fog", 5299f, -5189f, 82.6f, 0f, 0f, 0f, 1f, false, false, false, false);
                                }
                            }
                            if (fAudio.IsAudioSceneActive("PROLOGUE_MUTE_SPRINKLERS"))
                            {
                                fAudio.StopAudioScene("PROLOGUE_MUTE_SPRINKLERS");
                            }
                            fMisc.SetTimeScale(0.5f);
                        }
                        while (fTimer.TimerA() <= 0)
                            Wait(0);
                        if (fTimer.TimerA() > 400)
                        {
                            fAudio.StartAlarm("PROLOGUE_VAULT_ALARMS", false);
                            if (BombPlantScene._permBomb != null)
                            {
                                BombPlantScene._permBomb.Delete();
                                BombPlantScene._permBomb = null;
                            }
                            fTimer.SetTimerA(0);
                            vaultScene = 4;
                        }
                        break;
                    case 4:
                        while (fTimer.TimerA() <= 0)
                            Wait(0);
                        if (fTimer.TimerA() > 250)
                        {
                            fTimer.SetTimerA(0);
                            vaultScene = 5;
                        }
                        break;
                    case 5:
                        while (fTimer.TimerA() <= 0)
                            Wait(0);
                        if (fTimer.TimerA() > 450)
                        {
                            fMisc.SetTimeScale(1f);
                            if (fAudio.IsAudioSceneActive("PROLOGUE_VAULT_RAYFIRE"))
                            {
                                fAudio.StopAudioScene("PROLOGUE_VAULT_RAYFIRE");
                            }
                            Function.Call(Hash.STOP_CAM_SHAKING, vaultCamera, false);
                            fGraphics.ClearTimecycleModifier();
                            if (reactionCamera == null)
                            {
                                reactionCamera = fCam.CreateAnimatedCam();
                            }
                            timerScene.Create();
                            camScene.Create();
                            Vector3 vector3 = new Vector3(82.5187f, -5207.1147f, 5307.475f) + new Vector3(0f, 0.32f, -0.08f);
                            fPlayer.PedPos(vector3.Z, vector3.Y, vector3.X, 272.3664f);
                            ScriptCameraDirector.StopRendering();
                            Function.Call(Hash.TASK_PLAY_ANIM, fPlayer.ped, fStreaming.RequestAnimDict("missprologueig_3@react_to_explosion"), "react_to_explosion_player_zero", 1000f, -8f, -1, 0, (0.075f + 0.05f), false, false, false);
                            timerScene.PlayPed(invisibleTimerPed, fStreaming.RequestAnimDict("missprologueig_3@react_to_explosion"), "react_to_explosion_player_two");
                            fPed.ForcePedAiAndAnimationUpdate(fPlayer.ped, false, false);
                            camScene.Camera = reactionCamera;
                            camScene.PlayCam(fStreaming.RequestAnimDict("missprologueig_3"), "react_to_explosion_cam");
                            timerScene.Phase = (0.075f + 0.05f);
                            camScene.Phase = 0.421f;
                            Game.DisableControlThisFrame(Control.Jump);
                            Game.DisableControlThisFrame(Control.Duck);
                            Game.DisableControlThisFrame(Control.MoveUpOnly);
                            Game.DisableControlThisFrame(Control.MoveLeftOnly);
                            Game.DisableControlThisFrame(Control.MoveRightOnly);
                            Game.DisableControlThisFrame(Control.MoveDownOnly);
                            Game.DisableControlThisFrame(Control.MoveUpDown);
                            Game.DisableControlThisFrame(Control.MoveLeftRight);
                            Game.DisableControlThisFrame(Control.Cover);
                            Game.DisableControlThisFrame(Control.MeleeAttackHeavy);
                            Game.DisableControlThisFrame(Control.MeleeAttackLight);
                            Game.DisableControlThisFrame(Control.MeleeAttack1);
                            Game.DisableControlThisFrame(Control.MeleeAttack2);
                            Game.DisableControlThisFrame(Control.MeleeBlock);
                            fHud.HideHudComponentThisFrame(3);
                            fInterior.SetRoomForGameViewportByName("V_CashD_reception");
                            if (camScene.IsRunning && timerScene.IsRunning)
                            {
                                fTimer.SetTimerA(0);
                                vaultScene = 6;
                            }
                        }
                        break;
                    case 6:
                        fStreaming.RequestNamedPTFXAsset("scr_prologue");
                        fStreaming.UseParticleFXAsset("scr_prologue");
                        if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                        {
                            Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5310.245f, -5205.663f, 85.2259f, 0f, 0f, 0f, 1f, false, false, false);
                        }
                        fTimer.SetTimerA(0);
                        vaultScene = 7;
                        break;
                    case 7:
                        fStreaming.RequestNamedPTFXAsset("scr_prologue");
                        fStreaming.UseParticleFXAsset("scr_prologue");
                        if (timerScene.Phase >= (0.085f + 0.05f))
                        {
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                            {
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5309.8f, -5207.6f, 85.40824f, 0f, 0f, 90f, 1f, false, false, false);
                            }
                            fTimer.SetTimerA(0);
                            vaultScene = 8;
                        }
                        break;
                    case 8:
                        fStreaming.RequestNamedPTFXAsset("scr_prologue");
                        fStreaming.UseParticleFXAsset("scr_prologue");
                        if (timerScene.Phase >= (0.087f + 0.05f))
                        {
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                            {
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5313.9927f, -5207.3f, 85.34588f, 0f, 0f, 180f, 1f, false, false, false);
                            }
                            fTimer.SetTimerA(0);
                            vaultScene = 9;
                        }
                        break;
                    case 9:
                        if (timerScene.Phase >= (0.35f + 0.05f))
                        {
                            Function.Call(Hash.STOP_CAM_SHAKING, vaultCamera, true);
                            if (vaultZoomInCam == null)
                            {
                                vaultZoomInCam = Function.Call<Camera>(Hash.CREATE_CAMERA, fMisc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), true);
                            }
                            Function.Call(Hash.SET_CAM_PARAMS, vaultZoomInCam, 5295.859f, -5188.994f, 82.99249f, 3.961173f, -0.003078f, -90.428894f, 35.788742f, 0, 0, 0, 2);
                            Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultZoomInCam, 8f);
                            Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultZoomInCam, 1f);
                            Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultZoomInCam, 1f);
                            Function.Call(Hash.SET_CAM_DOF_MAX_NEAR_IN_FOCUS_DISTANCE_BLEND_LEVEL, vaultZoomInCam, 0f);
                            Function.Call(Hash.SHAKE_CAM, vaultZoomInCam, "HAND_SHAKE", 0.5f);
                            Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5296.3735f, -5188.994f, 83.0277f, 3.408814f, -0.003078f, -91.27811f, 35.788742f, 0, 0, 0, 2);
                            if (reactionCamera != null)
                            {
                                reactionCamera.Delete();
                                reactionCamera = null;
                            }
                            Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 8f);
                            Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 1f);
                            Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 1.3f);
                            Function.Call(Hash.SET_CAM_DOF_MAX_NEAR_IN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 0f);
                            Function.Call(Hash.SHAKE_CAM, vaultCamera, "HAND_SHAKE", 0.5f);
                            Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, vaultCamera, vaultZoomInCam, 3000, 0, 0);
                            Function.Call(Hash.CLEAR_PED_TASKS, fPlayer.ped);
                            Function.Call(Hash.CLEAR_PED_TASKS, invisibleTimerPed);
                            Game.DisableControlThisFrame(Control.Jump);
                            Game.DisableControlThisFrame(Control.Duck);
                            Game.DisableControlThisFrame(Control.MoveUpOnly);
                            Game.DisableControlThisFrame(Control.MoveLeftOnly);
                            Game.DisableControlThisFrame(Control.MoveRightOnly);
                            Game.DisableControlThisFrame(Control.MoveDownOnly);
                            Game.DisableControlThisFrame(Control.MoveUpDown);
                            Game.DisableControlThisFrame(Control.MoveLeftRight);
                            Game.DisableControlThisFrame(Control.Cover);
                            Game.DisableControlThisFrame(Control.MeleeAttackHeavy);
                            Game.DisableControlThisFrame(Control.MeleeAttackLight);
                            Game.DisableControlThisFrame(Control.MeleeAttack1);
                            Game.DisableControlThisFrame(Control.MeleeAttack2);
                            Game.DisableControlThisFrame(Control.MeleeBlock);
                            fHud.HideHudComponentThisFrame(3);
                            fInterior.SetRoomForGameViewportByName("V_CashD_vault");
                            if (dust == null)
                            {
                                dust = Function.Call<Prop>(Hash.CREATE_OBJECT_NO_OFFSET, fMisc.GetHashKey("v_ilev_cd_dust"), 5312.14f, -5209.04f, 83.02f, true, true, false, 0);
                            }
                            else
                            {
                                Function.Call(Hash.FREEZE_ENTITY_POSITION, dust, true);
                                Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, fMisc.GetHashKey("v_ilev_cd_dust"));
                            }
                            fStreaming.RequestNamedPTFXAsset("scr_prologue");
                            fStreaming.UseParticleFXAsset("scr_prologue");
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                            {
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5298.206f, -5189.0635f, 85.281166f, 0f, 0f, 180f, 1f, false, false, false);
                            }
                            fTimer.SetTimerA(0);
                            vaultScene = 10;
                        }
                        break;
                    case 10:
                        if (fTimer.TimerA() > 3000)
                        {
                            if (invisibleTimerPed != null)
                            {
                                invisibleTimerPed.Delete();
                                invisibleTimerPed = null;
                            }
                            Game.Player.SetControlState(true, SetPlayerControlFlags.AmbientScript);
                            fPlayer.PedPos(5308.856f, -5206.294f, (85.7187f - 3.2f), 355.824f);
                            fPed.ForcePedMotionState(fPlayer.ped, fPed.MotionStates.MS_ACTIONMODE_IDLE);
                            fHud.RadarAndHud(true, true);
                            Game.DisableControlThisFrame(Control.Jump);
                            Game.DisableControlThisFrame(Control.Duck);
                            Game.DisableControlThisFrame(Control.MoveUpOnly);
                            Game.DisableControlThisFrame(Control.MoveLeftOnly);
                            Game.DisableControlThisFrame(Control.MoveRightOnly);
                            Game.DisableControlThisFrame(Control.MoveDownOnly);
                            Game.DisableControlThisFrame(Control.MoveUpDown);
                            Game.DisableControlThisFrame(Control.MoveLeftRight);
                            Game.DisableControlThisFrame(Control.Cover);
                            Game.DisableControlThisFrame(Control.MeleeAttackHeavy);
                            Game.DisableControlThisFrame(Control.MeleeAttackLight);
                            Game.DisableControlThisFrame(Control.MeleeAttack1);
                            Game.DisableControlThisFrame(Control.MeleeAttack2);
                            Game.DisableControlThisFrame(Control.MeleeBlock);
                            fHud.HideHudComponentThisFrame(3);
                            if (vaultCamera != null)
                            {
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 2.8f);
                                vaultCamera.Delete();
                                vaultCamera = null;
                            }
                            if (vaultCamera == null)
                            {
                                vaultCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
                            }
                            ScriptCameraDirector.StopRendering();
                            Function.Call(Hash.CASCADE_SHADOWS_INIT_SESSION);
                            fPlayer.ped.IsInvincible = false;
                            Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
                            if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, 0) == 4)
                            {
                                Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, -5f, 1f);
                            }
                            else
                            {
                                Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, -30f, 1f);
                            }
                            if (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT))
                            {
                                Function.Call(Hash.DO_SCREEN_FADE_IN, 800);
                            }
                            vaultScene = 11;
                        }
                        break;
                    case 11:
                        Function.Call(Hash.SET_CAM_ACTIVE, vaultCamera, true);
                        if (reactionCamera != null)
                        {
                            reactionCamera.Delete();
                            reactionCamera = null;
                        }
                        if (vaultZoomInCam != null)
                        {
                            vaultZoomInCam.Delete();
                            vaultZoomInCam = null;
                        }
                        fGraphics.ClearTimecycleModifier();
                        if (invisibleTimerPed != null)
                        {
                            invisibleTimerPed.Delete();
                            invisibleTimerPed = null;
                        }
                        if (vaultDoorScene != null)
                        {
                            vaultDoorScene.Delete();
                            vaultDoorScene = null;
                        }
                        if (BombPlantScene._permBomb != null)
                        {
                            BombPlantScene._permBomb.Delete();
                            BombPlantScene._permBomb = null;
                        }
                        if (dust == null)
                        {
                            dust = Function.Call<Prop>(Hash.CREATE_OBJECT_NO_OFFSET, fMisc.GetHashKey("v_ilev_cd_dust"), 5312.14f, -5209.04f, 83.02f, true, true, false, 0);
                        }
                        Function.Call(Hash.DEACTIVATE_AUDIO_SLOWMO_MODE, "SLOWMO_PROLOGUE_VAULT");
                        if (fAudio.IsAudioSceneActive("PROLOGUE_THREATEN_HOSTAGES"))
                        {
                            fAudio.StopAudioScene("PROLOGUE_THREATEN_HOSTAGES");
                        }
                        if (fAudio.IsAudioSceneActive("PROLOGUE_VAULT_RAYFIRE"))
                        {
                            fAudio.StopAudioScene("PROLOGUE_VAULT_RAYFIRE");
                        }
                        if (fAudio.IsAudioSceneActive("PROLOGUE_MUTE_SPRINKLERS"))
                        {
                            fAudio.StopAudioScene("PROLOGUE_MUTE_SPRINKLERS");
                        }
                        if (fAudio.IsAudioSceneActive("PROLOGUE_DETONATE_CHARGES"))
                        {
                            fAudio.StopAudioScene("PROLOGUE_DETONATE_CHARGES");
                        }
                        fHud.ClearHelp(true);
                        fMisc.SetTimeScale(1f);
                        if (vaultCamera != null)
                        {
                            if (Function.Call<bool>(Hash.IS_CAM_RENDERING, vaultCamera))
                            {
                                ScriptCameraDirector.StopRendering();
                                Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
                                Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, -10f, 1f);
                            }
                            else
                            {
                                vaultCamera.Delete();
                                vaultCamera = null;
                            }
                        }
                        fPlayer.ped.Task.ClearAllImmediately();
                        if (fRayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                        {
                            fRayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 9);
                        }
                        fTimer.SetTimerA(0);
                        vaultScene = 12;
                        break;
                }
            }
            public static void PrologueVaultRayfireSceneCleanup()
            {
                Game.Player.SetControlState(true, SetPlayerControlFlags.AmbientScript);
                camScene.Dispose();
                timerScene.Dispose();
                if (fAudio.IsAudioSceneActive("PROLOGUE_THREATEN_HOSTAGES"))
                {
                    fAudio.StopAudioScene("PROLOGUE_THREATEN_HOSTAGES");
                }
                if (fAudio.IsAudioSceneActive("PROLOGUE_VAULT_RAYFIRE"))
                {
                    fAudio.StopAudioScene("PROLOGUE_VAULT_RAYFIRE");
                }
                if (fAudio.IsAudioSceneActive("PROLOGUE_MUTE_SPRINKLERS"))
                {
                    fAudio.StopAudioScene("PROLOGUE_MUTE_SPRINKLERS");
                }
                if (fAudio.IsAudioSceneActive("PROLOGUE_DETONATE_CHARGES"))
                {
                    fAudio.StopAudioScene("PROLOGUE_DETONATE_CHARGES");
                }
                fStreaming.RemoveNamedPTFXAsset("scr_prologue");
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, 0);
                Function.Call(Hash.REMOVE_PARTICLE_FX, PTFX1, 0);
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, 0);
                Function.Call(Hash.REMOVE_PARTICLE_FX, PTFX2, 0);
                Function.Call(Hash.DEACTIVATE_AUDIO_SLOWMO_MODE, "SLOWMO_PROLOGUE_VAULT");
                Function.Call(Hash.STOP_ALARM, "PROLOGUE_VAULT_ALARMS", true);
                Function.Call(Hash.STOP_STREAM);
                Function.Call(Hash.STOP_AUDIO_SCENES);
                Function.Call(Hash.REMOVE_PTFX_ASSET);
                if (vaultZoomInCam != null)
                {
                    vaultZoomInCam.Delete();
                    vaultZoomInCam = null;
                }
                if (invisibleTimerPed != null)
                {
                    invisibleTimerPed.Delete();
                    invisibleTimerPed = null;
                }
                if (dust != null)
                {
                    dust.Delete();
                    dust = null;
                }
                if (vaultDoorScene != null)
                {
                    vaultDoorScene.Delete();
                    vaultDoorScene = null;
                }
                if (reactionCamera != null)
                {
                    reactionCamera.Delete();
                    reactionCamera = null;
                }
                if (vaultCamera != null)
                {
                    vaultCamera.Delete();
                    vaultCamera = null;
                }
                if (BombPlantScene._permBomb != null)
                {
                    BombPlantScene._permBomb.Delete();
                    BombPlantScene._permBomb = null;
                }
            }
        }
    }
    public class fRayfire
    {
        public static Prop GetRayfireMapObject(Vector3 xyz, float radius, string name)
        {
            return Function.Call<Prop>(Hash.GET_​RAYFIRE_​MAP_​OBJECT, xyz.X, xyz.Y, xyz.Z, radius, name);
        }
        public static void SetStateOfRayfireMapObject(Prop prop, int state)
        {
            Function.Call(Hash.SET_​STATE_​OF_​RAYFIRE_​MAP_​OBJECT, prop, state);
        }
        public static int GetStateOfRayfireMapObject(Prop prop)
        {
            return Function.Call<int>(Hash.GET_​STATE_​OF_​RAYFIRE_​MAP_​OBJECT, prop);
        }
        public static bool DoesRayfireMapObjectExist(Prop prop)
        {
            return Function.Call<bool>(Hash.DOES_​RAYFIRE_​MAP_​OBJECT_​EXIST, prop);
        }
        public static float GetRayfireMapObjectAnimPhase(Prop prop)
        {
            return Function.Call<float>(Hash.GET_RAYFIRE_MAP_OBJECT_ANIM_PHASE, prop);
        }
    }
}
