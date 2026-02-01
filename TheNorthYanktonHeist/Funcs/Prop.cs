using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fProp
    {
        public static void DeletePropsInList(List<Prop> propList)
        {
            if (propList.Count > 0)
            {
                for (int i = 0; i < propList.Count; i++)
                {
                    if (propList[i] != null)
                        propList[i].Delete();
                }
                propList.Clear();
            }
        }
        public static void CreatePropForList(bool noOffset, List<Prop> propList, Model model, Vector3 pos, Vector3 rot, bool dynamic, bool placeOnGround = true)
        {
            if (noOffset)
            {
                Prop prop = World.CreatePropNoOffset(model, pos, rot, dynamic);
                while (prop != null && !prop.Exists())
                {
                    Script.Wait(0);
                }
                if (!propList.Contains(prop))
                    propList.Add(prop);
            }
            else
            {
                Prop prop = World.CreateProp(model, pos, rot, dynamic, placeOnGround);
                while (prop != null && !prop.Exists())
                {
                    Script.Wait(0);
                }
                if (!propList.Contains(prop))
                    propList.Add(prop);
            }
        }
        public static Prop CreatePropNoOffset(Model model, Vector3 pos, Vector3 rot, bool dynamic)
        {
            return World.CreatePropNoOffset(model, pos, rot, dynamic);
        }
        public static Prop CreateProp(Model model, Vector3 pos, Vector3 rot, bool dynamic, bool placeOnGround)
        {
            return World.CreateProp(model, pos, rot, dynamic, placeOnGround);
        }
    }
}
