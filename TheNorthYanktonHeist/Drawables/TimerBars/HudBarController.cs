using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using GTA;
    using GTA.Native;
    using GTA.UI;
    using System;
    using System.Collections.Generic;
    using TheNorthYanktonHeist.Funcs;

    /// <summary>
    /// Manages and renders all active HUD timerbars
    /// </summary>
    public class HudBarController : Script
    {
        private static readonly List<HudBar> _activeBars = new List<HudBar>();

        public static bool ForceMoveUp { get; set; }

        public HudBarController()
        {
            Tick += HandleTick;
        }

        public static void Register(HudBar bar)
        {
            if (bar != null && !_activeBars.Contains(bar))
                _activeBars.Add(bar);
        }

        public static void Unregister(HudBar bar)
        {
            _activeBars.Remove(bar);
        }

        public static void ClearAll()
        {
            _activeBars.Clear();
        }

        public static bool IsRegistered(HudBar bar)
            => bar != null && _activeBars.Contains(bar);

        public static int Count => _activeBars.Count;

        private void HandleTick(object sender, EventArgs e)
        {
            if (!CanDraw())
                return;

            // Request texture dict
            fGraphics.RequestStreamedTextureDict("timerbars");
            if (!fGraphics.HasStreamedTextureDictLoaded("timerbars"))
                return;

            HideDefaultHud();
            DrawBars();
        }

        private static bool CanDraw()
        {
            if (_activeBars.Count == 0)
                return false;

            if (!Function.Call<bool>(Hash.IS_HUD_PREFERENCE_SWITCHED_ON))
                return false;

            if (Function.Call<bool>(Hash.IS_HUD_HIDDEN))
                return false;

            if (Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
                return false;

            if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
                return false;

            return Game.Player.Character.IsAlive;
        }

        private static void HideDefaultHud()
        {
            fHud.HideHudComponentThisFrame(6);
            fHud.HideHudComponentThisFrame(7);
            fHud.HideHudComponentThisFrame(8);
            fHud.HideHudComponentThisFrame(9);

            fGraphics.SetScriptGfxAlign(82, 66);
            fGraphics.SetScriptGfxAlignParams(0, 0, HudBarDraw.AlignWidth, HudBarDraw.AlignHeight);
        }

        private static void DrawBars()
        {
            float y = (LoadingPrompt.IsActive || ForceMoveUp)
                ? HudBarDraw.BusyY
                : HudBarDraw.DefaultY;

            foreach (var bar in _activeBars)
            {
                bar.Draw(y);
                y -= bar.VerticalSpacing;
            }

            fGraphics.ResetScriptGfxAlign();
        }
    }
}
