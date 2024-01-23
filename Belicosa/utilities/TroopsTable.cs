using Belicosa.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.utilities
{
    public class TroopsTable
    {
        private Dictionary<Continent, int> TroopsByContinents { get; set; } = new();
        private List<int> TroopsByExchange { get; set; } = new();


        public TroopsTable() { }
        public TroopsTable(List<int> byExchange, Dictionary<Continent, int> byContinent)
        {
            this.TroopsByExchange = byExchange;
            this.TroopsByContinents = byContinent;
        }

        //public void SetTroopsByContinent(Continent continent, int extraTroops) 
        //{
        //    TroopsByContinents[continent] = extraTroops;
        //}

        //public void SetTroopsByExchangeList(List<int> troopsByExchange)
        //{
        //    // Overwrites!
        //    this.TroopsByExchange = troopsByExchange;
        //}

        public int GetTroopsQuantity(Continent continent)
        {
            return TroopsByContinents[continent];
        }

        public int GetTroopsQuantity(Exchange exchange)
        {
            if (exchange.Number < TroopsByExchange.Count)
            {
                return TroopsByExchange[exchange.Number];
            }

            // TODO: Extract this logic to strategy
            int lastExchangeValue = TroopsByExchange.Last();
            int genericIncrementPerExchange = 5;

            return lastExchangeValue + (exchange.Number - (TroopsByExchange.Count - 1)) * genericIncrementPerExchange;
        }

    }
}
