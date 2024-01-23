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

        public abstract bool IsReached(Player player);
    }
}
