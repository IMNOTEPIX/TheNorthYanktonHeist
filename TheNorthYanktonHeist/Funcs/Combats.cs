using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using Script = GTA.Script;

namespace TheNorthYanktonHeist.Funcs
{
    public class CombatList
    {
        public List<Ped> ListPeds = new List<Ped>();
        public void Check()
        {
            if (ListPeds.Count > 0)
            {
                for (int i = ListPeds.Count - 1; i >= 0; i--)
                {
                    if (ListPeds[i] != null)
                    {
                        if (fInterior.GetInteriorFromEntity(ListPeds[i]) == fInterior.GetInteriorFromEntity(Game.Player.Character))
                        {
                            if (fInterior.GetRoomKeyFromEntity(ListPeds[i]) == fInterior.GetRoomKeyFromEntity(Game.Player.Character))
                            {
                                if (ListPeds[i].AttachedBlip != null)
                                    ListPeds[i].AttachedBlip.Alpha = 255;
                            }
                            else
                            {
                                if (ListPeds[i].AttachedBlip != null)
                                    ListPeds[i].AttachedBlip.Alpha = 0;
                            }
                        }
                        else
                        {
                            if (ListPeds[i].AttachedBlip != null)
                                ListPeds[i].AttachedBlip.Alpha = 0;
                        }
                        if (ListPeds[i].IsDead)
                        {
                            if (ListPeds[i].AttachedBlip != null)
                                ListPeds[i].AttachedBlip.Delete();
                            ListPeds[i].MarkAsNoLongerNeeded();
                            ListPeds.RemoveAt(i);
                        }
                    }
                }
            }
        }
        public void SetBlips(string blipName = "Enemy", float scale = 0.7f, BlipColor color = BlipColor.Red, int SpriteID = 270)
        {
            if (ListPeds.Count > 0)
            {
                for (int i = 0; i < ListPeds.Count; i++)
                {
                    if (ListPeds[i] != null)
                    {
                        if (ListPeds[i].AttachedBlip == null)
                        {
                            ListPeds[i].AddBlip();
                            for (; ; )
                            {
                                Blip attachedBlip = ListPeds[i].AttachedBlip;
                                if (attachedBlip != null && attachedBlip.Exists())
                                    break;
                                Script.Wait(0);
                            }
                            ListPeds[i].AttachedBlip.Sprite = (BlipSprite)SpriteID;
                            ListPeds[i].AttachedBlip.Color = color;
                            ListPeds[i].AttachedBlip.Name = blipName;
                            ListPeds[i].AttachedBlip.Scale = scale;
                            ListPeds[i].AttachedBlip.IsShortRange = true;
                            ListPeds[i].AttachedBlip.DisplayType = BlipDisplayType.MiniMapOnly;
                        }
                    }
                }
            }
        }
        public Ped CreateCombats(Model model, Vector3 pos, float heading)
        {
            Ped ped = World.CreatePed(model, pos, heading);
            while (ped != null && !ped.Exists())
            {
                Script.Wait(0);
            }
            if (!ListPeds.Contains(ped))
                ListPeds.Add(ped);
            return ped;
        }
        public void DeleteCombats()
        {
            if (ListPeds.Count > 0)
            {
                for (int i = 0; i < ListPeds.Count; i++)
                {
                    if (ListPeds[i].AttachedBlip != null)
                        ListPeds[i].AttachedBlip.Delete();
                    if (ListPeds[i] != null)
                        ListPeds[i].Delete();
                }
                ListPeds.Clear();
            }
        }
        public void ManageAttributes(fPed.CombatAttributes attribute, bool toggle)
        {
            foreach (Ped ped in ListPeds)
            {
                fPed.SetPedCombatAttributes(ped, attribute, toggle);

            }
        }
        public void UseDefaultAttributes()
        {
            foreach (Ped ped in ListPeds)
            {
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
            if (!includePoor)
            {
                foreach (Ped p in ListPeds)
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
            }
            else
            {
                foreach (Ped p in ListPeds)
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
                fPed.SetPedCombatMovement(ped, movement);
            }

        }
        public void ManageAbilityLevels(fPed.CombatAbilityLevel abilityLevel)
        {
            foreach (Ped ped in ListPeds)
            {
                fPed.SetPedCombatAbility(ped, abilityLevel);
            }
        }

    }
    public class CombatsList
    {
        public static List<Combats> GetCombats = new List<Combats>();
        public static void Combats_Check()
        {
            for (int i = 0; i < GetCombats.Count; i++) { GetCombats[i].Check(); }
        }
        public static void Combats_Create()
        {
            for (int i = 0; i < GetCombats.Count; i++) { GetCombats[i].Create(); }
        }
        public static void Combats_Dispose()
        {
            for (int i = 0; i < GetCombats.Count; i++) { GetCombats[i].Dispose(); }
        }
        public static void Combats_Activate()
        {
            for (int i = 0; i < GetCombats.Count; i++) { GetCombats[i].Activate(); }
        }

    }
    public class Combats : IDisposable
    {
        public Ped handle {  get; protected set; }
        public Model Model { get; protected set; } 
        public Vector3 Position {  get; protected set; } 
        public float Heading { get; protected set; } 
        public bool Exists => handle != null && handle.Exists();
        public bool IsAlive => handle.IsAlive;
        private readonly int AiTeam = World.AddRelationshipGroup("aiteam").Hash;
        private readonly int playersTeam = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");
        public fPed.CombatAbilityLevel AbilityLevel { get; protected set; }
        public fPed.CombatMovement Movement { get; protected set; }
        public fPed.CombatAbilityLevel[] AbilityLevels { get; protected set; }
        public fPed.CombatMovement[] Movements { get; protected set; }
        public fPed.CombatAttributes[] EnableAttributes { get; protected set; }
        public fPed.CombatAttributes[] DisableAttributes { get; protected set; }
        public WeaponHash Weapon { get; protected set; }
        public WeaponHash[] Weapons { get; protected set; }
        public bool CanHurt { get; protected set; } = true;
        public bool CanWrithe { get; protected set; } = true;
        public bool IsCop { get; protected set; } = false;

