using GTA;
using GTA.Graphics;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.Win32;
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
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
//using static TheNorthYanktonHeist.Functions;
using Hash = GTA.Native.Hash;
using Screen = GTA.UI.Screen;

namespace TheNorthYanktonHeist
{
    /*
    internal class Functions
    {
        public class fRespawn : Script
        {
            static Wanted PlayerWantedLevel;
            public class Vector4
            {
                public Vector4(float x, float y, float z, float h)
                {
                    X = x;
                    Y = y;
                    Z = z;
                    H = h;
                }

                public static Vector4 Zero
                {
                    get
                    {
                        return new Vector4(0f, 0f, 0f, 0f);
                    }
                }

                public float X;

                public float Y;

                public float Z;

                public float H;
            }

            public static int ReturnWantedLevel = 0;

            public static int ReturnHour = fClock.GetClockHours();

            public static int ReturnMinute = fClock.GetClockMinutes();

            public static int ReturnSecond = fClock.GetClockSeconds();

            public static Vector4 playerSpawn = null;

            public enum Spawnpointflags
            {
                SPAWNPOINTS_FLAG_DEFAULT,
                SPAWNPOINTS_FLAG_MAY_SPAWN_IN_INTERIOR,
                SPAWNPOINTS_FLAG_MAY_SPAWN_IN_EXTERIOR,
                SPAWNPOINTS_FLAG_ALLOW_NOT_NETWORK_SPAWN_CANDIDATE_POLYS = 4,
                SPAWNPOINTS_FLAG_ALLOW_ISOLATED_POLYS = 8,
                SPAWNPOINTS_FLAG_ALLOW_ROAD_POLYS = 16,
                SPAWNPOINTS_FLAG_ONLY_POINTS_AGAINST_EDGES = 32
            }
            public fRespawn()
            {
                Tick += OnTick;
            }

            private void OnTick(object sender, EventArgs e)
            {
                if (GlobalVariable.Get(5).Read<int>() == 1)
                {
                    RespawnControl();
                }
            }
            public unsafe static void RespawnControl()
            {
                int num = Game.GameTime + 2000;
                bool flag = false;
                if (Game.Player.Character.IsDead)
                {
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                    Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED");
                    Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED", false, -1);
                    Function.Call(Hash.START_AUDIO_SCENE, "DEATH_SCENE");
                    Script.Wait(50);
                    Scaleform scaleform = Scaleform.RequestMovie("MP_BIG_MESSAGE_FREEMODE");
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds", true);
                    GameplayCamera.Shake(CameraShake.DeathFail, 1.5f);
                    fGraphics.AnimpostFXPlay("DeathFailMPIn", 0, false);
                    int red = 255;
                    int num2 = 0;
                    int num3 = 0;
                    while (!Screen.IsFadedOut)
                    {
                        if (Game.GameTime > num)
                        {
                            scaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", new object[]
                            {
                    fGraphics.ToColorHexString(Color.FromArgb(255, red, num2, num3), "WASTED"),
                    "",
                    0,
                    true,
                    true
                            });
                            scaleform.Render2D();
                            if (!flag)
                            {
                                fGraphics.SetTransitionTimecycleModifier("NG_deathfail_BW_base", 10f);
                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TextHit", "WastedSounds", true);
                                flag = true;
                            }
                            else
                            {
                                if (num2 < 255)
                                {
                                    num2 += 5;
                                }
                                if (num3 < 255)
                                {
                                    num3 += 5;
                                }
                            }
                        }
                        Script.Yield();
                    }
                    while (!Screen.IsFadingIn)
                    {
                        scaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", new object[]
                        {
                fGraphics.ToColorHexString(Color.FromArgb(255, red, num2, num3), "WASTED"),
                "",
                0,
                true,
                true
                        });
                        scaleform.Render2D();
                        Script.Yield();
                    }
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                    Function.Call(Hash.STOP_AUDIO_SCENE, "DEATH_SCENE");
                    Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED");
                    scaleform.Dispose();
                    fClock.SetClockTime(ReturnHour, ReturnMinute, ReturnSecond);
                    Screen.StopEffects();
                    fGraphics.AnimpostFXStopAll();
                    fGraphics.ClearTimecycleModifier();
                    GameplayCamera.StopShaking();
                    fHud.RadarAndHud(true, true);
                    if (playerSpawn == null)
                    {
                        Vector3 position = Game.Player.Character.Position;
                        int num4 = 2;
                        float value = 150f;
                        if (fInterior.GetInteriorFromEntity(Game.Player.Character) > 0U)
                        {
                            value = 150f;
                            num4 = 1;
                        }
                        if (Function.Call<bool>(Hash.SPAWNPOINTS_IS_SEARCH_ACTIVE))
                        {
                            Function.Call(Hash.SPAWNPOINTS_CANCEL_SEARCH);
                        }
                        Function.Call(Hash.SPAWNPOINTS_START_SEARCH, position.X, position.Y, position.Z, value, 5f, 24 | num4 | 32, -1f, 20000);
                        while (!Function.Call<bool>(Hash.SPAWNPOINTS_IS_SEARCH_COMPLETE))
                        {
                            Script.Wait(0);
                        }
                        float GroundZ;
                        float GroundZ2;
                        int num5 = Function.Call<int>(Hash.SPAWNPOINTS_GET_NUM_SEARCH_RESULTS);
                        int value2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, num5);
                        Function.Call(Hash.SPAWNPOINTS_GET_SEARCH_RESULT, value2, &position.X, &position.Y, &position.Z);
                        World.GetGroundHeight(new Vector3(position.X, position.Y, position.Z), out GroundZ, GetGroundHeightMode.Normal);
                        playerSpawn = new Vector4(position.X, position.Y, GroundZ, 0f);
                        World.GetGroundHeight(new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z), out GroundZ2, GetGroundHeightMode.Normal);
                        if (num5 == 0)
                        {
                            playerSpawn = new Vector4(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, GroundZ2, 0f);
                        }
                    }
                    fPlayer.PedPos(playerSpawn.X, playerSpawn.Y, playerSpawn.Z, playerSpawn.H);
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                    playerSpawn = null;
                    PlayerWantedLevel.SetWantedLevel(ReturnWantedLevel, false);
                    Script.Wait(2000);
                    Screen.FadeIn(500);
                    if (fHud.IsHelpMessageBeingDisplayed())
                    {
                        fHud.ClearHelp(true);
                    }
                }
            }

            public static void SetRespawnStat(bool toggle)
            {
                if (toggle)
                {
                    GlobalVariable.Get(5).Write<int>(1);
                    Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                    fMisc.DisableHospitalRestart(0, true);
                    fMisc.DisableHospitalRestart(1, true);
                    fMisc.DisableHospitalRestart(2, true);
                    fMisc.DisableHospitalRestart(3, true);
                    fMisc.DisableHospitalRestart(4, true);
                    fMisc.DisableHospitalRestart(5, true);
                    fMisc.DisablePoliceRestart(0, true);
                    fMisc.DisablePoliceRestart(1, true);
                    fMisc.DisablePoliceRestart(2, true);
                    fMisc.DisablePoliceRestart(3, true);
                    fMisc.DisablePoliceRestart(4, true);
                    fMisc.DisablePoliceRestart(5, true);
                    fMisc.DisablePoliceRestart(6, true);
                }
                else
                {
                    GlobalVariable.Get(5).Write<int>(0);
                    Function.Call(Hash.REQUEST_SCRIPT, "respawn_controller");
                    while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, "respawn_controller"))
                        Script.Wait(0);
                    Function.Call<int>(Hash.START_NEW_SCRIPT, "respawn_controller", 128);
                    Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, "respawn_controller");
                    fMisc.DisableHospitalRestart(0, false);
                    fMisc.DisableHospitalRestart(1, false);
                    fMisc.DisableHospitalRestart(2, false);
                    fMisc.DisableHospitalRestart(3, false);
                    fMisc.DisableHospitalRestart(4, false);
                    fMisc.DisableHospitalRestart(5, false);
                    fMisc.DisablePoliceRestart(0, false);
                    fMisc.DisablePoliceRestart(1, false);
                    fMisc.DisablePoliceRestart(2, false);
                    fMisc.DisablePoliceRestart(3, false);
                    fMisc.DisablePoliceRestart(4, false);
                    fMisc.DisablePoliceRestart(5, false);
                    fMisc.DisablePoliceRestart(6, false);
                }
            }
        }

        

        public class fAnimations
        {
            public static bool IsSynchronizedSceneRunning(int sceneID)
            {
                return Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, sceneID);
            }
            public static int CreateSynchronizedScene(Vector3 xyz, float roll = 0, float pitch = 0, float yaw = 0, int p6 = 2)
            {
                return Function.Call<int>(Hash.CREATE_​SYNCHRONIZED_​SCENE, xyz.X, xyz.Y, xyz.Z, roll, pitch, yaw, p6);
            }
            public static void SetSynchronizedScenePhase(int sceneID, float phase)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, sceneID, phase);
            }
            public static float GetSynchronizedScenePhase(int sceneID)
            {
                return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, sceneID);
            }
            public static void SetSynchronizedSceneRate(int sceneID, float rate)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, sceneID, rate);
            }
            public static float GetSynchronizedSceneRate(int sceneID)
            {
                return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, sceneID);
            }
            public static void SetSynchronizedSceneLooped(int sceneID, bool toggle)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sceneID, toggle);
            }
            public static bool IsSynchronizedSceneLooped(int sceneID)
            {
                return Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_LOOPED, sceneID);
            }
            public static void SetSynchronizedSceneHoldLastFrame(int sceneID, bool toggle)
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, sceneID, toggle);
            }
            public static bool IsSynchronizedSceneHoldLastFrame(int sceneID)
            {
                return Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, sceneID);
            }
            public static void PlaySynchronizedEntityAnim(Entity entity, int syncedScene, string animation, string AnimDict, float p5, int p6, float p4 = 1000.0f, float p7 = 1000.0f)
            {
                Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, entity, syncedScene, animation, AnimDict, p4, p5, p6, p7);
            }
            public static void TaskPlayAnim(Ped ped, string AnimDictName, string AnimName, float BlendInDelta = 8f, float BlendOutDelta = 8f, int timeToPlay = -1,
                AnimFlags AnimFlags = AnimFlags.AF_DEFAULT, float startPhase = 0f, bool phaseControlled = false, IkControlFlags ikFlags = IkControlFlags.AIK_NONE, bool allowOverrideCloneUpdate = false)
            {
                Function.Call(Hash.TASK_PLAY_ANIM, ped, AnimDictName, AnimName, BlendInDelta, BlendOutDelta, timeToPlay, AnimFlags, startPhase, phaseControlled, ikFlags, allowOverrideCloneUpdate);
            }
            public static void TaskPlayAnimAdvanced(Ped ped, string AnimDictName, string AnimName, Vector3 pos, Vector3 rot, float BlendInDelta = 8f, float BlendOutDelta = 8f,
                int TimeToPlay = -1, AnimFlags AnimFlags = AnimFlags.AF_DEFAULT, float startPhase = 0f, int RotOrder = 2, IkControlFlags ikFlags = IkControlFlags.AIK_NONE)
            {
                Function.Call(Hash.TASK_PLAY_ANIM_ADVANCED, ped, AnimDictName, AnimName, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, BlendInDelta, BlendOutDelta, TimeToPlay, AnimFlags, startPhase, RotOrder, ikFlags);
            }
            public static float GetAnimDuration(string animDict, string animName)
            {
                return Function.Call<float>(Hash.GET_ANIM_DURATION, animDict, animName);
            }
            public static float GetEntityAnimCurrentTime(Entity entity, string animDict, string animName)
            {
                return Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, entity, animDict, animName);
            }
            public static float GetEntityAnimTotalTime(Entity entity, string animDict, string animName)
            {
                return Function.Call<float>(Hash.GET_ENTITY_ANIM_TOTAL_TIME, entity, animDict, animName);
            }
            public static void TaskSynchronizedScene(Ped ped, int sceneID, string animDict, string animName, float blendIn, float blendOut, SyncScenePlaybackFlags flags, int ragdollBlockFlags, float moverBlendDelta, int ikFlags)
            {
                Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ped, sceneID, animDict, animName, blendIn, blendOut, flags, ragdollBlockFlags, moverBlendDelta, ikFlags);
            }
            public static void TakeOwnershipOfSynchronizedScene(int sceneID)
            {
                Function.Call(Hash.TAKE_OWNERSHIP_OF_SYNCHRONIZED_SCENE, sceneID);
            }

            public enum SyncScenePlaybackFlags
            {
                SYNCED_SCENE_NONE,
                SYNCED_SCENE_USE_PHYSICS,
                SYNCED_SCENE_TAG_SYNC_OUT,
                SYNCED_SCENE_DONT_INTERRUPT = 4,
                SYNCED_SCENE_ON_ABORT_STOP_SCENE = 8,
                SYNCED_SCENE_ABORT_ON_WEAPON_DAMAGE = 16,
                SYNCED_SCENE_BLOCK_MOVER_UPDATE = 32,
                SYNCED_SCENE_LOOP_WITHIN_SCENE = 64,
                SYNCED_SCENE_PRESERVE_VELOCITY = 128,
                SYNCED_SCENE_EXPAND_PED_CAPSULE_FROM_SKELETON = 256,
                SYNCED_SCENE_ACTIVATE_RAGDOLL_ON_COLLISION = 512,
                SYNCED_SCENE_HIDE_WEAPON = 1024,
                SYNCED_SCENE_ABORT_ON_DEATH = 2048,
                SYNCED_SCENE_VEHICLE_ABORT_ON_LARGE_IMPACT = 4096,
                SYNCED_SCENE_VEHICLE_ALLOW_PLAYER_ENTRY = 8192,
                SYNCED_SCENE_PROCESS_ATTACHMENTS_ON_START = 16384,
                SYNCED_SCENE_NET_ON_EARLY_NON_PED_STOP_RETURN_TO_START = 32768,
                SYNCED_SCENE_SET_PED_OUT_OF_VEHICLE_AT_START = 65536,
                SYNCED_SCENE_NET_DISREGARD_ATTACHMENT_CHECKS = 131072
            }
            public enum AnimFlags
            {
                AF_DEFAULT,
                AF_LOOPING,
                AF_HOLD_LAST_FRAME,
                AF_REPOSITION_WHEN_FINISHED = 4,
                AF_NOT_INTERRUPTABLE = 8,
                AF_UPPERBODY = 16,
                AF_SECONDARY = 32,
                AF_REORIENT_WHEN_FINISHED = 64,
                AF_ABORT_ON_PED_MOVEMENT = 128,
                AF_ADDITIVE = 256,
                AF_TURN_OFF_COLLISION = 512,
                AF_OVERRIDE_PHYSICS = 1024,
                AF_IGNORE_GRAVITY = 2048,
                AF_EXTRACT_INITIAL_OFFSET = 4096,
                AF_EXIT_AFTER_INTERRUPTED = 8192,
                AF_TAG_SYNC_IN = 16384,
                AF_TAG_SYNC_OUT = 32768,
                AF_TAG_SYNC_CONTINUOUS = 65536,
                AF_FORCE_START = 131072,
                AF_USE_KINEMATIC_PHYSICS = 262144,
                AF_USE_MOVER_EXTRACTION = 524288,
                AF_HIDE_WEAPON = 1048576,
                AF_ENDS_IN_DEAD_POSE = 2097152,
                AF_ACTIVATE_RAGDOLL_ON_COLLISION = 4194304,
                AF_DONT_EXIT_ON_DEATH = 8388608,
                AF_ABORT_ON_WEAPON_DAMAGE = 16777216,
                AF_DISABLE_FORCED_PHYSICS_UPDATE = 33554432,
                AF_PROCESS_ATTACHMENTS_ON_START = 67108864,
                AF_EXPAND_PED_CAPSULE_FROM_SKELETON = 134217728,
                AF_USE_ALTERNATIVE_FP_ANIM = 268435456,
                AF_BLENDOUT_WRT_LAST_FRAME = 536870912,
                AF_USE_FULL_BLENDING = 1073741824
            }
            public enum IkControlFlags
            {
                AIK_NONE,
                AIK_DISABLE_LEG_IK,
                AIK_DISABLE_ARM_IK,
                AIK_DISABLE_HEAD_IK = 4,
                AIK_DISABLE_TORSO_IK = 8,
                AIK_DISABLE_TORSO_REACT_IK = 16,
                AIK_USE_LEG_ALLOW_TAGS = 32,
                AIK_USE_LEG_BLOCK_TAGS = 64,
                AIK_USE_ARM_ALLOW_TAGS = 128,
                AIK_USE_ARM_BLOCK_TAGS = 256,
                AIK_PROCESS_WEAPON_HAND_GRIP = 512,
                AIK_USE_FP_ARM_LEFT = 1024,
                AIK_USE_FP_ARM_RIGHT = 2048,
                AIK_DISABLE_TORSO_VEHICLE_IK = 4096,
                AIK_LINKED_FACIAL = 8192
            }
        }

    }*/
    
}
