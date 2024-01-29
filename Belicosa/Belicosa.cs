using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Enums;
using Belicosa.Handlers;
using Belicosa.Records;
using Belicosa.utilities;

namespace Belicosa
{
    public class Belicosa
    {
        public int CurrentCardExchangeCount { get; private set; } = 0;
        public List<Player> Players { get; private set; } = new List<Player>();
        private List<GoalCard> GoalCards { get; set; } = new List<GoalCard>();
        private List<TerritoryCard> TerritoryCards { get; set; } = new List<TerritoryCard>();
        private List<Territory> Territories { get; set; } = new List<Territory>();
        public List<Continent> Continents { get; private set; } = new List<Continent>();
        public TroopsTable? TroopsTable { get; private set; }
        private Type? InteractionHandlerClass { get; set; }

        public Belicosa() { }

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

        public void SetTroopsTable(TroopsTable troopsTable)
        {
            TroopsTable = troopsTable;
        }

        public void SetInteractionHandlerClass(Type interactionHandlerClass)
        {
            if (!interactionHandlerClass.IsSubclassOf(typeof(InteractionHandler)))
            {
                throw new Exception("Interaction handler class must be subclass of InteractionHandler class");
            }

            InteractionHandlerClass = interactionHandlerClass;
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
                Players.Add(new Player(playerNames[i], colorsDeck[i], goalCardsDeck[i], this));
            }

            // "Distributes" territory cards
            for (int i = 0; i < territoryCardsDeck.Count; i++)
            {
                Player player = Players[i % Players.Count];
                TerritoryCard card = territoryCardsDeck[i];

                Territory cardTerritory = card.GetTerritory();
                cardTerritory.SetOcuppyingPlayer(player);
                cardTerritory.AddTroops(1);
            }

            // Shuffles territory cards (instance deck)
            TerritoryCards = TerritoryCards.OrderBy(_ => new Random().Next()).ToList();

            MainLoop();
        }

        private void MainLoop()
        {
            foreach (Player player in LoopIteratePlayers())
            {

                InteractionHandler handler = ((InteractionHandler) Activator.CreateInstance(InteractionHandlerClass, player, this)!);
                handler.Handle();

                if (player.ReachedGoal())
                {
                    Console.WriteLine($"{player.Name} venceu o jogo!");
                    return;
                }
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

        public bool ExchangeCards(Player player, List<TerritoryCard> cards)
        {
            if (cards.Count != 3)
                throw new Exception("Exactly 3 cards are needed for exchange");

            if (cards.Any(card => !player.TerritoryCards.Contains(card)))
                return false;

            if (AllCardsHaveDifferentShapes(cards) || AllCardsHaveTheSameShape(cards))
            {
                int troopsQuantity = TroopsTable.GetTroopsQuantity(new Exchange(CurrentCardExchangeCount));
                CurrentCardExchangeCount++;

                player.AddFreeTroops(troopsQuantity);

                cards.ForEach(card => {
                    player.RemoveTerritoryCard(card);
                    TerritoryCards.Prepend(card);
                });

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AllCardsHaveTheSameShape(List<TerritoryCard> cards)
        {
            return cards.TrueForAll(card => card.Shape == cards[0].Shape);
        }

        private bool AllCardsHaveDifferentShapes(List<TerritoryCard> cards)
        {
            // TODO: Improve this code
            for (int i = 0; i < 3; i++)
            {
                TerritoryCard card = cards[i];
                for (int j = 0; j < i; j++)
                {
                    if (card.Shape == cards[j].Shape)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<TerritoryCard> GetAvailableTerritoryCards()
        {
            return (from card in TerritoryCards where card.Holder == null select card).ToList();
        }

        public TerritoryCard GetRandomAvailableTerritoryCard()
        {
            List<TerritoryCard> availableCards = GetAvailableTerritoryCards();
            return availableCards[new Random().Next(0, availableCards.Count)] ;
        }

        public void GivePlayerATerritoryCard(Player player)
        {
            TerritoryCard territoryCard = GetRandomAvailableTerritoryCard();
            territoryCard.SetHolder(player);
        }

        public List<TerritoryCard> GetPlayerTerritoryCards(Player player)
        {
            return (from card in TerritoryCards where card.Holder == player select card).ToList();
        }

        public TerritoryCard GetTerritoryCardByName(string territoryName)
        {
            return (
                    from card in TerritoryCards
                    where card.Territory.Name == territoryName
                    select card
                ).First();
        }

        public List<Continent> GetContinentsDominatedByPlayer(Player player)
        {
            return (
                    from continent in Continents
                    where continent.Territories.All(territory => territory.GetOccupant() == player)
                    select continent
                ).ToList();
        }

        public IEnumerable<Player> LoopIteratePlayers()
        {
            for (int i = 0; ; i = (i+1) % Players.Count)
            {
                yield return Players[i];
            }
        }

        public void PrintGameResume()
        {
            foreach (var player in Players)
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