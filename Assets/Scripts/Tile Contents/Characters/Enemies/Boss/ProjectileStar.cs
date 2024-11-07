using System.Collections;
using UnityEngine;

public class ProjectileStar : Projectile
{
    private int startingOffset = 2, timeToLive;

    protected override void projectileSpecificOnBeat()
    {
        timeToLive--;
        var content = Dungeon.instance.isTileOccupied((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
        if (content is Player)
        {
            ((Player)content).takeDamage(damage);
            timeToLive = -1;
        }
        if (timeToLive < 0)
        {
            destroy();
        }
    }

    protected override void projectileSpecificSetUp()
    {
        timeToLive = startingOffset + 1;
        var player = Dungeon.instance.getPlayer();
        transform.position = new Vector3(player.getX() - startingOffset * direction.x, player.getY() - startingOffset * direction.y);
    }

    protected override float getSpeed()
    {
        return 1f;
    }
}
