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
    public class Fire
    {
        public int Id { get; private set; }
        public Vector3 Position { get; private set; }
        public Entity Entity { get; private set; }
        public bool GasPowered { get; private set; }

        private int _spreadLimit;
        public int SpreadLimit
        {
            get => _spreadLimit;
            set => _spreadLimit = Math.Max(0, Math.Min(25, value));
        }

        // For script fires (world position)
        public bool IsValid => Id != 0;

        // For entity fires, check the entity. For script fires, we can only rely on IsValid
        // since there's no native to check a script fire handle's active state directly
        public bool IsActive => Entity != null ? Function.Call<bool>(Hash.IS_ENTITY_ON_FIRE, Entity) : IsValid;

        private bool IsEntityFire => Entity != null;

        // Script fire constructor
        public Fire(Vector3 position, int spreadLimit = 1, bool gasPowered = false)
        {
            Position = position;
            SpreadLimit = spreadLimit; // goes through clamped setter
            GasPowered = gasPowered;
        }

        // Entity fire constructor
        public Fire(Entity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            Position = entity.Position;
        }

        public void Start()
        {
            if (IsEntityFire)
                Id = Function.Call<int>(Hash.START_ENTITY_FIRE, Entity);
            else
                Id = Function.Call<int>(Hash.START_SCRIPT_FIRE, Position.X, Position.Y, Position.Z, SpreadLimit, GasPowered);
        }

        public void Remove()
        {
            if (IsEntityFire)
                Function.Call(Hash.STOP_ENTITY_FIRE, Entity);
            else
                Function.Call(Hash.REMOVE_SCRIPT_FIRE, Id);

            Id = 0;
        }

        // --- Static world queries ---

        public static bool IsEntityOnFire(Entity entity)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_ON_FIRE, entity);
        }

        public static int GetCountInRange(Vector3 position, float radius)
        {
            return Function.Call<int>(Hash.GET_NUMBER_OF_FIRES_IN_RANGE, position.X, position.Y, position.Z, radius);
        }

        public static void SetSpreadRate(float multiplier)
        {
            Function.Call(Hash.SET_FLAMMABILITY_MULTIPLIER, multiplier);
        }

        public static void StopInRange(Vector3 position, float radius)
        {
            Function.Call(Hash.STOP_FIRE_IN_RANGE, position.X, position.Y, position.Z, radius);
        }

        public static bool TryGetClosestPosition(Vector3 position, out Vector3 firePosition)
        {
            unsafe
            {
                Vector3 result = default;
                bool found = Function.Call<bool>(Hash.GET_CLOSEST_FIRE_POS, &result, position.X, position.Y, position.Z);
                firePosition = found ? result : default;
                return found;
            }
        }
        /// <summary>
        /// Spawns fires scattered randomly within a radius around a center point.
        /// Returns all created Fire instances so you can remove them later.
        /// </summary>
        public static List<Fire> CreateFireInRadius(Vector3 center, float radius, int count, int spreadLimit = 1, bool gasPowered = false)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0.");

            var fires = new List<Fire>();
            var rng = new Random();

            for (int i = 0; i < count; i++)
            {
                // Random point within a circle
                double angle = rng.NextDouble() * Math.PI * 2;
                double distance = Math.Sqrt(rng.NextDouble()) * radius; // sqrt for uniform distribution

                var position = new Vector3(
                    center.X + (float)(Math.Cos(angle) * distance),
                    center.Y + (float)(Math.Sin(angle) * distance),
                    center.Z
                );

                var fire = new Fire(position, spreadLimit, gasPowered);
                fire.Start();
                fires.Add(fire);
            }

            return fires;
        }

        /// <summary>
        /// Spawns fires in a uniform grid across a rectangular area defined by two corners.
        /// </summary>
        public static List<Fire> CreateFireInArea(Vector3 min, Vector3 max, float spacing, int spreadLimit = 1, bool gasPowered = false)
        {
            if (spacing <= 0) throw new ArgumentOutOfRangeException(nameof(spacing), "Spacing must be greater than 0.");

            var fires = new List<Fire>();

            for (float x = min.X; x <= max.X; x += spacing)
            {
                for (float y = min.Y; y <= max.Y; y += spacing)
                {
                    // Sample ground height at this X,Y instead of using a fixed Z
                    float groundZ;
                    unsafe
                    {
                        float gz = 0f;
                        Function.Call(Hash.GET_GROUND_Z_FOR_3D_COORD, x, y, min.Z + 100f, &gz, false);
                        groundZ = gz;
                    }

                    var position = new Vector3(x, y, groundZ);
                    var fire = new Fire(position, spreadLimit, gasPowered);
                    fire.Start();
                    fires.Add(fire);
                }
            }

            return fires;
        }

        /// <summary>
        /// Removes all fires returned by CreateFireInRadius or CreateFireInArea.
        /// </summary>
        public static void RemoveAll(List<Fire> fires)
        {
            foreach (var fire in fires)
                fire.Remove();

            fires.Clear();
        }
    }
}
