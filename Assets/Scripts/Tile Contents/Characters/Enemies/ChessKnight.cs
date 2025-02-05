using UnityEngine;

public class ChessHorse : Enemy
{
    private int moveTick;
    private int direction;

    private string up = "lookUp", forward = "lookForwards", down = "lookDown", back = "lookBackwards"; 

    public override void collideWithCharacter(Character character)
    {
        if (character is Player)
        {
            character.takeDamage(getScaledDamage(2));
        }
    }
    protected override void Start()
    {
        base.Start();
        direction = Random.Range(0, 4);
        moveTick = Random.Range(0, 5);
    }

    public override int getBaseMaxHp()
    {
        return 2;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 1;
    }
    protected override int getUpdateEveryNTicks()
    {
        return 1;
    }

    protected override void onTick()
    {
        if (moveTick >= 0 && moveTick < 3)
        {
            if ((Movement)direction == Movement.Right)
            {
                if (turnedRight)
                {
                    animator.SetTrigger(forward);
                }
                else
                {
                    animator.SetTrigger(back);
                }
            }
            else if ((Movement)direction == Movement.Down)
            {
                animator.SetTrigger(down);
            }
            else if ((Movement)direction == Movement.Left)
            {
                if (turnedRight)
                {
                    animator.SetTrigger(back);
                }
                else
                {
                    animator.SetTrigger(forward);
                }
            }
            else
            {
                animator.SetTrigger(up);
            }
        }
        if (moveTick == 3)
        {
            move((Movement)direction, 2);
            direction = Random.value > 0.5 ? direction + 1 : direction + 3;
            direction %= 4;
        }
        else if (moveTick == 4) 
        {
            move((Movement)direction, 1);
            direction = Random.Range(0, 4);
        }
        moveTick = (moveTick + 1) % 5;
        
    }
}
