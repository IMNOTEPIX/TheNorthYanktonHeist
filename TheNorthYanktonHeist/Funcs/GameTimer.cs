using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class GameTimer
    {
        private float _durationMs;
        private float _remainingMs;
        private bool _running;

        public bool Loop { get; set; }

        public bool IsRunning => _running;
        public float RemainingMilliseconds => _remainingMs;

        public event Action<GameTimer> Expired;

        public GameTimer(float durationMs)
        {
            _durationMs = durationMs;
            _remainingMs = durationMs;
        }

        public void Start() => _running = true;

        public void Stop() => _running = false;

        public void Reset()
        {
            _remainingMs = _durationMs;
        }

        public void AddTime(float ms)
        {
            _remainingMs += ms;
        }

        public void RemoveTime(float ms)
        {
            _remainingMs = Math.Max(0f, _remainingMs - ms);
        }

        public void Update(float timeScale = 1f)
        {
            if (!_running)
                return;

            _remainingMs -= Game.LastFrameTime * 1000f * timeScale;

            if (_remainingMs <= 0f)
            {
                Expired?.Invoke(this);

                if (Loop)
                    _remainingMs = _durationMs;
                else
                    Stop();
            }
        }
    }

}
