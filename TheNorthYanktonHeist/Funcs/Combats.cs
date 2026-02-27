using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace TheNorthYanktonHeist.Funcs
{
    /// <summary>
    /// Simple combat list manager (legacy support)
    /// </summary>
    public class CombatList
    {
        public List<Ped> ListPeds = new List<Ped>();

        // Performance optimization: throttle checks
        private float lastCheckTime;
        private const float CHECK_INTERVAL = 1000f; // Check every 1 second instead of 60 times/sec

        public void Check()
        {
            // Throttle checks for performance
            if (Game.GameTime - lastCheckTime < CHECK_INTERVAL)
                return;

            lastCheckTime = Game.GameTime;

            if (ListPeds.Count > 0)
            {
                for (int i = ListPeds.Count - 1; i >= 0; i--)
                {
                    if (ListPeds[i] == null || !ListPeds[i].Exists())
                    {
                        ListPeds.RemoveAt(i);
                        continue;
                    }

                    UpdateBlipVisibility(ListPeds[i]);

                    // Clean up dead peds
                    if (ListPeds[i].IsDead)
                    {
                        ListPeds[i].AttachedBlip?.Delete();
                        ListPeds[i].MarkAsNoLongerNeeded();
                        ListPeds.RemoveAt(i);
                    }
                }
            }
        }

        private void UpdateBlipVisibility(Ped ped)
        {
            if (ped.AttachedBlip == null) return;

            // Show blip if in same interior and room as player
            bool sameInterior = fInterior.GetInteriorFromEntity(ped) == fInterior.GetInteriorFromEntity(Game.Player.Character);
            bool sameRoom = fInterior.GetRoomKeyFromEntity(ped) == fInterior.GetRoomKeyFromEntity(Game.Player.Character);

            ped.AttachedBlip.Alpha = (sameInterior && sameRoom) ? 255 : 0;
        }

        public void SetBlips(string blipName = "Enemy", float scale = 0.7f, BlipColor color = BlipColor.Red, int SpriteID = 270)
        {
            if (ListPeds.Count == 0) return;

            foreach (var ped in ListPeds)
            {
                if (ped == null || !ped.Exists()) continue;

                if (ped.AttachedBlip == null)
                {
                    ped.AddBlip();

                    // NO BLOCKING LOOP! Just check once and continue
                    if (ped.AttachedBlip != null && ped.AttachedBlip.Exists())
                    {
                        ped.AttachedBlip.Sprite = (BlipSprite)SpriteID;
                        ped.AttachedBlip.Color = color;
                        ped.AttachedBlip.Name = blipName;
                        ped.AttachedBlip.Scale = scale;
                        ped.AttachedBlip.IsShortRange = true;
                        ped.AttachedBlip.DisplayType = BlipDisplayType.MiniMapOnly;
                    }
                }
            }
        }

        public Ped CreateCombats(Model model, Vector3 pos, float heading)
        {
            // Request model first
            model.Request(500);

            Ped ped = World.CreatePed(model, pos, heading);

            if (ped != null && ped.Exists())
            {
                if (!ListPeds.Contains(ped))
                    ListPeds.Add(ped);
            }

            return ped;
        }

        public void DeleteCombats()
        {
            if (ListPeds.Count > 0)
            {
                foreach (var ped in ListPeds)
                {
                    if (ped != null && ped.Exists())
                    {
                        ped.AttachedBlip?.Delete();
                        ped.Delete();
                    }
                }
                ListPeds.Clear();
            }
        }

        public void ManageAttributes(fPed.CombatAttributes attribute, bool toggle)
        {
            foreach (Ped ped in ListPeds)
            {
                if (ped != null && ped.Exists())
                    fPed.SetPedCombatAttributes(ped, attribute, toggle);
            }
        }

        public void UseDefaultAttributes()
        {
            foreach (Ped ped in ListPeds)
            {
                if (ped == null || !ped.Exists()) continue;

                fPed.SetPedConfigFlag(ped, fPed.PedConfigFlags.PCF_DontBlipCop, true);
                fPed.SetPedAsCop(ped, false);
                fPed.SetPedCombatAttributes(ped, fPed.CombatAttributes.CA_DISABLE_REACT_TO_BUDDY_SHOT, false);
                fPed.SetPedCombatAttributes(ped, fPed.CombatAttributes.CA_PLAY_REACTION_ANIMS, false);
                fPed.SetPedCombatAttributes(ped, fPed.CombatAttributes.CA_DISABLE_BULLET_REACTIONS, false);
                fPed.SetPedCombatAttributes(ped, fPed.CombatAttributes.CA_IS_A_GUARD, true);
                fPed.SetPedCombatAttributes(ped, fPed.CombatAttributes.CA_DISABLE_FLEE_FROM_COMBAT, true);
                fPed.SetPedCombatAttributes(ped, fPed.CombatAttributes.CA_USE_COVER, true);
            }
        }

        public void RandomizeMovement()
        {
            foreach (Ped p in ListPeds)
            {
                if (p == null || !p.Exists()) continue;

                switch (fMisc.GetRandomIntInRange(1, 4))
                {
                    case 1:
                        fPed.SetPedCombatMovement(p, fPed.CombatMovement.CM_WillAdvance);
                        break;
                    case 2:
                        fPed.SetPedCombatMovement(p, fPed.CombatMovement.CM_WillRetreat);
                        break;
                    case 3:
                        fPed.SetPedCombatMovement(p, fPed.CombatMovement.CM_Defensive);
                        break;
                }
            }
        }

        public void RandomizeAbilityLevel(bool includePoor = true)
        {
            foreach (Ped p in ListPeds)
            {
                if (p == null || !p.Exists()) continue;

                if (!includePoor)
                {
                    switch (fMisc.GetRandomIntInRange(1, 3))
                    {
                        case 1:
                            fPed.SetPedCombatAbility(p, fPed.CombatAbilityLevel.CAL_AVERAGE);
                            break;
                        case 2:
                            fPed.SetPedCombatAbility(p, fPed.CombatAbilityLevel.CAL_PROFESSIONAL);
                            break;
                    }
                }
                else
                {
                    switch (fMisc.GetRandomIntInRange(1, 4))
                    {
                        case 1:
                            fPed.SetPedCombatAbility(p, fPed.CombatAbilityLevel.CAL_POOR);
                            break;
                        case 2:
                            fPed.SetPedCombatAbility(p, fPed.CombatAbilityLevel.CAL_AVERAGE);
                            break;
                        case 3:
                            fPed.SetPedCombatAbility(p, fPed.CombatAbilityLevel.CAL_PROFESSIONAL);
                            break;
                    }
                }
            }
        }

        public void ManageMovements(fPed.CombatMovement movement)
        {
            foreach (Ped ped in ListPeds)
            {
                if (ped != null && ped.Exists())
                    fPed.SetPedCombatMovement(ped, movement);
            }
        }

        public void ManageAbilityLevels(fPed.CombatAbilityLevel abilityLevel)
        {
            foreach (Ped ped in ListPeds)
            {
                if (ped != null && ped.Exists())
                    fPed.SetPedCombatAbility(ped, abilityLevel);
            }
        }
    }

    /// <summary>
    /// Static manager for multiple Combats instances
    /// </summary>
    public static class CombatsList
    {
        public static List<Combats> GetCombats = new List<Combats>();

        // Performance optimization: throttle checks
        private static float lastCheckTime;
        private static float lastActivateTime;
        private const float CHECK_INTERVAL = 700f;

        public static void Combats_Check()
        {
            // Throttle checks for performance
            if (Game.GameTime - lastCheckTime < CHECK_INTERVAL)
                return;

            lastCheckTime = Game.GameTime;

            for (int i = 0; i < GetCombats.Count; i++)
            {
                GetCombats[i]?.Check();
            }
        }

        public static void Combats_Create()
        {
            for (int i = 0; i < GetCombats.Count; i++)
            {
                GetCombats[i]?.Create();
            }
        }

        public static void Combats_Dispose()
        {
            for (int i = 0; i < GetCombats.Count; i++)
            {
                GetCombats[i]?.Dispose();
            }
            GetCombats.Clear();
        }

        public static void Combats_Activate()
        {
            if (Game.GameTime - lastActivateTime < CHECK_INTERVAL)
                return;

            lastActivateTime = Game.GameTime;

            for (int i = 0; i < GetCombats.Count; i++)
            {
                GetCombats[i]?.Activate();
            }
        }
    }

    /// <summary>
    /// Individual combat ped with full configuration
    /// OPTIMIZED: No blocking loops, better state tracking
    /// </summary>
    public class Combats : IDisposable
    {
        #region Properties

        public Ped handle { get; protected set; }
        public Model Model { get; protected set; }
        public Vector3 Position { get; protected set; }
        public float Heading { get; protected set; }
        public bool Exists => handle != null && handle.Exists();
        public bool IsAlive => Exists && handle.IsAlive;

        private readonly int AiTeam = World.AddRelationshipGroup("aiteam").Hash;
        private readonly int playersTeam = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");

        public fPed.CombatAbilityLevel AbilityLevel { get; protected set; }
        public fPed.CombatMovement Movement { get; protected set; }
        public fPed.CombatAttributes[] EnableAttributes { get; protected set; }
        public fPed.CombatAttributes[] DisableAttributes { get; protected set; }
        public WeaponHash Weapon { get; protected set; }
        public bool CanHurt { get; protected set; } = true;
        public bool CanWrithe { get; protected set; } = true;
        public bool IsCop { get; protected set; } = false;

        // State tracking
        private bool isActivated = false;
        private bool isDisposed = false;

        #endregion

        #region Constructor

        public Combats(
            Model model,
            Vector3 position,
            float heading,
            fPed.CombatMovement movement,
            fPed.CombatAbilityLevel abilityLevel,
            WeaponHash weapon,
            fPed.CombatAttributes[] disableAttributes,
            fPed.CombatAttributes[] enableAttributes)
        {
            Model = model;
            Position = position;
            Heading = heading;
            Movement = movement;
            AbilityLevel = abilityLevel;
            EnableAttributes = enableAttributes ?? new fPed.CombatAttributes[0];
            DisableAttributes = disableAttributes ?? new fPed.CombatAttributes[0];
            Weapon = weapon;

            if (!CombatsList.GetCombats.Contains(this))
                CombatsList.GetCombats.Add(this);
        }

        #endregion

        #region Methods

        public void Check()
        {
            if (!Exists || isDisposed) return;

            // Update blip visibility based on location
            UpdateBlipVisibility();

            // Clean up if dead
            if (handle.IsDead)
            {
                handle.AttachedBlip?.Delete();
                handle.MarkAsNoLongerNeeded();
            }
        }

        private void UpdateBlipVisibility()
        {
            if (handle.AttachedBlip == null) return;

            bool sameInterior = fInterior.GetInteriorFromEntity(handle) == fInterior.GetInteriorFromEntity(Game.Player.Character);
            bool sameRoom = fInterior.GetRoomKeyFromEntity(handle) == fInterior.GetRoomKeyFromEntity(Game.Player.Character);

            handle.AttachedBlip.Alpha = (sameInterior && sameRoom) ? 255 : 0;
        }

        public void Create(int timeout = 500)
        {
            if (Exists || isDisposed) return;

            // Request model
            Model.Request(timeout - 100);

            // Create ped
            handle = World.CreatePed(Model, Position, Heading);

            // NO BLOCKING LOOP! Just wait one frame if needed
            if (handle != null && !handle.Exists())
            {
                Script.Yield();
            }

            // If still doesn't exist after one frame, it failed
            if (!Exists)
            {
                throw new Exception("Failed to create combat ped.");
            }
        }

        public void Activate()
        {
            if (!Exists || isActivated || isDisposed) return;

            // Create blip (NO BLOCKING LOOP!)
            if (handle.AttachedBlip == null)
            {
                handle.AddBlip();

                // Check once, don't wait
                if (handle.AttachedBlip != null && handle.AttachedBlip.Exists())
                {
                    handle.AttachedBlip.Sprite = (BlipSprite)270;
                    handle.AttachedBlip.Color = BlipColor.Red;
                    handle.AttachedBlip.Name = "Guard";
                    handle.AttachedBlip.Scale = 0.7f;
                    handle.AttachedBlip.IsShortRange = true;
                    handle.AttachedBlip.DisplayType = BlipDisplayType.MiniMapOnly;
                }
            }

            // Configure flags
            fPed.SetPedConfigFlag(handle, 281, !CanWrithe);
            fPed.SetPedConfigFlag(handle, 188, !CanHurt);
            fPed.SetPedConfigFlag(handle, fPed.PedConfigFlags.PCF_DontBlipCop, !IsCop);
            fPed.SetPedAsCop(handle, IsCop);

            // Set relationships
            fPed.SetRelationshipBetweenGroups(fPed.RelationshipTypes.ACQUAINTANCE_TYPE_PED_HATE, AiTeam, playersTeam);
            fPed.SetRelationshipBetweenGroups(fPed.RelationshipTypes.ACQUAINTANCE_TYPE_PED_HATE, playersTeam, AiTeam);

            // Combat settings
            fPed.SetPedCombatAbility(handle, AbilityLevel);
            fPed.SetPedCombatMovement(handle, Movement);

            // Give weapon
            handle.Weapons.Give(Weapon, 10000, true, true);
            handle.Weapons.Select(Weapon);
            handle.Armor = 200;

            // Apply attributes
            foreach (var attribute in EnableAttributes)
            {
                fPed.SetPedCombatAttributes(handle, attribute, true);
            }

            foreach (var attribute in DisableAttributes)
            {
                fPed.SetPedCombatAttributes(handle, attribute, false);
            }

            isActivated = true;
        }

        public void Dispose()
        {
            if (isDisposed) return;

            if (Exists)
            {
                handle.AttachedBlip?.Delete();
                handle.Delete();
                handle = null;
            }

            Model.MarkAsNoLongerNeeded();
            isDisposed = true;
        }

        #endregion
    }
}