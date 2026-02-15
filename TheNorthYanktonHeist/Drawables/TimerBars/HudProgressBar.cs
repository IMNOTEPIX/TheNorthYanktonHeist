using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using System.Collections.Generic;
    using System.Drawing;
    using TheNorthYanktonHeist.Funcs;

    /// <summary>
    /// Marker on a progress bar
    /// </summary>
    public class ProgressBarMarker
    {
        public float Percentage { get; set; }
        public Color Color { get; set; }

        public ProgressBarMarker(float percentage, Color? color = null)
        {
            Percentage = fMath.Clamp(percentage, 0f, 1f);
            Color = color ?? Color.White;
        }
    }

    /// <summary>
    /// Timerbar with a progress bar and optional markers
    /// </summary>
    public class HudProgressBar : HudBar
    {
        private float _percentage;

        public Color BackgroundColor { get; set; } = Color.FromArgb(155, 240, 240, 240);
        public Color ForegroundColor { get; set; } = Color.FromArgb(255, 240, 240, 240);

        /// <summary>
        /// Markers to display on the progress bar
        /// </summary>
        public List<ProgressBarMarker> Markers { get; } = new List<ProgressBarMarker>();

        public float Percentage
        {
            get => _percentage;
            set => _percentage = fMath.Clamp(value, 0f, 1f);
        }

        public HudProgressBar(string label, float percentage = 0f)
            : base(label, thin: true)
        {
            Percentage = percentage;
        }

        public override void Draw(float y)
        {
            base.Draw(y);

            y += 0.012f;

            const float barWidth = 0.069f;
            const float barHeight = 0.011f;
            const float barX = 0.913f;

            // Draw background
            fGraphics.DrawRect(barX, y,
                barWidth, barHeight,
                BackgroundColor.R,
                BackgroundColor.G,
                BackgroundColor.B,
                BackgroundColor.A,
                false);

            // Draw foreground (progress)
            float fillWidth = barWidth * Percentage;
            float fillX = 0.8785f + fillWidth * 0.5f;

            fGraphics.DrawRect(fillX, y,
                fillWidth, barHeight,
                ForegroundColor.R,
                ForegroundColor.G,
                ForegroundColor.B,
                ForegroundColor.A,
                false);

            // Draw markers
            foreach (var marker in Markers)
            {
                float markerX = 0.8785f + (barWidth * marker.Percentage);
                fGraphics.DrawRect(markerX, y,
                    0.002f, barHeight,
                    marker.Color.R,
                    marker.Color.G,
                    marker.Color.B,
                    255,
                    false);
            }
        }
    }
}
