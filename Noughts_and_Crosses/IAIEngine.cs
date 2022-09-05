﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public interface IAIEngine
    {
        int[] PlayTurn(BoardPosition boardPos, int turn);
    }
}