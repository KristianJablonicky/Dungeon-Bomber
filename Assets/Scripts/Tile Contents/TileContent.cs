using UnityEngine;

public abstract class TileContent : MonoBehaviour
{
    protected int x, y;
    protected Dungeon dungeon;
    protected Metronome metronome;
    protected virtual void Start()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        dungeon = Dungeon.instance;
        metronome = Metronome.instance;
    }

    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }

    public virtual bool bounceOnCollision()
    {
        return true;
    }
}
