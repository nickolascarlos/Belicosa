using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Enums;

namespace Belicosa
{
    public class Belicosa
    {
        private static Belicosa? _instance;

        private int CurrentCardExchangeCount { get; set; } = 0;
        private List<Player> Players { get; set; } = new List<Player>();
        private IEnumerable<Player> DefaultPlayersTurnIterator { get; set; }
        private List<GoalCard> GoalCards { get; set; } = new List<GoalCard>();
        private List<TerritoryCard> TerritoryCards { get; set; } = new List<TerritoryCard>();
        private List<Territory> Territories { get; set; } = new List<Territory>();
        private List<Continent> Continents { get; set; } = new List<Continent>();

        private Belicosa() { }

        public static Belicosa GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Belicosa();
            }

            return _instance;
        }

        public void AddContinents(List<Continent> continents)
        {
            foreach (Continent continent in continents)
            {
                Continents.Add(continent);
            }
        }

        public void AddTerritories(List<Territory> territories)
        {
            foreach (Territory territory in territories)
            {
                Territories.Add(territory);
            }
        }

        public void AddTerritoryCards(List<TerritoryCard> territoryCards)
        {
            foreach (TerritoryCard card in territoryCards)
            {
                TerritoryCards.Add(card);
            }
        }

        public void AddGoalCards(List<GoalCard> goalCards)
        {
            foreach (GoalCard goalCard in goalCards)
            {
                GoalCards.Add(goalCard);
            }
        }

        public void StartGame(List<string> playerNames)
        {

            // Shuffled decks
            List<TerritoryCard> territoryCardsDeck = new List<TerritoryCard>(TerritoryCards)
                                        .OrderBy(_ => new Random().Next())
                                        .ToList();

            List<GoalCard> goalCardsDeck = new List<GoalCard>(GoalCards)
                                        .OrderBy(_ => new Random().Next())
                                        .ToList();

            List<Color> colorsDeck = new List<Color>(Enum.GetValues(typeof(Color)).Cast<Color>().ToList())
                                        .OrderBy(_ => new Random().Next())
                                        .ToList();

            // Creates players, distributing simultaneously colors and goal cards
            for (int i = 0; i < playerNames.Count; i++)
            {
                Players.Add(new Player(playerNames[i], colorsDeck[i], goalCardsDeck[i]));
            }

            // Distributes territory cards
            for (int i = 0; i < territoryCardsDeck.Count; i++)
            {
                Player player = Players[i % Players.Count];
                TerritoryCard card = territoryCardsDeck[i];

                player.AddTerritoryCard(card);
                Territory cardTerritory = card.GetTerritory();
                cardTerritory.SetOcuppyingPlayer(player);
                cardTerritory.AddTroops(1);
            }
        }

        public Player GetPlayerByName(string name)
        {
            return (from player in Players where player.Name == name select player).First<Player>();
        }

        public List<Territory> GetPlayerTerritories(Player player)
        {
            return (from territory in Territories where territory.GetOccupant() == player select territory).ToList();
        }

        public Continent GetTerritoryContinent(Territory territory)
        {
            return (from continent in Continents where continent.Territories.Contains(territory) select continent).First();
        }



    }
}