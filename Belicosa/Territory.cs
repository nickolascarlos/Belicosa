using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class Territory
    {
        private string name {  get; set; }
        private List<Territory> borderTerritories = new();
        private Player? ocuppyingPlayer {  get; set; }
        private int troopCount { get; set; } = 0;

        public Territory(string name) {
            this.name = name;
        }

    }
}
