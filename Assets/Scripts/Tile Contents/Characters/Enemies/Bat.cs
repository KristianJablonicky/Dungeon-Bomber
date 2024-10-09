using System;

public class Bat : Enemy
{
    private bool moveUp = true;

    public override void collideWithCharacter(Character character)
    {
        if (character is Player)
        {
            character.takeDamage(getScaledDamage(1));
        }
    }

    public override int getBaseMaxHp()
    {
        return 2;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 3;
    }

    protected override void onTick()
    {
        Movement? playerDirection = detectPlayer();
        if (playerDirection != null)
        {
            move(playerDirection.Value);
        }
        else if (moveUp)
        {
            move(Movement.Up);
            moveUp = false;
        }
        else
        {
            move(Movement.Down);
            moveUp = true;
        }
    }
}
