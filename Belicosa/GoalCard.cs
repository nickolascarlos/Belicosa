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
        private IReachable Goal { get; set; }
        private string Description { get; set; }

        public GoalCard(IReachable goal, string description)
        {
            this.Goal = goal;
            this.Description = description;
        }
    }
}
