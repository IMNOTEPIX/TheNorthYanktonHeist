using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheNorthYanktonHeist.Funcs
{
    public class fDebug
    {
        public static int DebugSceneID = 0;
        public static int DebugSceneID2 = 0;
        public static List<Vehicle> DebugVehicles = new List<Vehicle>();
        public static List<string> DebugAnimDicts = new List<string>();
        public static List<Prop> DebugProps = new List<Prop>();

        [STAThread]
        public static void CopyPlayerPosWithAddons()
        {
            Vector3 GetPlayerPosition()
            {
                Vector3 PlayerPos = new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);
                return PlayerPos;
            }
            Vector3 vector = GetPlayerPosition();
            string vectorX = vector.X.ToString();
            string vectorY = vector.Y.ToString();
            string vectorZ = vector.Z.ToString();
            string XYZ = vector.ToString().Replace("X:", string.Empty).Replace("Y:", string.Empty).Replace("Z:", string.Empty);
            string Text2 = XYZ.Replace(vectorX, vectorX + "f,");
            string Text3 = Text2.Replace(vectorY, vectorY + "f,");
            string Final = Text3.Replace(vectorZ, vectorZ + "f");
            GTA.UI.Screen.ShowSubtitle("~p~Copied~s~: to clipboard!");
            Thread thread = new Thread((ThreadStart)(() => Clipboard.SetText(Final)));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        [STAThread]
        public static void CopyToClipboard(string text)
        {
            GTA.UI.Screen.ShowSubtitle("~p~Copied~s~: to clipboard!");
            Thread thread = new Thread((ThreadStart)(() => Clipboard.SetText(text)));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        [STAThread]
        public static void CopyPedVariationToClipboard(Ped ped)
        {
            var data = fPed.GetPedVariationData(ped);
            if (data == null) return;

            StringBuilder sb = new StringBuilder();

            foreach (var kvp in data)
            {
                int compId = kvp.Key;
                int drawable = kvp.Value.Item1;
                int texture = kvp.Value.Item2;

                sb.AppendLine($"{compId}:{drawable},{texture}");
            }

            CopyToClipboard(sb.ToString());
        }
        [STAThread]
        public static void CopyPedPropDataToClipboard(Ped ped)
        {
            var data = fPed.GetPedPropData(ped);
            if (data == null) return;

            StringBuilder sb = new StringBuilder();

            foreach (var kvp in data)
            {
                int compId = kvp.Key;
                int drawable = kvp.Value.Item1;
                int texture = kvp.Value.Item2;

                sb.AppendLine($"{compId}:{drawable},{texture}");
            }

            CopyToClipboard(sb.ToString());
        }
    }
}
