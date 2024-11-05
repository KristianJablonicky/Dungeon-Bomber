using UnityEngine;

public class Spider : Enemy
{
    private bool readyToMove = true;
    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(getScaledDamage(1));
        readyToMove = true;
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
        if (!readyToMove)
        {
            readyToMove = true;
            return;
        }
        Movement? playerDirection = detectPlayer();
        if (playerDirection != null)
        {
            move(playerDirection.Value);
            readyToMove = false;
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
