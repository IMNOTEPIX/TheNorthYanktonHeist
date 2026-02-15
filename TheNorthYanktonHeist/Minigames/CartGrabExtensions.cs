using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Minigames
{
    public static class CartGrabExtensions
    {
        /// <summary>
        /// Hook up cart to automatically update a value bar
        /// </summary>
        public static CartGrab ConnectToValueBar(this CartGrab cart, Drawables.TimerBars.HudValueBar bar)
        {
            cart.ValueAdded += (sender, e) => bar.Value += e.Amount;
            return cart;
        }
    }
}
