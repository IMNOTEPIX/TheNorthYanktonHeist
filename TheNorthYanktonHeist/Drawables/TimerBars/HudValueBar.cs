using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using GTA.Native;
    using System.Drawing;
    using TheNorthYanktonHeist.Funcs;

    /// <summary>
    /// Timerbar that displays a numeric value (fixed X positions)
    /// </summary>
    public class HudValueBar : HudBar
    {
        public int Value { get; set; }
        public bool IsMoney { get; set; }
        public Color ValueColor { get; set; } = Color.White;

        public HudValueBar(string label, bool isMoney = false, bool thin = false)
            : base(label, thin)
        {
            IsMoney = isMoney;
        }

        public override void Draw(float y)
        {
            // Format value
            string valueText = IsMoney ? $"${Value:N0}" : Value.ToString("N0");
            int length = Value.ToString().Length;

            // Get config based on value length
            var config = GetConfigForLength(length);

            // Apply vertical offset
            float offset = Thin ? 0.012f : 0.008f;
            y += offset;

            // Request texture dict
            fGraphics.RequestStreamedTextureDict("timerbars");
            if (!fGraphics.HasStreamedTextureDictLoaded("timerbars"))
                return;

            // ✅ FIX: Use HudBarDraw.BaseX as center reference (works with alignment)
            float baseX = HudBarDraw.BaseX;

            // Draw bar background
            fGraphics.DrawSprite("timerbars", "all_black_bg",
                baseX, y,  // ✅ Use aligned position
                config.Width, config.Height,
                0, 255, 255, 255, 140);

            // Draw highlight if set
            if (Highlight.HasValue)
            {
                fGraphics.DrawSprite("timerbars", "all_white_bg",
                    baseX, y,
                    config.Width, config.Height,
                    0, Highlight.Value.R, Highlight.Value.G, Highlight.Value.B, 140);
            }

            // Draw accent if set
            if (Accent.HasValue)
            {
                float accentX = baseX + (config.Width / 2f) - 0.001f;
                fGraphics.DrawRect(accentX, y,
                    0.002f, config.Height,
                    Accent.Value.R, Accent.Value.G, Accent.Value.B, 255, false);
            }

            // Calculate text positions relative to bar center
            float labelY = y - 0.010f;
            float valueY = y - 0.0165f;

            // ✅ FIX: Calculate X positions relative to bar center
            float labelX = baseX - (config.Width / 2f) + 0.030f;
            float valueX = baseX + (config.Width / 2f) - 0.008f;
            float wrapEnd = valueX + 0.005f;
            float wrapStart = labelX + 0.05f;

            // Draw label
            if (!string.IsNullOrEmpty(Label))
            {
                Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Label);
                Function.Call(Hash.SET_TEXT_COLOUR, LabelColor.R, LabelColor.G, LabelColor.B, LabelColor.A);
                Function.Call(Hash.SET_TEXT_SCALE, 0f, 0.32f);
                Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, labelX, labelY, 0);
            }

            // Draw value
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, valueText);
            Function.Call(Hash.SET_TEXT_COLOUR, ValueColor.R, ValueColor.G, ValueColor.B, ValueColor.A);
            Function.Call(Hash.SET_TEXT_JUSTIFICATION, 2);
            Function.Call(Hash.SET_TEXT_WRAP, wrapStart, wrapEnd);
            Function.Call(Hash.SET_TEXT_SCALE, 0f, config.ValueScale);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, valueX, valueY, 0);
        }

        /// <summary>
        /// Gets bar configuration based on the number of digits in the value.
        /// As the number gets bigger, the bar gets wider and text gets smaller to fit properly.
        /// </summary>
        /// <param name="length">Number of digits in the value (e.g., 1234567 = 7 digits)</param>
        /// <returns>Configuration with width, height, and text scale for that value size</returns>
        private BarConfig GetConfigForLength(int length)
        {
            // Small numbers (up to 999,999) - Normal size bar
            if (length < 7)
            {
                return new BarConfig
                {
                    Width = 0.165f,      // Standard bar width
                    Height = 0.035f,     // Standard bar height
                    ValueScale = 0.483f  // Normal text size
                };
            }
            // 7 digits (1,000,000 to 9,999,999) - Slightly wider, slightly smaller text
            else if (length == 7)
            {
                return new BarConfig
                {
                    Width = 0.175f,      // Bar 10% wider
                    Height = 0.034f,     // Slightly thinner
                    ValueScale = 0.45f   // Text slightly smaller
                };
            }
            
            // 8 digits (10,000,000 to 99,999,999) - Even wider, smaller text
            else //(length == 8)
            {
                return new BarConfig
                {
                    Width = 0.185f,      // Bar even wider
                    Height = 0.0325f,    // Thinner height
                    ValueScale = 0.43f   // Text smaller to fit
                };
            }
            /*
            // 9 digits (100,000,000 to 999,999,999) - Wider still
            else if (length == 9)
            {
                return new BarConfig
                {
                    Width = 0.19f,       // Bar wider
                    Height = 0.0325f,    // Thin height
                    ValueScale = 0.4275f // Text even smaller
                };
            }
            // 10+ digits (1,000,000,000+) - Widest bar, smallest text
            else
            {
                return new BarConfig
                {
                    Width = 0.195f,      // Maximum bar width
                    Height = 0.0325f,    // Thin height
                    ValueScale = 0.4275f // Smallest text scale
                };
            }*/
        }

        private struct BarConfig
        {
            public float Width;
            public float Height;
            public float ValueScale;
        }
    }
}
