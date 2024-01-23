using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belicosa.enums;

namespace Belicosa
{
    public class Player
    {
        private string name {  get; set; }
        private Color color { get; set; }
        private GoalCard goalCard { get; set; }
        private int availableFreeDistributionTroops { get; set; } = 0;
        private int availableContinentalDistributionTroops { get; set; } = 0;

        public Player(string name, Color color, GoalCard goalCard)
        {
            this.name = name;
            this.color = color;
            this.goalCard = goalCard;
            
        }
    }
}
