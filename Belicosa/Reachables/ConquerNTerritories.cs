using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Reachables
{
    public class ConquerNTerritories : ReachableDecorator
    {
        private int TerritoriesQuantity { get; set; }

        public ConquerNTerritories(int territoriesQuantity)
        {
            TerritoriesQuantity = territoriesQuantity;
        }

        public ConquerNTerritories(ReachableDecorator decorated, int territoriesQuantity)
        {
            this.Decorated = decorated;
            this.TerritoriesQuantity = territoriesQuantity;
        }

        public override bool CheckCurrentReached(Player player, Belicosa belicosa)
        {
            return belicosa.GetPlayerTerritories(player).Count > TerritoriesQuantity;
        }


    }
}
