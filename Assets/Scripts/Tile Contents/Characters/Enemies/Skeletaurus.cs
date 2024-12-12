using UnityEngine;

public class Skeletaurus : Enemy
{
    [SerializeField] private Animator skeletaurusAnimator;

    private bool isPreparing = false;
    private bool isCharging = false;
    private bool isResting = false;
    private int moveTick;
    Movement chargeMovement = Movement.Right;

    // Start is called before the first frame update

    public override void collideWithCharacter(Character character)
    {
        if (character is Player && isCharging)
        {
            character.takeDamage(getScaledDamage(3));
        }
        else if (character is Player)
        {
            character.takeDamage(getScaledDamage(1));
        }
    }
    protected override void Start()
    {
        base.Start();
        moveTick = Random.Range(0, 2);
        metronome.onBeat += startGrooving;
    }

    private void startGrooving(object sender, System.EventArgs e)
    {
        metronome.onBeat -= startGrooving;
        skeletaurusAnimator.speed = 1f / Metronome.instance.getBeatLength();
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
        var player = dungeon.getPlayer();
        int playerX = player.getX();
        int playerY = player.getY();

        int enemyX = getX();
        int enemyY = getY();

        if (enemyY != playerY && !isPreparing && !isCharging && !isResting && moveTick % 2 == 1)
        {
            moveTick = 0;
        }

        else if (enemyY != playerY && !isPreparing && !isCharging && !isResting)
        {
            if (enemyY < playerY)
            {
                move(Movement.Up);
            }
            else 
            {
                move(Movement.Down);
            }
            moveTick++;
        }

        else if (enemyY == playerY && !isPreparing && !isCharging)
        {
            isPreparing = true;
            skeletaurusAnimator.SetTrigger("Prepare");
            moveTick = 1;
        }

        else if (isPreparing)
        {
            if (moveTick == 4)
            {
                isPreparing = false;
                isCharging = true;
                skeletaurusAnimator.SetTrigger("Charge");
                if (enemyX > playerX)
                {
                    chargeMovement = Movement.Left;
                }
                else
                {
                    chargeMovement = Movement.Right;
                }
            }
            else
            {
                moveTick++;
            }
        }
        else if (isCharging)
        {
            Vector3 targetPosition1;
            Vector3 targetPosition2;
            bool crashed = false;

            if (chargeMovement == Movement.Left)
            {
                targetPosition1 = new Vector3(enemyX - 1, enemyY);
                targetPosition2 = new Vector3(enemyX - 2, enemyY);
            }
            else
            {
                targetPosition1 = new Vector3(enemyX + 1, enemyY);
                targetPosition2 = new Vector3(enemyX + 2, enemyY);   
            }

            if (Dungeon.instance.isTileOccupied(targetPosition1) || Dungeon.instance.isTileOccupied(targetPosition2))
            {
                crashed = true;
            }

            move(chargeMovement, 2);
            if(crashed)
            {
                isCharging = false;
                isResting = true;
                skeletaurusAnimator.SetTrigger("Rest");
                moveTick = 0;
            }
        }

        else if (isResting)     //DONE
        {
            moveTick++;
            if (moveTick == 4)
            {
                moveTick = 0;
                isResting = false;
                skeletaurusAnimator.SetTrigger("GoIdle");
            }
        }
    }
}
