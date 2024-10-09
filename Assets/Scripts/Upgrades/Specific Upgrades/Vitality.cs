using UnityEngine;

public class Vitality : Upgrade
{
    private int increase = 2;
    public override void equipEffect(Player player)
    {
    }
    protected override void oneTimeEffect(Player player)
    {
        player.increaseMaxHp(increase, true);
    }

    public override string getDescription()
    {
        return $"Increase your max health by {increase}.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.neutral;
    }
}
