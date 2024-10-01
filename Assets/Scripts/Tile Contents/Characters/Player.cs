using System.Collections;
using UnityEngine;

public class Player : Character
{

    [SerializeField] private Bomb bomb;
    private int horizontalLength = 2, verticalLength = 2, diagonalLength = 2, areaSize = 1;
    private int ticksUntilExplosion = 2, damage = 1;
    private int bombCoolDown = 3, currentCoolDown = 0;

    public bool movementEnabled = false, placedBombAlready = false;

    private bombTypes currentBombType = bombTypes.square;

    public override int getMaxHp()
    {
        return 3;
    }
    protected override void Start()
    {
        base.Start();
        /*
        metronome.userInputStart += enableInput;
        metronome.userInputEnd += disableInput;
        */
        StartCoroutine(fallDown());
        metronome.onPlayerInputStart += enableInput;
        //metronome.onBombUpdate += disableInput;

        metronome.onBeat += placeBomb;
    }

    private void placeBomb(object sender, System.EventArgs e)
    {
        if (currentCoolDown < 0 && !placedBombAlready)
        {
            spawnBomb();
        }
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
        metronome.onPlayerInputStart -= enableInput;
        //metronome.onBombUpdate -= disableInput;
        base.die();
    }

    private void enableInput(object sender, System.EventArgs e)
    {
        currentCoolDown--;

        placedBombAlready = false;
        movementEnabled = true;
    }
    private void disableInput(object sender, System.EventArgs e)
    {
        movementEnabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBombType = bombTypes.square;
        }
        else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBombType = bombTypes.plus;
        }
        else if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBombType = bombTypes.x;
        }


        if (movementEnabled)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                jump(Movement.Right);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                jump(Movement.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                jump(Movement.Down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                jump(Movement.Left);
            }
            /*
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                if (currentCoolDown == 0)
                {
                    spawnBomb();
                    currentCoolDown = bombCoolDown;
                }
            }
            */
        }
    }
    private Movement delayedMovementDirection;
    private void jump(Movement movement)
    {
        if (metronome.isInPlayerWindowInputStart())
        {
            metronome.prematureBeat();
            /*
            metronome.onBeatLowerPriority += delayMove;
            delayedMovementDirection = movement;
            disableInput(this, System.EventArgs.Empty);
            return;
            */
        }
        int sourceX = x, sourceY = y;
        var collider = move(movement);
        if (collider is Ladder)
        {
            ((Ladder)collider).onPlayerContact();
        }

        if (currentCoolDown < 0)
        {
            spawnBomb();
            placedBombAlready = true;
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
        currentCoolDown = bombCoolDown;
        var newBomb = Instantiate(bomb, new Vector3(x, y), Quaternion.identity);
        int[] shape = { 0, 0, 0 };
        shape[(int)currentBombType] = 1;
        newBomb.setUp(ticksUntilExplosion, damage, shape[1] * horizontalLength, shape[1] * verticalLength,
            shape[2] * diagonalLength, shape[0] * areaSize);

        //newBomb.setUp(ticksUntilExplosion, damage, horizontalLength, verticalLength, diagonalLength, areaSize);
    }

    public override void collideWithCharacter(Character character)
    {
        if (character is Enemy)
        {
            ((Enemy)character).collideWithCharacter(this);
        }
    }


    private enum bombTypes
    {
        square,
        plus,
        x
    }

}
