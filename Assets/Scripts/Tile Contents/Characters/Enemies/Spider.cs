using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(1);
    }

    public override int getMaxHp()
    {
        return 1;
    }

    protected override void onTick()
    {
        int roll = Random.Range(0, 4);
        if (roll == 0)
        {
            move(Movement.Left);
        }
        else if (roll == 1)
        {
            move(Movement.Up);
        }
        else if (roll == 2)
        {
            move(Movement.Down);
        }
        else
        {
            move(Movement.Right);
        }
    }
}
