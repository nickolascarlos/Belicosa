using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Interfaces;

namespace Belicosa.Reachables
{
    public abstract class ReachableDecorator : IReachable
    {
        protected IReachable? Decorated { get; set; }
        public abstract bool CheckCurrentReached(Player player, Belicosa belicosa);

        public bool IsReached(Player player, Belicosa belicosa)
        {
            if (Decorated is not null && !Decorated.IsReached(player, belicosa))
            {
                return false;
            }

            return CheckCurrentReached(player, belicosa);
        }

    }
}
