using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using System.Drawing;

    /// <summary>
    /// Timerbar styled like GTA Online player names
    /// </summary>
    public class HudPlayerBar : HudBar
    {
        public string PlayerName { get; set; }
        public string RightText { get; set; }
        public Color RightTextColor { get; set; } = Color.White;

        public HudPlayerBar(string playerName, string rightText = "")
            : base("", thin: false)
        {
            PlayerName = playerName ?? string.Empty;
            RightText = rightText ?? string.Empty;
        }

        public override void Draw(float y)
        {
            // Draw base without label
            base.Draw(y);

            y -= 0.0113f;

            // Draw player name on left (styled differently)
            if (!string.IsNullOrEmpty(PlayerName))
            {
                HudBarDraw.DrawText(
                    PlayerName,
                    HudBarDraw.TitleX,
                    y + 0.001f,
                    0,
                    0.494f,
                    Color.White,
                    1,
                    0.867f);
            }

            // Draw right text
            if (!string.IsNullOrEmpty(RightText))
            {
                float textX = HudBarDraw.BaseX + (HudBarDraw.Width / 2f) - 0.008f;

                HudBarDraw.DrawText(
                    RightText,
                    textX,
                    y + 0.001f,
                    0,
                    0.494f,
                    RightTextColor,
                    2,
                    textX);
            }
        }
    }
}
