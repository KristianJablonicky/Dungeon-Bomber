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
        return $"+ {increase} {Icons.vitality}";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Neutral;
    }
}
