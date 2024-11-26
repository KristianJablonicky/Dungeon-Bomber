using UnityEngine;

public class SkeletonWarrior : Enemy
{
    protected float randomMoveChance = 0.5f;
    private int direction = 1;

    public override void collideWithCharacter(Character character)
    {
        if (character is Player)
        {
            character.takeDamage(getScaledDamage(1));
        }
    }

    public override int getBaseMaxHp()
    {
        return 4;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 3;
    }

    protected override void onTick()
    {
        /*
        Movement? playerDirection = detectPlayer();
        if (playerDirection != null)
        {
            move(playerDirection.Value);
        }
        else */
        if (randomMoveChance > UnityEngine.Random.value)
        {
            move((Movement)Random.Range(0, 4), 1);
            return;
        }
        move((Movement)direction, 1);
        direction = (direction + 2) % 4;
    }
}
