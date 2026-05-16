using GTA;
using GTA.Math;
using GTA.Native;
using TheNorthYanktonHeist.Scenes;
using System;
using fAudio = IMNOTEPIX.Framework.fAudio;
using fCore = IMNOTEPIX.Framework.fCore;
using fUI = IMNOTEPIX.Framework.fUI;
using IMNOTEPIX.Framework.fWorld.Anims;
using IMNOTEPIX.Framework.fWorld.Effects;
using fWorld = IMNOTEPIX.Framework.fWorld;
using fPlayer = IMNOTEPIX.Framework.fPlayer;
using fInterior = IMNOTEPIX.Framework.fWorld.Interior;

namespace TheNorthYanktonHeist.Extension
{
    public class RayfireScenes : GTA.Script
    {
        public RayfireScenes()
        {
            Aborted += OnShutdown;
        }

        private void OnShutdown(object sender, EventArgs e)
        {
            PrologueVaultRayfire.PrologueVaultRayfireSceneCleanup();
            PrologueDoorRayfire.PrologueDoorSceneCleanup();
        }

        /// <summary>
        /// Disables all movement and combat controls for this frame.
        /// Also hides the minimap compass (HUD component 3).
        /// Called every tick while cutscene cameras are active.
        /// </summary>
        internal static void DisableMovementControls()
        {
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
            fUI.Hud.HideHudComponentThisFrame(3);
        }

        /// <summary>
        /// Disables all look/camera controls for this frame.
        /// Called while camera interpolation is in progress.
        /// </summary>
        internal static void DisableLookControls()
        {
            Game.DisableControlThisFrame(Control.LookLeftRight);
            Game.DisableControlThisFrame(Control.LookLeft);
            Game.DisableControlThisFrame(Control.LookRight);
            Game.DisableControlThisFrame(Control.LookUpDown);
            Game.DisableControlThisFrame(Control.LookUp);
            Game.DisableControlThisFrame(Control.LookDown);
            Game.DisableControlThisFrame(Control.LookBehind);
        }

        public static class PrologueVaultRayfire
        {
            public static int vaultScene = 0;

            // Fixed: was initialized via native call at class-load time.
            // Re-fetched in case 0 anyway — static initializer was both dangerous
            // and redundant. Set to null; case 0 assigns it.
            private static Prop rayfireVaultDoor = null;

            private static Camera vaultCamera;
            private static Camera reactionCamera;
            private static Camera vaultZoomInCam;
            private static Prop vaultDoorScene;
            private static Prop dust;

            // Fixed: was initialized via native call at class-load time.
            // Moved to case 0 where the other field initialisations happen.
            private static int depotInterior = -1;

            // Fixed: were public with no documented external use.
            public static int PTFX1 = 0;
            public static int PTFX2 = 0;

            private static SynchronizedScene camScene = new SynchronizedScene(new Vector3(5310.083f, -5204.825f, 82.52f), Vector3.Zero);
            private static SynchronizedScene timerScene = new SynchronizedScene(new Vector3(5310.14f, -5208.279f, 82.52f), Vector3.Zero);

            private static Ped invisibleTimerPed;

