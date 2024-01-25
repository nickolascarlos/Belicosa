using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class Territory
    {
        public string Name {  get; set; }
        public List<Territory> BorderTerritories { get; private set; }  = new();
        private Player? OcuppyingPlayer { get; set; }
        public int TroopCount { get; private set; } = 0;

        public Territory(string name) {
            this.Name = name;
        }

        public void SetOcuppyingPlayer(Player ocuppyingPlayer)
        {
            OcuppyingPlayer = ocuppyingPlayer;
        }

        public void AddTroops(int quantity)
        {
            TroopCount += quantity;
        }

        public void RemoveTroops(int quantity)
        {
            TroopCount -= quantity;
        }

        public void AddBorders(List<Territory> territories)
        {
            foreach (Territory territory in territories)
            {
                BorderTerritories.Add(territory);
            }
        }

        public Player GetOccupant()
        {
            return OcuppyingPlayer!;
        }

    }
}
