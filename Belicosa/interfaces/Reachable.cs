using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.Interfaces
{
    public interface IReachable
    {
        bool IsReached(Player player, Belicosa belicosa);
    }
}
