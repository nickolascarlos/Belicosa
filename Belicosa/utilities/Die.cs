using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belicosa.utilities
{
    class Die
    {
        public static int Throw()
        {
            return new Random().Next(1, 7);
        }

        public static List<int> Throw(int throwsCount)
        {
            return (from _ in Enumerable.Range(1, throwsCount) select Throw()).ToList();
        }
    }
}