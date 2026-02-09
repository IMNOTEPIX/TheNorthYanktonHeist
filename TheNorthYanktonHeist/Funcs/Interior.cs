using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fInterior
    {
        public static void ForceRoomForEntity(Entity entity, int interior, Hash roomHashKey)
        {
            Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, entity, interior, roomHashKey);
        }
        public static void SetRoomForGameViewportByName(string roomName)
        {
            Function.Call(Hash.SET_​ROOM_​FOR_​GAME_​VIEWPORT_​BY_​NAME, roomName);
        }
        public static uint GetInteriorFromEntity(Entity entity)
        {
            return Function.Call<uint>(Hash.GET_INTERIOR_FROM_ENTITY, entity);
        }
        public static uint GetRoomKeyFromEntity(Entity entity)
        {
            return Function.Call<uint>(Hash.GET_ROOM_KEY_FROM_ENTITY, entity);
        }
        public static uint GetInteriorFromCoords(Vector3 coords)
        {
            return Function.Call<uint>(Hash.GET_INTERIOR_AT_COORDS, coords.X, coords.Y, coords.Z);
        }
        public static int GetInteriorAtCoordsWithType(Vector3 xyz, string interiorType)
        {
            return Function.Call<int>(Hash.GET_​INTERIOR_​AT_​COORDS_​WITH_​TYPE, xyz.X, xyz.Y, xyz.Z, interiorType);
        }
        public static void PinInteriorInMemory(int interior)
        {
            Function.Call(Hash.PIN_​INTERIOR_​IN_​MEMORY, interior);
        }
        public static bool IsInteriorReady(int interior)
        {
            return Function.Call<bool>(Hash.IS_​INTERIOR_​READY, interior);
        }

        public class MPMansion
        {
            private static void SetEmitter(string emitterName, bool enabled, int station)
            {
                Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, emitterName, enabled);
                Function.Call(Hash.SET_EMITTER_RADIO_STATION, emitterName, Function.Call<string>(Hash.GET_RADIO_STATION_NAME, station));
            }

            private static Prop CreateFrozenInvincibleProp(Model model, Vector3 pos, float heading, Vector4 quat)
            {
                Prop prop = World.CreateProp(model, pos, new Vector3(0f, 0f, heading), false, false);
                prop.Position -= new Vector3(0f, 0f, 0.55f);
                Function.Call(Hash.SET_ENTITY_HEADING, prop, heading);
                Function.Call(Hash.SET_ENTITY_QUATERNION, prop, quat.X, quat.Y, quat.Z, quat.W);
                Function.Call(Hash.SET_ENTITY_INVINCIBLE, prop, true);
                Function.Call(Hash.FREEZE_ENTITY_POSITION, prop, true);
                Function.Call(Hash.SET_ENTITY_VISIBLE, prop, true, false);
                return prop;
            }

            private static Model RequestModel(string Name)
            {
                Model model = new Model(Name);
                model.Request(250);
                bool flag = model.IsInCdImage && model.IsValid;
                Model result;
                if (flag)
                {
                    while (!model.IsLoaded)
                    {
                        Script.Wait(1);
                    }
                    result = model;
                }
                else
                {
                    model.MarkAsNoLongerNeeded();
                    result = model;
                }
                return result;
            }

            public static void SetupMansionChairsAndEmitters(int MansionRadio, int MansionClubRadio, MansionID ID)
            {
                switch (ID)
                {
                    case MansionID.Richman:
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                bool flag = Mansionchairs[i] != null && Mansionchairs[i].Exists();
                                if (flag)
                                {
                                    Mansionchairs[i].Delete();
                                    Mansionchairs[i] = null;
                                }
                            }
                            SetEmitter("se_dlc25-2_basement_armoury_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_garage_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_lobby_02_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_lobby_03_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_mod_garage_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_arcade_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_gallery_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_kitchen_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_lobby_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_trophy_room_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_office_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_cigar_room_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_guest_bedroom_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_master_bedroom_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_garage_top_loc_2", true, MansionRadio);
                            SetEmitter("dlc25-2_mansion_exterior_central_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_exterior_pool_bar_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_exterior_terrace_loc_2", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_club_01_loc_2", true, MansionClubRadio);
                            SetEmitter("se_dlc25-2_basement_club_02_loc_2", true, MansionClubRadio);
                            Vector4 quat = new Vector4(0f, 0f, -0.587628f, 0.809132f);
                            Vector4 quat2 = new Vector4(0f, 0f, 0.813719f, 0.581259f);
                            try
                            {
                                Mansionchairs[0] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1643.57f, 474.875f, 128.836f), 288.022f, quat);
                                Mansionchairs[1] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1643.93f, 475.989f, 128.836f), 288.022f, quat);
                                Mansionchairs[2] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1644.27f, 477.036f, 128.836f), 288.022f, quat);
                                Mansionchairs[3] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1644.6f, 478.074f, 128.836f), 288.022f, quat);
                                Mansionchairs[4] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1645.48f, 474.288f, 128.836f), 108.922f, quat2);
                                Mansionchairs[5] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1645.88f, 475.433f, 128.836f), 108.922f, quat2);
                                Mansionchairs[6] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1646.23f, 476.435f, 128.836f), 108.922f, quat2);
                                Mansionchairs[7] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-1646.6f, 477.486f, 128.836f), 108.922f, quat2);
                                Vector4 quat3 = new Vector4(0f, 0f, -0.483377f, 0.875413f);
                                Mansionchairs[8] = CreateFrozenInvincibleProp(RequestModel("ex_prop_offchair_exec_04"), new Vector3(-1630.88f, 484.486f, 128.861f), 302.188f, quat3);
                            }
                            catch
                            {
                                Notification.Show("~b~EAI~w~:~r~Error~w~ Could not spawn 'm25_2_prop_m52_diningchair_01a'. This is likely due to your mods folder missing model: 'm25_2_prop_m52_diningchair_01a', please remove mods folder and see if the error persists.", false);
                            }
                            break;
                        }
                    case MansionID.Vinewood:
                        for (int j = 9; j < 18; j++)
                        {
                            bool flag2 = Mansionchairs[j] != null && Mansionchairs[j].Exists();
                            if (flag2)
                            {
                                Mansionchairs[j].Delete();
                                Mansionchairs[j] = null;
                            }
                        }
                        SetEmitter("se_dlc25-2_basement_armoury_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_basement_garage_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_basement_lobby_02_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_basement_lobby_03_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_basement_mod_garage_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_arcade_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_gallery_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_kitchen_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_lobby_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_trophy_room_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_office_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_cigar_room_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_guest_bedroom_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_master_bedroom_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_basement_garage_top_loc_3", true, MansionRadio);
                        SetEmitter("dlc25-2_mansion_exterior_central_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_exterior_pool_bar_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_mansion_exterior_terrace_loc_3", true, MansionRadio);
                        SetEmitter("se_dlc25-2_basement_club_01_loc_3", true, MansionClubRadio);
                        SetEmitter("se_dlc25-2_basement_club_02_loc_3", true, MansionClubRadio);
                        try
                        {
                            Vector4 quat4 = new Vector4(0f, 0f, 0.947196f, -0.320656f);
                            Vector4 quat5 = new Vector4(0f, 0f, 0.337979f, 0.941154f);
                            Mansionchairs[9] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(543.985f, 726.466f, 201.961f), 217.405f, quat4);
                            Mansionchairs[10] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(544.961f, 727.216f, 201.961f), 304.358f, quat4);
                            Mansionchairs[11] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(545.857f, 727.906f, 201.961f), 217.405f, quat4);
                            Mansionchairs[12] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(546.706f, 728.559f, 201.961f), 217.405f, quat4);
                            Mansionchairs[13] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(542.76f, 727.947f, 201.961f), 39.5076f, quat5);
                            Mansionchairs[14] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(543.737f, 728.754f, 201.961f), 39.5076f, quat5);
                            Mansionchairs[15] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(544.66f, 729.515f, 201.961f), 39.5076f, quat5);
                            Mansionchairs[16] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(545.437f, 730.156f, 201.961f), 39.5076f, quat5);
                            Vector4 quat6 = new Vector4(0f, 0f, 0.891669f, -0.452687f);
                            Mansionchairs[17] = CreateFrozenInvincibleProp(RequestModel("ex_prop_offchair_exec_04"), new Vector3(557.304f, 717.821f, 201.942f), 233.832f, quat6);
                        }
                        catch
                        {
                            Notification.Show("~b~EAI~w~:~r~Error~w~ Could not spawn 'm25_2_prop_m52_diningchair_01a'. This is likely due to your mods folder missing model: 'm25_2_prop_m52_diningchair_01a', please remove mods folder and see if the error persists.", false);
                        }
                        break;
                    case MansionID.Tongva:
                        {
                            for (int k = 18; k < 27; k++)
                            {
                                bool flag3 = Mansionchairs[k] != null && Mansionchairs[k].Exists();
                                if (flag3)
                                {
                                    Mansionchairs[k].Delete();
                                    Mansionchairs[k] = null;
                                }
                            }
                            SetEmitter("se_dlc25-2_basement_armoury_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_garage_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_lobby_02_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_lobby_03_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_mod_garage_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_arcade_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_gallery_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_kitchen_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_lobby_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_trophy_room_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_office_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_cigar_room_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_guest_bedroom_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_master_bedroom_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_garage_top_loc_1", true, MansionRadio);
                            SetEmitter("dlc25-2_mansion_exterior_central_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_exterior_pool_bar_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_mansion_exterior_terrace_loc_1", true, MansionRadio);
                            SetEmitter("se_dlc25-2_basement_club_01_loc_1", true, MansionClubRadio);
                            SetEmitter("se_dlc25-2_basement_club_02_loc_1", true, MansionClubRadio);
                            Vector4 quat7 = new Vector4(0f, 0f, 0.999217f, -0.0395644f);
                            Vector4 quat8 = new Vector4(0f, 0f, 0.0526771f, 0.998612f);
                            try
                            {
                                Mansionchairs[18] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2594.76f, 1888.64f, 166.985f), 184.535f, quat7);
                                Mansionchairs[19] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2593.57f, 1888.72f, 166.985f), 184.535f, quat7);
                                Mansionchairs[20] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2592.44f, 1888.81f, 166.985f), 184.535f, quat7);
                                Mansionchairs[21] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2591.32f, 1888.89f, 166.985f), 184.535f, quat7);
                                Mansionchairs[22] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2594.85f, 1890.48f, 166.985f), 6.03915f, quat8);
                                Mansionchairs[23] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2593.66f, 1890.61f, 166.985f), 6.03915f, quat8);
                                Mansionchairs[24] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2592.6f, 1890.73f, 166.985f), 6.03915f, quat8);
                                Mansionchairs[25] = CreateFrozenInvincibleProp(RequestModel("m25_2_prop_m52_diningchair_01a"), new Vector3(-2591.53f, 1890.85f, 166.985f), 6.03915f, quat8);
                                Vector4 quat9 = new Vector4(0f, 0f, 0.97597f, -0.217906f);
                                Mansionchairs[26] = CreateFrozenInvincibleProp(RequestModel("ex_prop_offchair_exec_04"), new Vector3(-2588.06f, 1874.3f, 166.966f), 205.172f, quat9);
                            }
                            catch
                            {
                                Notification.Show("~b~EAI~w~:~r~Error~w~ Could not spawn 'm25_2_prop_m52_diningchair_01a'. This is likely due to your mods folder missing model: 'm25_2_prop_m52_diningchair_01a', please remove mods folder and see if the error persists.", false);
                            }
                            break;
                        }
                }
            }

            public enum MansionID
            {
                Richman,
                Vinewood,
                Tongva
            }
            public static void ActivateMansionEntitySets(int MainFloor, int LowerFloor, int GarageFloor, MansionID ID)
            {
                switch (ID)
                {
                    case MansionID.Richman:
                        {
                            if (The_Richman_Villa_AfterParty)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AFTERPARTY");
                            }
                            if (The_Richman_Villa_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_01");
                            }
                            if (The_Richman_Villa_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_02");
                            }
                            if (The_Richman_Villa_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_03");
                            }
                            if (The_Richman_Villa_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_COASTAL");
                            }
                            if (The_Richman_Villa_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_LOFT");
                            }
                            if (The_Richman_Villa_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_REGENCY");
                            }
                            if (The_Richman_Villa_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_CALI");
                            }
                            if (The_Richman_Villa_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_HOLLY");
                            }
                            if (The_Richman_Villa_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_LOFT");
                            }
                            if (The_Richman_Villa_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_SHELVING_PLANTER");
                            }
                            if (The_Richman_Villa_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_TROPHY_PLANTER");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_CARD");
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_POSTER");
                            if (The_Richman_Villa_KennelCat)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_CAT");
                            }
                            if (The_Richman_Villa_KennelDog)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_DOG");
                            }
                            if (The_Richman_Villa_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_SHELVING_PLANTER");
                            }
                            if (The_Richman_Villa_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_TROPHY_PLANTER");
                            }
                            if (The_Richman_Villa_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_SHELVING_PLANTER");
                            }
                            if (The_Richman_Villa_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STEP_COLLISION");
                            }
                            if (!The_Richman_Villa_DecorationsLunarNewYear)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                            }
                            if (The_Richman_Villa_DecorationsLunarNewYear)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                            }
                            if (!The_Richman_Villa_DecorationsBirthday)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                            }
                            if (The_Richman_Villa_DecorationsBirthday)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                            }
                            if (!The_Richman_Villa_DecorationsHalloween)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                            }
                            if (The_Richman_Villa_DecorationsHalloween)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                            }
                            if (!The_Richman_Villa_DecorationsXmas)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                            }
                            if (The_Richman_Villa_DecorationsXmas)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                            }
                            if (!The_Richman_Villa_DecorationsXmas)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                            }
                            if (The_Richman_Villa_DecorationsXmas)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                            }
                            if (!The_Richman_Villa_DecorationsHalloween)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                            }
                            if (The_Richman_Villa_DecorationsHalloween)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                            }
                            if (!The_Richman_Villa_DecorationsNewYear)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                            }
                            if (The_Richman_Villa_DecorationsNewYear)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                            }
                            if (!The_Richman_Villa_ArcadeRoom)
                            {
                                if (The_Richman_Villa_DecorStyle == 0)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_ARCADE_BLOCKER");
                                }
                                if (The_Richman_Villa_DecorStyle == 1)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ARCADE_BLOCKER");
                                }
                                if (The_Richman_Villa_DecorStyle == 2)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_ARCADE_BLOCKER");
                                }
                            }
                            if (!The_Richman_Villa_PodiumRoom)
                            {
                                if (The_Richman_Villa_DecorStyle == 0)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_PODIUM_BLOCKER");
                                }
                                if (The_Richman_Villa_DecorStyle == 1)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PODIUM_BLOCKER");
                                }
                                if (The_Richman_Villa_DecorStyle == 2)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_PODIUM_BLOCKER");
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[i].Item2);
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[i].Item3);
                            }
                            for (int j = 0; j < 6; j++)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionPattern[j].Item2);
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[The_Richman_Villa_DecorStyle].Item3);
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[The_Richman_Villa_DecorStyle].Item2);
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionPattern[The_Richman_Villa_WallpaperStyle].Item2);
                            Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, MainFloor, MansionDecor[The_Richman_Villa_DecorStyle].Item2, The_Tongva_Estate_Style);
                            Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, MainFloor, MansionPattern[The_Richman_Villa_WallpaperStyle].Item2, The_Tongva_Estate_Style);
                            if (The_Richman_Villa_WallpaperStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_COASTAL");
                            }
                            if (The_Richman_Villa_WallpaperStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_DECO");
                            }
                            if (The_Richman_Villa_WallpaperStyle == 3)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_POPART");
                            }
                            if (The_Richman_Villa_WallpaperStyle == 4)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_RUSTIC");
                            }
                            if (The_Richman_Villa_WallpaperStyle == 5)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SAFARI");
                            }
                            if (The_Richman_Villa_WallpaperStyle == 6)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SUBTLE");
                            }
                            if (The_Richman_Villa_WindowsBlackedOut)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ACCESS_BLOCKER");
                            }
                            if (!The_Richman_Villa_ArmouryRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ARMORY_BLOCKER");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS");
                            if (The_Richman_Villa_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_01");
                            }
                            if (The_Richman_Villa_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_02");
                            }
                            if (The_Richman_Villa_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_03");
                            }
                            if (The_Richman_Villa_VaultCash == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_00");
                            }
                            if (The_Richman_Villa_VaultCash == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_01");
                            }
                            if (The_Richman_Villa_VaultCash == 3)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_02");
                            }
                            if (The_Richman_Villa_VaultCash == 4)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_03");
                            }
                            if (The_Richman_Villa_VaultCash == 5)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_04");
                            }
                            if (The_Richman_Villa_VaultCash == 6)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_05");
                            }
                            if (The_Richman_Villa_VaultCash == 7)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_06");
                            }
                            if (The_Richman_Villa_VaultCash == 8)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_07");
                            }
                            if (The_Richman_Villa_VaultCash == 9)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_08");
                            }
                            if (The_Richman_Villa_VaultCash == 10)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_09");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ELEV_STD");
                            if (!The_Richman_Villa_VehicleModShopRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                            }
                            if (The_Richman_Villa_VehicleModShopRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                            }
                            if (!The_Richman_Villa_VaultRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_CLOSED");
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                            }
                            if (The_Richman_Villa_VaultRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS");
                            if (The_Richman_Villa_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_01");
                            }
                            if (The_Richman_Villa_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_02");
                            }
                            if (The_Richman_Villa_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_03");
                            }
                            if (!The_Richman_Villa_VehicleModShopRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                            }
                            if (The_Richman_Villa_VehicleModShopRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                            }
                            if (!The_Richman_Villa_PodiumRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                            }
                            if (The_Richman_Villa_PodiumRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                            }
                            Function.Call(Hash.REFRESH_INTERIOR, MainFloor);
                            Function.Call(Hash.REFRESH_INTERIOR, LowerFloor);
                            Function.Call(Hash.REFRESH_INTERIOR, GarageFloor);
                            break;
                        }
                    case MansionID.Vinewood:
                        {
                            if (The_Vinewood_Residence_AfterParty)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AFTERPARTY");
                            }
                            if (The_Vinewood_Residence_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_01");
                            }
                            if (The_Vinewood_Residence_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_02");
                            }
                            if (The_Vinewood_Residence_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_03");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_COASTAL");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_LOFT");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_REGENCY");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_CALI");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_HOLLY");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_LOFT");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_SHELVING_PLANTER");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_TROPHY_PLANTER");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_CARD");
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_POSTER");
                            if (The_Vinewood_Residence_KennelCat)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_CAT");
                            }
                            if (The_Vinewood_Residence_KennelDog)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_DOG");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_SHELVING_PLANTER");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_TROPHY_PLANTER");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_SHELVING_PLANTER");
                            }
                            if (The_Vinewood_Residence_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STEP_COLLISION");
                            }
                            if (!The_Vinewood_Residence_DecorationsLunarNewYear)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                            }
                            if (The_Vinewood_Residence_DecorationsLunarNewYear)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                            }
                            if (!The_Vinewood_Residence_DecorationsHalloween)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                            }
                            if (The_Vinewood_Residence_DecorationsHalloween)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                            }
                            if (!The_Vinewood_Residence_DecorationsBirthday)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                            }
                            if (The_Vinewood_Residence_DecorationsBirthday)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                            }
                            if (!The_Vinewood_Residence_DecorationsXmas)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                            }
                            if (The_Vinewood_Residence_DecorationsXmas)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                            }
                            if (!The_Vinewood_Residence_DecorationsHalloween)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                            }
                            if (The_Vinewood_Residence_DecorationsHalloween)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                            }
                            if (!The_Vinewood_Residence_DecorationsNewYear)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                            }
                            if (The_Vinewood_Residence_DecorationsNewYear)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                            }
                            if (!The_Vinewood_Residence_DecorationsXmas)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                            }
                            if (The_Vinewood_Residence_DecorationsXmas)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                            }
                            if (!The_Vinewood_Residence_ArcadeRoom)
                            {
                                if (The_Vinewood_Residence_DecorStyle == 0)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_ARCADE_BLOCKER");
                                }
                                if (The_Vinewood_Residence_DecorStyle == 1)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ARCADE_BLOCKER");
                                }
                                if (The_Vinewood_Residence_DecorStyle == 2)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_ARCADE_BLOCKER");
                                }
                            }
                            if (!The_Vinewood_Residence_PodiumRoom)
                            {
                                if (The_Vinewood_Residence_DecorStyle == 0)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_PODIUM_BLOCKER");
                                }
                                if (The_Vinewood_Residence_DecorStyle == 1)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PODIUM_BLOCKER");
                                }
                                if (The_Vinewood_Residence_DecorStyle == 2)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_PODIUM_BLOCKER");
                                }
                            }
                            for (int k = 0; k < 3; k++)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[k].Item2);
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[k].Item3);
                            }
                            for (int l = 0; l < 6; l++)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionPattern[l].Item2);
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[The_Vinewood_Residence_DecorStyle].Item3);
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[The_Vinewood_Residence_DecorStyle].Item2);
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionPattern[The_Vinewood_Residence_WallpaperStyle].Item2);
                            Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, MainFloor, MansionDecor[The_Vinewood_Residence_DecorStyle].Item2, The_Vinewood_Residence_Style);
                            Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, MainFloor, MansionPattern[The_Vinewood_Residence_WallpaperStyle].Item2, The_Vinewood_Residence_Style);
                            if (The_Vinewood_Residence_WallpaperStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_COASTAL");
                            }
                            if (The_Vinewood_Residence_WallpaperStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_DECO");
                            }
                            if (The_Vinewood_Residence_WallpaperStyle == 3)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_POPART");
                            }
                            if (The_Vinewood_Residence_WallpaperStyle == 4)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_RUSTIC");
                            }
                            if (The_Vinewood_Residence_WallpaperStyle == 5)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SAFARI");
                            }
                            if (The_Vinewood_Residence_WallpaperStyle == 6)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SUBTLE");
                            }
                            if (The_Vinewood_Residence_WindowsBlackedOut)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ACCESS_BLOCKER");
                            }
                            if (!The_Vinewood_Residence_ArmouryRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ARMORY_BLOCKER");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS");
                            if (The_Vinewood_Residence_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_01");
                            }
                            if (The_Vinewood_Residence_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_02");
                            }
                            if (The_Vinewood_Residence_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_03");
                            }
                            if (The_Vinewood_Residence_VaultCash == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_00");
                            }
                            if (The_Vinewood_Residence_VaultCash == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_01");
                            }
                            if (The_Vinewood_Residence_VaultCash == 3)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_02");
                            }
                            if (The_Vinewood_Residence_VaultCash == 4)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_03");
                            }
                            if (The_Vinewood_Residence_VaultCash == 5)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_04");
                            }
                            if (The_Vinewood_Residence_VaultCash == 6)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_05");
                            }
                            if (The_Vinewood_Residence_VaultCash == 7)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_06");
                            }
                            if (The_Vinewood_Residence_VaultCash == 8)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_07");
                            }
                            if (The_Vinewood_Residence_VaultCash == 9)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_08");
                            }
                            if (The_Vinewood_Residence_VaultCash == 10)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_09");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ELEV_STD");
                            if (!The_Vinewood_Residence_VehicleModShopRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                            }
                            if (The_Vinewood_Residence_VehicleModShopRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                            }
                            if (!The_Vinewood_Residence_VaultRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_CLOSED");
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                            }
                            if (The_Vinewood_Residence_VaultRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS");
                            if (The_Vinewood_Residence_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_01");
                            }
                            if (The_Vinewood_Residence_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_02");
                            }
                            if (The_Vinewood_Residence_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_03");
                            }
                            if (!The_Vinewood_Residence_VehicleModShopRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                            }
                            if (!The_Vinewood_Residence_PodiumRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                            }
                            if (The_Vinewood_Residence_VehicleModShopRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                            }
                            if (The_Vinewood_Residence_PodiumRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                            }
                            Function.Call(Hash.REFRESH_INTERIOR, MainFloor);
                            Function.Call(Hash.REFRESH_INTERIOR, LowerFloor);
                            Function.Call(Hash.REFRESH_INTERIOR, GarageFloor);
                            break;
                        }
                    case MansionID.Tongva:
                        {
                            if (The_Tongva_Estate_AfterParty)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AFTERPARTY");
                            }
                            if (The_Tongva_Estate_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_01");
                            }
                            if (The_Tongva_Estate_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_02");
                            }
                            if (The_Tongva_Estate_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_03");
                            }
                            if (The_Tongva_Estate_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_COASTAL");
                            }
                            if (The_Tongva_Estate_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_LOFT");
                            }
                            if (The_Tongva_Estate_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_REGENCY");
                            }
                            if (The_Tongva_Estate_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_CALI");
                            }
                            if (The_Tongva_Estate_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_HOLLY");
                            }
                            if (The_Tongva_Estate_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_LOFT");
                            }
                            if (!The_Tongva_Estate_DecorationsLunarNewYear)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                            }
                            if (The_Tongva_Estate_DecorationsLunarNewYear)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                            }
                            if (!The_Tongva_Estate_DecorationsBirthday)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                            }
                            if (The_Tongva_Estate_DecorationsBirthday)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                            }
                            if (!The_Tongva_Estate_DecorationsHalloween)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                            }
                            if (The_Tongva_Estate_DecorationsHalloween)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                            }
                            if (!The_Tongva_Estate_DecorationsXmas)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                            }
                            if (The_Tongva_Estate_DecorationsXmas)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                            }
                            if (!The_Tongva_Estate_DecorationsHalloween)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                            }
                            if (The_Tongva_Estate_DecorationsHalloween)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                            }
                            if (!The_Tongva_Estate_DecorationsNewYear)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                            }
                            if (The_Tongva_Estate_DecorationsNewYear)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                            }
                            if (!The_Tongva_Estate_DecorationsXmas)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                            }
                            if (The_Tongva_Estate_DecorationsXmas)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                            }
                            if (The_Tongva_Estate_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_SHELVING_PLANTER");
                            }
                            if (The_Tongva_Estate_DecorStyle == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_TROPHY_PLANTER");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_CARD");
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_POSTER");
                            if (The_Tongva_Estate_KennelCat)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_CAT");
                            }
                            if (The_Tongva_Estate_KennelDog)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_DOG");
                            }
                            if (The_Tongva_Estate_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_SHELVING_PLANTER");
                            }
                            if (The_Tongva_Estate_DecorStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_TROPHY_PLANTER");
                            }
                            if (The_Tongva_Estate_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_SHELVING_PLANTER");
                            }
                            if (The_Tongva_Estate_DecorStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STEP_COLLISION");
                            }
                            if (!The_Tongva_Estate_ArcadeRoom)
                            {
                                if (The_Tongva_Estate_DecorStyle == 0)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_ARCADE_BLOCKER");
                                }
                                if (The_Tongva_Estate_DecorStyle == 1)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ARCADE_BLOCKER");
                                }
                                if (The_Tongva_Estate_DecorStyle == 2)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_ARCADE_BLOCKER");
                                }
                            }
                            if (!The_Tongva_Estate_PodiumRoom)
                            {
                                if (The_Tongva_Estate_DecorStyle == 0)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_PODIUM_BLOCKER");
                                }
                                if (The_Tongva_Estate_DecorStyle == 1)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PODIUM_BLOCKER");
                                }
                                if (The_Tongva_Estate_DecorStyle == 2)
                                {
                                    Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_PODIUM_BLOCKER");
                                }
                            }
                            for (int m = 0; m < 3; m++)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[m].Item2);
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[m].Item3);
                            }
                            for (int n = 0; n < 6; n++)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionPattern[n].Item2);
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[The_Tongva_Estate_DecorStyle].Item3);
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionDecor[The_Tongva_Estate_DecorStyle].Item2);
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, MansionPattern[The_Tongva_Estate_WallpaperStyle].Item2);
                            Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, MainFloor, MansionDecor[The_Tongva_Estate_DecorStyle].Item2, The_Tongva_Estate_Style);
                            Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, MainFloor, MansionPattern[The_Tongva_Estate_WallpaperStyle].Item2, The_Tongva_Estate_Style);
                            Function.Call(Hash.REFRESH_INTERIOR, MainFloor);
                            if (The_Tongva_Estate_WallpaperStyle == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_COASTAL");
                            }
                            if (The_Tongva_Estate_WallpaperStyle == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_DECO");
                            }
                            if (The_Tongva_Estate_WallpaperStyle == 3)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_POPART");
                            }
                            if (The_Tongva_Estate_WallpaperStyle == 4)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_RUSTIC");
                            }
                            if (The_Tongva_Estate_WallpaperStyle == 5)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SAFARI");
                            }
                            if (The_Tongva_Estate_WallpaperStyle == 6)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SUBTLE");
                            }
                            if (The_Tongva_Estate_WindowsBlackedOut)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ACCESS_BLOCKER");
                            }
                            if (!The_Tongva_Estate_ArmouryRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ARMORY_BLOCKER");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS");
                            if (The_Tongva_Estate_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_01");
                            }
                            if (The_Tongva_Estate_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_02");
                            }
                            if (The_Tongva_Estate_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_03");
                            }
                            if (The_Tongva_Estate_VaultCash == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_00");
                            }
                            if (The_Tongva_Estate_VaultCash == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_01");
                            }
                            if (The_Tongva_Estate_VaultCash == 3)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_02");
                            }
                            if (The_Tongva_Estate_VaultCash == 4)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_03");
                            }
                            if (The_Tongva_Estate_VaultCash == 5)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_04");
                            }
                            if (The_Tongva_Estate_VaultCash == 6)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_05");
                            }
                            if (The_Tongva_Estate_VaultCash == 7)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_06");
                            }
                            if (The_Tongva_Estate_VaultCash == 8)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_07");
                            }
                            if (The_Tongva_Estate_VaultCash == 9)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_08");
                            }
                            if (The_Tongva_Estate_VaultCash == 10)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_09");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ELEV_STD");
                            if (!The_Tongva_Estate_VehicleModShopRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                            }
                            if (The_Tongva_Estate_VehicleModShopRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                            }
                            if (!The_Tongva_Estate_VaultRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_CLOSED");
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                            }
                            if (The_Tongva_Estate_VaultRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                            }
                            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS");
                            if (The_Tongva_Estate_AI == 2)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS_01");
                            }
                            if (The_Tongva_Estate_AI == 1)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS_02");
                            }
                            if (The_Tongva_Estate_AI == 0)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS_03");
                            }
                            if (!The_Tongva_Estate_VehicleModShopRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                            }
                            if (!The_Tongva_Estate_PodiumRoom)
                            {
                                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                            }
                            if (The_Tongva_Estate_VehicleModShopRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                            }
                            if (The_Tongva_Estate_PodiumRoom)
                            {
                                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                            }
                            Function.Call(Hash.REFRESH_INTERIOR, MainFloor);
                            Function.Call(Hash.REFRESH_INTERIOR, LowerFloor);
                            Function.Call(Hash.REFRESH_INTERIOR, GarageFloor);
                            break;
                        }
                }
            }

            public static void DeactivateMansionEntitySets(int MainFloor, int LowerFloor, int GarageFloor)
            {
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AFTERPARTY");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_01");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_02");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_AI_TABLETS_03");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ARCADE_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_COASTAL");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_LOFT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ART_REGENCY");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_BIRTHDAY");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_CALI");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_HOLLY");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_ELEV_LOFT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_HALLOWEEN");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_ARCADE_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_PODIUM_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_SHELVING_PLANTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LOFT_TROPHY_PLANTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_LUNAR");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_CARD");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_MICHAEL_POSTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_CAT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PET_DOG");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_PODIUM_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_ARCADE_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_PODIUM_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_SHELVING_PLANTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_REG_TROPHY_PLANTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_SHELVING_PLANTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STEP_COLLISION");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STYLE_CALI");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STYLE_CALI_TINT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STYLE_HOLLY");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STYLE_LOFT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STYLE_LOFT_TINT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_STYLE_REG_TINT");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_TROPHY_PLANTER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_TROPHY_SHELVES");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_COASTAL");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_DECO");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_POPART");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_RUSTIC");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SAFARI");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_WALLPAPER_SUBTLE");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, MainFloor, "SET_XMAS");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ACCESS_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ARMORY_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_01");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_02");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_AI_TABLETS_03");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_HALLOWEEN");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_NEW_YEAR");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_00");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_01");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_02");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_03");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_04");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_05");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_06");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_07");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_08");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_VAULT_09");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_BASE_XMAS");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_ELEV_STD");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_MOD_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_CLOSED");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, LowerFloor, "SET_VAULT_DOOR_OPEN");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS_01");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS_02");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_AI_TABLETS_03");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_MOD_BLOCKER");
                Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, GarageFloor, "SET_GAR_PODIUM_BLOCKER");
                Function.Call(Hash.REFRESH_INTERIOR, MainFloor);
                Function.Call(Hash.REFRESH_INTERIOR, LowerFloor);
                Function.Call(Hash.REFRESH_INTERIOR, GarageFloor);
            }

            public static List<string> RadioStations = new List<string>();

            public static Prop[] Mansionchairs = new Prop[64];

            public static List<Tuple<string, int>> MansionTint = new List<Tuple<string, int>>
        {
            new Tuple<string, int>("Cream", 0),
            new Tuple<string, int>("Mint", 1),
            new Tuple<string, int>("Lavender", 2),
            new Tuple<string, int>("Salmon", 3)
        };

            public static List<Tuple<string, string>> MansionPattern = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("None", ""),
            new Tuple<string, string>("Palms", "SET_WALLPAPER_COASTAL"),
            new Tuple<string, string>("Floral", "SET_WALLPAPER_DECO"),
            new Tuple<string, string>("Lipstick", "SET_WALLPAPER_POPART"),
            new Tuple<string, string>("Tiles", "SET_WALLPAPER_RUSTIC"),
            new Tuple<string, string>("Zebras", "SET_WALLPAPER_SAFARI"),
            new Tuple<string, string>("Pebbles", "SET_WALLPAPER_SUBTLE")
        };

            public static List<Tuple<string, string, string>> MansionDecor = new List<Tuple<string, string, string>>
        {
            new Tuple<string, string, string>("Tint1", "SET_STYLE_LOFT_TINT", "SET_STYLE_LOFT"),
            new Tuple<string, string, string>("Tint2", "SET_STYLE_CALI_TINT", "SET_STYLE_CALI"),
            new Tuple<string, string, string>("Tint3", "SET_STYLE_REG_TINT", "SET_STYLE_HOLLY")
        };

            public static bool ASafehouseOnTheHills_Enable_The_Richman_Villa = true;
            public static int The_Richman_Villa_Style = 1;
            public static int The_Richman_Villa_AI = 1;
            public static bool The_Richman_Villa_WindowsBlackedOut = false;
            public static bool The_Richman_Villa_ArcadeRoom = true;
            public static bool The_Richman_Villa_PodiumRoom = true;
            public static bool The_Richman_Villa_ArmouryRoom = true;
            public static bool The_Richman_Villa_VehicleModShopRoom = true;
            public static bool The_Richman_Villa_VaultRoom = true;
            public static int The_Richman_Villa_VaultCash = 0;
            public static bool The_Richman_Villa_AfterParty;
            public static int The_Richman_Villa_DecorStyle = 1;
            public static int The_Richman_Villa_WallpaperStyle = 1;
            public static bool The_Richman_Villa_KennelCat;
            public static bool The_Richman_Villa_KennelDog = true;
            public static bool The_Richman_Villa_DecorationsXmas;
            public static bool The_Richman_Villa_DecorationsHalloween;
            public static bool The_Richman_Villa_DecorationsNewYear;
            public static bool The_Richman_Villa_DecorationsLunarNewYear;
            public static bool The_Richman_Villa_DecorationsBirthday;
            public static int The_Richman_Villa_MansionRadio;
            public static int The_Richman_Villa_MansionClubRadio;
            public static bool ASafehouseOnTheHills_Enable_The_Vinewood_Residence = true;
            public static int The_Vinewood_Residence_Style = 1;
            public static int The_Vinewood_Residence_AI = 1;
            public static bool The_Vinewood_Residence_WindowsBlackedOut = false;
            public static bool The_Vinewood_Residence_ArcadeRoom = true;
            public static bool The_Vinewood_Residence_PodiumRoom = true;
            public static bool The_Vinewood_Residence_ArmouryRoom = true;
            public static bool The_Vinewood_Residence_VehicleModShopRoom = true;
            public static bool The_Vinewood_Residence_VaultRoom = true;
            public static int The_Vinewood_Residence_VaultCash = 0;
            public static bool The_Vinewood_Residence_AfterParty;
            public static int The_Vinewood_Residence_DecorStyle = 1;
            public static int The_Vinewood_Residence_WallpaperStyle = 1;
            public static bool The_Vinewood_Residence_KennelCat;
            public static bool The_Vinewood_Residence_KennelDog = true;
            public static bool The_Vinewood_Residence_DecorationsXmas;
            public static bool The_Vinewood_Residence_DecorationsHalloween;
            public static bool The_Vinewood_Residence_DecorationsNewYear;
            public static bool The_Vinewood_Residence_DecorationsLunarNewYear;
            public static bool The_Vinewood_Residence_DecorationsBirthday;
            public static int The_Vinewood_Residence_MansionRadio;
            public static int The_Vinewood_Residence_MansionClubRadio;
            public static bool ASafehouseOnTheHills_Enable_The_Tongva_Estate = true;
            public static int The_Tongva_Estate_Style = 1;
            public static int The_Tongva_Estate_AI = 1;
            public static bool The_Tongva_Estate_WindowsBlackedOut = false;
            public static bool The_Tongva_Estate_ArcadeRoom = true;
            public static bool The_Tongva_Estate_PodiumRoom = true;
            public static bool The_Tongva_Estate_ArmouryRoom = true;
            public static bool The_Tongva_Estate_VehicleModShopRoom = true;
            public static bool The_Tongva_Estate_VaultRoom = true;
            public static int The_Tongva_Estate_VaultCash = 0;
            public static bool The_Tongva_Estate_AfterParty;
            public static int The_Tongva_Estate_DecorStyle = 1;
            public static int The_Tongva_Estate_WallpaperStyle = 1;
            public static bool The_Tongva_Estate_KennelCat;
            public static bool The_Tongva_Estate_KennelDog = true;
            public static bool The_Tongva_Estate_DecorationsXmas;
            public static bool The_Tongva_Estate_DecorationsHalloween;
            public static bool The_Tongva_Estate_DecorationsNewYear;
            public static bool The_Tongva_Estate_DecorationsLunarNewYear;
            public static bool The_Tongva_Estate_DecorationsBirthday;
            public static int The_Tongva_Estate_MansionRadio;
            public static int The_Tongva_Estate_MansionClubRadio;
            public static void RequestMansion_TheRichmanVilla()
            {
                int mainFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1666.36f, 478.92f, 128.22f);
                int lowerFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1649.63f, 480.97f, 117.36f);
                int garageFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1679.87f, 493.59f, 112.93f);
                bool asafehouseOnTheHills_Enable_The_Richman_Villa = ASafehouseOnTheHills_Enable_The_Richman_Villa;
                if (asafehouseOnTheHills_Enable_The_Richman_Villa)
                {
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_generic");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_railings_m");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_railings_p");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_shutters");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_06e_mansion_interior_a");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_06e_mansion_interior_b");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_06e_mansion_interior_c");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_06e_mansion_interior_d");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_original");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_props_original");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_roads_original");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_shared");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06f_mansion_shared");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_private");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_roads_mansion");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_player_bounds");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_railings_m");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_furniture");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_firepit");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_mansion_gym");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_dog_house");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_dog_house");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06f_mansion_original");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06f_mansion_shared");
                    DeactivateMansionEntitySets(mainFloor, lowerFloor, garageFloor);
                    ActivateMansionEntitySets(mainFloor, lowerFloor, garageFloor, MansionID.Richman);
                    Script.Wait(500);
                    SetupMansionChairsAndEmitters(The_Richman_Villa_MansionRadio, The_Tongva_Estate_MansionClubRadio, MansionID.Richman);
                }
                else
                {
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_ground");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_original_terrain");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06f_mansion_shared");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_06e_mansion_interior_a");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_06e_mansion_interior_b");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_06e_mansion_interior_c");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_06e_mansion_interior_d");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_mansion_original");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_06e_props_original");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_roads_original");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_shared");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_generic");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_private");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_player_bounds");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_railings_m");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_railings_p");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_furniture");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_shutters");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_06e_mansion_firepit");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_mansion_gym");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_dog_house");
                }
            }
            public static void RequestMansion_TheTongvaEstate()
            {
                int mainFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -2586.065f, 1909.995f, 166.37f);
                int lowerFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -2587.495f, 1893.193f, 155.51f);
                int garageFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -2568.933f, 1920.202f, 151.08f);
                bool asafehouseOnTheHills_Enable_The_Tongva_Estate = ASafehouseOnTheHills_Enable_The_Tongva_Estate;
                if (asafehouseOnTheHills_Enable_The_Tongva_Estate)
                {
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_generic");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_private");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_railings_m");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_railings_p");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_shutters");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_09_mansion_interior_a");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_09_mansion_interior_b");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_09_mansion_interior_c");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch1_09_mansion_interior_d");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_original");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_props_original");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_roads_original");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_shared");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_private");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_player_bounds");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_railings_m");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_furniture");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_firepit");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_tongva_mansion_gym");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_tongva_dog_house");
                    DeactivateMansionEntitySets(mainFloor, lowerFloor, garageFloor);
                    ActivateMansionEntitySets(mainFloor, lowerFloor, garageFloor, MansionID.Tongva);
                    Script.Wait(500);
                    SetupMansionChairsAndEmitters(The_Tongva_Estate_MansionRadio, The_Tongva_Estate_MansionClubRadio, MansionID.Tongva);
                }
                else
                {
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_09_mansion_interior_a");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_09_mansion_interior_b");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_09_mansion_interior_c");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch1_09_mansion_interior_d");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_mansion_original");
                    Function.Call(Hash.REQUEST_IPL, "hei_ch1_09_props_original");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_shared");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_generic");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_private");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_player_bounds");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_railings_m");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_railings_p");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_furniture");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_shutters");
                    Function.Call(Hash.REMOVE_IPL, "hei_ch1_09_mansion_firepit");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_tongva_mansion_gym");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_tongva_dog_house");
                }
            }
            public static void RequestMansion_TheVinewoodResidence()
            {
                int mainFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 539.7012f, 749.089f, 201.36f);
                int lowerFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 547.4955f, 734.136f, 190.5f);
                int garageFloor = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 548.6964f, 766.88f, 186.07f);
                bool asafehouseOnTheHills_Enable_The_Vinewood_Residence = ASafehouseOnTheHills_Enable_The_Vinewood_Residence;
                if (asafehouseOnTheHills_Enable_The_Vinewood_Residence)
                {
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_generic");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_private");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_railings_m");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_railings_p");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_shutters");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch2_04_mansion_interior_a");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch2_04_mansion_interior_b");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch2_04_mansion_interior_c");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_ch2_04_mansion_interior_d");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_original");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_props_original");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_grass");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_shared");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_private");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_player_bounds");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_railings_m");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_furniture");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_firepit");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_east_mansion_gym");
                    Function.Call(Hash.REQUEST_IPL, "m25_2_east_dog_house");
                    DeactivateMansionEntitySets(mainFloor, lowerFloor, garageFloor);
                    ActivateMansionEntitySets(mainFloor, lowerFloor, garageFloor, MansionID.Vinewood);
                    Script.Wait(500);
                    SetupMansionChairsAndEmitters(The_Vinewood_Residence_MansionRadio, The_Vinewood_Residence_MansionClubRadio, MansionID.Vinewood);
                }
                else
                {
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch2_04_mansion_interior_a");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch2_04_mansion_interior_b");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch2_04_mansion_interior_c");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_ch2_04_mansion_interior_d");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_original");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_props_original");
                    Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_mansion_grass");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_shared");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_generic");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_private");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_player_bounds");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_railings_m");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_railings_p");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_furniture");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_shutters");
                    Function.Call(Hash.REMOVE_IPL, "apa_ch2_04_mansion_firepit");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_east_mansion_gym");
                    Function.Call(Hash.REMOVE_IPL, "m25_2_east_dog_house");
                }
            }
            public static void RequestMansions()
            {
                RequestMansion_TheRichmanVilla();
                RequestMansion_TheVinewoodResidence();
                RequestMansion_TheTongvaEstate();
            }
        }

        public class MPMaps
        {
            public static bool IsMpMapLoaded
            {
                get
                {
                    if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, "xm_hatch_closed"))
                        return false;
                    else
                        return true;
                }
            }

            public static void LoadMpMaps()
            {
                if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, "xm_hatch_closed"))
                {
                    Function.Call(Hash.ON_ENTER_MP);
                    LoadingPrompt.Hide();
                    Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
                }
                foreach (string str in RemoveOnlyIPLS)
                {
                    Function.Call(Hash.REMOVE_IPL, str);
                }
                foreach (string str2 in LoadAllIPLS)
                {
                    Function.Call(Hash.REMOVE_IPL, str2);
                    Function.Call(Hash.REQUEST_IPL, str2);
                }
            }

            public static List<string> LoadAllIPLS = new List<string>
        {
            "xm_hatch_01_cutscene",
            "xm_hatch_02_cutscene",
            "xm_hatch_03_cutscene",
            "xm_hatch_04_cutscene",
            "xm_hatch_05_cutscene",
            "xm_hatch_06_cutscene",
            "xm_hatch_07_cutscene",
            "xm_hatch_08_cutscene",
            "xm_hatch_09_cutscene",
            "xm_hatch_10_cutscene",
            "xm_hatch_closed",
            "xm_hatches_terrain",
            "xm_hatches_terrain_lod",
            "sm_smugdlc_interior_placement",
            "xm_mpchristmasadditions",
            "xm_siloentranceclosed_x17",
            "id2_14_during1",
            "shr_int",
            "xm_x17dlc_int_placement_interior_8_x17dlc_int_sub_milo_",
            "bkr_bi_hw1_13_int",
            "bkr_bi_id1_23_door",
            "vw_dlc_casino_door",
            "xm_x17dlc_int_placement_interior_4_x17dlc_int_facility_milo_",
            "xm_x17dlc_int_placement_interior_5_x17dlc_int_facility2_milo_",
            "xm_x17dlc_int_placement_interior_0_x17dlc_int_base_ent_milo_",
            "xm_x17dlc_int_placement_interior_1_x17dlc_int_base_loop_milo_",
            "xm_x17dlc_int_placement_interior_2_x17dlc_int_bse_tun_milo_",
            "xm_x17dlc_int_placement_interior_3_x17dlc_int_base_milo_",
            "xm_x17dlc_int_placement_interior_6_x17dlc_int_silo_01_milo_",
            "xm_x17dlc_int_placement_interior_7_x17dlc_int_silo_02_milo_",
            "xm_x17dlc_int_placement_interior_10_x17dlc_int_tun_straight_milo_",
            "xm_x17dlc_int_placement_interior_11_x17dlc_int_tun_slope_flat_milo_",
            "xm_x17dlc_int_placement_interior_12_x17dlc_int_tun_flat_slope_milo_",
            "xm_x17dlc_int_placement_interior_13_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_14_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_15_x17dlc_int_tun_straight_milo_",
            "xm_x17dlc_int_placement_interior_16_x17dlc_int_tun_straight_milo_",
            "xm_x17dlc_int_placement_interior_17_x17dlc_int_tun_slope_flat_milo_",
            "xm_x17dlc_int_placement_interior_18_x17dlc_int_tun_slope_flat_milo_",
            "xm_x17dlc_int_placement_interior_19_x17dlc_int_tun_flat_slope_milo_",
            "xm_x17dlc_int_placement_interior_20_x17dlc_int_tun_flat_slope_milo_",
            "xm_x17dlc_int_placement_interior_21_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_22_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_23_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_24_x17dlc_int_tun_30d_r_milo_",
            "xm_x17dlc_int_placement_interior_25_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_26_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_27_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_28_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_29_x17dlc_int_tun_30d_l_milo_",
            "xm_x17dlc_int_placement_interior_34_x17dlc_int_lab_milo_",
            "xm_x17dlc_int_placement_interior_35_x17dlc_int_tun_entry_milo_",
            "xm_x17dlc_int_placement_strm_0",
            "xm_x17dlc_int_placement_interior_33_x17dlc_int_02_milo_",
            "xm_prop_x17_tem_control_01",
            "SP1_10_real_interior",
            "post_hiest_unload",
            "facelobby",
            "FIBlobby",
            "Coroner_Int_on",
            "h4_ch2_mansion_final",
            "hei_ch1_06e_strm_1",
            "ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo",
            "FINBANK",
            "h4_islandairstrip_doorsopen",
            "v_tunnel_hole",
            "plane_crash_trench",
            "hei_dlc_casino_door",
            "vw_casino_main",
            "vw_casino_carpark",
            "vw_casino_garage",
            "vw_casino_penthouse",
            "hei_dlc_casino_aircon",
            "hei_dlc_casino_aircon_lod",
            "hei_dlc_casino_door",
            "hei_dlc_casino_door_lod",
            "hei_dlc_vw_roofdoors_locked",
            "hei_dlc_windows_casino",
            "hei_dlc_windows_casino_lod",
            "ch_chint09_closed",
            "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo",
            "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo",
            "h4_island_padlock_props",
            "h4_BoatBlockers",
            "h4_Mansion_Gate_Closed",
            "xm3_collision_fixes",
            "xm3_cutscene_doors",
            "xm3_doc_sign",
            "xm3_doc_sign_lod",
            "xm3_garage_fix",
            "xm3_garage_fix_lod",
            "xm3_security_fix",
            "xm3_stash_cams",
            "xm3_sum2_fix",
            "xm3_sum2_fix_lod",
            "xm3_warehouse",
            "xm3_warehouse_grnd",
            "xm3_warehouse_lod",
            "m23_2_cargoship",
            "m23_2_cargoship_bridge"
        };

            public static List<string> RemoveOnlyIPLS = new List<string>
        {
            "xm_bunkerentrance_door",
            "chemgrill_grp1",
            "id2_14_pre_no_int",
            "id2_14_post_no_int",
            "id2_14_on_fire",
            "id2_14_during_door",
            "id2_14_during2",
            "burnt_switch_off",
            "id2_14_during1",
            "fakeint",
            "fakeint_boards",
            "shr_int",
            "carshowroom_boarded",
            "carshowroom_broken",
            "SP1_10_fake_interior",
            "bh1_16_refurb",
            "jewel2fake",
            "FIBlobbyfake",
            "h4_islandairstrip_doorsclosed",
            "hei_po1_07_strm_2",
            "v_tunnel_hole_swap",
            "dt1_03_shutter",
            "dt1_03_gr_closed",
            "atriumglcut",
            "atriumglstatic",
            "atriumglmission",
            "FBI_repair",
            "FBI_colPLUG",
            "DT1_05_rubble",
            "DT1_05_HC_REMOVE",
            "DT1_05_HC_REQ",
            "dt1_05_slod",
            "dt1_05_damage_slod",
            "dt1_05_build1_damage_lod",
            "dt1_05_build1_damage",
            "dt1_05_build1_h",
            "DT1_05_REQUEST",
            "FBI_repair_lod",
            "dt1_05_build1_h",
            "dt1_05_build1_damage",
            "dt1_05_build1_damage_lod",
            "h4_island_padlock_props",
            "h4_islandxdock_water_hatch",
            "h4_islandx_barrack_hatch",
            "h4_BoatBlockers",
            "h4_underwater_gate_closed",
            "h4_Mansion_Gate_Broken",
            "h4_Mansion_Gate_Closed",
            "hei_ch1_06e_strm_2",
            "hei_ch1_06e_strm_1"
        };
        }

        public class PrologueMap : Script
        {
            public static bool YankTon = false;
            public static bool YankTonRemove = true;
            int YanktonIPLSLoaded;
            public PrologueMap()
            {
                Aborted += PrologueMap_Aborted;
            }

            private void PrologueMap_Aborted(object sender, EventArgs e)
            {
                UnloadYankton();
            }

            void Yankton()
            {
                if (!YankTon)
                {
                    #region EnableYankton + CashDepot
                    fInterior.RequestIpl("prologue06_int");
                    int l_1196 = fInterior.GetInteriorAtCoordsWithType(new Vector3(5311.236f, -5212.563f, (85.7187f - 3.2f)), "V_CashDepot");
                    fInterior.PinInteriorInMemory(l_1196);
                    while (true)
                    {
                        if (fInterior.IsInteriorReady(l_1196))
                        {
                            break;
                        }
                        Wait(0);
                    }
                    fInterior.RequestIpl("prologue01");
                    fInterior.RequestIpl("prologue02");
                    fInterior.RequestIpl("prologue03");
                    fInterior.RequestIpl("prologue04");
                    fInterior.RequestIpl("prologue05");
                    fInterior.RequestIpl("prologue06");
                    fInterior.RequestIpl("prologuerd");
                    fInterior.RequestIpl("Prologue01c");
                    fInterior.RequestIpl("Prologue01d");
                    fInterior.RequestIpl("Prologue01e");
                    fInterior.RequestIpl("Prologue01f");
                    fInterior.RequestIpl("Prologue01g");
                    fInterior.RequestIpl("prologue01h");
                    fInterior.RequestIpl("prologue01i");
                    fInterior.RequestIpl("prologue01j");
                    fInterior.RequestIpl("prologue01k");
                    fInterior.RequestIpl("prologue01z");
                    fInterior.RequestIpl("prologue03b");
                    fInterior.RequestIpl("prologue04b");
                    fInterior.RequestIpl("prologue05b");
                    fInterior.RequestIpl("prologue06b");
                    fInterior.RequestIpl("prologuerdb");
                    fInterior.RequestIpl("DES_ProTree_start");
                    fInterior.RequestIpl("DES_ProTree_start_lod");
                    fInterior.RequestIpl("prologue04_cover");
                    fInterior.RequestIpl("prologue03_grv_fun");
                    fInterior.RequestIpl("prologue03_grv_cov");
                    fInterior.RequestIpl("prologue_LODLights");
                    fInterior.RequestIpl("prologue_DistantLights");
                    int zone = fZone.GetZoneFromNameID("PrLog");
                    fZone.SetZoneEnabled(zone, true);
                    fHud.ToggleNorthYanktonMap(true);
                    fPathfind.SetAllowStreamPrologueNodes(true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, true, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, true, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, true, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, true, true);
                    Entity e = fPlayer.PlayerPedID();
                    if (fPed.IsPedInAnyVehicle((Ped)e, false))
                        e = fPed.GetVehiclePedIsUsing((Ped)e);
                    fEntity.SetEntityCoordsNoOffset(e, new Vector3(5342.45f, -5189.75f, 82.77f), false, false, true);
                    YankTon = true;
                    #endregion
                }
                else if (YankTonRemove)
                {
                    #region YanktonRemove
                    fInterior.RemoveIpl("prologue06_int");
                    fInterior.RemoveIpl("prologue01");
                    fInterior.RemoveIpl("prologue02");
                    fInterior.RemoveIpl("prologue03");
                    fInterior.RemoveIpl("prologue04");
                    fInterior.RemoveIpl("prologue05");
                    fInterior.RemoveIpl("prologue06");
                    fInterior.RemoveIpl("prologuerd");
                    fInterior.RemoveIpl("Prologue01c");
                    fInterior.RemoveIpl("Prologue01d");
                    fInterior.RemoveIpl("Prologue01e");
                    fInterior.RemoveIpl("Prologue01f");
                    fInterior.RemoveIpl("Prologue01g");
                    fInterior.RemoveIpl("prologue01h");
                    fInterior.RemoveIpl("prologue01i");
                    fInterior.RemoveIpl("prologue01j");
                    fInterior.RemoveIpl("prologue01k");
                    fInterior.RemoveIpl("prologue01z");
                    fInterior.RemoveIpl("prologue03b");
                    fInterior.RemoveIpl("prologue04b");
                    fInterior.RemoveIpl("prologue05b");
                    fInterior.RemoveIpl("prologue06b");
                    fInterior.RemoveIpl("prologuerdb");
                    fInterior.RemoveIpl("DES_ProTree_start");
                    fInterior.RemoveIpl("DES_ProTree_start_lod");
                    fInterior.RemoveIpl("prologue04_cover");
                    fInterior.RemoveIpl("prologue03_grv_fun");
                    fInterior.RemoveIpl("prologue03_grv_cov");
                    fInterior.RemoveIpl("prologue_LODLights");
                    fInterior.RemoveIpl("prologue_DistantLights");
                    int zone = fZone.GetZoneFromNameID("PrLog");
                    fZone.SetZoneEnabled(zone, false);
                    fHud.ToggleNorthYanktonMap(false);
                    fPathfind.SetAllowStreamPrologueNodes(false);
                    fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, false, true);
                    YankTon = false;
                    #endregion
                }
            }
            public static void LoadYankton(bool teleportToDepot = false)
            {
                if (!YankTon)
                {
                    fInterior.RequestIpl("prologue06_int");
                    int l_1196 = fInterior.GetInteriorAtCoordsWithType(new Vector3(5311.236f, -5212.563f, (85.7187f - 3.2f)), "V_CashDepot");
                    fInterior.PinInteriorInMemory(l_1196);
                    while (true)
                    {
                        if (fInterior.IsInteriorReady(l_1196))
                            break;
                        Wait(0);
                    }
                    fInterior.RequestIpl("prologue01");
                    fInterior.RequestIpl("prologue02");
                    fInterior.RequestIpl("prologue03");
                    fInterior.RequestIpl("prologue04");
                    fInterior.RequestIpl("prologue05");
                    fInterior.RequestIpl("prologue06");
                    fInterior.RequestIpl("prologuerd");
                    fInterior.RequestIpl("Prologue01c");
                    fInterior.RequestIpl("Prologue01d");
                    fInterior.RequestIpl("Prologue01e");
                    fInterior.RequestIpl("Prologue01f");
                    fInterior.RequestIpl("Prologue01g");
                    fInterior.RequestIpl("prologue01h");
                    fInterior.RequestIpl("prologue01i");
                    fInterior.RequestIpl("prologue01j");
                    fInterior.RequestIpl("prologue01k");
                    fInterior.RequestIpl("prologue01z");
                    fInterior.RequestIpl("prologue03b");
                    fInterior.RequestIpl("prologue04b");
                    fInterior.RequestIpl("prologue05b");
                    fInterior.RequestIpl("prologue06b");
                    fInterior.RequestIpl("prologuerdb");
                    fInterior.RequestIpl("DES_ProTree_start");
                    fInterior.RequestIpl("DES_ProTree_start_lod");
                    fInterior.RequestIpl("prologue04_cover");
                    fInterior.RequestIpl("prologue03_grv_fun");
                    fInterior.RequestIpl("prologue03_grv_cov");
                    fInterior.RequestIpl("prologue_LODLights");
                    fInterior.RequestIpl("prologue_DistantLights");
                    int zone = fZone.GetZoneFromNameID("PrLog");
                    fZone.SetZoneEnabled(zone, true);
                    fHud.ToggleNorthYanktonMap(true);
                    fPathfind.SetAllowStreamPrologueNodes(true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, true, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, true, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, true, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, true, true);
                    if (teleportToDepot)
                    {
                        Entity e = fPlayer.PlayerPedID();
                        if (fPed.IsPedInAnyVehicle((Ped)e, false))
                            e = fPed.GetVehiclePedIsUsing((Ped)e);
                        fEntity.SetEntityCoordsNoOffset(e, new Vector3(5342.45f, -5189.75f, 82.77f), false, false, true);
                    }
                    YankTon = true;
                }
            }
            public static void UnloadYankton()
            {
                if (YankTon && YankTonRemove)
                {
                    fInterior.RemoveIpl("prologue06_int");
                    fInterior.RemoveIpl("prologue01");
                    fInterior.RemoveIpl("prologue02");
                    fInterior.RemoveIpl("prologue03");
                    fInterior.RemoveIpl("prologue04");
                    fInterior.RemoveIpl("prologue05");
                    fInterior.RemoveIpl("prologue06");
                    fInterior.RemoveIpl("prologuerd");
                    fInterior.RemoveIpl("Prologue01c");
                    fInterior.RemoveIpl("Prologue01d");
                    fInterior.RemoveIpl("Prologue01e");
                    fInterior.RemoveIpl("Prologue01f");
                    fInterior.RemoveIpl("Prologue01g");
                    fInterior.RemoveIpl("prologue01h");
                    fInterior.RemoveIpl("prologue01i");
                    fInterior.RemoveIpl("prologue01j");
                    fInterior.RemoveIpl("prologue01k");
                    fInterior.RemoveIpl("prologue01z");
                    fInterior.RemoveIpl("prologue03b");
                    fInterior.RemoveIpl("prologue04b");
                    fInterior.RemoveIpl("prologue05b");
                    fInterior.RemoveIpl("prologue06b");
                    fInterior.RemoveIpl("prologuerdb");
                    fInterior.RemoveIpl("DES_ProTree_start");
                    fInterior.RemoveIpl("prologue_grv_torch");
                    fInterior.RemoveIpl("prologue03_grv_dug");
                    fInterior.RemoveIpl("DES_ProTree_start_lod");
                    fInterior.RemoveIpl("prologue04_cover");
                    fInterior.RemoveIpl("prologue03_grv_fun");
                    fInterior.RemoveIpl("prologue03_grv_cov");
                    fInterior.RemoveIpl("prologue_LODLights");
                    fInterior.RemoveIpl("prologue_DistantLights");
                    int zone = fZone.GetZoneFromNameID("PrLog");
                    fZone.SetZoneEnabled(zone, false);
                    fHud.ToggleNorthYanktonMap(false);
                    fPathfind.SetAllowStreamPrologueNodes(false);
                    fPathfind.SetRoadsInAngledArea(new Vector3(5526.24f, -5137.23f, 61.78925f), new Vector3(3679.327f, -4973.879f, 125.0828f), 192.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3691.211f, -4941.24f, 94.59368f), new Vector3(3511.115f, -4689.191f, 126.7621f), 16.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3510.004f, -4865.81f, 94.69557f), new Vector3(3204.424f, -4833.8147f, 126.8152f), 16.0f, false, false, true);
                    fPathfind.SetRoadsInAngledArea(new Vector3(3186.534f, -4832.798f, 109.8148f), new Vector3(3204.187f, -4833.993f, 114.815f), 16.0f, false, false, true);
                    YankTon = false;
                }
            }

            public static void EnableNorthYanktonTrainTracks(bool enabled)
            {
                if (enabled)
                {
                    fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonPrologueMissionTrain, true);
                    fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonTrain, true);
                    return;
                }
                fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonPrologueMissionTrain, false);
                fVehicle.SwitchTrainTrack(fVehicle.TrainTracks.YanktonTrain, false);
            }
        }

        public static bool IsIplActive(string iplName)
        {
            return Function.Call<bool>(Hash.IS_​IPL_​ACTIVE, iplName);
        }

        public static void RequestIpl(string iplName)
        {
            Function.Call(Hash.REQUEST_IPL, iplName);
        }

        public static void RemoveIpl(string iplName)
        {
            Function.Call(Hash.REMOVE_IPL, iplName);
        }
    }
}
