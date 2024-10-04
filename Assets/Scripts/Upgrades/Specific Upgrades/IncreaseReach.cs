using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseReach : Upgrade
{
    private int increase = 2;
    public override void equipEffect(Player player)
    {
        player.bombSquare.horizontalLength += increase;
        player.bombPlus.horizontalLength += increase;
        player.bombX.horizontalLength += increase;
    }

    public override string getDescription()
    {
        return $"Increase your horizontal range of all bombs by {increase}.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.neutral;
    }
}
