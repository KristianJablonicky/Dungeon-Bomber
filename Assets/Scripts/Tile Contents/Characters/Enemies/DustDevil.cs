using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Enemy
{
    private int direction;
    private int speed = 2;
    private int beatCounter = 0;

    private void Awake()
    {
        direction = Random.Range(0, 4);
    }

    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(1);
    }

    public override int getBaseMaxHp()
    {
        return 1;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 0;
    }

    protected override void onTick()
    {
        beatCounter++;
        if (beatCounter % speed == 0)
        {
            move((Movement)direction, 2);
            direction++;
            direction %= 4;
        }
        if (beatCounter == 32) beatCounter = 0;
    }
}
