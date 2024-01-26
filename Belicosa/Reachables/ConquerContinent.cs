using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Reachables
{
    public class ConquerContinent : ReachableDecorator
    {
        Continent ContinentToBeConquered { get; set; }

        public ConquerContinent(Continent continent) 
        {
            this.ContinentToBeConquered = continent;
        }

        public ConquerContinent(ReachableDecorator decorated, Continent continent)
        {
            this.Decorated = decorated;
            this.ContinentToBeConquered = continent;
        }

        public override bool CheckCurrentReached(Player player)
        {
            return ContinentToBeConquered.Territories.TrueForAll(territory => territory.GetOccupant() == player);
        }
    }
}
