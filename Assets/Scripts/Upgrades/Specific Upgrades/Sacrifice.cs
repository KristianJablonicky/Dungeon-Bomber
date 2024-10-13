public class Sacrifice : Upgrade
{
    private int healAmount = 5;
    protected override void oneTimeEffect(Player player)
    {
        // heal to full
        //player.heal(player.getMaxHp() - player.getHp());
        player.heal(healAmount, damageTags.Heal);
    }
    public override void equipEffect(Player player)
    {
    }

    public override string getDescription()
    {
        return $"Restore {healAmount} health.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Sacrifice;
    }
}
