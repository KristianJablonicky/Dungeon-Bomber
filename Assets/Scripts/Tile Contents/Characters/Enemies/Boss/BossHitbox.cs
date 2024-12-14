using System.Diagnostics;

public class BossHitbox : TileContent
{
    private Boss boss;
    public void setUp(Boss boss)
    {
        this.boss = boss;
    }
    public void takeDamage(int damage, damageTags tags, spiritType type)
    {
        if (boss.getHp() <= 0)
        {
            return;
        }
        boss.takeDamage(damage, tags, type);
    }
}
