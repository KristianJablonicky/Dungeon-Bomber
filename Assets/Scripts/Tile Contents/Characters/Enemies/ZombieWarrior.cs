using System;
using UnityEngine;

public class ZombieWarrior : Enemy
{
    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(getScaledDamage(2));
    }

    public override int getBaseMaxHp()
    {
        return 3;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 6;
    }
    protected override int getUpdateEveryNTicks()
    {
        return 2;
    }
    protected override void onTick()
    {
        Movement? playerDirection = detectPlayer();
        if (playerDirection != null)
        {
            move(playerDirection.Value);
        }
        else
        {
            int roll = UnityEngine.Random.Range(0, 4);
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
}
