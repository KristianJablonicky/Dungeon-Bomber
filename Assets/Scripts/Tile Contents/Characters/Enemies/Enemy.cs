using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected override void Start()
    {
        base.Start();
        metronome.onBeatEnemy += onTick;
    }

    public override void die()
    {
        metronome.onBeatEnemy -= onTick;
        base.die();
    }

    private void onTick(object sender, System.EventArgs e)
    {
        onTick();
        //StartCoroutine(waitHalfBeat());
    }
    private IEnumerator waitHalfBeat()
    {
        yield return new WaitForSeconds(metronome.getBeatLength() * 0.5f);
        onTick();
    }

    protected abstract void onTick();

    protected Movement? detectPlayer()
    {
        var p = Dungeon.instance.getPlayer();
        int distance = getDistance(p.getX(), p.getY(), getX(), getY());
        if (distance <= getPlayerDetectionRadius())
        {
            if (p.getY() > getY())
            {
                return Movement.Up;
            }
            else if (p.getY() < getY()) {
                return Movement.Down;
            }
            else if (p.getX() > getX())
            {
                return Movement.Right;
            }
            else
            {
                return Movement.Left;
            }
        }
        else return null;
    }

    private int getDistance(int a_x, int a_y, int b_x, int b_y)
    {
        return Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow(a_x - b_x, 2) + Mathf.Pow(a_y - b_y, 2)));
    }

    protected abstract int getPlayerDetectionRadius();
}
