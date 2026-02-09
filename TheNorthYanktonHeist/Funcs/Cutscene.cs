using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheNorthYanktonHeist.Funcs.RegisterEntityChunk;

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
            int result;
            if (Function.Call<bool>(Hash.HAS_CUT_FILE_LOADED, cutsceneName))
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
        public static void GetCutscenePedOutfitCutscene(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_1"))
            {
                CutscenePed1VariationData = new Dictionary<int, PedVariationData>
            {
                { 0, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0)) },
                { 1, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1)) },
                { 2, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2)) },
                { 3, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3)) },
                { 4, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4)) },
                { 5, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5)) },
                { 6, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6)) },
                { 7, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7)) },
                { 8, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8)) },
                { 9, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9)) },
                { 10, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10)) },
                { 11, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11)) },
            };
            }
        }

        public static void SetCutscenePedOutfit(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_1"))
            {
                foreach (var data in CutscenePed1VariationData)
                {
                    int componentId = data.Key;
                    int drawable = data.Value.Drawable;
                    int texture = data.Value.Texture;

                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, componentId, drawable, texture, 1);
                }
            }
        }

        public static void GetCutscenePedOutfitCutscene_MP2(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_2"))
            {
                CutscenePed2VariationData = new Dictionary<int, PedVariationData>
            {
                { 0, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0)) },
                { 1, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1)) },
                { 2, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2)) },
                { 3, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3)) },
                { 4, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4)) },
                { 5, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5)) },
                { 6, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6)) },
                { 7, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7)) },
                { 8, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8)) },
                { 9, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9)) },
                { 10, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10)) },
                { 11, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11)) },
            };
            }
        }

        public static void SetCutscenePedOutfit_MP2(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_2"))
            {
                foreach (var data in CutscenePed2VariationData)
                {
                    int componentId = data.Key;
                    int drawable = data.Value.Drawable;
                    int texture = data.Value.Texture;

                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, componentId, drawable, texture, 1);
                }
            }
        }

        public static void GetCutscenePedOutfitCutscene_MP3(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_3"))
            {
                CutscenePed3VariationData = new Dictionary<int, PedVariationData>
            {
                { 0, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0)) },
                { 1, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1)) },
                { 2, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2)) },
                { 3, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3)) },
                { 4, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4)) },
                { 5, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5)) },
                { 6, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6)) },
                { 7, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7)) },
                { 8, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8)) },
                { 9, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9)) },
                { 10, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10)) },
                { 11, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11)) },
            };
            }
        }

        public static void SetCutscenePedOutfit_MP3(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_3"))
            {
                foreach (var data in CutscenePed3VariationData)
                {
                    int componentId = data.Key;
                    int drawable = data.Value.Drawable;
                    int texture = data.Value.Texture;

                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, componentId, drawable, texture, 1);
                }
            }
        }

        public static void GetCutscenePedOutfitCutscene_MP4(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_4"))
            {
                CutscenePed4VariationData = new Dictionary<int, PedVariationData>
            {
                { 0, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0)) },
                { 1, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1)) },
                { 2, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2)) },
                { 3, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3)) },
                { 4, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4)) },
                { 5, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5)) },
                { 6, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6)) },
                { 7, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7)) },
                { 8, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8)) },
                { 9, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9)) },
                { 10, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10)) },
                { 11, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11)) },
            };
            }
        }

        public static void SetCutscenePedOutfit_MP4(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_4"))
            {
                foreach (var data in CutscenePed4VariationData)
                {
                    int componentId = data.Key;
                    int drawable = data.Value.Drawable;
                    int texture = data.Value.Texture;

                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, componentId, drawable, texture, 1);
                }
            }
        }

        public static void GetCutscenePedOutfitCutscene_MP5(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_5"))
            {
                CutscenePed5VariationData = new Dictionary<int, PedVariationData>
            {
                { 0, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0)) },
                { 1, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1)) },
                { 2, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2)) },
                { 3, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3)) },
                { 4, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4)) },
                { 5, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5)) },
                { 6, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6)) },
                { 7, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7)) },
                { 8, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8)) },
                { 9, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9)) },
                { 10, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10)) },
                { 11, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11)) },
            };
            }
        }

        public static void SetCutscenePedOutfit_MP5(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_5"))
            {
                foreach (var data in CutscenePed5VariationData)
                {
                    int componentId = data.Key;
                    int drawable = data.Value.Drawable;
                    int texture = data.Value.Texture;

                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, componentId, drawable, texture, 1);
                }
            }
        }

        public static void GetCutscenePedOutfitCutscene_MP6(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_6"))
            {
                CutscenePed6VariationData = new Dictionary<int, PedVariationData>
            {
                { 0, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0)) },
                { 1, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1)) },
                { 2, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2)) },
                { 3, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3)) },
                { 4, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4)) },
                { 5, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5)) },
                { 6, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6)) },
                { 7, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7)) },
                { 8, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8)) },
                { 9, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9)) },
                { 10, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10)) },
                { 11, new PedVariationData(Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11)) },
            };
            }
        }

        public static void SetCutscenePedOutfit_MP6(string MP, Ped NonCutscene)
        {
            if (MP.Equals("MP_6"))
            {
                foreach (var data in CutscenePed6VariationData)
                {
                    int componentId = data.Key;
                    int drawable = data.Value.Drawable;
                    int texture = data.Value.Texture;

                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, componentId, drawable, texture, 1);
                }
            }
        }

        private static Dictionary<int, PedVariationData> CutscenePed1VariationData =
