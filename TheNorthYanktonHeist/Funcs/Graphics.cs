using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fGraphics
    {
        public static void SetTimeCycleModifier(string modifierName)
        {
            Function.Call(Hash.SET_​TIMECYCLE_​MODIFIER, modifierName);
        }
        public static void DrawMarkerSphere(Vector3 xyz, float radius, int red, int green, int blue, float alpha)
        {
            Function.Call(Hash.DRAW_​MARKER_​SPHERE, xyz.X, xyz.Y, xyz.Z, radius, red, green, blue, alpha);
        }
        public static void DrawRect(float x, float y, float width, float height, int r, int g, int b, int a, bool stereo)
        {
            Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a, stereo);
        }

        public static void DrawMarker(MarkerTypes type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, int red, int green, int blue, int alpha,
    bool bobUpAndDown = false, bool faceCamera = false, int p19 = 2, bool rotateY = false, string textureDict = null, string textureName = null, bool drawOnEnts = false)
        {
            Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, alpha, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
        }
        private static bool toggle1 = true;
        private static int integer1 = 0;
        public static void DrawMarker(bool FadeOutFadeIn, MarkerTypes type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, int red, int green, int blue, int alpha,
            float distanceTillFadeInActive = 20f,
    bool bobUpAndDown = false, bool faceCamera = false, int p19 = 2, bool rotateY = false, string textureDict = null, string textureName = null, bool drawOnEnts = false)
        {
            if (FadeOutFadeIn)
            {
                if (fPlayer.GetDistanceTo(pos) < distanceTillFadeInActive)
                {
                    if (toggle1)
                        integer1 = 1;
                    if (integer1 == 1)
                    {
                        for (int i = 0; i < alpha - 1; i += 2)
                        {
                            if (fHud.IsHudComponentActive(HudComponent.WeaponWheel) || fHud.IsHudComponentActive(HudComponent.RadioStationsWheel) || fHud.IsPauseMenuActive())
                                break;
                            if (i == alpha - 1)
                                break;
                            else
                            {
                                Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, i, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
                                Script.Yield();
                            }
                        }
                        integer1 = default;
                        toggle1 = false;
                    }
                    Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, alpha, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
                }
                else
                {
                    if (!toggle1)
                        integer1 = 1;
                    if (integer1 == 1)
                    {
                        for (int i = alpha; i > 0; i -= 2)
                        {
                            if (fHud.IsHudComponentActive(HudComponent.WeaponWheel) || fHud.IsHudComponentActive(HudComponent.RadioStationsWheel) || fHud.IsPauseMenuActive())
                                break;
                            if (i == 0)
                                break;
                            else
                            {
                                Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, i, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
                                Script.Yield();
                            }
                        }
                        integer1 = default;
                        toggle1 = true;
                    }
                }
            }
            else
                Function.Call(Hash.DRAW_​MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, red, green, blue, alpha, bobUpAndDown, faceCamera, p19, rotateY, textureDict, textureName, drawOnEnts);
        }

        public enum MarkerTypes
        {
            UpsideDownCone = 0,
            VerticalCylinder,
            ThickChevronUp,
            ThinChevronUp,
            CheckeredFlagRect,
            CheckeredFlagCircle,
            VerticleCircle,
            PlaneModel,
            LostMCDark,
            LostMCLight,
            Number0,
            Number1,
            Number2,
            Number3,
            Number4,
            Number5,
            Number6,
            Number7,
            Number8,
            Number9,
            ChevronUpx1,
            ChevronUpx2,
            ChevronUpx3,
            HorizontalCircleFat,
            ReplayIcon,
            HorizontalCircleSkinny,
            HorizontalCircleSkinny_Arrow,
            HorizontalSplitArrowCircle,
            DebugSphere,
            DallorSign,
            HorizontalBars,
            WolfHead,
            MarkerTypeQuestionMark,
            MarkerTypePlaneSymbol,
            MarkerTypeHelicopterSymbol,
            MarkerTypeBoatSymbol,
            MarkerTypeCarSymbol,
            MarkerTypeMotorcycleSymbol,
            MarkerTypeBikeSymbol,
            TruckSymbol,
            ParachuteSymbol,
            Thruster_Jetpack,
            SawbladeSymbol,
            Box
        };

        public static void AnimpostFXStopAll()
        {
            Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
        }
        public static void AnimpostFXPlay(string effectName, int duration, bool looped)
        {
            Function.Call(Hash.ANIMPOSTFX_PLAY, effectName, duration, looped);
        }
        public static string ColorToHex(Color color)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }
        public static string ToColorHexString(Color thisColor, string text)
        {
            return string.Concat(new string[]
            {
                "<FONT COLOR='#",
                fGraphics.ColorToHex(thisColor),
                "'>",
                text,
                "</FONT>"
            });
        }
        public static void SetTransitionTimecycleModifier(string modName, float transitionTime)
        {
            Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, modName, transitionTime);
        }
        public static void ClearTimecycleModifier()
        {
            Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
        }
    }
}
