public class DamageAmp : Upgrade
{
    private readonly int increase = 1;
    public override void equipEffect(Player player)
    {
        getSpirit(player, specificUpgradeType).damage += increase;
    }

    public override string getDescription()
    {
        return $"+ {increase} {getTypeString()} {Icons.damage}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
