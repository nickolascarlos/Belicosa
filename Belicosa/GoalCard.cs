using Belicosa.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa
{
    public class GoalCard
    {
        private Reachable goal {  get; set; }
        private string description { get; set; }

        public GoalCard(Reachable goal, string description)
        {
            this.goal = goal;
            this.description = description;
        }
    }
}
