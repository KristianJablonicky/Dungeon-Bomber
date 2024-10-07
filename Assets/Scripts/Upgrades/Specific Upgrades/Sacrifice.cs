public class Sacrifice : Upgrade
{
    protected override void oneTimeEffect(Player player)
    {
        // heal to full
        player.heal(player.getMaxHp() - player.getHp());
    }
    public override void equipEffect(Player player)
    {
    }

    public override string getDescription()
    {
        return "Restore your health to full.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.sacrifice;
    }
}
