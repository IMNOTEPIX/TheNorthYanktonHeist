namespace TheNorthYanktonHeist.Scenes
{
    using System;
    using System.Collections.Generic;
    using GTA;
    using GTA.Native;
    using TheNorthYanktonHeist.Funcs;

    public static class BagManager
    {
        private const int FreemodeComponentId = 5;
        private const int StoryModeComponentId = 9;

        public enum BagVariantTypes
        {
            Invalid = -1,
            OriginalHeists,
            CasinoYungAncestor,
            CasinoRegular,
            CasinoMaintenance,
            CasinoBugstars,
            CasinoGeometric,
            CasinoPattern,
            CasinoGeometric2,
            CasinoAggressive1,
            CasinoAggressive2,
            CasinoAggressive3,
            DrugWarsChemicals,
            ChopShopWeaponsBag,
        }

        public struct BagData
        {
            public int Drawable;
            public int Texture;

            public BagData(int drawable, int texture)
            {
                Drawable = drawable;
                Texture = texture;
            }
        }

        private static readonly Dictionary<BagVariantTypes, BagData> BagLookup =
            new Dictionary<BagVariantTypes, BagData>
        {
                { BagVariantTypes.OriginalHeists, fPlayer.IsStoryPed ? new BagData(1, 0) : new BagData(45, 0) },
                { BagVariantTypes.CasinoYungAncestor, new BagData(82, 9) },
                { BagVariantTypes.CasinoRegular, new BagData(82, 0) },
                { BagVariantTypes.CasinoMaintenance, new BagData(82, 3) },
                { BagVariantTypes.CasinoBugstars, new BagData(82, 8) },
                { BagVariantTypes.CasinoGeometric, new BagData(82, 13) },
                { BagVariantTypes.CasinoPattern, new BagData(82, 12) },
                { BagVariantTypes.CasinoGeometric2, new BagData(82, 15) },
                { BagVariantTypes.CasinoAggressive1, new BagData(82, 10) },
                { BagVariantTypes.CasinoAggressive2, new BagData(82, 11) },
                { BagVariantTypes.CasinoAggressive3, new BagData(82, 14) },
                { BagVariantTypes.DrugWarsChemicals, new BagData(82, 2) },
                { BagVariantTypes.ChopShopWeaponsBag, new BagData(82, 4) },
        };

        public static int GetComponentId(Ped ped)
        {
            if (fPlayer.IsFreemodePed)
                return FreemodeComponentId;

            return StoryModeComponentId; // fallback
        }

        public static BagData GetBagData(BagVariantTypes type)
        {
            if (BagLookup.TryGetValue(type, out var data))
                return data;

            throw new ArgumentException("Invalid bag variant type.");
        }

        public static BagVariantTypes GetBagVariantFromPed(Ped ped)
        {
            int componentId = GetComponentId(ped);
            int currentDrawable = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, ped, componentId);
            int currentTexture = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, ped, componentId);

            foreach (var entry in BagLookup)
            {
                if (entry.Value.Drawable == currentDrawable &&
                    entry.Value.Texture == currentTexture)
                {
                    return entry.Key;
                }
            }

            return BagVariantTypes.Invalid;
        }

        public static void ApplyBag(Ped ped, BagVariantTypes type)
        {
            var data = GetBagData(type);
            int componentId = GetComponentId(ped);

            Function.Call(Hash.SET_PED_COMPONENT_VARIATION,
                ped,
                componentId,
                data.Drawable,
                data.Texture,
                0);
        }

        public static void RemoveBag(Ped ped)
        {
            int componentId = GetComponentId(ped);

            Function.Call(Hash.SET_PED_COMPONENT_VARIATION,
                ped,
                componentId,
                0,
                0,
                0);
        }

        public static Prop CreateBagPropFromPed(Ped ped)
        {
            var variant = GetBagVariantFromPed(ped);

            if (variant == BagVariantTypes.Invalid)
                return null;

            string modelName = GetBagModelName(variant);

            if (string.IsNullOrEmpty(modelName))
                return null;

            return World.CreateProp(modelName, ped.Position, false, false);
        }

        private static string GetBagModelName(BagVariantTypes type)
        {
            switch (type)
            {
                case BagVariantTypes.OriginalHeists: return "hei_p_m_bag_var22_arm_s";
                case BagVariantTypes.CasinoYungAncestor: return "ch_p_m_bag_var01_arm_s";
                case BagVariantTypes.CasinoRegular: return "ch_p_m_bag_var02_arm_s";
                case BagVariantTypes.CasinoMaintenance: return "ch_p_m_bag_var03_arm_s";
                case BagVariantTypes.CasinoBugstars: return "ch_p_m_bag_var04_arm_s";
                case BagVariantTypes.CasinoGeometric: return "ch_p_m_bag_var05_arm_s";
                case BagVariantTypes.CasinoPattern: return "ch_p_m_bag_var06_arm_s";
                case BagVariantTypes.CasinoGeometric2: return "ch_p_m_bag_var07_arm_s";
                case BagVariantTypes.CasinoAggressive1: return "ch_p_m_bag_var08_arm_s";
                case BagVariantTypes.CasinoAggressive2: return "ch_p_m_bag_var09_arm_s";
                case BagVariantTypes.CasinoAggressive3: return "ch_p_m_bag_var10_arm_s";
                case BagVariantTypes.DrugWarsChemicals: return "xm3_p_xm3_m_bag_var22_arm_s";
                case BagVariantTypes.ChopShopWeaponsBag: return "m23_2_p_m32_m_bag_var22_arm_s_g";
                default: return null;
            }
        }

        public static int BagVariantCount =>
            Enum.GetValues(typeof(BagVariantTypes)).Length - 1; // exclude Invalid
    }

}
