using UnityEngine;

public class Spider : Enemy
{
    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(getScaledDamage(1));
    }

    public override int getBaseMaxHp()
    {
        return 2;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 6;
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
}
