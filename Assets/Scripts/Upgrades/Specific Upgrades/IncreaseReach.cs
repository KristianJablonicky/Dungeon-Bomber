public class IncreaseReach : Upgrade
{
    private readonly int increase = 2, decrease = 2;
    public override void equipEffect(Player player)
    {
        var spirit = getSpirit(player, specificUpgradeType);
        spirit.increaseRange(increase);
    }
    protected override void oneTimeEffect(Player player)
    {
        if (player.getHp() > decrease)
        {
            player.increaseMaxHp(-decrease, true);
        }
    }

    public override string getDescription()
    {
        return $"+ {increase} {getTypeString()} {Icons.range},\n-{decrease} {Icons.vitality}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
