using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public static class RandomNumberGenerator
    {
        private static Object _randLock = new object();

        private static Random _rand = new Random();

        public static int Next(int maxValue)
        {
            lock (_randLock)
            {
                return _rand.Next(maxValue);
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            lock (_randLock)
            {
                return _rand.Next(minValue, maxValue);
            }
        }
    }
}
