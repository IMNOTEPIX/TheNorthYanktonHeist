using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fPathfind
    {
        public unsafe static bool GetClosestVehicleNode(Vector3 xyz, out Vector3 outPosition, int nodeFlags = 0, float p5 = 0, float p6 = 0)
        {
            Vector3 vector = default(Vector3);
            bool result = Function.Call<bool>(Hash.GET_​CLOSEST_​VEHICLE_​NODE, xyz.X, xyz.Y, xyz.Z, &vector, nodeFlags, p5, p6);
            outPosition = vector;
            return result;
        }
        public unsafe static bool GetClosestVehicleNode(Vector3 xyz, out Vector2 outPosition, int nodeFlags = 0, float p5 = 0, float p6 = 0)
        {
            Vector3 vector = default(Vector3);
            bool result = Function.Call<bool>(Hash.GET_​CLOSEST_​VEHICLE_​NODE, xyz.X, xyz.Y, xyz.Z, &vector, nodeFlags, p5, p6);
            outPosition = new Vector2(vector.X, vector.Z);
            return result;
        }
        public static bool IsPointOnRoad(Vector3 xyz)
        {
            return Function.Call<bool>(Hash.IS_​POINT_​ON_​ROAD, xyz.X, xyz.Y, xyz.Z);
        }
        public static void SetAllowStreamPrologueNodes(bool toggle)
        {
            Function.Call(Hash.SET_ALLOW_STREAM_PROLOGUE_NODES, toggle);
        }
        /// <summary>
        /// nodeType: 0 = main roads, 1 = any dry path, 3 = water
        /// </summary>
        public unsafe static bool GetClosestVehicleNodeWithHeading(Vector3 pos, out Vector3 outPosition, out float outHeading, int nodeType, float p6 = 3.0f, float p7 = 0)
        {
            Vector3 vector = default(Vector3);
            float num = default(float);
            bool result = Function.Call<bool>(Hash.GET_​CLOSEST_​VEHICLE_​NODE_​WITH_​HEADING, pos.X, pos.Y, pos.Z, &vector, &num, nodeType, p6, p7);
            outPosition = vector;
            outHeading = num;
            return result;
        }
        public unsafe static bool GetPositionBySideOfRoad(Vector3 pos, out Vector3 outPosition)
        {
            int p3 = -1;
            Vector3 vector = default(Vector3);
            bool result = Function.Call<bool>(Hash.GET_​POSITION_​BY_​SIDE_​OF_​ROAD, pos.X, pos.Y, pos.Z, p3, &vector);
            outPosition = vector;
            return result;
        }
        public static void SetRoadsInAngledArea(Vector3 xyz1, Vector3 xyz2, float width, bool unknown1, bool unknown2, bool unknown3)
        {
            Function.Call(Hash.SET_​ROADS_​IN_​ANGLED_​AREA, xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, width, unknown1, unknown2, unknown3);
        }
        public static void SetRoadsInArea(Vector3 xyz1, Vector3 xyz2, bool nodeEnabled, bool unknown2)
        {
            Function.Call(Hash.SET_​ROADS_​IN_​AREA, xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, nodeEnabled, unknown2);
        }
    }
}
