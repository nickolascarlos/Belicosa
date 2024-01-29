using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Enums;
using Belicosa.utilities;

namespace Belicosa
{
    public class Player
    {
        public string Name {  get; private set; }
        public Color Color { get; private set; }
        public GoalCard GoalCard { get; private set; }

        public List<TerritoryCard> TerritoryCards {
            get
            {
                return Belicosa.GetInstance().GetPlayerTerritoryCards(this);
            }
            set { }
        }
        public int AvailableFreeDistributionTroops { get; private set; } = 0;
        public Dictionary<Continent, int> AvailableContinentalDistributionTroops { get; private set; } = new();

        public Player(string name, Color color, GoalCard goalCard)
        {
            Name = name;
            Color = color;
            GoalCard = goalCard;
        }

        public void AddTerritoryCards(List<TerritoryCard> territoryCards) {
            foreach (TerritoryCard card in territoryCards)
            {
                card.SetHolder(this);
            }
        }

        public void AddTerritoryCard(TerritoryCard card)
        {
            card.SetHolder(this);
        }

        public Tuple<int, int> Attack(Territory attackerTerritory, Territory defenderTerritory, int troopsQuantity)
        {
            return Belicosa.GetInstance().PerformAttack(attackerTerritory, defenderTerritory, troopsQuantity);
        }

        public List<int> Throw(int quantity)
        {
            return Die.Throw(quantity);
        }

        public void TransferTroops(Territory from, Territory to, int troopsQuantity)
        {
            if (from.GetOccupant() != this || to.GetOccupant() != this)
                throw new Exception("Both territories must be occupied by the player");

            from.RemoveTroops(troopsQuantity);
            to.AddTroops(troopsQuantity);
        }

        public void AddFreeTroops(int troopsQuantity)
        {
            AvailableFreeDistributionTroops += troopsQuantity;
        }

        public void AddContinentalTroops(Continent continent, int troopsQuantity)
        {
            AvailableContinentalDistributionTroops[continent] = AvailableContinentalDistributionTroops.GetValueOrDefault(continent, 0) + troopsQuantity;
        }

        public int GetTotalContinentalTroops()
        {
            return AvailableContinentalDistributionTroops.Select(c => c.Value).Aggregate(0, (acc, availableTroops) => acc + availableTroops);
        }

        public bool DistributeFreeTroops(Territory territory, int troopsQuantity)
        {
            if (troopsQuantity > AvailableFreeDistributionTroops)
                return false;

            AvailableFreeDistributionTroops -= troopsQuantity;
            territory.AddTroops(troopsQuantity);
            return true;
        }

        public bool DistributeContinentalTroops(Territory territory, int troopsQuantity)
        {
            Continent territoryContinent = Belicosa.GetInstance().GetTerritoryContinent(territory);
            int availableContinentalTroops = AvailableContinentalDistributionTroops.GetValueOrDefault(territoryContinent, 0);

            if (availableContinentalTroops < troopsQuantity || territory.GetOccupant() != this)
                return false;

            AvailableContinentalDistributionTroops[territoryContinent] -= troopsQuantity;
            territory.AddTroops(troopsQuantity);
            return true;
        }

        public bool ReachedGoal()
        {
           return GoalCard.IsReached(this);
        }

        public void RemoveTerritoryCard(TerritoryCard card)
        {
            card.SetHolder(null);
        }

        public bool ExchangeCards(List<TerritoryCard> cards)
        {
            return Belicosa.GetInstance().ExchangeCards(this, cards);
        }
    }
}
