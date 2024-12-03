using UnityEngine;

public class Bonenado : Enemy
{
    private int direction;
    private int speed = 2;
    private int beatCounter = 0;
    protected float randomMoveChance = 0.1f;
    [SerializeField] private Animator bonenadoAnimator;

    private void Awake()
    {
        direction = Random.Range(0, 4);
    }
    protected override void Start()
    {
        base.Start();
        Metronome.instance.onBeat += startGrooving;
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

    protected override int getPlayerDetectionRadius()
    {
        return 0;
    }

    protected override void onTick()
    {
        beatCounter++;
        if (beatCounter % speed == 0)
        {
            if (randomMoveChance > UnityEngine.Random.value)
            {
                move((Movement)Random.Range(0, 4), 2);
                return;
            }
            move((Movement)direction, 2);
            direction++;
            direction %= 4;
        }
        if (beatCounter == 32) beatCounter = 0;
    }
}
