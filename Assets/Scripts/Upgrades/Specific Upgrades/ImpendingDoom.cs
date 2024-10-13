using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpendingDoom : Upgrade
{
    int damage = 1;
    int ticks = 1;

    public override void equipEffect(Player player)
    {
        var bomb = getBomb(player, specificUpgradeType);
        bomb.damage += damage;
        bomb.ticksUntilExplosion += ticks;
    }

    public override string getDescription()
    {
        return $"{specificUpgradeType} bomb's damage is increased by {damage}, but it takes {ticks} beats longer to explode.";
    }

    public override upgradeTypes getType()
    {
        return upgradeTypes.Random;
    }
}
