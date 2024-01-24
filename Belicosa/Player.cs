using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Enums;

namespace Belicosa
{
    public class Player
    {
        public string Name {  get; set; }
        public Color Color { get; set; }
        private GoalCard GoalCard { get; set; }

        public List<TerritoryCard> TerritoryCards { get; set; } = new List<TerritoryCard>();
        private int AvailableFreeDistributionTroops { get; set; } = 0;
        private int AvailableContinentalDistributionTroops { get; set; } = 0;

        public Player(string name, Color color, GoalCard goalCard, List<TerritoryCard> territoryCards)
        {
            Name = name;
            Color = color;
            GoalCard = goalCard;
            TerritoryCards = territoryCards;
        }

        public Player(string name, Color color, GoalCard goalCard)
        {
            Name = name;
            Color = color;
            GoalCard = goalCard;
        }

        public void AddTerritoryCards(List<TerritoryCard> territoryCards) {
            foreach (TerritoryCard card in territoryCards)
            {
                AddTerritoryCard(card);
            }
        }

        public void AddTerritoryCard(TerritoryCard card)
        {
            TerritoryCards.Add(card);
        }
    }
}
