using UnityEngine;

public class IncreaseArea : Upgrade
{
    float lengthIncrese = 1.5f;
    public override void equipEffect(Player player)
    {
        var bomb = getBomb(player, specificUpgradeType);
        bomb.horizontalLength = increaseRange(bomb.horizontalLength);
        bomb.verticalLength = increaseRange(bomb.verticalLength);
        bomb.diagonalLength = increaseRange(bomb.diagonalLength);
    }

    private int increaseRange(int length)
    {
        return (int)Mathf.Ceil(lengthIncrese * length);
    }

    public override string getDescription()
    {
        return $"Increase the length of your {specificUpgradeType} bomb by {lengthIncrese * 100f}%.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
