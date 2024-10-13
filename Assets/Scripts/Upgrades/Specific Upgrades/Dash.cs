public class Dash : Upgrade
{
    private int dashRange = 1;
    private Player player;
    public override void equipEffect(Player player)
    {
        this.player = player;
        player.bombPlaced += dash;
    }

    private void dash(object sender, System.EventArgs e)
    {
        if (sender == getBomb(player))
        {
            player.move(Movement.Right, dashRange);
        }
    }

    public override string getDescription()
    {
        return $"Dash {dashRange} tile to the right upon placing down the {specificUpgradeType} bomb.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
