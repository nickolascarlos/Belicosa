using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class Territory
    {
        private string Name {  get; set; }
        private List<Territory> BorderTerritories { get; set; }  = new();
        private Player? OcuppyingPlayer { get; set; }
        private int TroopCount { get; set; } = 0;

        public Territory(string name) {
            this.Name = name;
        }

    }
}
