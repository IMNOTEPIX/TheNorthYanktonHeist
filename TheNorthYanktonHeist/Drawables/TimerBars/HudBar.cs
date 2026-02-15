using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using System.Drawing;
    using TheNorthYanktonHeist.Funcs;

    /// <summary>
    /// Base class for all HUD timerbars
    /// </summary>
    public abstract class HudBar
    {
        public string Label { get; set; }
        public bool Thin { get; protected set; }

        public Color LabelColor { get; set; } = Color.White;
        public Color? Highlight { get; set; }
        public Color? Accent { get; set; }

        public float VerticalSpacing => Thin ? HudBarDraw.ThinMargin : HudBarDraw.Margin;

        protected HudBar(string label, bool thin = false)
        {
            Label = label ?? string.Empty;
            Thin = thin;
        }

        // ✅ Changed from abstract to virtual so base.Draw() works
        public virtual void Draw(float y)
        {
            float offset = Thin ? 0.012f : 0.008f;
            y += offset;

            float height = Thin ? HudBarDraw.ThinHeight : HudBarDraw.Height;

            // Draw black background
            fGraphics.DrawSprite("timerbars", "all_black_bg",
                HudBarDraw.BaseX, y,
                HudBarDraw.Width, height,
                0, 255, 255, 255, 140);

            // Draw highlight overlay if set
            if (Highlight.HasValue)
            {
                fGraphics.DrawSprite("timerbars", "all_white_bg",
                    HudBarDraw.BaseX, y,
                    HudBarDraw.Width, height,
                    0,
                    Highlight.Value.R,
                    Highlight.Value.G,
                    Highlight.Value.B,
                    140);
            }

            // Draw accent line on right edge if set
            if (Accent.HasValue)
            {
                float accentX = HudBarDraw.BaseX + (HudBarDraw.Width / 2f) - 0.001f;
                fGraphics.DrawRect(
                    accentX, y,
                    0.002f, height,
                    Accent.Value.R,
                    Accent.Value.G,
                    Accent.Value.B,
                    255,
                    false);
            }

            // Draw label text
            if (!string.IsNullOrEmpty(Label))
            {
                HudBarDraw.DrawText(
                    Label,
                    HudBarDraw.TitleX,
                    y - 0.011f,
                    0,
                    0.302f,
                    LabelColor,
                    1,  // Left justified
                    0.867f);
            }
        }
    }
}
