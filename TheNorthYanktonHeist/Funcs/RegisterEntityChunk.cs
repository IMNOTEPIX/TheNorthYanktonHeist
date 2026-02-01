using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class RegisterEntityChunk
    {
        public RegisterEntityChunk(Entity ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
        {
            this.Ent = ent;
            this.CHandle = cHandle;
            this.Usage = usage;
            this.ModelNames = modelNames;
            this.EntityOptionsFlag = entityOptionsFlag;
        }

        public RegisterEntityChunk(int ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
        {
            this.EntInt = ent;
            this.CHandle = cHandle;
            this.Usage = usage;
            this.ModelNames = modelNames;
            this.EntityOptionsFlag = entityOptionsFlag;
        }

        public void SetPedOutfitCutscene(Ped NonCutscene)
        {
            this.CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0).ToString();
            this.CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1).ToString();
            this.CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2).ToString();
            this.CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3).ToString();
            this.CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4).ToString();
            this.CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5).ToString();
            this.CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6).ToString();
            this.CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7).ToString();
            this.CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8).ToString();
            this.CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9).ToString();
            this.CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10).ToString();
            this.CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11).ToString() + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11).ToString();
            this.CutscenePedPropComp[0] = "0_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 0).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 0).ToString();
            this.CutscenePedPropComp[1] = "1_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 1).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 1).ToString();
            this.CutscenePedPropComp[2] = "2_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 2).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 2).ToString();
            this.CutscenePedPropComp[3] = "6_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 6).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 6).ToString();
            this.CutscenePedPropComp[4] = "7_" + Function.Call<int>(Hash.GET_PED_PROP_INDEX, NonCutscene, 7).ToString() + "_" + Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, NonCutscene, 7).ToString();
        }

        public void GetPedOutfitCutscene(Ped NonCutscene)
        {
            string[] array = this.CutscenePed1Comp[0].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[1].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[2].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[3].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[4].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[5].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[6].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[7].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[8].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[9].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[10].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
            array = this.CutscenePed1Comp[11].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
            string[] array2 = this.CutscenePedPropComp[0].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 0, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
            array2 = this.CutscenePedPropComp[1].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 1, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
            array2 = this.CutscenePedPropComp[2].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 2, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
            array2 = this.CutscenePedPropComp[3].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 6, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
            array2 = this.CutscenePedPropComp[4].Split(new char[]
            {
                '_'
            });
            Function.Call(Hash.SET_PED_PROP_INDEX, NonCutscene, 7, int.Parse(array2[1]), int.Parse(array2[2]), true, 0);
        }

        public Entity Ent;

        public int EntInt = -1;

        public string CHandle;

        public fCutscene.CutsceneUsage Usage;

        public int ModelNames;

        public fCutscene.CutsceneEntityOptionFlag EntityOptionsFlag;

        public List<string> CutscenePed1Comp = new List<string>
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

        public List<string> CutscenePedPropComp = new List<string>
        {
            "0_0_0",
            "1_0_0",
            "2_0_0",
            "6_0_0",
            "7_0_0"
        };
    }
}