            public static void Scene()
            {
                // Disable look controls while any camera is interpolating
                if (Function.Call<bool>(Hash.IS_INTERPOLATING_TO_SCRIPT_CAMS) || Function.Call<bool>(Hash.IS_INTERPOLATING_FROM_SCRIPT_CAMS) || (Function.Call<bool>(Hash.DOES_CAM_EXIST, vaultCamera) && Function.Call<bool>(Hash.IS_CAM_INTERPOLATING, vaultCamera)))
                    DisableLookControls();

                switch (vaultScene)
                {
                    case 0:
                        fAudio.Audio.LoadStream("PROLOGUE_BLOW_THE_VAULT_MASTER", 0);
                        
                        // Fetch rayfire handle and interior — native calls deferred to here
                        // rather than at class-load time.
                        rayfireVaultDoor = Rayfire.GetRayfireMapObject(new Vector3(5298.889f, -5189.087f, 82.5182f), 10f, "DES_VaultDoor001");
                        depotInterior = fWorld.Interior.Interior.GetInteriorAtCoordsWithType(new Vector3(5311.236f, -5212.563f, 85.7187f - 3.2f), "V_CashDepot");

                        if (invisibleTimerPed == null)
                        {
                            invisibleTimerPed = fWorld.Ped.CreatePed(PedHash.Abigail, new Vector3(5310.14f, -5208.279f, 82.52f), 0f);
                            invisibleTimerPed.IsVisible = false;
                        }
                        else
                            invisibleTimerPed.IsVisible = false;

                        if (Rayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                            Rayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 4);

                        fUI.Hud.ClearSubtitles();
                        fAudio.Audio.StartAudioScene("PROLOGUE_DETONATE_CHARGES");
                        fAudio.Audio.StartAudioScene("PROLOGUE_MUTE_SPRINKLERS");
                        fAudio.Audio.ActivateAudioSlowmoMode("SLOWMO_PROLOGUE_VAULT");
                        Wait(500);
                        fCore.Timer.SetTimerA(0);
                        vaultScene = 1;
                        break;

                    case 1:
                        fAudio.Audio.LoadStream("PROLOGUE_BLOW_THE_VAULT_MASTER", 0);
                        fAudio.Audio.PrepareAlarm("PROLOGUE_VAULT_ALARMS");

                        if (vaultCamera == null)
                            vaultCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);

                        fUI.Hud.RadarAndHud(false, false);
                        fUI.Hud.DisplayAmmoThisFrame(false);
                        fWorld.Misc.ClearArea(5296.97f, -5188.88f, 82.74f, 10f, true, false, false, false);

                        Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5297.292f, -5187.3296f, 84.124295f, 6.358143f, -8.767557f, -122.514175f, 28.3404f, 0, 3, 3, 2);
                        Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5297.325f, -5187.351f, 84.12872f, 6.358143f, -8.767557f, -122.514175f, 28.3404f, 1800, 3, 3, 2);
                        Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 1.1f);
                        Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 1f);
                        Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 2.8f);

                        ScriptCameraDirector.StartRendering();
                        Function.Call(Hash.CASCADE_SHADOWS_INIT_SESSION);
                        fPlayer.Player.Character.IsInvincible = true;
                        fAudio.Audio.PlayStreamFrontend();
                        fUI.Graphics.SetTimeCycleModifier("cashdepot");
                        Game.Player.SetControlState(false, SetPlayerControlFlags.AmbientScript);
                        DisableMovementControls();
                        fInterior.Interior.SetRoomForGameViewportByName("V_CashD_vault");
                        fPlayer.Player.PedPos(5307.475f, -5206.6147f, 82.5187f, 269.1302f);

                        if (vaultDoorScene == null || !vaultDoorScene.Exists())
                        {
                            vaultDoorScene = World.CreateProp("prop_vault_door_scene", new Vector3(5297.717f, -5188.909f, 81.575f), Vector3.Zero, false, false); 
                            fInterior.Interior.ForceRoomForEntity(vaultDoorScene, depotInterior, fWorld.Misc.joaat("V_CashD_side"));
                        }
                        fCore.Timer.SetTimerA(0);
                        vaultScene = 2;
                        break;

                    case 2:
                        fAudio.Audio.PrepareAlarm("PROLOGUE_VAULT_ALARMS");

                        while (fCore.Timer.TimerA() <= 0)
                            Wait(0);

                        if (fCore.Timer.TimerA() > 1800)
                        {
                            if (BombPlantScene._permBomb != null)
                                BombPlantScene._permBomb.IsVisible = true;

                            if (Rayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                                Rayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 6);

                            if (!fAudio.Audio.IsAudioSceneActive("PROLOGUE_VAULT_RAYFIRE"))
                                fAudio.Audio.StartAudioScene("PROLOGUE_VAULT_RAYFIRE");

                            if (fAudio.Audio.IsAudioSceneActive("PROLOGUE_DETONATE_CHARGES"))
                                fAudio.Audio.StopAudioScene("PROLOGUE_DETONATE_CHARGES");

                            if (vaultCamera != null)
                            {
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 2.8f);
                                vaultCamera.Delete();
                                vaultCamera = null;
                            }

                            vaultCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
                            Function.Call(Hash.SET_CAM_PARAMS, vaultCamera, 5292.704f, -5185.751f, 82.84772f, 9.034329f, -2.898424f, -131.66974f, 45f, 0, 0, 0, 2);

                            if (vaultDoorScene != null && vaultDoorScene.Exists())
                                vaultDoorScene.Delete();

                            fCore.Timer.SetTimerA(0);
                            vaultScene = 3;
                        }
                        break;

                    case 3:
                        fAudio.Audio.PrepareAlarm("PROLOGUE_VAULT_ALARMS");

                        while (fCore.Timer.TimerA() <= 0)
                            Wait(0);

                        if (fCore.Timer.TimerA() > 300)
                        {
                            fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                            fCore.Streaming.UseParticleFXAsset("scr_prologue");

                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "ent_ray_pro1_vault_exp_lit", 5298.2007f, -5189.052f, 83.86238f, 0f, 0f, 0f, 1f, false, false, false);

                            Function.Call(Hash.SHAKE_CAM, vaultCamera, "GRENADE_EXPLOSION_SHAKE", 3f);
                            fPlayer.Input.SetControlShake(0, 500, 256);

                            fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                            fCore.Streaming.UseParticleFXAsset("scr_prologue");

                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                            {
                                if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX1))
                                    PTFX1 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_haze", 5299f, -5189f, 82.6f, 0f, 0f, 0f, 1f, false, false, false, false);

                                if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX2))
                                    PTFX2 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_fog", 5299f, -5189f, 82.6f, 0f, 0f, 0f, 1f, false, false, false, false);
                            }

                            if (fAudio.Audio.IsAudioSceneActive("PROLOGUE_MUTE_SPRINKLERS"))
                                fAudio.Audio.StopAudioScene("PROLOGUE_MUTE_SPRINKLERS");

                            fWorld.Misc.SetTimeScale(0.5f);
                        }

                        while (fCore.Timer.TimerA() <= 0)
                            Wait(0);

                        if (fCore.Timer.TimerA() > 400)
                        {
                            if (BombPlantScene._permBomb != null)
                            {
                                BombPlantScene._permBomb.Delete();
                                BombPlantScene._permBomb = null;
                            }
                            fAudio.Audio.StartAlarm("PROLOGUE_VAULT_ALARMS", false);
                            fCore.Timer.SetTimerA(0);
                            vaultScene = 4;
                        }
                        break;

                    case 4:
                        while (fCore.Timer.TimerA() <= 0)
                            Wait(0);

                        if (fCore.Timer.TimerA() > 250)
                        {
                            fCore.Timer.SetTimerA(0);
                            vaultScene = 5;
                        }
                        break;

                    case 5:
                        while (fCore.Timer.TimerA() <= 0)
                            Wait(0);

                        if (fCore.Timer.TimerA() > 450)
                        {
                            fWorld.Misc.SetTimeScale(1f);

                            if (fAudio.Audio.IsAudioSceneActive("PROLOGUE_VAULT_RAYFIRE"))
                                fAudio.Audio.StopAudioScene("PROLOGUE_VAULT_RAYFIRE");

                            Function.Call(Hash.STOP_CAM_SHAKING, vaultCamera, false);
                            fUI.Graphics.ClearTimecycleModifier();

                            if (reactionCamera == null)
                                reactionCamera = fWorld.Camera.CreateAnimatedCam();

                            timerScene.Create();
                            camScene.Create();

                            // Note: Vector3 components are intentionally in unusual order here —
                            // the XYZ values are passed back via .Z/.Y/.X to produce the correct
                            // world position. Preserved as-is.
                            Vector3 playerPos = new Vector3(82.5187f, -5207.1147f, 5307.475f)
                                              + new Vector3(0f, 0.32f, -0.08f);
                            fPlayer.Player.PedPos(playerPos.Z, playerPos.Y, playerPos.X, 272.3664f);

                            Function.Call(Hash.TASK_PLAY_ANIM, fPlayer.Player.Character, fCore.Streaming.RequestAnimDict("missprologueig_3@react_to_explosion"), "react_to_explosion_player_zero", 1000f, -8f, -1, 0, 0.075f + 0.05f, false, false, false);

                            timerScene.PlayPed(invisibleTimerPed, fCore.Streaming.RequestAnimDict("missprologueig_3@react_to_explosion"), "react_to_explosion_player_two");
                            fWorld.Ped.ForcePedAiAndAnimationUpdate(fPlayer.Player.Character, false, false);
                            ScriptCameraDirector.StopRendering();

                            camScene.Camera = reactionCamera;
                            camScene.PlayCam(fCore.Streaming.RequestAnimDict("missprologueig_3"),"react_to_explosion_cam");

                            timerScene.Phase = 0.075f + 0.05f;
                            camScene.Phase = 0.421f;

                            DisableMovementControls();
                            fInterior.Interior.SetRoomForGameViewportByName("V_CashD_reception");

                            if (camScene.IsRunning && timerScene.IsRunning)
                            {
                                fCore.Timer.SetTimerA(0);
                                vaultScene = 6;
                            }
                        }
                        break;

                    case 6:
                        fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                        fCore.Streaming.UseParticleFXAsset("scr_prologue");

                        if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                            Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5310.245f, -5205.663f, 85.2259f, 0f, 0f, 0f, 1f, false, false, false);

                        fCore.Timer.SetTimerA(0);
                        vaultScene = 7;
                        break;

                    case 7:
                        fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                        fCore.Streaming.UseParticleFXAsset("scr_prologue");

                        if (timerScene.Phase >= 0.085f + 0.05f)
                        {
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5309.8f, -5207.6f, 85.40824f, 0f, 0f, 90f, 1f, false, false, false);

                            fCore.Timer.SetTimerA(0);
                            vaultScene = 8;
                        }
                        break;

                    case 8:
                        fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                        fCore.Streaming.UseParticleFXAsset("scr_prologue");

                        if (timerScene.Phase >= 0.087f + 0.05f)
                        {
                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5313.9927f, -5207.3f, 85.34588f, 0f, 0f, 180f, 1f, false, false, false);

                            fCore.Timer.SetTimerA(0);
                            vaultScene = 9;
                        }
                        break;

                    case 9:
                        if (timerScene.Phase >= 0.35f + 0.05f)
                        {
                            Function.Call(Hash.STOP_CAM_SHAKING, vaultCamera, true);

                            if (vaultZoomInCam == null)
                                vaultZoomInCam = Function.Call<Camera>(Hash.CREATE_CAMERA, fWorld.Misc.joaat("DEFAULT_SCRIPTED_CAMERA"), true);

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

                            Function.Call(Hash.CLEAR_PED_TASKS, fPlayer.Player.Character);
                            Function.Call(Hash.CLEAR_PED_TASKS, invisibleTimerPed);

                            DisableMovementControls();
                            fInterior.Interior.SetRoomForGameViewportByName("V_CashD_vault");

                            if (dust == null)
                                dust = Function.Call<Prop>(Hash.CREATE_OBJECT_NO_OFFSET, fWorld.Misc.joaat("v_ilev_cd_dust"), 5312.14f, -5209.04f, 83.02f, true, true, false, 0);
                            else
                            {
                                Function.Call(Hash.FREEZE_ENTITY_POSITION, dust, true);
                                Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, fWorld.Misc.joaat("v_ilev_cd_dust"));
                            }

                            fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                            fCore.Streaming.UseParticleFXAsset("scr_prologue");

                            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_ceiling_debris", 5298.206f, -5189.0635f, 85.281166f, 0f, 0f, 180f, 1f, false, false, false);

                            fCore.Timer.SetTimerA(0);
                            vaultScene = 10;
                        }
                        break;

                    case 10:
                        if (fCore.Timer.TimerA() > 3000)
                        {
                            if (invisibleTimerPed != null)
                            {
                                invisibleTimerPed.Delete();
                                invisibleTimerPed = null;
                            }

                            Game.Player.SetControlState(true, SetPlayerControlFlags.AmbientScript);
                            fPlayer.Player.PedPos(5308.856f, -5206.294f, 85.7187f - 3.2f, 355.824f);
                            fWorld.Ped.ForcePedMotionState(fPlayer.Player.Character, fWorld.Ped.MotionStates.ActionModeIdle);
                            fUI.Hud.RadarAndHud(true, true);
                            DisableMovementControls();

                            if (vaultCamera != null)
                            {
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL, vaultCamera, 0f);
                                Function.Call(Hash.SET_CAM_DOF_FNUMBER_OF_LENS, vaultCamera, 2.8f);
                                vaultCamera.Delete();
                                vaultCamera = null;
                            }

                            vaultCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
                            ScriptCameraDirector.StopRendering();
                            Function.Call(Hash.CASCADE_SHADOWS_INIT_SESSION);
                            fPlayer.Player.Character.IsInvincible = false;
                            Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);

                            if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, 0) == 4)
                                Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, -5f, 1f);
                            else
                                Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, -30f, 1f);

                            if (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT))
                                Function.Call(Hash.DO_SCREEN_FADE_IN, 800);

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

                        fUI.Graphics.ClearTimecycleModifier();

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
                            dust = Function.Call<Prop>(Hash.CREATE_OBJECT_NO_OFFSET, fWorld.Misc.joaat("v_ilev_cd_dust"), 5312.14f, -5209.04f, 83.02f, true, true, false, 0);
                        }

                        Function.Call(Hash.DEACTIVATE_AUDIO_SLOWMO_MODE, "SLOWMO_PROLOGUE_VAULT");

                        StopAudioSceneIfActive("PROLOGUE_THREATEN_HOSTAGES");
                        StopAudioSceneIfActive("PROLOGUE_VAULT_RAYFIRE");
                        StopAudioSceneIfActive("PROLOGUE_MUTE_SPRINKLERS");
                        StopAudioSceneIfActive("PROLOGUE_DETONATE_CHARGES");

                        fUI.Hud.ClearHelp(true);
                        fWorld.Misc.SetTimeScale(1f);

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

                        fPlayer.Player.Character.Task.ClearAllImmediately();

                        if (Rayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                            Rayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 9);

                        fCore.Timer.SetTimerA(0);
                        vaultScene = 12;
                        break;
                }
            }

            public static void PrologueVaultRayfireSceneCleanup()
            {
                if (Rayfire.DoesRayfireMapObjectExist(rayfireVaultDoor))
                    Rayfire.SetStateOfRayfireMapObject(rayfireVaultDoor, 4);

                Game.Player.SetControlState(true, SetPlayerControlFlags.AmbientScript);
                camScene.Dispose();
                timerScene.Dispose();

                StopAudioSceneIfActive("PROLOGUE_THREATEN_HOSTAGES");
                StopAudioSceneIfActive("PROLOGUE_VAULT_RAYFIRE");
                StopAudioSceneIfActive("PROLOGUE_MUTE_SPRINKLERS");
                StopAudioSceneIfActive("PROLOGUE_DETONATE_CHARGES");

                fCore.Streaming.RemoveNamedPTFXAsset("scr_prologue");

                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, 0);
                Function.Call(Hash.REMOVE_PARTICLE_FX, PTFX1, 0);
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, 0);
                Function.Call(Hash.REMOVE_PARTICLE_FX, PTFX2, 0);

                Function.Call(Hash.DEACTIVATE_AUDIO_SLOWMO_MODE, "SLOWMO_PROLOGUE_VAULT");
                Function.Call(Hash.STOP_ALARM, "PROLOGUE_VAULT_ALARMS", true);
                Function.Call(Hash.STOP_STREAM);
                Function.Call(Hash.STOP_AUDIO_SCENES);
                Function.Call(Hash.REMOVE_PTFX_ASSET);

                vaultZoomInCam?.Delete(); vaultZoomInCam = null;
                reactionCamera?.Delete(); reactionCamera = null;
                vaultCamera?.Delete(); vaultCamera = null;
                vaultDoorScene?.Delete(); vaultDoorScene = null;
                dust?.Delete(); dust = null;

                if (invisibleTimerPed != null)
                {
                    invisibleTimerPed.Delete();
                    invisibleTimerPed = null;
                }
                if (BombPlantScene._permBomb != null)
                {
                    BombPlantScene._permBomb.Delete();
                    BombPlantScene._permBomb = null;
                }
            }
        }

        public static class PrologueDoorRayfire
        {
            public static int doorScene = 0;

            // Left public — may be read externally to check door state.
            // Fixed: was initialized via native at class-load time; re-fetched in case 0.
            public static Prop RayfireDoor = null;

            private static Prop bombC4;
            private static Prop bombC4green;
            private static int soundID = -1;
            private static int soundID2 = -1;
            private static int soundID3 = -1;
            private static int particleTimer = 0;
            private static int PTFX1 = 0;
            private static int PTFX2 = 0;

            private static SynchronizedScene bradScene = new SynchronizedScene(new Vector3(5316.087f, -5178.637f, 82.519f));

            public static void Scene()
            {
                switch (doorScene)
                {
                    case 0:
                        fCore.Streaming.RequestAnimDict("missprologueig_5@set_c4_mainaction");

                        // Native calls deferred to here rather than at class-load time.
                        RayfireDoor = Rayfire.GetRayfireMapObject(new Vector3(5318.2f, -5185.1f, 83.7f), 10f, "des_prologue_door");

                        if (Rayfire.DoesRayfireMapObjectExist(RayfireDoor))
                            Rayfire.SetStateOfRayfireMapObject(RayfireDoor, 2);

                        if (fPlayer.Player.Character.Weapons.Current.Group != WeaponGroup.AssaultRifle)
                            fPlayer.Player.Character.Weapons.Select(WeaponHash.PumpShotgun);

                        Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
                        fPlayer.Player.Character.SetResetFlag(PedResetFlagToggles.ForceScriptedCameraLowCoverAngleWhenEnteringCover, true);

                        if (!fAudio.Audio.IsAudioSceneActive("PROLOGUE_TAKE_COVER"))
                            fAudio.Audio.StartAudioScene("PROLOGUE_TAKE_COVER");

                        bradScene.Create();
                        bradScene.PlayPed(fPlayer.Player.Character, fCore.Streaming.RequestAnimDict("missprologueig_5@set_c4_mainaction"), "set_c4_mainaction_brad", 4f, -2f, (SyncedSceneFlags)5, (RagdollBlockingFlags)51, 1000f);
                        bradScene.Phase = 0f;
                        bradScene.IsLooping = false;

                        bombC4 = World.CreateProp(new Model(fWorld.Misc.joaat("prop_c4_final")), fWorld.Ped.GetPedBoneCoords(fPlayer.Player.Character, 60309, Vector3.Zero), Vector3.Zero, false, false);

                        while (bombC4 == null)
                            Wait(0);

                        fWorld.Entity.AttachEntityToEntity(bombC4, fPlayer.Player.Character, fWorld.Ped.GetPedBoneIndex(fPlayer.Player.Character, 60309), Vector3.Zero, Vector3.Zero, false, false, false, false, 2, true, 0);

                        bombC4green = World.CreateProp(new Model(fWorld.Misc.joaat("prop_c4_final_green")), new Vector3(5298.27f, -5187.85f, 83.87f), new Vector3(0f, 0f, -90.52732f), false, false);

                        while (bombC4green == null)
                            Wait(0);

                        fWorld.Entity.AttachEntityToEntity(bombC4green, fPlayer.Player.Character, fWorld.Ped.GetPedBoneIndex(fPlayer.Player.Character, 60309), Vector3.Zero, Vector3.Zero, false, false, false, false, 2, true, 0);

                        // Fixed: bombC4green?.IsVisible = false is invalid C# —
                        // null-conditional cannot appear on the left side of a property assignment.
                        if (bombC4green != null) bombC4green.IsVisible = false;

                        if (Rayfire.DoesRayfireMapObjectExist(RayfireDoor))
                            Rayfire.SetStateOfRayfireMapObject(RayfireDoor, 4);

                        doorScene = 1;
                        break;

                    case 1:
                        fAudio.Audio.LoadStream("PROLOGUE_BLAST_SECURITY_DOORS_MASTER", 0);
                        Game.DisableControlThisFrame(Control.NextCamera);

                        if (bradScene.IsRunning && bradScene.Phase > 0.239f)
                        {
                            if (bombC4 != null && bombC4.Exists())
                            {
                                fWorld.Entity.DetachEntity(bombC4, false, true);
                                fWorld.Entity.DetachEntity(bombC4green, false, true);
                                bombC4.IsPositionFrozen = true;
                                bombC4green.IsPositionFrozen = true;
                            }
                            doorScene = 2;
                        }
                        break;

                    case 2:
                        fAudio.Audio.LoadStream("PROLOGUE_BLAST_SECURITY_DOORS_MASTER", 0);
                        Game.DisableControlThisFrame(Control.NextCamera);

                        if (bradScene.IsRunning)
                        {
                            if (bradScene.Phase > 0.403f)
                            {
                                // Fixed: ?.IsVisible = value is invalid C#.
                                if (bombC4 != null) 
                                    bombC4.IsVisible = false;
                                if (bombC4green != null) 
                                    bombC4green.IsVisible = true;

                                if (soundID == -1)
                                {
                                    soundID = fAudio.Audio.GetSoundId();
                                    fAudio.Audio.PlaySoundFromEntity(soundID, "Security_Door_Bomb_Bleeps", bombC4green, "Prologue_Sounds");
                                }
                            }

                            if (bradScene.Phase > 0.453f)
                            {
                                if (bombC4 != null) 
                                    bombC4.IsVisible = false;
                                if (bombC4green != null) 
                                    bombC4green.IsVisible = true;

                                fWorld.Misc.ClearArea(5318.122f, -5185.5044f, 85.7186f - 3.2f, 4f, true, false, false, false);

                                fCore.Streaming.RequestNamedPTFXAsset("scr_prologue");
                                fCore.Streaming.UseParticleFXAsset("scr_prologue");

                                if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                    Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_prologue_door_blast", 5318f, -5185.06f, 83.82f, 0f, 0f, 0f, 1f, false, false, false);

                                if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                {
                                    if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX1))
                                        PTFX1 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_prologue_vault_fog", 5313.3545f, -5177.7656f, 82.5186f, 0f, 0f, 0f, 1f, false, false, false, false);
                                }

                                if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_prologue"))
                                {
                                    if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX2))
                                    {
                                        PTFX2 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "ent_ray_pro_door_embers", 5318.1626f, -5184.8325f, 82.5186f, 0f, 0f, 0f, 1f, false, false, false, false);
                                        particleTimer = Game.GameTime + 15000;
                                    }
                                }

                                fPlayer.Input.SetControlShake(0, 500, 256);
                                GameplayCamera.Shake(CameraShake.MediumExplosion, 0.25f);

                                soundID2 = fAudio.Audio.GetSoundId();
                                fAudio.Audio.PlaySoundFromCoord(soundID2, "COPS_ARRIVE", new Vector3(5359.9f, -5190f, 83f), "Prologue_Sounds");

                                soundID3 = fAudio.Audio.GetSoundId();
                                fAudio.Audio.PlaySoundFromCoord(soundID3, "Security_Door_Alarm", new Vector3(5318.2f, -5184.8f, 84.1f), "Prologue_Sounds");

                                if (soundID != -1)
                                {
                                    fAudio.Audio.StopSound(soundID);
                                    fAudio.Audio.ReleaseSoundId(soundID);
                                    soundID = -1;
                                }

                                Audio.SetAudioFlag(AudioFlags.DisableReplayScriptStreamRecording, true);
                                fAudio.Audio.PlayStreamFrontend();

                                if (Rayfire.DoesRayfireMapObjectExist(RayfireDoor))
                                    Rayfire.SetStateOfRayfireMapObject(RayfireDoor, 6);

                                bombC4?.Delete(); bombC4 = null;
                                bombC4green?.Delete(); bombC4green = null;

                                fPlayer.Player.Character.PlayAmbientSpeech("GENERIC_CURSE_HIGH", SpeechModifier.ForceShoutedClear);

                                doorScene = 3;
                            }
                        }
                        break;

                    case 3:
                        Game.DisableControlThisFrame(Control.NextCamera);

                        if (bradScene.IsRunning)
                        {
                            if (bradScene.Phase > 0.63f)
                            {
                                fUI.Hud.RadarAndHud(true, true);
                                GameplayCamera.DisableOnFootFirstPersonViewThisUpdate();
                                Game.Player.SetControlState(true);
                                fUI.Hud.ClearSubtitles();
                            }
                            if (bradScene.Phase > 0.825f)
                            {
                                bradScene.Dispose();
                                fPlayer.Player.Character.Task.ClearAll();
                                doorScene++;
                            }
                        }
                        break;
                }

                // Stop the ember PTFX after its timer expires
                if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PTFX2) && Game.GameTime >= particleTimer)
                    Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
            }

            public static void PrologueDoorSceneCleanup()
            {
                if (Rayfire.DoesRayfireMapObjectExist(RayfireDoor))
                    Rayfire.SetStateOfRayfireMapObject(RayfireDoor, 2);

                Game.Player.SetControlState(true, SetPlayerControlFlags.AmbientScript);
                bradScene.Dispose();

                StopAudioSceneIfActive("PROLOGUE_TAKE_COVER");

                ReleaseSoundId(ref soundID);
                ReleaseSoundId(ref soundID2);
                ReleaseSoundId(ref soundID3);

                fCore.Streaming.RemoveNamedPTFXAsset("scr_prologue");

                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, 0);
                Function.Call(Hash.REMOVE_PARTICLE_FX, PTFX1, 0);
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, 0);
                Function.Call(Hash.REMOVE_PARTICLE_FX, PTFX2, 0);

                Function.Call(Hash.STOP_STREAM);
                Function.Call(Hash.STOP_AUDIO_SCENES);

                bombC4?.Delete(); bombC4 = null;
                bombC4green?.Delete(); bombC4green = null;
            }

            // Stops and releases a sound ID, then resets it to -1.
            private static void ReleaseSoundId(ref int id)
            {
                if (id == -1) 
                    return;
                fAudio.Audio.StopSound(id);
                fAudio.Audio.ReleaseSoundId(id);
                id = -1;
            }
        }

        private static void StopAudioSceneIfActive(string sceneName)
        {
            if (fAudio.Audio.IsAudioSceneActive(sceneName))
                fAudio.Audio.StopAudioScene(sceneName);
        }
    }
}
