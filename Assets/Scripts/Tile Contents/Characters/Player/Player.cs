using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Bomb bomb;
    public BombAttributes bombSquare, bombPlus, bombX;
    private int bombCoolDown = 3, currentCoolDown = 0;
    public bool movementEnabled = false, placedBombAlready = false;

    public bombTypes currentBombType = bombTypes.square;

    public EventHandler bombChanged, positionUpdated;

    public override int getBaseMaxHp()
    {
        return 3;
    }

    public void increaseMaxHp(int increase)
    {
        maxHp += increase;
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(fallDown());
        metronome.onPlayerInputStart += enableInput;

        metronome.onBeat += placeBomb;

        setUpAttributes();
        if (DataStorage.instance.floor != 1)
        {
            hp = DataStorage.instance.playerHp;
        }
        DataStorage.instance.equipUpgrades(this);
        heal(0);
        
    }

    private void setUpAttributes()
    {
        bombSquare = new()
        {
            horizontalLength = 1,
            verticalLength = 1,
            diagonalLength = 1
        };

        bombPlus = new()
        {
            horizontalLength = 2,
            verticalLength = 2
        };

        bombX = new()
        {
            diagonalLength = 2
        };
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
        metronome.onBeat -= placeBomb;
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
            changeBomb(bombTypes.square);
        }
        else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeBomb(bombTypes.plus);
        }
        else if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeBomb(bombTypes.x);
        }

        else if (Input.GetKeyDown(KeyCode.H))
        {
            var sacrifice = new Sacrifice();
            sacrifice.equipEffect(this);
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

    public void changeBomb(bombTypes type)
    {
        currentBombType = type;
        bombChanged?.Invoke(this, EventArgs.Empty);
    }

    private Movement delayedMovementDirection;
    public void jump(Movement movement)
    {
        if (!movementEnabled)
        {
            return;
        }

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

    private void delayMove(object sender, EventArgs e)
    {
        jump(delayedMovementDirection);
        metronome.onBeatLowerPriority -= delayMove;
    }

    protected override void movementMethod()
    {
        positionUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void spawnBomb()
    {
        currentCoolDown = bombCoolDown;
        var newBomb = Instantiate(bomb, new Vector3(x, y), Quaternion.identity);
        int[] shape = { 0, 0, 0 };
        shape[(int)currentBombType] = 1;
        /*
        newBomb.setUp(ticksUntilExplosion, damage, shape[1] * horizontalLength, shape[1] * verticalLength,
            shape[2] * diagonalLength, shape[0] * areaSize);
        */
        BombAttributes attributes = bombSquare;
        if (currentBombType == bombTypes.square)
        {
            attributes = bombSquare;
        }
        else if (currentBombType == bombTypes.plus)
        {
            attributes = bombPlus;
        }
        else if (currentBombType == bombTypes.x)
        {
            attributes = bombX;
        }

        newBomb.setUp(attributes.ticksUntilExplosion, attributes.damage,
            attributes.horizontalLength, attributes.verticalLength, attributes.diagonalLength);
        //newBomb.setUp(ticksUntilExplosion, damage, horizontalLength, verticalLength, diagonalLength, areaSize);
    }

    public override void collideWithCharacter(Character character)
    {
        if (character is Enemy)
        {
            ((Enemy)character).collideWithCharacter(this);
        }
    }
}
public enum bombTypes
{
    square,
    plus,
    x
}
