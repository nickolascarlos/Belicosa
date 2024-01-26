using Belicosa.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class TerritoryCard
    {
        public Territory Territory { get; private set; }
        public Shape Shape { get; private set; }

        public TerritoryCard(Territory territory, Shape shape)
        {
            this.Territory = territory;
            this.Shape = shape;
        }

        public Territory GetTerritory()
        {
            return Territory;
        }

        public override string ToString()
        {
            return $"TerritoryCard Name={Territory.Name} Shape={Shape.ToString()}";
        }

    }
}
