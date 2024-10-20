public class IncreaseReach : Upgrade
{
    private readonly int increase = 2, decrease = 1;
    public override void equipEffect(Player player)
    {
        var bomb = getBomb(player, specificUpgradeType);
        bomb.increaseRange(increase);
        bomb.damage -= decrease;
    }

    public override string getDescription()
    {
        return $"Increase your {specificUpgradeType} bomb's range by {increase}, but lower its damage by {decrease}.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
