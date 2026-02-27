using GTA;
using GTA.Math;
using GTA.Native;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class Explosion
    {
        public Ped Ped { get; protected set; }
        public Vector3 Position { get; protected set; }
        public int Type { get; protected set; }
        public float DamageAmount { get; protected set; }
        public bool IsAudible { get; protected set; }
        public bool IsInvisible { get; protected set; }
        public float CameraShake { get; protected set; }
        public int CustomVfx { get; protected set; }

        public bool DamagePlayer { get; protected set; }

        // Shared setup logic, called by all constructors
        private void Init(Vector3 position, ExplosionType type, float damage, bool isAudible, bool isInvisible, float cameraShake, bool damagePlayer)
        {
            Position = position;
            Type = (int)type;
            DamageAmount = damage;
            IsAudible = isAudible;
            IsInvisible = isInvisible;
            CameraShake = cameraShake;
            DamagePlayer = damagePlayer;
        }

        public Explosion(Vector3 position, ExplosionType type, float damage, bool isAudible, bool isInvisible, float cameraShake, bool damagePlayer)
        {
            Init(position, type, damage, isAudible, isInvisible, cameraShake, damagePlayer);
        }

        public Explosion(Ped ped, Vector3 position, ExplosionType type, float damage, bool isAudible, bool isInvisible, float cameraShake)
        {
            Init(position, type, damage, isAudible, isInvisible, cameraShake, default);
            Ped = ped;
        }

        public Explosion(string customVfx, Vector3 position, ExplosionType type, float damage, bool isAudible, bool isInvisible, float cameraShake)
        {
            Init(position, type, damage, isAudible, isInvisible, cameraShake, default);
            CustomVfx = Function.Call<int>(Hash.GET_HASH_KEY, customVfx);
        }

        // --- Trigger methods ---

        public void Add()
        {
            Function.Call(Hash.ADD_EXPLOSION,
                Position.X, Position.Y, Position.Z,
                Type, DamageAmount, IsAudible, IsInvisible, CameraShake, DamagePlayer);
        }

        public void AddPedOwned()
        {
            if (Ped == null || !Ped.Exists())
                throw new InvalidOperationException("A valid Ped must be assigned before calling AddPedOwned.");

            Function.Call(Hash.ADD_OWNED_EXPLOSION,
                Ped, Position.X, Position.Y, Position.Z,
                Type, DamageAmount, IsAudible, IsInvisible, CameraShake);
        }

        public void AddWithCustomVfx()
        {
            if (CustomVfx == 0)
                throw new InvalidOperationException("CustomVfx is not set. Use the customVfx constructor overload.");

            Function.Call(Hash.ADD_EXPLOSION_WITH_USER_VFX,
                Position.X, Position.Y, Position.Z,
                Type, CustomVfx, DamageAmount, IsAudible, IsInvisible, CameraShake);
        }

        // --- Static detection methods ---

        /// <summary>
        /// Checks if an explosion of the given type exists within an axis-aligned bounding box.
        /// Pass ExplosionType of -1 to match any explosion type.
        /// </summary>
        public static bool IsInArea(ExplosionType type, Vector3 min, Vector3 max)
        {
            return Function.Call<bool>(Hash.IS_EXPLOSION_IN_AREA,
                (int)type,
                min.X, min.Y, min.Z,
                max.X, max.Y, max.Z);
        }

        /// <summary>
        /// Like IsInArea, but only returns true while the explosion is still actively doing damage.
        /// </summary>
        public static bool IsActiveInArea(ExplosionType type, Vector3 min, Vector3 max)
        {
            return Function.Call<bool>(Hash.IS_EXPLOSION_ACTIVE_IN_AREA,
                (int)type,
                min.X, min.Y, min.Z,
                max.X, max.Y, max.Z);
        }

        /// <summary>
        /// Checks if an explosion of the given type exists within a sphere.
        /// Pass ExplosionType of -1 to match any explosion type.
        /// </summary>
        public static bool IsInSphere(ExplosionType type, Vector3 center, float radius)
        {
            return Function.Call<bool>(Hash.IS_EXPLOSION_IN_SPHERE,
                (int)type,
                center.X, center.Y, center.Z,
                radius);
        }

        /// <summary>
        /// Returns the owner Entity of any explosion found within a sphere, or null if none.
        /// </summary>
        public static Entity GetOwnerInSphere(ExplosionType type, Vector3 center, float radius)
        {
            int handle = Function.Call<int>(Hash.GET_OWNER_OF_EXPLOSION_IN_SPHERE,
                (int)type,
                center.X, center.Y, center.Z,
                radius);

            return handle != 0 ? Entity.FromHandle(handle) : null;
        }

        /// <summary>
        /// Checks if an explosion of the given type exists within an angled (oriented) area
        /// defined by two points and a width.
        /// </summary>
        public static bool IsInAngledArea(ExplosionType type, Vector3 origin, Vector3 edge, float width)
        {
            return Function.Call<bool>(Hash.IS_EXPLOSION_IN_ANGLED_AREA,
                (int)type,
                origin.X, origin.Y, origin.Z,
                edge.X, edge.Y, edge.Z,
                width);
        }

        /// <summary>
        /// Returns the owner Entity of any explosion found within an angled area, or null if none.
        /// </summary>
        public static Entity GetOwnerInAngledArea(ExplosionType type, Vector3 origin, Vector3 edge, float width)
        {
            int handle = Function.Call<int>(Hash.GET_OWNER_OF_EXPLOSION_IN_ANGLED_AREA,
                (int)type,
                origin.X, origin.Y, origin.Z,
                edge.X, edge.Y, edge.Z,
                width);

            return handle != 0 ? Entity.FromHandle(handle) : null;
        }
    }
}
