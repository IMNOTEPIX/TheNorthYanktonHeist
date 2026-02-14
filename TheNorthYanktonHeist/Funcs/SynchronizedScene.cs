using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class SynchronizedScene : IDisposable
    {
        public int _handle { get; protected set; } = -1;
        public bool IsActive => _handle != -1;
        public bool IsRunning { get => Function.Call<bool>(Hash.IS_​SYNCHRONIZED_​SCENE_​RUNNING, _handle); }
        public float Phase
        {
            get => Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, _handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, _handle, value);
        }
        public float Rate
        {
            get => Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, _handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, _handle, value);
        }
        public bool IsLooping
        {
            get => Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_LOOPED, _handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, _handle, value);
        }
        public bool HoldLastFrame
        {
            get => Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, _handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, _handle, value);
        }
        public bool IsFinished => Phase >= 1.000f;

        public Vector3 Position { get; protected set; }
        public Vector3 Rotation { get; protected set; }

        public Camera Camera { get; set; }

        public SynchronizedScene(Vector3 position, float heading = 0f)
        {
            Position = position;
            Rotation = new Vector3(0f, 0f, heading);
        }

        public SynchronizedScene(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public SynchronizedScene(Prop prop)
        {
            Position = prop.Position;
            Rotation = prop.Rotation;
        }

        public void Create(int timeout = 500)
        {
            _handle = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, 2);
            int gameTime = Game.GameTime;
            while (!IsActive)
            {
                if (Game.GameTime - gameTime > timeout)
                    throw new TimeoutException("Timed out trying to create synchronized scene.");
                Script.Yield();
            }
        }
        public void PlayEntity(Entity entity, string animDict, string animName, float blendIn = 8f, float blendOut = 8f, SyncedSceneFlags flag = SyncedSceneFlags.None, float moveBlend = 1148846080)
        {
            if (IsActive && entity != null && entity.Exists())
            {
                Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, entity, _handle, animName, animDict, blendIn, blendOut, (int)flag, moveBlend);
                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, entity);
            }
        }
        public void PlayPed(Ped ped, string animDict, string animName, float blendIn = 8f, float blendOut = 8f, SyncedSceneFlags flag = SyncedSceneFlags.None, RagdollBlockingFlags ragdollBlockingFlags = RagdollBlockingFlags.None,float moveBlendRatio = 1148846080, AnimationIKControlFlags ikFlags = AnimationIKControlFlags.None)
        {
            if (IsActive && ped != null && ped.Exists())
            {
                Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ped, _handle, animDict, animName, blendIn, blendOut, (int)flag, (int)ragdollBlockingFlags, moveBlendRatio, (int)ikFlags);
                Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, ped, false, false);
            }
        }
        public void PlayCam(string animDict, string animName)
        {
            if (IsActive)
            {
                if (Camera == null)
                    Camera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", true);
                while (Camera == null)
                    Script.Wait(0);
                Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, Camera, _handle, animName, animDict);
                ScriptCameraDirector.StartRendering();
            }
        }
        public void DeleteCam(bool stopRendering)
        {
            if (Camera != null && Camera.Exists())
            {
                if (stopRendering)
                    ScriptCameraDirector.StopRendering(false);
                Camera.Delete();
                Camera = null;
            }
        }

        public bool PlayAudioEvent()
        {
            return IsActive && Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_AUDIO_EVENT, _handle);
        }
        public void AttachToEntity(Entity entity, EntityBone Bone)
        {
            if (IsActive && IsRunning)
                Function.Call(Hash.ATTACH_​SYNCHRONIZED_​SCENE_​TO_​ENTITY, _handle, entity, Bone.Index);
        }
        public void DetachScene()
        {
            if (IsActive && IsRunning)
                Function.Call(Hash.DETACH_​SYNCHRONIZED_​SCENE, _handle);
        }
        public void Dispose()
        {
            if (IsActive)
            {
                Function.Call(Hash.TAKE_​OWNERSHIP_​OF_​SYNCHRONIZED_​SCENE, _handle);
                _handle = -1;
                DeleteCam(true);
            }
        }
    }
}
