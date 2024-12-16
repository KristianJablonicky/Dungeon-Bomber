using UnityEngine;

public class SkeletonWarrior : Enemy
{
    private int direction;
    private int distance;
    private int changeDirectionCooldown;

    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(getScaledDamage(1));
    }

    protected override void Start()
    {
        base.Start();
        direction = (Random.Range(0, 2) == 0) ? 1 : 3;
        distance = Random.Range(0, 2);
        changeDirectionCooldown = distance;
    }

    public override int getBaseMaxHp()
    {
        return 4;
    }

    protected override int getUpdateEveryNTicks()
    {
        return 2;
    }

    protected override void onTick()
    {   
        move((Movement)direction, 1);
        distance = (distance + 1) % 2;
        changeDirectionCooldown = (changeDirectionCooldown + 1) % 8;
        if (distance == 0)
        {
            direction = (direction + 2) % 4;
        }
        if (changeDirectionCooldown == 0)
        {
            direction = (direction + 1) % 4;
        }
    }
}
