public class Bat : Enemy
{
    private bool moveUp = true;

    public override void collideWithCharacter(Character character)
    {
        if (character is Player)
        {
            character.takeDamage(1);
        }
    }

    public override int getMaxHp()
    {
        return 2;
    }

    protected override void onTick()
    {
        if (moveUp)
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
