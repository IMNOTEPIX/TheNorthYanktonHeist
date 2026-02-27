using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fCam
    {
        public static void StopCamShaking(Camera cam, bool p1)
        {
            Function.Call(Hash.STOP_​CAM_​SHAKING, cam, p1);
        }
        public static void StopRenderingScriptCamsIntoFirstPerson(bool bShouldApplyAcrossAllThreads, float distanceToBlend = 0.0f, eCamSplineSmoothingFlags blendType = eCamSplineSmoothingFlags.CAM_SPLINE_NO_SMOOTH)
        {
            Function.Call(Hash.STOP_RENDERING_SCRIPT_CAMS_USING_CATCH_UP, bShouldApplyAcrossAllThreads, distanceToBlend, (int)blendType);
        }
        public enum eCamSplineSmoothingFlags
        {
            // No smoothing just moves at a constant rate
            CAM_SPLINE_NO_SMOOTH = 0,
            // Decelerates when approaching a node
            CAM_SPLINE_SLOW_IN_SMOOTH = 1,
            // Accelerates slowly when leaving a node
            CAM_SPLINE_SLOW_OUT_SMOOTH = 2,
            // Decelerates when approaching a node and accelerates slowly when leaving a node
            CAM_SPLINE_SLOW_IN_OUT_SMOOTH = 3,
            CAM_SPLINE_VERY_SLOW_IN = 4,
            CAM_SPLINE_VERY_SLOW_OUT = 5,
            CAM_SPLINE_VERY_SLOW_IN_SLOW_OUT = 6,
            CAM_SPLINE_SLOW_IN_VERY_SLOW_OUT = 7,
            CAM_SPLINE_VERY_SLOW_IN_VERY_SLOW_OUT = 8,
            CAM_SPLINE_EASE_IN = 9,
            CAM_SPLINE_EASE_OUT = 10,
            CAM_SPLINE_QUADRATIC_EASE_IN = 11,
            CAM_SPLINE_QUADRATIC_EASE_OUT = 12,
            CAM_SPLINE_QUADRATIC_EASE_IN_OUT = 13,
            CAM_SPLINE_CUBIC_EASE_IN = 14,
            CAM_SPLINE_CUBIC_EASE_OUT = 15,
            CAM_SPLINE_CUBIC_EASE_IN_OUT = 16,
            CAM_SPLINE_QUARTIC_EASE_IN = 17,
            CAM_SPLINE_QUARTIC_EASE_OUT = 18,
            CAM_SPLINE_QUARTIC_EASE_IN_OUT = 19,
            CAM_SPLINE_QUINTIC_EASE_IN = 20,
            CAM_SPLINE_QUINTIC_EASE_OUT = 21,
            CAM_SPLINE_QUINTIC_EASE_IN_OUT = 22,
            CAM_SPLINE_CIRCULAR_EASE_IN = 23,
            CAM_SPLINE_CIRCULAR_EASE_OUT = 24,
            CAM_SPLINE_CIRCULAR_EASE_IN_OUT = 25
        };
        public static void ShakeCam(Camera cam, string type, float amplitude)
        {
            Function.Call(Hash.SHAKE_CAM, cam, type, amplitude);
        }
        public static void SetCamFov(Camera cam, float fov)
        {
            Function.Call(Hash.SET_CAM_FOV, cam, fov);
        }
        public static Camera CreateCam(string camName, bool createCamera = true)
        {
            return Function.Call<Camera>(Hash.CREATE_CAM, camName, createCamera);
        }
        public static Camera CreateScriptedCam()
        {
            Camera camera = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
            while (camera != null && !camera.Exists())
            {
                Script.Wait(0);
            }
            return camera;
        }
        public static Camera CreateAnimatedCam()
        {
            Camera camera = CreateCam("DEFAULT_ANIMATED_CAMERA", true);
            while (camera != null && !camera.Exists())
            {
                Script.Wait(0);
            }
            return camera;
        }
        public static void SetupMovingCam(Camera cam, Vector3 pos, Vector3 rot, float fov, CameraShake camShakeType, float camShakeIntensity)
        {
            if (cam != null)
            {
                cam.Detach();
                cam.StopPointing();
                cam.Position = pos;
                cam.Rotation = rot;
                Function.Call(Hash.SET_CAM_FOV, cam, fov);
                cam.Shake(camShakeType, camShakeIntensity);
            }
        }
        public static void SetCamActiveWithInterp(Camera DestinationCam, Camera OriginCam, int Duration, CamGraphType GraphTypePos = CamGraphType.GRAPH_TYPE_SIN_ACCEL_DECEL, CamGraphType GraphTypeRot = CamGraphType.GRAPH_TYPE_SIN_ACCEL_DECEL)
        {
            Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, DestinationCam, OriginCam, Duration, GraphTypePos, GraphTypeRot);
        }
        public static void RenderScriptCams(bool render, bool ease, int easeTime, bool bShouldLockInterpolationSourceFrame = true, bool bShouldApplyAcrossAllThreads = false, RenderingOptionFlag RenderingOptions = RenderingOptionFlag.RO_NO_OPTIONS)
        {
            Function.Call(Hash.RENDER_SCRIPT_CAMS, render, ease, easeTime, bShouldLockInterpolationSourceFrame, bShouldApplyAcrossAllThreads, RenderingOptions);
        }
        public static bool PlayCamAnim(Camera cam, string animName, string animDict, Vector3 pos, Vector3 rot, CamAnimationFlags animFlags, int rotOrder = 2)
        {
            return Function.Call<bool>(Hash.PLAY_CAM_ANIM, cam, animName, animDict, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, animFlags, rotOrder);
        }
        public static bool PlaySynchronizedCamAnim(Camera cam, int sceneID, string animName, string animDictionary)
        {
            return Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, cam, sceneID, animName, animDictionary);
        }
        public static bool IsCamPlayingAnim(Camera cam, string animName, string animDictionary)
        {
            return Function.Call<bool>(Hash.IS_CAM_PLAYING_ANIM, cam, animName, animDictionary);
        }

        public enum CamAnimationFlags
        {
            CAF_DEFAULT,
            CAF_LOOPING
        }
        public enum CamGraphType
        {
            GRAPH_TYPE_LINEAR,
            GRAPH_TYPE_SIN_ACCEL_DECEL,
            GRAPH_TYPE_ACCEL,
            GRAPH_TYPE_DECEL,
            GRAPH_TYPE_SLOW_IN,
            GRAPH_TYPE_SLOW_OUT,
            GRAPH_TYPE_SLOW_IN_OUT,
            GRAPH_TYPE_VERY_SLOW_IN,
            GRAPH_TYPE_VERY_SLOW_OUT,
            GRAPH_TYPE_VERY_SLOW_IN_SLOW_OUT,
            GRAPH_TYPE_SLOW_IN_VERY_SLOW_OUT,
            GRAPH_TYPE_VERY_SLOW_IN_VERY_SLOW_OUT,
            GRAPH_TYPE_EASE_IN,
            GRAPH_TYPE_EASE_OUT,
            GRAPH_TYPE_QUADRATIC_EASE_IN,
            GRAPH_TYPE_QUADRATIC_EASE_OUT,
            GRAPH_TYPE_QUADRATIC_EASE_IN_OUT,
            GRAPH_TYPE_CUBIC_EASE_IN,
            GRAPH_TYPE_CUBIC_EASE_OUT,
            GRAPH_TYPE_CUBIC_EASE_IN_OUT,
            GRAPH_TYPE_QUARTIC_EASE_IN,
            GRAPH_TYPE_QUARTIC_EASE_OUT,
            GRAPH_TYPE_QUARTIC_EASE_IN_OUT,
            GRAPH_TYPE_QUINTIC_EASE_IN,
            GRAPH_TYPE_QUINTIC_EASE_OUT,
            GRAPH_TYPE_QUINTIC_EASE_IN_OUT,
            GRAPH_TYPE_CIRCULAR_EASE_IN,
            GRAPH_TYPE_CIRCULAR_EASE_OUT,
            GRAPH_TYPE_CIRCULAR_EASE_IN_OUT,
            GRAPH_TYPE_MAX
        }
        public enum RenderingOptionFlag
        {
            RO_NO_OPTIONS,
            RO_STOP_RENDERING_OPTION_WHEN_PLAYER_EXITS_INTO_COVER
        }
    }
}
