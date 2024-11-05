using UnityEngine;

public class IncreaseArea : Upgrade
{
    private readonly int lengthIncrese = 1;
    public override void equipEffect(Player player)
    {
        var spirit = getSpirit(player, specificUpgradeType);
        spirit.increaseRange(lengthIncrese);
    }

    public override string getDescription()
    {
        return $"+ {lengthIncrese} {getTypeString()} {Icons.range}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
