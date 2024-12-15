using UnityEngine;

public class SpiritAttributes
{

    public int ticksUntilExplosion = 2, damage = 1, horizontalLength = 0, verticalLength = 0, diagonalLength = 0;
    public double criticalHitChance, criticalHitMultiplier = 2d;
    public SpiritAttributes()
    {
        var baseCritChance = PlayerPrefs.GetInt("RuneCriticalChance", 0);
        criticalHitChance = CriticalHitChanceCalculator.getCritChance(baseCritChance);
    }
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
