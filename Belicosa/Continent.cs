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
        private List<Territory> territories { get; set; } = new();

        public Continent(string name)
        {
            this.Name = name;
        }

    }
}
