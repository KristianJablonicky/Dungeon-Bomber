using UnityEngine;

public class ChessHorse : Enemy
{
    private int moveTick;
    private int direction;

    public override void collideWithCharacter(Character character)
    {
        if (character is Player)
        {
            character.takeDamage(getScaledDamage(1));
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
        return 6;
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
