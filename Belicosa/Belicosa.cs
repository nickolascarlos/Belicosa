using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        public List<Player> Players { get; private set; } = new List<Player>();
        private IEnumerable<Player> DefaultPlayersTurnIterator { get; set; }
        private List<GoalCard> GoalCards { get; set; } = new List<GoalCard>();
        private List<TerritoryCard> TerritoryCards { get; set; } = new List<TerritoryCard>();
        private List<Territory> Territories { get; set; } = new List<Territory>();
        public List<Continent> Continents { get; private set; } = new List<Continent>();

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

        /// <summary>
        /// Compares two sets of dice throws in a conflict between attackers and defenders.
        /// </summary>
        /// <param name="attackThrows">A list of integers representing the dice throws of the attackers.</param>
        /// <param name="defenseThrows">A list of integers representing the dice throws of the defenders.</param>
        /// <returns>
        /// A tuple containing the number of troops lost by the attacker and the number of troops lost by the defender.
        /// The first item of the tuple represents troops lost by the attacker, and the second item represents troops lost by the defender.
        /// </returns>
        public static Tuple<int, int> CompareThrows(List<int> attackThrows, List<int> defenseThrows)
        {
            // TODO: Implement data validation

            attackThrows = attackThrows.OrderByDescending(x => x).ToList();
            defenseThrows = defenseThrows.OrderByDescending(x => x).ToList();

            var (troopsLostByAttacker, troopsLostByDefender) = (0, 0);
            
            for (int i = 0; i < Math.Min(attackThrows.Count, defenseThrows.Count); i++)
            {
                int attackThrow = attackThrows[i];
                int defenseThrow = defenseThrows[i];

                if (attackThrow > defenseThrow)
                {
                    troopsLostByDefender++;
                } else
                {
                    troopsLostByAttacker++;
                }
            }

            return new Tuple<int, int>(troopsLostByAttacker, troopsLostByDefender);
        }
    
        public Tuple<int, int> PerformAttack(Territory attackerTerritory, Territory defenderTerritory, int attackerTroopsQuantity)
        {
            if (attackerTroopsQuantity > 3 || attackerTroopsQuantity < 1)
                throw new Exception("Invalid attack troops quantity");

            if (attackerTroopsQuantity >= attackerTerritory.TroopCount)
                throw new Exception("Not enough troops");

            if (attackerTerritory.GetOccupant() == defenderTerritory.GetOccupant())
                throw new Exception("One cannot attack its own territory");

            Player attacker = attackerTerritory.GetOccupant();
            Player defender = defenderTerritory.GetOccupant();

            List<int> attackerThrows = attacker.Throw(attackerTroopsQuantity);
            List<int> defenderThrows = defender.Throw(Math.Min(3, defenderTerritory.TroopCount));

            Tuple<int, int> battleResult = CompareThrows(attackerThrows, defenderThrows);

            int troopsLostByAttacker = battleResult.Item1;
            int troopsLostByDefender = battleResult.Item2;

            attackerTerritory.RemoveTroops(troopsLostByAttacker);
            defenderTerritory.RemoveTroops(troopsLostByDefender);

            if (defenderTerritory.TroopCount == 0)
            {
                defenderTerritory.SetOcuppyingPlayer(attacker);
                attacker.TransferTroops(attackerTerritory, defenderTerritory, 1);
            }

            return battleResult;
        }

        public Territory GetTerritoryByName(string name)
        {   
            return (from territory in Territories where territory.Name == name select territory).First();
        }

        public Continent GetContinentByName(string name)
        {
            return (from continent in Continents where continent.Name == name select continent).First();
        }

        public void PrintGameResume()
        {
            foreach (var player in Belicosa.GetInstance().Players)
            {
                Console.WriteLine(player.Name);
                Console.WriteLine($"\t Goal: {player.GoalCard.Description}");
                Console.WriteLine("\t Territories");
                foreach (var territory in GetPlayerTerritories(player))
                {
                    Console.WriteLine($"\t\t * {territory.Name} ({territory.TroopCount})");
                }
            }
        }
    }
}