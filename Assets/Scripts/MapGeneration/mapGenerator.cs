using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    /*
     * Inspiration:
     * https://www.youtube.com/watch?v=xNqqfABXTNQ
     */

    [Range(0, 100)] public int initialChance;
    [Range(1, 8)] public int birthLimit;
    [Range(1, 8)] public int deathLimit;
    [Range(1, 10)] public int numberOfRepetitions;
    public Vector3Int tmapSize;
    public Tilemap topMap;
    public Tilemap bottomMap; // bottom tile/not filled tile/lava/water - in our case nothing???
    public Tile topTile;
    //public Tile bottomTile; // we may not have this

    private int[,] terrainMap; // 0 - not filled, 1 - filled
    private int[,] pathToLadder;
    private bool[,] visitedMap;

    int width;
    int height;

    private int playerStartX = 1;
    private int playerStartY;
    private int ladderX;
    private int ladderY;

    public void startTileGeneration(int repetitions, int widthInput, int heightInput)
    {
        height = heightInput;
        width = widthInput;
        playerStartY = Random.Range(1, height - 1);
        visitedMap = new bool[width, height];

        //clearMap(false);
        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            pathToLadder = new int[width, height];
            initPositions(); // random initialization
        }

        // generating tiles
        for (int i = 0; i < numberOfRepetitions; i++)
        {
            terrainMap = generateTilePositions(terrainMap);
        }
        generateNewPath();
        printMap(pathToLadder);
        mergeMaps();

        /*
         * Either set the tiles here or return terrainMap itself.
         * TerrainMap is 2D array of 0s and 1s, which can be used to generate maps.
         */
        //for (int w = 0; w < width; w++)
        //{
        //    for (int h = 0; h < height; h++)
        //    {
        //        if (terrainMap[w, h] == 1)
        //        {
        //            topMap.SetTile(new Vector3Int(-w + width / 2, -h + height / 2), topTile);
        //        }
        //    }
        //}
    }

    public int[,] getTerrainMap()
    {
        return terrainMap;
    }

    public void clearMap(bool complete)
    {
        // clear in case maps are not empty
        topMap.ClearAllTiles();
        bottomMap.ClearAllTiles();

        if (complete)
        {
            terrainMap = null;
            width = tmapSize.x; height = tmapSize.y;
        }
    }

    public void initPositions()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                terrainMap[w, h] = Random.Range(1, 101) < initialChance ? 1 : 0; // initiate random terrain map
                pathToLadder[w, h] = 1;
            }
        }
    }

    public int[,] generateTilePositions(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighbours;
        BoundsInt boundingBox = new BoundsInt(-1, -1, 0, 3, 3, 1); // 3x3 bounding box to check if neighbouring tiles are empty or not

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++) // check neighbour count inside bb
            {
                neighbours = 0;
                foreach (var box in boundingBox.allPositionsWithin)
                {
                    if (box.x == 0 && box.y == 0)
                        continue;
                    if (
                        w + box.x >= 0 &&
                        w + box.x < width &&
                        h + box.y >= 0 &&
                        h + box.y < height
                    )
                    {
                        neighbours += oldMap[w + box.x, h + box.y];
                    }
                    else
                        neighbours++;
                }

                // checking death and birth limits
                if (oldMap[w, h] == 1)
                {
                    if (neighbours < deathLimit)
                        newMap[w, h] = 0;
                    else
                        newMap[w, h] = 1;
                }
                else
                {
                    if (neighbours > birthLimit)
                        newMap[w, h] = 1;
                    else
                        newMap[w, h] = 0;
                }
            }
        }
        return newMap;
    }
    
    private void printMap(int[,] map)
    {
        string output = "";
        for (int y = height - 1; y > 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                output += map[x, y].ToString();
            }
            output += "\n";
        }
        Debug.Log(output);
    }
    

    private void generateNewPath()
    {
        void ClearPath(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                pathToLadder[x, y] = 0;
            }
        }

        void ClearAdjacent(int x, int y, bool directionVertical)
        {
            ClearPath(x, y);
            if (directionVertical)
            {
                if (x + 1 < width)
                {
                    ClearPath(x + 1, y);
                }
                else
                {
                    ClearPath(x - 1, y);
                }
            }
            else
            {
                if (y + 1 < height - 1)
                {
                    ClearPath(x, y + 1);
                }
                else
                {
                    ClearPath(x, y - 1);
                }
            }
        }

        bool[,] visitedMapSecond = new bool[width, height];
        int currentY = playerStartY;
        int currentX = 1;
        int randomDirection = 0;
        int momentum = 1;
        bool isMomentum = false;
        int currentMomentum = 0;

        ClearAdjacent(currentX, currentY, false);
        visitedMapSecond[currentX, currentY] = true;

        while (currentX != width - 2)
        {
            if (!isMomentum)
            {
                isMomentum = true;
                randomDirection = Random.Range(0, 3);
                momentum = Random.Range(1, 5);
                currentMomentum = momentum;
            }
            
            if (randomDirection == 0)       // to up
            {
                if (currentY + 1 < height - 1 && !visitedMapSecond[currentX, currentY + 1])
                {
                    currentY++;
                    ClearAdjacent(currentX, currentY, true);
                    visitedMapSecond[currentX, currentY] = true;
                    momentum--;
                    if (momentum <= 0)
                    {
                        isMomentum = false;
                    }
                }
                else
                {
                    isMomentum = false;
                }
            }
            else if (randomDirection == 1)  // to right
            {
                if (currentX + 1 < width - 1 && !visitedMapSecond[currentX + 1, currentY])
                {
                    currentX++;
                    ClearAdjacent(currentX, currentY, false);
                    visitedMapSecond[currentX, currentY] = true;
                    momentum--;
                    if (momentum <= 0)
                    {
                        isMomentum = false;
                    }
                }
                else
                {
                    isMomentum = false;
                }
            }
            else   // to down
            {
                if (currentY - 1 > 1 && !visitedMapSecond[currentX, currentY - 1])
                {
                    currentY--;
                    ClearAdjacent(currentX, currentY, true);
                    visitedMapSecond[currentX, currentY] = true;
                    momentum--;
                    if (momentum <= 0)
                    {
                        isMomentum = false;
                    }
                }
                else
                {
                    isMomentum = false;
                }
            }
        }
        ladderX = currentX;
        ladderY = currentY;
    }

    private void ensureConnectivity()
    {
        void DFS(int x, int y)
        {
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1 && !visitedMap[x, y] && terrainMap[x, y] == 0)
            {
                visitedMap[x, y] = true;
                DFS(x, y + 1);
                DFS(x + 1, y);
                DFS(x, y - 1);
                DFS(x - 1, y);
            }
        }
        
        DFS(playerStartX, playerStartY);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (terrainMap[x, y] == 0 && !visitedMap[x, y])
                {
                    terrainMap[x, y] = 1;
                }
            }
        }
    }

    private void mergeMaps()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] *= pathToLadder[x, y];
            }
        }
        ensureConnectivity();
    }

    public int getPlayerX()
    {
        return playerStartX;
    }
    public int getPlayerY()
    {
        return playerStartY;
    }
    public int getLadderX()
    {
        return ladderX;
    }
    public int getLadderY()
    {
        return ladderY;
    }
}
