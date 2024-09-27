using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Enemy
{
    private int direction = 0;
    public override void collideWithCharacter(Character character)
    {
        character.takeDamge(1);
    }

    public override int getMaxHp()
    {
        return 1;
    }

    protected override void onTick()
    {
        move((Movement)direction, 2);
        direction++;
        direction %= 4;
    }
}
