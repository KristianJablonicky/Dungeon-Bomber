public class ImpendingDoom : Upgrade
{
    private readonly int damageMult = 2;
    private readonly int ticks = 1;

    public override void equipEffect(Player player)
    {
        var bomb = getBomb(player, specificUpgradeType);
        bomb.damage *= damageMult;
        bomb.ticksUntilExplosion += ticks;
    }

    public override string getDescription()
    {
        return $"{specificUpgradeType} bomb's damage is multiplied by {damageMult}, but it takes {ticks} beat longer to explode.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
