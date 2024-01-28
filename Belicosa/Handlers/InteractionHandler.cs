using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Handlers
{
    abstract public class InteractionHandler
    {
        public Player Player { get; private set; }

        public InteractionHandler(Player player)
        {
            Player = player;
        }

        abstract public void Handle();
    }
}
