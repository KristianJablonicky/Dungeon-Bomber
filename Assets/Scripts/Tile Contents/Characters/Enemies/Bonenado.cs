using UnityEngine;

public class Bonenado : Enemy
{
    private int direction;
    private int changeDirectionCooldown;
    [SerializeField] private Animator bonenadoAnimator;

    protected override void Start()
    {
        base.Start();
        Metronome.instance.onBeat += startGrooving;
        direction = Random.Range(0, 4);
        changeDirectionCooldown = Random.Range(0, 8);
    }

    private void startGrooving(object sender, System.EventArgs e)
    {
        bonenadoAnimator.SetTrigger("Groove");
        bonenadoAnimator.speed = 1f / Metronome.instance.getBeatLength();
        Metronome.instance.onBeat -= startGrooving;
    }

    public override void collideWithCharacter(Character character)
    {
        character.takeDamage(getScaledDamage(2));
    }

    public override int getBaseMaxHp()
    {
        return 2;
    }
    protected override int getUpdateEveryNTicks()
    {
        return 2;
    }

    protected override int getPlayerDetectionRadius()
    {
        return 0;
    }

    protected override void onTick()
    {
        move((Movement)direction, 2);
        direction = (direction + 1) % 4;
        changeDirectionCooldown = (changeDirectionCooldown + 1) % 8;
        if (changeDirectionCooldown == 0)
        {
            direction = (direction + Random.Range(0, 3)) % 4;
        }
    }
}