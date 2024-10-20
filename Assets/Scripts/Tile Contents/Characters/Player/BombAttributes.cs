using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttributes
{
    public int ticksUntilExplosion = 2, damage = 1, horizontalLength = 0, verticalLength = 0, diagonalLength = 0;
    public void increaseRange(int increaseAmount)
    {
        if (horizontalLength > 0)
        {
            horizontalLength += increaseAmount;
        }
        if (verticalLength > 0)
        {
            verticalLength += increaseAmount;
        }
        if (diagonalLength > 0)
        {
            diagonalLength += increaseAmount;
        }
    }
}
