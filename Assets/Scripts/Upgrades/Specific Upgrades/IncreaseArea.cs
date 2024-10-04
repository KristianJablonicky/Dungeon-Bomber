using UnityEngine;

public class IncreaseArea : Upgrade
{
    int areaIncrease = 1;

    public override void equipEffect(Player player)
    {
        getBomb(player, specificUpgradeType).areaSize += areaIncrease;
    }

    public override string getDescription()
    {
        return $"Increase the area size of your {specificUpgradeType} bomb by {areaIncrease}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.random;
    }
}
