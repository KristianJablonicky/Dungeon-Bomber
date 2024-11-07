using UnityEngine;

public class ProjectileWave : Projectile
{
    private bool dormant = true;
    private int beatsUntilAwakening = 2;
    protected override float getSpeed()
    {
        return 2f;
    }

    protected override void projectileSpecificOnBeat()
    {
        if (!dormant)
        {
            int x = (int)Mathf.Round(transform.position.x), y = (int)Mathf.Round(transform.position.y);
            checkTile(x + (int)direction.x, y);
            checkTile(x, y);
        }
        else
        {
            beatsUntilAwakening--;
            if (beatsUntilAwakening == 0)
            {
                dormant = false;
                enabled = true;
            }
        }
    }

    private void checkTile(int x, int y)
    {
        var content = Dungeon.instance.isTileOccupied(x, y);
        if (content is Player)
        {
            ((Player)content).takeDamage(damage);
            destroy();
        }
        else if (content is Wall)
        {
            destroy();
        }
    }

    protected override void projectileSpecificSetUp()
    {
        enabled = false;

    }

    public void setPosition(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}
