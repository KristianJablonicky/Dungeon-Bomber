using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private TileContent tile, wall, ladder;
    [SerializeField] private Player player;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private List<Destructable> destructables;

    [SerializeField] private MapGenerator mapGenerator;


    [SerializeField] private GameObject hitSplatsParent;
    [SerializeField] private HitSplat hitSplat;

    private TileContent[,] layout;
    private int dungeonWidth = 22, dungeonHeight = 12;
    private int floor = 1;
    private Color currentFloorColor;

    public static Dungeon instance;
    private Player playerInstance;

    public event EventHandler playerSpawned, ladderReached;

    private void Update()
    {
        // Reset the run
        if (Input.GetKeyUp(KeyCode.R))
        {
            reset();
        }
        // Skip floor
        else if (Input.GetKeyUp(KeyCode.T))
        {
            callLadderReached();
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void reset()
    {
            DataStorage.instance.reset();
            nextFloor();
    }

    private void Awake()
    {
        instance = this;
        if (DataStorage.instance != null)
        {
            floor = DataStorage.instance.floor;
        }
        else
        {
            floor = 1;
        }

        if (floor == 1)
        {
            currentFloorColor = new Color(0.34f, 0.53f, 0.53f);
        }
        else if (floor == 2)
        {
            currentFloorColor = new Color(0.1f, 0.6f, 0.2f);
        }
        else if (floor == 3)
        {
            currentFloorColor = new Color(0.5f, 0.5f, 0.1f);
        }
        else
        {
            currentFloorColor = new Color(0.3f, 0.1f, 0.01f);
        }
        generateDungeon();

    }

    private void generateDungeon()
    {
        layout = new TileContent[dungeonWidth, dungeonHeight];
        mapGenerator.startTileGeneration(3, dungeonWidth, dungeonHeight);
        var obstacles = mapGenerator.getTerrainMap();


        for (int x = 0; x < dungeonWidth; x++)
        {
            for(int y = 0; y < dungeonHeight; y++)
            {
                if (x == 0 || y == 0 || x == (dungeonWidth - 1) || y == (dungeonHeight - 1))
                {
                    layout[x, y] = instantiate(wall, x, y);
                }
                else if (x == 1 && y == 1)
                {
                    instantiate(tile, x, y);
                    playerInstance = (Player)instantiate(player, x, y, false);
                    layout[x, y] = playerInstance;
                }
                else if (x == (dungeonWidth - 2) && y == (dungeonHeight / 2))
                {
                    instantiate(tile, x, y);
                    layout[x, y] = instantiate(ladder, x, y);
                }
                else if (obstacles[x, y] == 1)
                {
                    layout[x, y] = instantiate(wall, x, y);
                }
                else
                {
                    instantiate(tile, x, y);

                    if (UnityEngine.Random.Range(0, 15) == 0)
                    {
                        layout[x, y] = instantiate(enemies[UnityEngine.Random.Range(0, enemies.Count)], x, y);
                    }
                    else if (UnityEngine.Random.Range(0, 15) == 0)
                    {
                        layout[x, y] = instantiate(destructables[UnityEngine.Random.Range(0, destructables.Count)], x, y);
                    }
                }
            }
        }
    }

    private TileContent instantiate(TileContent content, int x, int y, bool setParent = true)
    {
        var tile = Instantiate(content, new Vector2(x, y), Quaternion.identity);
        if (setParent)
        {
            tile.transform.SetParent(transform);
        }
        return tile;
    }

    private TileContent instantiate(Character content, int x, int y, bool setParent = true)
    {
        var character = Instantiate(content, new Vector2(x, y), Quaternion.identity);
        if (setParent)
        {
            character.transform.SetParent(transform);
        }
        character.onHpChange += instantiateHitsplat;
        return character;
    }

    private void Start()
    {
        StartCoroutine(waitOneFrame());
    }

    private IEnumerator waitOneFrame()
    {
        yield return new WaitForEndOfFrame();
        playerSpawned?.Invoke(playerInstance, EventArgs.Empty);
    }
    public TileContent isTileOccupied(Vector3 tile)
    {
        int x = (int)tile.x;
        int y = (int)tile.y;
        return isTileOccupied(x, y);
    }
    public TileContent isTileOccupied(int x, int y)
    {
        if (x < 0 || y < 0 || x >= dungeonWidth || y >= dungeonHeight)
        {
            return layout[0, 0];
        }

        return layout[x, y];
    }

    public void removeFromTile(TileContent tile)
    {
        layout[tile.getX(), tile.getY()] = null;
    }
    public void removeFromTile(int x, int y)
    {
        layout[x,y] = null;
    }

    public void moveToTile(TileContent content, int x, int y)
    {
        layout[x,y] = content;
    }

    public void callLadderReached()
    {
        ladderReached?.Invoke(this, EventArgs.Empty);
    }

    public void nextFloor()
    {
        DataStorage.instance.floor++;
        DataStorage.instance.playerHp = playerInstance.getHp();
        SceneManager.LoadScene("Gameplay");
        /*
        floor++;
        currentFloorColor = Color.red;
        generateDungeon();
        */
    }

    public Color getFloorColor()
    {
        return currentFloorColor;
    }

    public Player getPlayer()
    {
        return playerInstance;
    }

    private void instantiateHitsplat(object sender, DamageArgs e)
    {
        if (e.damage == 0)
        {
            return;
        }
        var hitSplatInstance = Instantiate(hitSplat, ((Character)sender).transform.position, Quaternion.identity);
        hitSplatInstance.transform.SetParent(hitSplatsParent.transform, false);
        hitSplatInstance.transform.position = ((Character)sender).transform.position;
        hitSplatInstance.setUp(e.damage);
    }

}
