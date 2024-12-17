using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private TileContent tile, wall, ladder;
    [SerializeField] private Player player;
    [SerializeField] private Boss boss;
    [SerializeField] private BossHitbox bossHitbox;

    [SerializeField] private BossHealthBar bossHealthBar;

    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private List<Destructable> destructables;

    [SerializeField] private MapGenerator mapGenerator;


    [SerializeField] private GameObject hitSplatsParent;
    [SerializeField] private HitSplat hitSplat;

    private TileContent[,] layout;
    private int dungeonWidth = 27, dungeonHeight = 12;
    private int floor = 1;
    private Color currentFloorColor;

    public static Dungeon instance;
    private Player playerInstance;
    private Boss bossInstance;

    public event EventHandler playerSpawned, ladderReached, enemyKilled;

    private void Update()
    {
        // Reset the run
        if (Input.GetKeyUp(KeyCode.R))
        {
            playerInstance.die();
            //reset();
        }
        // Skip floor
        else if (Input.GetKeyUp(KeyCode.T))
        {
            callLadderReached();
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            DataStorage.instance.resetCurrecies();
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    private IEnumerator resetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        reset();
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
            if (floor == 1) // fix for a tiny bug
            {
                DataStorage.instance.resetStartingGold();
            }
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
            currentFloorColor = new Color(0.14f, 0.14f, 0.35f);
        }
        else
        {
            currentFloorColor = new Color(0.3f, 0.1f, 0.01f);
        }

        if (floor == 4)
        {
            generateBossRoom();
        }
        else
        {
            generateDungeon();
        }
    }

    private void generateBossRoom()
    {
        dungeonWidth = 12;
        dungeonHeight = 8;
        layout = new TileContent[dungeonWidth, dungeonHeight];
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
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
                else if (x == 5 && y == 3)
                {
                    instantiate(tile, x, y);
                    bossInstance = (Boss)instantiate(boss, x, y, false);
                    layout[x, y] = bossInstance;
                    for (int xOffset = 0; xOffset <= 1; xOffset++)
                    {
                        for (int yOffset = 0; yOffset <= 1; yOffset++)
                        {
                            if (xOffset == 0 && yOffset == 0)
                            {
                                continue;
                            }
                            var bossHitboxInstance = (BossHitbox)instantiate(bossHitbox, x + xOffset, y + yOffset, false);
                            layout[x + xOffset, y + yOffset] = bossHitboxInstance;
                            bossHitboxInstance.setUp(bossInstance);
                        }
                    }

                    bossHealthBar.setUp(bossInstance);

                }
                else if ((x == 3 && (y == 2 || y == 5)) ||
                    (x == 8 && (y == 2 || y == 5)))
                {
                    layout[x, y] = instantiate(wall, x, y);
                }
                else
                {
                    instantiate(tile, x, y);
                }
            }
        }
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
                else if (x == mapGenerator.getPlayerX() && y == mapGenerator.getPlayerY())
                {
                    instantiate(tile, x, y);
                    playerInstance = (Player)instantiate(player, x, y, false);
                    layout[x, y] = playerInstance;
                }
                else if (x == mapGenerator.getLadderX() && y == mapGenerator.getLadderY())
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

                    if ((x > 6 || y > 6 ) && UnityEngine.Random.Range(0, 15) == 0)
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
        character.hpChanged += instantiateHitsplat;
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
        DataStorage.instance.playerMaxHp = playerInstance.getMaxHp();
        DataStorage.instance.playerExp = playerInstance.getExp();
        DataStorage.instance.playerLevel = playerInstance.getPlayerLevel();
        SceneManager.LoadScene("Gameplay");
        /*
        floor++;
        currentFloorColor = Color.red;
        generateDungeon();
        */
    }

    public int getFloor()
    {
        return floor;
    }
    public Color getFloorColor()
    {
        return currentFloorColor;
    }

    public Player getPlayer()
    {
        return playerInstance;
    }

    public Boss? getBoss()
    {
        return bossInstance;
    }

    private void instantiateHitsplat(object sender, DamageArgs e)
    {
        if (e.amount == 0)
        {
            return;
        }
        var senderC = sender as Character;
        if (senderC.getHitSplatInstance() != null)
        {
            return;
        }
        var hitSplatInstance = Instantiate(hitSplat, senderC.transform.position, Quaternion.identity);
        hitSplatInstance.transform.SetParent(hitSplatsParent.transform, false);
        hitSplatInstance.transform.position = senderC.transform.position;
        hitSplatInstance.setUp(e, senderC);
        senderC.setHitSplatInstance(hitSplatInstance);
    }

    public void onEnemyKilled()
    {
        enemyKilled?.Invoke(playerInstance, EventArgs.Empty);
    }

    public int getHeight()
    {
        return dungeonHeight;
    }

    public int getWidth()
    {
        return dungeonWidth;
    }

    public void summonEnemy()
    {
        var emptyTileIndex = getEmptyTile();
        if (emptyTileIndex.HasValue)
        {
            int x = emptyTileIndex.Value.Item1, y = emptyTileIndex.Value.Item2;
            layout[x, y] = instantiate(enemies[UnityEngine.Random.Range(0, enemies.Count)], x, y);
        }
    }

    private (int, int)? getEmptyTile()
    {
        var emptyTiles = new List<(int x, int y)>();
        for (int x = 0; x < layout.GetLength(0); x++)
        {
            for (int y = 0; y < layout.GetLength(1); y++)
            {
                if (layout[x, y] == null)
                {
                    emptyTiles.Add((x, y));
                }
            }
        }
        if (emptyTiles.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyTiles.Count);

        return emptyTiles[randomIndex];
    }

}
