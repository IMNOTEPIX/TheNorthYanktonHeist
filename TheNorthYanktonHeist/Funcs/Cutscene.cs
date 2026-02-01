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
    public class fCutscene
    {
        public static int GetCutsceneTotalDuration()
        {
            return Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION);
        }
        public static string LoadCutsceneWithFlag(string cutscene, int playbackflag)
        {
            while (!Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED, cutscene))
            {
                Function.Call(Hash.REQUEST_CUTSCENE_WITH_PLAYBACK_LIST, cutscene, playbackflag, 8);
                Script.Yield();
            }
            return cutscene;
        }

        public static string LoadCutscene(string cutscene)
        {
            while (!Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED, cutscene))
            {
                Function.Call(Hash.REQUEST_CUTSCENE, cutscene, 8);
                Script.Yield();
            }
            return cutscene;
        }

        public static string LoadCutfile(string cutscene)
        {
            while (!Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutscene))
            {
                Function.Call(Hash.REQUEST_CUT_FILE, cutscene);
                Script.Yield();
            }
            return cutscene;
        }

        public static string RemoveCutfile(string cutscene)
        {
            while (Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutscene))
            {
                Function.Call(Hash.REMOVE_CUT_FILE, cutscene);
                Script.Yield();
            }
            return cutscene;
        }

        public static int GetNumOfConcats(string cutsceneName)
        {
            Function.Call(Hash.REQUEST_CUT_FILE, cutsceneName);
            bool flag = Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutsceneName);
            int result;
            if (flag)
            {
                int num = Function.Call<int>(Hash.GET_CUT_FILE_CONCAT_COUNT, cutsceneName);
                Function.Call(Hash.REMOVE_CUT_FILE, cutsceneName);
                result = num;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public static int GetCutsceneTime()
        {
            return Function.Call<int>(Hash.GET_CUTSCENE_TIME);
        }

        public static bool HasCutsceneFinished()
        {
            return Function.Call<bool>(Hash.HAS_CUTSCENE_FINISHED);
        }

        public static bool HasCutsceneLoaded()
        {
            return Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED);
        }

        public static bool IsCutscenePlaying()
        {
            return Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING);
        }

        public static void StopCutsceneImmediately()
        {
            Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
        }

        public static void SetCutsceneCanBeSkipped(bool toggle)
        {
            Function.Call(Hash.SET_CUTSCENE_CAN_BE_SKIPPED, toggle);
        }

        public static void RegisterEntityForCutscene(Entity entity, string cutsceneEntityName, int cutsceneUsage = 2, int modelname = 0, int cutsceneEntityOptionFlag = 0)
        {
            int entityHandle = entity.Exists() ? entity.Handle : 0;
            Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, entityHandle, cutsceneEntityName, cutsceneUsage, modelname, cutsceneEntityOptionFlag);
        }

        public static void RegisterEntityForCutscene(int entity, string cutsceneEntityName, int cutsceneUsage = 2, int modelname = 0, int cutsceneEntityOptionFlag = 0)
        {
            Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, entity, cutsceneEntityName, cutsceneUsage, modelname, cutsceneEntityOptionFlag);
        }

        public static void SetCutsceneOrigin(Vector3 cutscenePos, float cutsceneHeading)
        {
            Function.Call(Hash.SET_CUTSCENE_ORIGIN, cutscenePos.X, cutscenePos.Y, cutscenePos.Z, cutsceneHeading, 0);
        }

        public static void SetCutsceneOriginAndRotation(string cutsceneName, Vector3 position, Vector3 rotation)
        {
            int numOfConcats = GetNumOfConcats(cutsceneName);
            for (int i = 0; i < numOfConcats; i++)
            {
                Function.Call(Hash.SET_CUTSCENE_ORIGIN_AND_ORIENTATION, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, i);
            }
        }

        public static void StartCutscene(int flag = 0)
        {
            Function.Call(Hash.START_CUTSCENE, flag);
        }

        public static void StartCutscene(CutscenePlaybackFlags flag = (CutscenePlaybackFlags)0)
        {
            Function.Call(Hash.START_CUTSCENE, flag);
        }

        public static void RemoveCutscene()
        {
            Function.Call(Hash.REMOVE_CUTSCENE);
        }

        public static void StopCutsceneAudio()
        {
            Function.Call(Hash.STOP_CUTSCENE_AUDIO, 0);
        }

        public static void SetCutsceneFadeValues(bool fadeOutAtStart = false, bool fadeInAtStart = false, bool fadeOutAtEnd = false, bool fadeInAtEnd = false)
        {
            Function.Call(Hash.SET_CUTSCENE_FADE_VALUES, fadeOutAtStart, fadeInAtStart, fadeOutAtEnd, fadeInAtEnd);
        }

        #region PedOutfitCutscene
        public static void SetPedOutfitCutscene(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_1");
            if (flag)
            {
                CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            }
        }

        public static void GetPedOutfitCutscene(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_1");
            if (flag)
            {
                string[] array = CutscenePed1Comp[0].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[1].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[2].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[3].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[4].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[5].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[6].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[7].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[8].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[9].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[10].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed1Comp[11].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            }
        }

        public static void SetPedOutfitCutscene_MP2(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_2");
            if (flag)
            {
                CutscenePed2Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                CutscenePed2Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                CutscenePed2Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                CutscenePed2Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                CutscenePed2Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                CutscenePed2Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                CutscenePed2Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                CutscenePed2Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                CutscenePed2Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                CutscenePed2Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                CutscenePed2Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                CutscenePed2Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            }
        }

        public static void GetPedOutfitCutscene_MP2(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_2");
            if (flag)
            {
                string[] array = CutscenePed2Comp[0].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[1].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[2].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[3].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[4].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[5].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[6].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[7].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[8].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[9].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[10].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed2Comp[11].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            }
        }

        public static void SetPedOutfitCutscene_MP3(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_3");
            if (flag)
            {
                CutscenePed3Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                CutscenePed3Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                CutscenePed3Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                CutscenePed3Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                CutscenePed3Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                CutscenePed3Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                CutscenePed3Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                CutscenePed3Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                CutscenePed3Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                CutscenePed3Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                CutscenePed3Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                CutscenePed3Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            }
        }

        public static void GetPedOutfitCutscene_MP3(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_3");
            if (flag)
            {
                string[] array = CutscenePed3Comp[0].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[1].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[2].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[3].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[4].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[5].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[6].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[7].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[8].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[9].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[10].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed3Comp[11].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            }
        }

        public static void SetPedOutfitCutscene_MP4(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_4");
            if (flag)
            {
                CutscenePed4Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                CutscenePed4Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                CutscenePed4Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                CutscenePed4Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                CutscenePed4Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                CutscenePed4Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                CutscenePed4Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                CutscenePed4Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                CutscenePed4Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                CutscenePed4Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                CutscenePed4Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                CutscenePed4Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            }
        }

        public static void GetPedOutfitCutscene_MP4(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_4");
            if (flag)
            {
                string[] array = CutscenePed4Comp[0].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[1].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[2].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[3].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[4].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[5].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[6].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[7].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[8].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[9].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[10].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed4Comp[11].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            }
        }

        public static void SetPedOutfitCutscene_MP5(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_5");
            if (flag)
            {
                CutscenePed5Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                CutscenePed5Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                CutscenePed5Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                CutscenePed5Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                CutscenePed5Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                CutscenePed5Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                CutscenePed5Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                CutscenePed5Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                CutscenePed5Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                CutscenePed5Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                CutscenePed5Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                CutscenePed5Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            }
        }

        public static void GetPedOutfitCutscene_MP5(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_5");
            if (flag)
            {
                string[] array = CutscenePed5Comp[0].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[1].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[2].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[3].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[4].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[5].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[6].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[7].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[8].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[9].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[10].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed5Comp[11].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            }
        }

        public static void SetPedOutfitCutscene_MP6(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_6");
            if (flag)
            {
                CutscenePed6Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
                CutscenePed6Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
                CutscenePed6Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
                CutscenePed6Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
                CutscenePed6Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
                CutscenePed6Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
                CutscenePed6Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
                CutscenePed6Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
                CutscenePed6Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
                CutscenePed6Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
                CutscenePed6Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
                CutscenePed6Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            }
        }

        public static void GetPedOutfitCutscene_MP6(string MP, Ped NonCutscene)
        {
            bool flag = MP.Equals("MP_6");
            if (flag)
            {
                string[] array = CutscenePed6Comp[0].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[1].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[2].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[3].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[4].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[5].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[6].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[7].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[8].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[9].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[10].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
                array = CutscenePed6Comp[11].Split(new char[]
                {
                    '_'
                });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            }
        }

        public static List<string> CutscenePed1Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

        public static List<string> CutscenePed2Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

        public static List<string> CutscenePed3Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

        public static List<string> CutscenePed4Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

        public static List<string> CutscenePed5Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };

        public static List<string> CutscenePed6Comp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "3_0_0",
            "4_0_0",
            "5_0_0",
            "6_0_0",
            "7_0_0",
            "8_0_0",
            "9_0_0",
            "10_0_0",
            "11_0_0"
        };
        #endregion

        public enum CutsceneUsage
        {
            CU_ANIMATE_EXISTING_SCRIPT_ENTITY,
            CU_ANIMATE_AND_DELETE_EXISTING_SCRIPT_ENTITY,
            CU_CREATE_AND_ANIMATE_NEW_SCRIPT_ENTITY,
            CU_DONT_ANIMATE_ENTITY
        }

        public enum CutsceneEntityOptionFlag
        {
            CEO_NONE,
            CEO_PRESERVE_FACE_BLOOD_DAMAGE,
            CEO_PRESERVE_BODY_BLOOD_DAMAGE,
            CEO_REMOVE_BODY_BLOOD_DAMAGE = 4,
            CEO_CLONE_DAMAGE_TO_CS_MODEL = 8,
            CEO_RESET_CAPSULE_AT_END = 16,
            CEO_IS_CASCADE_SHADOW_FOCUS_ENTITY_DURING_EXIT = 32,
            CEO_IGNORE_MODEL_NAME = 64,
            CEO_PRESERVE_HAIR_SCALE = 128,
            CEO_INSTANT_HAIR_SCALE_SETUP = 256,
            CEO_DONT_RESET_PED_CAPSULE = 512,
            CEO_UPDATE_AS_REAL_DOOR = 1024
        }

        public enum CutsceneSection
        {
            CS_SECTION_1 = 1,
            CS_SECTION_2,
            CS_SECTION_3 = 4,
            CS_SECTION_4 = 8,
            CS_SECTION_5 = 16,
            CS_SECTION_6 = 32,
            CS_SECTION_7 = 64,
            CS_SECTION_8 = 128,
            CS_SECTION_9 = 256,
            CS_SECTION_10 = 512,
            CS_SECTION_11 = 1024,
            CS_SECTION_12 = 2048,
            CS_SECTION_13 = 4096,
            CS_SECTION_14 = 8192,
            CS_SECTION_15 = 16384,
            CS_SECTION_16 = 32768,
            CS_SECTION_17 = 65536,
            CS_SECTION_18 = 131072,
            CS_SECTION_19 = 262144,
            CS_SECTION_20 = 524288,
            CS_SECTION_21 = 1048576,
            CS_SECTION_22 = 2097152,
            CS_SECTION_23 = 4194304,
            CS_SECTION_24 = 8388608,
            CS_SECTION_25 = 16777216,
            CS_SECTION_26 = 33554432,
            CS_SECTION_27 = 67108864,
            CS_SECTION_28 = 134217728,
            CS_SECTION_29 = 268435456,
            CS_SECTION_30 = 536870912,
            CS_SECTION_31 = 1073741824
        }

        public enum CutscenePlaybackFlags
        {
            CUTSCENE_REQUESTED_FROM_WIDGET = 1,
            CUTSCENE_REQUESTED_DIRECTLY_FROM_SKIP,
            CUTSCENE_REQUESTED_FROM_Z_SKIP = 4,
            CUTSCENE_REQUESTED_IN_MISSION = 8,
            CUTSCENE_PLAYBACK_FORCE_LOAD_AUDIO_EVENT = 16
        }
    }
}
