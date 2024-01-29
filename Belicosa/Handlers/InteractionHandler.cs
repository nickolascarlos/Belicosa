using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Handlers
{
    abstract public class InteractionHandler
    {
        public Belicosa Belicosa {  get; private set; }
        public Player Player { get; private set; }

        public InteractionHandler(Player player, Belicosa belicosa)
        {
            Player = player;
            Belicosa = belicosa;
        }

        abstract public void Handle();
    }
}
