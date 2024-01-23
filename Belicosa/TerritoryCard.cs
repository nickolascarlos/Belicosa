using Belicosa.enums;
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
        private Territory territory {  get; set; }
        private Shape shape { get; set; }

        public TerritoryCard(Territory territory, Shape shape)
        {
            this.territory = territory;
            this.shape = shape;
        }

    }
}
