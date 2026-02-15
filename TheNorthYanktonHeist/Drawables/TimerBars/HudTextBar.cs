using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using System.Drawing;

    /// <summary>
    /// Timerbar that displays text on the right side
    /// </summary>
    public class HudTextBar : HudBar
    {
        public string Text { get; set; }
        public Color TextColor { get; set; } = Color.White;

        public HudTextBar(string label, string text = "")
            : base(label, thin: false)
        {
            Text = text ?? string.Empty;
        }

        public override void Draw(float y)
        {
            base.Draw(y);

            if (string.IsNullOrEmpty(Text))
                return;

            y -= 0.0113f;

            // Calculate proper X position
            float textX = HudBarDraw.BaseX + (HudBarDraw.Width / 2f) - 0.008f;

            HudBarDraw.DrawText(
                Text,
                textX,
                y + 0.001f,
                0,
                0.494f,
                TextColor,
                2,  // Right justified
                textX);
        }
    }
}
