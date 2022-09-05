using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public static class GameSymbol
    {
        public static char MapIntToSymbol(int value)
        {
            if (value == -1) return 'X';
            if (value == 0) return ' ';
            if (value == 1) return 'O';
            throw new Exception($"Invalid game symbol value {value}");
        }

        public static int MapSymbolToInt(char value)
        {
            if (value == 'X') return -1;
            if (value == ' ') return 0;
            if (value == 'O') return 1;
            throw new Exception($"Invalid game symbol {value}");
        }
    }
}
