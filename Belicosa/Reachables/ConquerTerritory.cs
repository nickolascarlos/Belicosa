using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Reachables
{
    class ConquerTerritory : ReachableDecorator
    {
        private Territory Territory;

        public ConquerTerritory(ReachableDecorator decorated, Territory territory)
        {
            this.Decorated = decorated;
            this.Territory = territory;
        }

        public ConquerTerritory(Territory territory)
        {
            this.Territory = territory;
        }

        public override bool CheckCurrentReached(Player player, Belicosa belicosa)
        {
            return belicosa.GetPlayerTerritories(player).Any(territory => territory == Territory);
        }
    }
}
