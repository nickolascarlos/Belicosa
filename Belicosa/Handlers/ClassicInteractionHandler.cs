using Belicosa.Records;
using Belicosa.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Handlers
{
    class ClassicInteractionHandler : InteractionHandler
    {
        bool ConqueredAnyTerritory = false;

        public ClassicInteractionHandler(Player player, Belicosa belicosa) : base(player, belicosa) { }

        public override void Handle()
        {
            HandleCardExchange();
            HandleContinentalDistribution();
            HandleFreeDistribution();
            HandleAttacks();
            HandleTroopsMoving();
            HandleCardGivingForConqueringAnyTerritory();
        }

        private string ReadLineUntil(Func<string, bool> validate, string failMessage)
        {
            string valueRead;
            while (true)
            {
                valueRead = Console.ReadLine()!;

                if (!validate(valueRead))
                {
                    Console.WriteLine(failMessage);
                }
                else
                {
                    return valueRead;
                }
            }
        }
        
        private void HandleCardExchange()
        {
            Console.WriteLine($"Suas cartas: {String.Join(", ", Player.TerritoryCards.Select(c => $"{c.Territory.Name} - {c.Shape}"))}");
            

            if (Player.TerritoryCards.Count < 3)
            {
                Console.Write("Você não tem cartas suficientes para troca");
                return;
            }

            if (Player.TerritoryCards.Count < 5)
            {
                Console.WriteLine("Você deseja realizar uma troca de cartas? (s/n) ");
                if (Console.ReadLine()!.ToLower().Trim() != "s")
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("Como você tem pelo menos 5 cartas, você deve realizar um troca.");
            }

            Console.WriteLine($"Suas cartas:");
            foreach (TerritoryCard card in Player.TerritoryCards)
            {
                Console.WriteLine($"\t{card.Territory.Name} ({card.Shape})");
            }

            Console.WriteLine("Quais cartas deseja trocar?");

            List<TerritoryCard> territoryCards = ReadLineUntil(
                readValue => readValue.Split(" ").ToList().Count > 3, "Selecione exatamente 3 cartas").Split(" ").ToList()
                .Select(territoryName => Belicosa.GetTerritoryCardByName(territoryName)).ToList();

            int troopsQuantity = Belicosa.TroopsTable.GetTroopsQuantity(new Exchange(Belicosa.CurrentCardExchangeCount));
            bool exchangeSucceeded = Player.ExchangeCards(territoryCards);

            if (exchangeSucceeded)
            {
                Console.WriteLine($"[!] {Player.Name} recebeu {troopsQuantity} para distribuir livremente");
            }
            else
            {
                Console.WriteLine($"[!] Não foi possível realizar a troca!");
            }

        }
    
        private void HandleContinentalDistribution()
        {
            foreach (Continent continent in Belicosa.GetContinentsDominatedByPlayer(Player))
            {
                int continentalTroopsQuantity = Belicosa.TroopsTable.GetTroopsQuantity(continent);
                Player.AddContinentalTroops(continent, continentalTroopsQuantity);

                Console.WriteLine($"[!] {Player.Name} ganha {continentalTroopsQuantity} por dominar o continente {continent.Name}");
            }

            while (Player.GetTotalContinentalTroops() > 0)
            {
                Console.WriteLine("Distribua suas tropas continentais.");
                foreach (Continent continent in Player.AvailableContinentalDistributionTroops.Where(t => t.Value > 0).Select(t => t.Key))
                {
                    Console.WriteLine($"\t{continent.Name} ({Player.AvailableContinentalDistributionTroops[continent]} tropas disponíveis)");
                }

                Console.Write("Território > ");
                Territory territory = Belicosa.GetTerritoryByName(Console.ReadLine()!.Trim());
                Continent territoryContinent = Belicosa.GetTerritoryContinent(territory);
                if (territory.GetOccupant() != Player)
                {
                    Console.WriteLine($"O território {territory.Name} não pertence ao jogador {Player.Name}");
                    continue;
                }

                Console.Write("Tropas > ");
                int troopsQuantity = int.Parse(Console.ReadLine()!);
                if (troopsQuantity > Player.AvailableContinentalDistributionTroops[territoryContinent])
                {
                    Console.WriteLine($"Insira uma quantidade disponível de tropas");
                }

                Player.DistributeContinentalTroops(territory, troopsQuantity);
            }
        }
    
        private void HandleFreeDistribution()
        {
            Console.WriteLine("Seus territórios:");
            foreach (Territory territory in Belicosa.GetPlayerTerritories(Player))
            {
                Console.WriteLine($"\t{territory.Name} ({territory.TroopCount})");
            }

            int troopsToReceive = Belicosa.GetPlayerTerritories(Player).Count / 2;
            Player.AddFreeTroops(Math.Max(3, troopsToReceive));

            Console.WriteLine($"[!] {Player.Name} recebeu {troopsToReceive} tropas para distribuir livremente");

            while (Player.AvailableFreeDistributionTroops > 0)
            {
                Console.WriteLine("Distribua suas tropas livremente");
                Console.WriteLine($"Tropas disponíveis: {Player.AvailableFreeDistributionTroops}");

                Console.Write("Território > ");
                Territory territory = Belicosa.GetTerritoryByName(Console.ReadLine()!.Trim());
                if (territory.GetOccupant() != Player)
                {
                    Console.WriteLine($"O território {territory.Name} não pertence ao jogador {Player.Name}");
                    continue;
                }

                Console.Write("Tropas > ");
                int troopsQuantity = int.Parse(Console.ReadLine()!);
                if (troopsQuantity > Player.AvailableFreeDistributionTroops)
                {
                    Console.WriteLine($"Insira uma quantidade disponível de tropas");
                }

                Player.DistributeFreeTroops(territory, troopsQuantity);
            }
        }

        private void HandleAttacks()
        {
            while (true)
            {
                Console.WriteLine("Você quer atacar? (s/n)");
                bool attack = Console.ReadLine()!.Trim().ToLower() == "s";
                if (!attack)
                {
                    return;
                }

                Console.WriteLine("Alvo > ");
                Territory defenderTerritory = Belicosa.GetTerritoryByName(Console.ReadLine()!);

                if (defenderTerritory.GetOccupant() == Player)
                {
                    Console.WriteLine("[!] Você não pode atacar o próprio território");
                }

                Console.WriteLine("Atacante > ");
                Territory attackerTerritory = Belicosa.GetTerritoryByName(Console.ReadLine()!);

                if (!attackerTerritory.BorderTerritories.Contains(defenderTerritory))
                {
                    Console.WriteLine("[!] O território alvo deve fazer fronteira com o território atacante");
                    return;
                }

                Console.WriteLine("Quantidade de tropas (máx. 3) >");
                int troopsQuantity = int.Parse(Console.ReadLine()!);

                if (troopsQuantity > 3 ||  troopsQuantity < 1)
                {
                    Console.WriteLine("[!] Número de tropas inválido");
                    return;
                }

                if (troopsQuantity >= attackerTerritory.TroopCount)
                {
                    Console.WriteLine("[!] Número de tropas insuficientes para realizar o ataque");
                    return;
                }

                Tuple<int, int> result = Player.Attack(attackerTerritory, defenderTerritory, troopsQuantity);

                Console.WriteLine($"Resumo da Batalha:\n\tVocê perdeu {result.Item1} tropas\n\t{defenderTerritory.GetOccupant().Name} perdeu {result.Item2} tropas");
                
                if (defenderTerritory.GetOccupant() == Player)
                {
                    Console.WriteLine($"[!] Parabéns! Você conquistou um novo território: {defenderTerritory.Name}");
                    ConqueredAnyTerritory = true;
                }
            }
        }

        private void HandleTroopsMoving()
        {
            Console.WriteLine("Você quer mover tropas? (s/n)");
            bool moveTroops = Console.ReadLine()!.Trim().ToLower() == "s";
            
            if (!moveTroops)
            {
                return;
            }

            Console.WriteLine("De > ");
            Territory from = Belicosa.GetTerritoryByName(Console.ReadLine()!.Trim());

            if (from.GetOccupant() != Player)
            {
                Console.WriteLine("[!] Você deve informar um território que pertence a você");
                return;
            }

            Console.WriteLine("Para > ");
            Territory to = Belicosa.GetTerritoryByName(Console.ReadLine()!.Trim());

            if (to.GetOccupant() != Player)
            {
                Console.WriteLine("[!] Você deve informar um território que pertence a você");
                return;
            }

            Console.WriteLine("Quantidade de tropas >");
            int troopsQuantity = int.Parse(Console.ReadLine()!.Trim());

            if (troopsQuantity < 1 || troopsQuantity > from.TroopCount - 1)
            {
                Console.WriteLine("[!] Quantidade de tropas insuficiente ou inválida");
                return;
            }

            Player.TransferTroops(from, to, troopsQuantity);
        }

        private void HandleCardGivingForConqueringAnyTerritory()
        {
            if (ConqueredAnyTerritory)
            {
                Belicosa.GivePlayerATerritoryCard(Player);
                Console.WriteLine($"[!] O jogador {Player.Name} ganhou 1 carta por haver conquistado territórios nessa rodada");
            }
        }

    }
}
