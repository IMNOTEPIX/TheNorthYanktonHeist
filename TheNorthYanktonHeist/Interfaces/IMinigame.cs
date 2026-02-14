using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Interfaces
{
    public interface IMinigame
    {
        void Update();
        void Dispose();
        void PushResetOnDeath();
    }
}
