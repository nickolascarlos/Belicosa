using Belicosa.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class GoalCard
    {
        public IReachable Goal { get; private set; }
        public string Description { get; private set; }

        public GoalCard(IReachable goal, string description)
        {
            this.Goal = goal;
            this.Description = description;
        }

        public bool IsReached(Player player, Belicosa belicosa)
        {
            return Goal.IsReached(player, belicosa);
        }
    }
}
