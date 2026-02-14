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
    public class fEntity
    {
        public static void SetEntityProofs(Entity entity, bool bulletProof, bool fireProof, bool explosionProof, bool collisionProof, bool meleeProof, bool steamProof, bool dontResetOnCleanup, bool waterProof)
        {
            Function.Call(Hash.SET_​ENTITY_​PROOFS, entity, bulletProof, fireProof, explosionProof, collisionProof, meleeProof, steamProof, dontResetOnCleanup, waterProof);
        }
        public static Vector3 GetAnimInitialOffsetPosition(string animDict, string animName, Vector3 pos, Vector3 rot, float p8 = 0f, int p9 = 2)
        {
            return Function.Call<Vector3>(Hash.GET_​ANIM_​INITIAL_​OFFSET_​POSITION, animDict, animName, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, p8, p9);
        }
        public static Vector3 GetAnimInitialOffsetPosition(string animDict, string animName, float x, float y, float z, float xRot, float yRot, float zRot, float p8 = 0f, int p9 = 2)
        {
            return Function.Call<Vector3>(Hash.GET_​ANIM_​INITIAL_​OFFSET_​POSITION, animDict, animName, x, y, z, xRot, yRot, zRot, p8, p9);
        }
        public static Vector3 GetOffsetFromEntityInWorldCoords(Entity entity, Vector3 offset)
        {
            return Function.Call<Vector3>(Hash.GET_​OFFSET_​FROM_​ENTITY_​IN_​WORLD_​COORDS, entity, offset.X, offset.Y, offset.Z);
        }
        public static Vector3 GetOffsetFromEntityInWorldCoords(Entity entity, float offsetX, float offsetY, float offsetZ)
        {
            return Function.Call<Vector3>(Hash.GET_​OFFSET_​FROM_​ENTITY_​IN_​WORLD_​COORDS, entity, offsetX, offsetY, offsetZ);
        }
        public static bool HasAnimEventFired(Entity entity, string actionHash)
        {
            return Function.Call<bool>(Hash.HAS_​ANIM_​EVENT_​FIRED, entity, Function.Call<int>(Hash.GET_HASH_KEY, actionHash));
        }
        public static bool HasAnimEventFired(Entity entity, int actionHash)
        {
            return Function.Call<bool>(Hash.HAS_​ANIM_​EVENT_​FIRED, entity, actionHash);
        }
        public static bool HasAnimEventFired(Entity entity, Hash actionHash)
        {
            return Function.Call<bool>(Hash.HAS_​ANIM_​EVENT_​FIRED, entity, actionHash);
        }
        public static bool DoesEntityExist(Entity entity)
        {
            return Function.Call<bool>(Hash.DOES_​ENTITY_​EXIST, entity);
        }
        public static bool IsEntityPlayingAnim(Entity entity, string animDict, string animName, int taskFlag = 3)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, entity, animDict, animName, taskFlag);
        }
        public static void ForceEntityAiAndAnimationUpdate(Entity entity)
        {
            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, entity);
        }
        public static void SetCanClimbOnEntity(Entity entity, bool toggle)
        {
            Function.Call(Hash.SET_CAN_CLIMB_ON_ENTITY, entity, toggle);
        }
        public static bool IsEntityInArea(Entity entity, Vector3 xyz1, Vector3 xyz2, bool p7 = false, bool p8 = true, int p9 = 0)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, entity, xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, p7, p8, p9);
        }
        /// <summary>
        /// By "Angled Area" they mean a rectangle.
        /// </summary>
        public static bool IsEntityInAngledArea(Entity entity, Vector3 xyz1, Vector3 xyz2, float width, bool debug = false, bool includeZ = true, int p10 = 0)
        {
            return Function.Call<bool>(Hash.IS_​ENTITY_​IN_​ANGLED_​AREA, entity, xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, width, debug, includeZ, p10);
        }
        public static bool HasCollisionLoadedAroundEntity(Entity entity)
        {
            return Function.Call<bool>(Hash.HAS_​COLLISION_​LOADED_​AROUND_​ENTITY, entity);
        }
        public static void SetEntityLoadCollisionFlag(Entity entity, bool toggle, int p2 = 1)
        {
            Function.Call(Hash.SET_​ENTITY_​LOAD_​COLLISION_​FLAG, entity, toggle, p2);
        }
        /// <summary>
        /// Axis - Invert Axis Flags
        /// </summary>
        public static void SetEntityCoordsNoOffset(Entity entity, Vector3 coords, bool xAxis, bool yAxis, bool zAxis)
        {
            Function.Call(Hash.SET_​ENTITY_​COORDS_​NO_​OFFSET, entity, coords.X, coords.Y, coords.Z, xAxis, yAxis, zAxis);
        }
        /* Returns an integer value of entity's current health.
            Example of range for ped:
            - Player [0 to 200]
            - Ped [100 to 200]
            - Vehicle [0 to 1000]
            - Object [0 to 1000]

            Health is actually a float value but this native casts it to int.
            In order to get the actual value, do:
            float health = *(float *)(entityAddress + 0x280);
        */
        /// <summary>
        /// Player [0 to 200], Ped [100 to 200], Vehicle [0 to 1000], Object [0 to 1000]
        /// </summary>
        public static int GetEntityHealth(Entity entity)
        {
            return Function.Call<int>(Hash.GET_​ENTITY_​HEALTH, entity);
        }
        public unsafe static float GetEntityHealthFloat(Entity entity)
        {
            return *(float*)(entity.MemoryAddress + 0x280);
        }
        /// <summary>
        /// health >= 0, male ped ~= 100 - 200, female ped ~= 0 - 100
        /// </summary>
        public static void SetEntityHealth(Entity entity, int health, Entity instigator, Hash weaponType)
        {
            Function.Call(Hash.SET_​ENTITY_​HEALTH, entity, health, instigator, weaponType);
        }
        public static void SetEntityHealth(Entity entity, int health)
        {
            Function.Call(Hash.SET_​ENTITY_​HEALTH, entity, health, 0);
        }
        public static void SetEntityCanBeDamaged(Entity entity, bool toggle)
        {
            Function.Call(Hash.SET_​ENTITY_​CAN_​BE_​DAMAGED, entity, toggle);
        }
    }
}
