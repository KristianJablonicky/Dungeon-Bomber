using System.Collections;
using UnityEngine;

public class Player : Character
{

    [SerializeField] private Bomb bomb;
    private int horizontalLength = 2, verticalLength = 2, diagonalLength = 0, areaSize = 1;
    private int ticksUntilExplosion = 2, damage = 1;
    int bombCoolDown = 3, currentCoolDown = 0;
    public override int getMaxHp()
    {
        return 3;
    }
    protected override void Start()
    {
        base.Start();
        enabled = false;
        /*
        metronome.userInputStart += enableInput;
        metronome.userInputEnd += disableInput;
        */
        StartCoroutine(fallDown());
        metronome.userInputStart += enableInput;
        metronome.onBombUpdate += disableInput;
    }

    private IEnumerator fallDown()
    {
        float duration = metronome.getBeatLength() * 0.5f, timeElapsed = 0;
        Vector3 start = transform.position + new Vector3(0, 1), end = transform.position;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.position = start - new Vector3(0, Mathf.Sqrt(timeElapsed / duration));
            
            yield return null;
        }
        transform.position = end;
    }

    public override void die()
    {
        metronome.userInputStart -= enableInput;
        metronome.onBombUpdate -= disableInput;
        base.die();
    }

    private void enableInput(object sender, System.EventArgs e)
    {
        enabled = true;
        if (currentCoolDown > 0)
        {
            currentCoolDown--;
        }
    }
    private void disableInput(object sender, System.EventArgs e)
    {
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            jump(Movement.Right);
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            jump(Movement.Up);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            jump(Movement.Down);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            jump(Movement.Left);
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Keypad0))
        {
            if (currentCoolDown == 0)
            {
                spawnBomb();
                currentCoolDown = bombCoolDown;
            }
        }
    }
    private Movement delayedMovementDirection;
    private void jump(Movement movement)
    {
        if (metronome.isInPlayerWindowInputStart())
        {
            metronome.onBeatLowerPriority += delayMove;
            delayedMovementDirection = movement;
            disableInput(this, System.EventArgs.Empty);
            return;
        }
        int sourceX = x, sourceY = y;
        var collider = move(movement);
        if (collider is Ladder)
        {
            ((Ladder)collider).onPlayerContact();
        }
        disableInput(this, System.EventArgs.Empty);
    }

    private void delayMove(object sender, System.EventArgs e)
    {
        jump(delayedMovementDirection);
        metronome.onBeatLowerPriority -= delayMove;
    }
    private class sourceAndDestinationTiles
    {
        public int sourceX, soureY, destinationX, destinationY;
    }

    private void delayMovement()
    {

    }

    private void spawnBomb()
    {
        var newBomb = Instantiate(bomb, transform.position, Quaternion.identity);
        newBomb.setUp(ticksUntilExplosion, damage, horizontalLength, verticalLength, diagonalLength, areaSize);
        disableInput(this, System.EventArgs.Empty);
    }

    public override void collideWithCharacter(Character character)
    {
        if (character is Enemy)
        {
            ((Enemy)character).collideWithCharacter(this);
        }
    }
}
