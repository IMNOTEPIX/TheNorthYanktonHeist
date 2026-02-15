using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Interfaces
{
    public interface IScaleform
    {
        void RequestScaleform();
        void DrawScaleform();
        void DeleteScaleform();
        void Dispose();
    }
}
