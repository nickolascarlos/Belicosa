using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Enums;

namespace Belicosa.Reachables
{
    public class DefeatArmy : ReachableDecorator
    {
        private Color Color { get; set; }

        public DefeatArmy(Color color) 
        {
            this.Color = color;
        }

        public DefeatArmy(ReachableDecorator decorated, Color color) 
        {
            this.Decorated = decorated;
            this.Color = color;
        }

        public override bool CheckCurrentReached(Player player, Belicosa belicosa)
        {
            return false;
        }
    }
}
