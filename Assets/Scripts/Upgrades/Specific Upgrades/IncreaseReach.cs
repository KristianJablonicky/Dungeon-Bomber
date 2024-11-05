public class IncreaseReach : Upgrade
{
    private readonly int increase = 2, decrease = 1;
    public override void equipEffect(Player player)
    {
        var spirit = getSpirit(player, specificUpgradeType);
        spirit.increaseRange(increase);
        spirit.damage -= decrease;
    }

    public override string getDescription()
    {
        return $"+ {increase} {getTypeString()} {Icons.range},\n-{decrease} {Icons.damage}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
