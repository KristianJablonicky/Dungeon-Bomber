public class Sacrifice : Upgrade
{
    public override void equipEffect(Player player)
    {
        // heal to full
        player.heal(player.getMaxHp() - player.getHp());
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
