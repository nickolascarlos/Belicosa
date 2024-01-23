using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belicosa.Enums;

namespace Belicosa
{
    public class Player
    {
        private string Name {  get; set; }
        private Color Color { get; set; }
        private GoalCard GoalCard { get; set; }
        private int AvailableFreeDistributionTroops { get; set; } = 0;
        private int AvailableContinentalDistributionTroops { get; set; } = 0;

        public Player(string name, Color color, GoalCard goalCard)
        {
            this.Name = name;
            this.Color = color;
            this.GoalCard = goalCard;
            
        }
    }
}