        public Combats(Model model, Vector3 position, float heading, fPed.CombatMovement movement, fPed.CombatAbilityLevel abilityLevel, WeaponHash weapon, fPed.CombatAttributes[] disableAttributes, fPed.CombatAttributes[] enableAttributes)
        {
            Model = model;
            Position = position;
            Heading = heading;
            Movement = movement;
            AbilityLevel = abilityLevel;
            EnableAttributes = enableAttributes;
            DisableAttributes = disableAttributes;
            Weapon = weapon;
            if (!CombatsList.GetCombats.Contains(this))
                CombatsList.GetCombats.Add(this);
        }

        public void Check()
        {
            if (handle != null)
            {
                if (fInterior.GetInteriorFromEntity(handle) == fInterior.GetInteriorFromEntity(Game.Player.Character))
                {
                    if (fInterior.GetRoomKeyFromEntity(handle) == fInterior.GetRoomKeyFromEntity(Game.Player.Character))
                    {
                        if (handle.AttachedBlip != null)
                            handle.AttachedBlip.Alpha = 255;
                    }
                    else
                    {
                        if (handle.AttachedBlip != null)
                            handle.AttachedBlip.Alpha = 0;
                    }
                }
                else
                {
                    if (handle.AttachedBlip != null)
                        handle.AttachedBlip.Alpha = 0;
                }
                if (handle.IsDead)
                {
                    if (handle.AttachedBlip != null)
                        handle.AttachedBlip.Delete();
                    handle.MarkAsNoLongerNeeded();
                }
            }
        }
        public void Create(int timeout = 500)
        {
            if (!Exists)
            {
                Model.Request(timeout - 100);
                handle = World.CreatePed(Model, Position, Heading);
            }
            int gameTime = Game.GameTime;
            while (!Exists)
            {
                if (Game.GameTime - gameTime > timeout)
                    throw new TimeoutException("Timed out trying to create combat ped.");
                Script.Yield();
            }
        }
        public void Activate()
        {
            if (Exists && IsAlive)
            {
                if (handle.AttachedBlip == null)
                {
                    handle.AddBlip();
                    for (; ; )
                    {
                        Blip attachedBlip = handle.AttachedBlip;
                        if (attachedBlip != null && attachedBlip.Exists())
                        {
                            break;
                        }
                        Script.Wait(0);
                    }
                    handle.AttachedBlip.Sprite = (BlipSprite)270;
                    handle.AttachedBlip.Color = BlipColor.Red;
                    handle.AttachedBlip.Name = "Guard";
                    handle.AttachedBlip.Scale = 0.7f;
                    handle.AttachedBlip.IsShortRange = true;
                    handle.AttachedBlip.DisplayType = BlipDisplayType.MiniMapOnly;
                }
                if (CanWrithe)
                    fPed.SetPedConfigFlag(handle, 281, false);
                else
                    fPed.SetPedConfigFlag(handle, 281);
                if (CanHurt)
                    fPed.SetPedConfigFlag(handle, 188, false);
                else
                    fPed.SetPedConfigFlag(handle, 188);
                if (!IsCop)
                {
                    fPed.SetPedConfigFlag(handle, fPed.PedConfigFlags.PCF_DontBlipCop, true);
                    fPed.SetPedAsCop(handle, false);
                }
                else
                {
                    fPed.SetPedConfigFlag(handle, fPed.PedConfigFlags.PCF_DontBlipCop, false);
                    fPed.SetPedAsCop(handle, true);
                }
                fPed.SetRelationshipBetweenGroups(fPed.RelationshipTypes.ACQUAINTANCE_TYPE_PED_HATE, AiTeam, playersTeam);
                fPed.SetRelationshipBetweenGroups(fPed.RelationshipTypes.ACQUAINTANCE_TYPE_PED_HATE, playersTeam, AiTeam);
                fPed.SetPedCombatAbility(handle, AbilityLevel);
                fPed.SetPedCombatMovement(handle, Movement);
                handle.Weapons.Give(Weapon, 10000, true, true);
                handle.Weapons.Select(Weapon);
                foreach (fPed.CombatAttributes attribute in EnableAttributes)
                {
                    fPed.SetPedCombatAttributes(handle, attribute, true);
                }
                foreach (fPed.CombatAttributes attribute in DisableAttributes)
                {
                    fPed.SetPedCombatAttributes(handle, attribute, false);
                }
            }
        }
        public void Dispose()
        {
            if (Exists)
            {
                if (handle.AttachedBlip != null)
                {
                    handle.AttachedBlip.Delete();
                }
                handle.Delete();
                handle = null;
                Model.MarkAsNoLongerNeeded();
            }
        }
    }
}
