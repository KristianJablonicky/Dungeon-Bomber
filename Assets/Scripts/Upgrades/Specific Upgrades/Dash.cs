public class Dash : Upgrade
{
    private int dashRange = 1, increase = 1;
    private Player player;
    public override void equipEffect(Player player)
    {
        this.player = player;
        player.spiritSummoned += dash;
        getSpirit(player, specificUpgradeType).damage += increase;
        getSpirit(player, specificUpgradeType).increaseRange(increase);
    }

    private void dash(object sender, System.EventArgs e)
    {
        if (sender == getSpirit(player))
        {
            player.move(Movement.Right, dashRange);
        }
    }

    public override string getDescription()
    {
        return $"+ {dashRange} {getTypeString()} {Icons.dash}\n" +
            $"+ {increase} {getTypeString()} {Icons.damage}\n" +
            $"+ {increase} {getTypeString()} {Icons.range}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
