using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using GTA;
    using System;
    using System.Drawing;

    /// <summary>
    /// Timerbar with countdown timer (mm:ss format)
    /// </summary>
    public class HudCountdownBar : HudBar
    {
        private readonly int _duration;
        private int _endTime;
        private bool _finished;

        public Action OnFinished { get; set; }
        public Color TimeColor { get; set; } = Color.White;

        public int RemainingTime => Math.Max(0, _endTime - Game.GameTime);
        public bool IsFinished => _finished;

        public HudCountdownBar(string label, int durationMs)
            : base(label, thin: false)
        {
            _duration = durationMs;
            _endTime = Game.GameTime + durationMs;
        }

        public void Restart()
        {
            _finished = false;
            _endTime = Game.GameTime + _duration;
        }

        public override void Draw(float y)
        {
            base.Draw(y);

            y -= 0.0113f;

            int remaining = RemainingTime;

            if (remaining <= 0 && !_finished)
            {
                _finished = true;
                OnFinished?.Invoke();
                HudBarController.Unregister(this);
            }

            TimeSpan time = TimeSpan.FromMilliseconds(remaining);

            // Calculate proper X position
            float textX = HudBarDraw.BaseX + (HudBarDraw.Width / 2f) - 0.008f;

            HudBarDraw.DrawText(
                time.ToString("mm\\:ss"),
                textX,
                y + 0.001f,
                0,
                0.494f,
                TimeColor,
                2,
                textX);
        }
    }
}
