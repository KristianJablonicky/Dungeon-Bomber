using UnityEngine;

public class IncreaseArea : Upgrade
{
    private readonly int lengthIncrese = 1;
    public override void equipEffect(Player player)
    {
        var bomb = getBomb(player, specificUpgradeType);
        bomb.increaseRange(lengthIncrese);
    }

    public override string getDescription()
    {
        return $"Increase the length of your {specificUpgradeType} bomb by {lengthIncrese}.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
