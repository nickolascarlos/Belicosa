using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class Continent
    {
        
        public string Name { get; private set; }
        public List<Territory> Territories { get; private set; } = new();

        public Continent(string name)
        {
            this.Name = name;
        }

        public void AddTerritories(List<Territory> territories)
        {
            foreach (Territory territory in territories)
            {
                Territories.Add(territory);
            } 
        }

    }
}
