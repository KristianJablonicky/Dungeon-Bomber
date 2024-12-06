using UnityEngine;

public class SkeletonWarrior : Enemy
{
    protected float randomMoveChance = 0.5f;
    private int direction = 1;
    private bool moveTick;

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
        moveTick = UnityEngine.Random.value > 0.5f;
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

        moveTick = !moveTick;
        if (!moveTick)
        {
            return;
        }
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
