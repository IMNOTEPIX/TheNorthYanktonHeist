using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Drawables.TimerBars
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using TheNorthYanktonHeist.Funcs;

    /// <summary>
    /// State of a checkpoint
    /// </summary>
    public enum CheckpointState
    {
        InProgress,
        Completed,
        Failed
    }

    /// <summary>
    /// Individual checkpoint in a checkpoints bar
    /// </summary>
    public class Checkpoint
    {
        public CheckpointState State { get; set; } = CheckpointState.InProgress;
        public bool IsCrossedOut { get; set; }
        public Color CompletedColor { get; set; } = Color.FromArgb(255, 114, 204, 114);  // Green
    }

    /// <summary>
    /// Timerbar that displays checkpoint circles (like heist objectives)
    /// </summary>
    public class HudCheckpointsBar : HudBar
    {
        public List<Checkpoint> Checkpoints { get; }

        public HudCheckpointsBar(string label, int checkpointCount)
            : base(label, thin: true)
        {
            Checkpoints = Enumerable.Range(0, checkpointCount)
                .Select(_ => new Checkpoint())
                .ToList();
        }

        public override void Draw(float y)
        {
            base.Draw(y);

            y += 0.012f;

            if (Checkpoints.Count == 0)
                return;

            const float totalWidth = 0.069f;
            const float circleSize = 0.008f;
            const float startX = 0.879f;

            float spacing = totalWidth / (Checkpoints.Count + 1);

            for (int i = 0; i < Checkpoints.Count; i++)
            {
                var checkpoint = Checkpoints[i];
                float circleX = startX + spacing * (i + 1);

                // Determine circle color based on state
                Color circleColor = checkpoint.State switch
                {
                    CheckpointState.Completed => checkpoint.CompletedColor,
                    CheckpointState.Failed => Color.FromArgb(255, 224, 50, 50),  // Red
                    _ => Color.FromArgb(155, 240, 240, 240)  // Gray (in progress)
                };

                // Draw circle
                fGraphics.DrawRect(circleX, y,
                    circleSize, circleSize,
                    circleColor.R,
                    circleColor.G,
                    circleColor.B,
                    circleColor.A,
                    false);

                // Draw cross if marked
                if (checkpoint.IsCrossedOut)
                {
                    fGraphics.DrawRect(circleX, y,
                        circleSize, 0.002f,
                        255, 255, 255, 255,
                        false);
                    fGraphics.DrawRect(circleX, y,
                        0.002f, circleSize,
                        255, 255, 255, 255,
                        false);
                }
            }
        }
    }
}
