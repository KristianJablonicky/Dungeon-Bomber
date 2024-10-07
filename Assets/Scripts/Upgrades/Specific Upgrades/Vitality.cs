using UnityEngine;

public class Vitality : Upgrade
{
    private int increase = 3;
    public override void equipEffect(Player player)
    {
        player.increaseMaxHp(increase);
    }
    protected override void oneTimeEffect(Player player)
    {
        player.heal(increase);    
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