new Dictionary<int, PedVariationData>
{
                { 0, new PedVariationData(0, 0) },
                { 1, new PedVariationData(0, 0) },
                { 2, new PedVariationData(0, 0) },
                { 3, new PedVariationData(0, 0) },
                { 4, new PedVariationData(0, 0) },
                { 5, new PedVariationData(0, 0) },
                { 6, new PedVariationData(0, 0) },
                { 7, new PedVariationData(0, 0) },
                { 8, new PedVariationData(0, 0) },
                { 9, new PedVariationData(0, 0) },
                { 10, new PedVariationData(0, 0) },
                { 11, new PedVariationData(0, 0) },
};
        private static Dictionary<int, PedVariationData> CutscenePed2VariationData =
new Dictionary<int, PedVariationData>
{
                { 0, new PedVariationData(0, 0) },
                { 1, new PedVariationData(0, 0) },
                { 2, new PedVariationData(0, 0) },
                { 3, new PedVariationData(0, 0) },
                { 4, new PedVariationData(0, 0) },
                { 5, new PedVariationData(0, 0) },
                { 6, new PedVariationData(0, 0) },
                { 7, new PedVariationData(0, 0) },
                { 8, new PedVariationData(0, 0) },
                { 9, new PedVariationData(0, 0) },
                { 10, new PedVariationData(0, 0) },
                { 11, new PedVariationData(0, 0) },
};
        private static Dictionary<int, PedVariationData> CutscenePed3VariationData =
new Dictionary<int, PedVariationData>
{
                { 0, new PedVariationData(0, 0) },
                { 1, new PedVariationData(0, 0) },
                { 2, new PedVariationData(0, 0) },
                { 3, new PedVariationData(0, 0) },
                { 4, new PedVariationData(0, 0) },
                { 5, new PedVariationData(0, 0) },
                { 6, new PedVariationData(0, 0) },
                { 7, new PedVariationData(0, 0) },
                { 8, new PedVariationData(0, 0) },
                { 9, new PedVariationData(0, 0) },
                { 10, new PedVariationData(0, 0) },
                { 11, new PedVariationData(0, 0) },
};
        private static Dictionary<int, PedVariationData> CutscenePed4VariationData =
new Dictionary<int, PedVariationData>
{
                { 0, new PedVariationData(0, 0) },
                { 1, new PedVariationData(0, 0) },
                { 2, new PedVariationData(0, 0) },
                { 3, new PedVariationData(0, 0) },
                { 4, new PedVariationData(0, 0) },
                { 5, new PedVariationData(0, 0) },
                { 6, new PedVariationData(0, 0) },
                { 7, new PedVariationData(0, 0) },
                { 8, new PedVariationData(0, 0) },
                { 9, new PedVariationData(0, 0) },
                { 10, new PedVariationData(0, 0) },
                { 11, new PedVariationData(0, 0) },
};
        private static Dictionary<int, PedVariationData> CutscenePed5VariationData =
new Dictionary<int, PedVariationData>
{
                { 0, new PedVariationData(0, 0) },
                { 1, new PedVariationData(0, 0) },
                { 2, new PedVariationData(0, 0) },
                { 3, new PedVariationData(0, 0) },
                { 4, new PedVariationData(0, 0) },
                { 5, new PedVariationData(0, 0) },
                { 6, new PedVariationData(0, 0) },
                { 7, new PedVariationData(0, 0) },
                { 8, new PedVariationData(0, 0) },
                { 9, new PedVariationData(0, 0) },
                { 10, new PedVariationData(0, 0) },
                { 11, new PedVariationData(0, 0) },
};
        private static Dictionary<int, PedVariationData> CutscenePed6VariationData =
new Dictionary<int, PedVariationData>
{
                { 0, new PedVariationData(0, 0) },
                { 1, new PedVariationData(0, 0) },
                { 2, new PedVariationData(0, 0) },
                { 3, new PedVariationData(0, 0) },
                { 4, new PedVariationData(0, 0) },
                { 5, new PedVariationData(0, 0) },
                { 6, new PedVariationData(0, 0) },
                { 7, new PedVariationData(0, 0) },
                { 8, new PedVariationData(0, 0) },
                { 9, new PedVariationData(0, 0) },
                { 10, new PedVariationData(0, 0) },
                { 11, new PedVariationData(0, 0) },
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
