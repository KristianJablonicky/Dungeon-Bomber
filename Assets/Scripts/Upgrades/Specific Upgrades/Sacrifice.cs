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
        return $"{healAmount} {Icons.heal}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Sacrifice;
    }
}
