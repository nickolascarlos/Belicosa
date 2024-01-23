using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class Belicosa
    {
        private static Belicosa? _instance;

        private int currentCardExchangeCount = 0;
        private List<Player> players { get; set; } = new List<Player>();
        private IEnumerable<Player> defaultPlayersTurnIterator { get; set; }
        private List<GoalCard> goalCards { get; set; } = new List<GoalCard>();
        private List<TerritoryCard> territoryCards { get; set; } = new List<TerritoryCard>();
        private List<Territory> territories { get; set; } = new List<Territory>();
        private List<Continent> continents { get; set; } = new List<Continent>();

        private Belicosa() { }

        public static Belicosa GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Belicosa();
            }

            return _instance;
        }

    }
}