public class ImpendingDoom : Upgrade
{
    private readonly int damageMult = 2;
    private readonly int ticks = 1;

    public override void equipEffect(Player player)
    {
        var spirit = getSpirit(player, specificUpgradeType);
        spirit.damage *= damageMult;
        spirit.ticksUntilExplosion += ticks;
    }

    public override string getDescription()
    {
        return $"{getTypeString()} {Icons.damage} x {damageMult},\n+ {ticks} {Icons.delay}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
