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
    public class fBlip
    {
        public static void ShowTickOnBlip(Blip blip, bool toggle)
        {
            Function.Call(Hash.SHOW_TICK_ON_BLIP, blip, toggle);
        }

        static bool IsBlipGameplayBlip(Blip blip)
        {
            if (blip.Sprite == (BlipSprite)0 || blip.Sprite == (BlipSprite)1 || blip.Sprite == (BlipSprite)2 || blip.Sprite == (BlipSprite)3 || blip.Sprite == (BlipSprite)4
                || blip.Sprite == (BlipSprite)5 || blip.Sprite == (BlipSprite)6 || blip.Sprite == (BlipSprite)7 || blip.Sprite == (BlipSprite)8 || blip.Sprite == (BlipSprite)9
                || blip.Sprite == (BlipSprite)10 || blip.Sprite == (BlipSprite)11 || blip.Sprite == (BlipSprite)12 || blip.Sprite == (BlipSprite)13 || blip.Sprite == (BlipSprite)14
                || blip.Sprite == (BlipSprite)15 || blip.Sprite == (BlipSprite)41 || blip.Sprite == (BlipSprite)42 || blip.Sprite == (BlipSprite)185 || blip.Sprite == (BlipSprite)162 /*(Point of interest)*/)
                return true;
            else
                return false;
        }
        static bool IsSpecificBlip_0(Blip blip)
        {
            if (blip.Sprite == (BlipSprite)71 || blip.Sprite == (BlipSprite)72 || blip.Sprite == (BlipSprite)73 || blip.Sprite == (BlipSprite)110
                || blip.Sprite == (BlipSprite)162 || blip.Sprite == (BlipSprite)313)
                return true;
            else
                return false;
        }
        static Blip[] allBlips => World.GetAllBlips();
        public static void SetMostBlipsInvisible(List<Blip> excludeListBlips)
        {
            for (int i = 0; i < allBlips.Length; i++)
            {
                if (allBlips[i] != null)
                {
                    if (!IsBlipGameplayBlip(allBlips[i]) && !IsSpecificBlip_0(allBlips[i]))
                    {
                        if (excludeListBlips.Count > 0 && excludeListBlips != null)
                        {
                            for (int ii = 0; ii < excludeListBlips.Count; ii++)
                            {
                                if (excludeListBlips[ii] != allBlips[i])
                                    allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                            }
                        }
                        else
                            allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                    }
                }
            }
        }
        public static void SetAllBlipsInvisible(List<Blip> excludeListBlips)
        {
            for (int i = 0; i < allBlips.Length; i++)
            {
                if (allBlips[i] != null)
                {
                    if (!IsBlipGameplayBlip(allBlips[i]))
                    {
                        if (excludeListBlips.Count > 0 && excludeListBlips != null)
                        {
                            for (int ii = 0; ii < excludeListBlips.Count; ii++)
                            {
                                if (excludeListBlips[ii] != allBlips[i])
                                    allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                            }
                        }
                        else
                            allBlips[i].DisplayType = BlipDisplayType.NoDisplay;
                    }
                }
            }
        }
        public static void SetAllBlipsVisible(List<Blip> excludeListBlips)
        {
            for (int i = 0; i < allBlips.Length; i++)
            {
                if (allBlips[i] != null)
                {
                    if (excludeListBlips.Count > 0 && excludeListBlips != null)
                    {
                        for (int ii = 0; ii < excludeListBlips.Count; ii++)
                        {
                            if (excludeListBlips[ii] != allBlips[i])
                                allBlips[i].DisplayType = BlipDisplayType.Default;
                        }
                    }
                    else
                        allBlips[i].DisplayType = BlipDisplayType.Default;
                }
            }
        }
        public static void SetAllBlipsVisible()
        {
            for (int i = 0; i < allBlips.Length; i++)
            {
                if (allBlips[i] != null)
                    allBlips[i].DisplayType = BlipDisplayType.Default;
            }
        }

        public static void DeleteListBlips(List<Blip> blipList)
        {
            if (blipList.Count > 0)
            {
                for (int i = 0; i < blipList.Count; i++)
                {
                    if (blipList[i] != null)
                        blipList[i].Delete();
                }
                blipList.Clear();
            }
        }

        public static Blip CreateBlipForRadiusWithParams(Vector3 pos, float radius, BlipColor Color)
        {
            Blip blip = AddBlipForRadius(pos, radius);
            if (blip != null)
            {
                blip.Color = Color;
                return blip;
            }
            return null;
        }
        public static Blip AddBlipForRadius(Vector3 pos, float radius)
        {
            return Function.Call<Blip>(Hash.ADD_BLIP_FOR_RADIUS, pos.X, pos.Y, pos.Z, radius);
        }

        public static Blip CreateBlipForCoordWithParams(Vector3 pos, BlipSprite Sprite, BlipColor Color, float Scale, string Name, int Alpha = 255)
        {
            Blip blip = AddBlipForCoord(pos);
            if (blip != null)
            {
                blip.Alpha = Alpha;
                blip.Sprite = Sprite;
                blip.Color = Color;
                blip.Scale = Scale;
                blip.Name = Name;
                return blip;
            }
            return null;
        }
        public static Blip CreateBlipForCoordWithParams(Vector3 pos, BlipSprite Sprite, BlipColor Color, float ScaleX, float ScaleY, string Name, int Alpha = 255)
        {
            Blip blip = AddBlipForCoord(pos);
            if (blip != null)
            {
                blip.Alpha = Alpha;
                blip.Sprite = Sprite;
                blip.Color = Color;
                blip.ScaleX = ScaleX;
                blip.ScaleY = ScaleY;
                blip.Name = Name;
                return blip;
            }
            return null;
        }
        public static Blip AddBlipForCoord(Vector3 xyz)
        {
            return Function.Call<Blip>(Hash.ADD_​BLIP_​FOR_​COORD, xyz.X, xyz.Y, xyz.Z);
        }

        public static Blip CreateGPSBlip(Vector3 pos, int spriteID, BlipColor color) // aka Mission Blip.
        {
            Blip blip = World.CreateBlip(pos);
            while (blip != null && !blip.Exists())
            {
                Script.Wait(0);
            }
            blip.Sprite = (BlipSprite)spriteID;
            blip.Color = color;
            SetBlipGPS(blip, 156, true, false);
            return blip;
        }
        public static void SetBlipGPS(Blip blip, int color, bool drawFromPlayer, bool displayonfoot)
        {
            if (blip != null)
            {
                Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
                Function.Call(Hash.START_GPS_MULTI_ROUTE, color, drawFromPlayer, displayonfoot);
                Function.Call(Hash.SET_GPS_MULTI_ROUTE_RENDER, true);
                Function.Call(Hash.ADD_POINT_TO_GPS_MULTI_ROUTE, blip.Position.X, blip.Position.Y, blip.Position.Z);
            }
        }
        public static void SetGPSMultiRouteRender(bool toggle)
        {
            Function.Call(Hash.SET_GPS_MULTI_ROUTE_RENDER, toggle);
        }
        public static void ClearGPSMultiRoute()
        {
            Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
        }

        static List<Blip> LongRangeBlips = GetLongRangeBlips();
        public static List<Blip> GetLongRangeBlips()
        {
            List<Blip> list = new List<Blip>();
            for (int i = 0; i < allBlips.Length; i++)
            {
                if (!allBlips[i].IsShortRange)
                    list.Add(allBlips[i]);
            }
            return list;
        }
        public static void ToggleShortRangeForLongRangeBlips(bool IsShortRange)
        {
            for (int i = 0; i < LongRangeBlips.Count; i++)
            {
                LongRangeBlips[i].IsShortRange = IsShortRange;
            }
        }
        public static Blip AddBlipForArea(Vector3 areaCenter, float width, float height)
        {
            return Function.Call<Blip>(Hash.ADD_​BLIP_​FOR_​AREA, areaCenter.X, areaCenter.Y, areaCenter.Z, width, height);
        }
        public static void SetBlipRotation(Blip blip, int rotation)
        {
            Function.Call(Hash.SET_​BLIP_​ROTATION, blip, rotation);
        }

    }
}
