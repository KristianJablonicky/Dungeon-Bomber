using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmp : Upgrade
{
    private readonly int increase = 1;
    public override void equipEffect(Player player)
    {
        getBomb(player, specificUpgradeType).damage += increase;
    }

    public override string getDescription()
    {
        return $"Increase your {specificUpgradeType} bomb's damage by {increase}.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
