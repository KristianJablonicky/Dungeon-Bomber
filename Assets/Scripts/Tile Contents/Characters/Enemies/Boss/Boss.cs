using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private ProjectileStar projectileStar;
    [SerializeField] private ProjectileWave projectileWave;
    [SerializeField] private SpriteRenderer shieldSpriteRenderer;

    private spiritType currentWeakness = spiritType.owl;

    private static readonly int maxCoolDown = 8;
    private int castCoolDown = maxCoolDown + 2;

    public override void collideWithCharacter(Character character)
    {
    }

    public override int getBaseMaxHp()
    {
        return 400;
    }

    protected override int getUpdateEveryNTicks()
    {
        return 1;
    }

    protected override void onTick()
    {
        castCoolDown--;

        if (castCoolDown == 1)
        {
            castCoolDown = maxCoolDown;
            bossAnimator.SetTrigger("Cast");
            castSpell();
        }
    }

    private void castSpell()
    {
        int roll = Random.Range(0, 5);
        if (roll <= 1)
        {
            shootProjectile();
        }
        else if (roll <= 3)
        {
            sendWave();
        }
        else
        {
            summonAid();
        }
        if (Random.value <= 0.25f)
        {
            summonAid();
        }
        switchWeakness();
    }

    private void switchWeakness()
    {
        currentWeakness = (spiritType)(((int)currentWeakness + 1) % 3);
        updateWeakness();
    }

    private void updateWeakness()
    {
        if (currentWeakness == spiritType.bear)
        {
            shieldSpriteRenderer.color = Color.red;
        }
        else if (currentWeakness == spiritType.wolf)
        {
            shieldSpriteRenderer.color = Color.green;
        }
        if (currentWeakness == spiritType.owl)
        {
            shieldSpriteRenderer.color = Color.blue;
        }
    }

    private void shootProjectile()
    {
        ProjectileStar projectileGO = Instantiate(projectileStar);
        projectileGO.setUp((Movement)Random.Range(0, 4), 4);
    }

    private void sendWave()
    {
        int startingX = 1;
        Movement direction = Movement.Right;
        if (Random.Range(0, 2) == 0)
        {
            startingX = Dungeon.instance.getWidth() - 2;
            direction = Movement.Left;
        }
        for (int y = 1; y < Dungeon.instance.getHeight() - 1; y++)
        {
            ProjectileWave projectileGO = Instantiate(projectileWave);
            projectileGO.setUp(direction, 3);
            projectileGO.setPosition(startingX, y);
        }
    }

    private void summonAid()
    {
        Dungeon.instance.summonEnemy();
    }

    public override void takeDamage(int damage, damageTags tag = damageTags.Damage, spiritType? type = null)
    {
        if (hp <= 0)
        {
            return;
        }
        var damageTag = tag;
        if (type == currentWeakness)
        {
            damage *= 2;
            damageTag = damageTags.CriticalDamage;
        }
        else
        {
            damage /= 2;
            if (damage == 0)
            {
                damage = 1;
            }
        }

        base.takeDamage(damage, damageTag, type);
    }

    protected override void Start()
    {
        base.Start();
        Metronome.instance.onBeat += startGrooving;
        weakness = spiritType.none;
        updateWeakness();
    }

    private void startGrooving(object sender, System.EventArgs e)
    {
        bossAnimator.SetTrigger("Groove");
        bossAnimator.speed = 1f / Metronome.instance.getBeatLength();
        Metronome.instance.onBeat -= startGrooving;
    }
}
