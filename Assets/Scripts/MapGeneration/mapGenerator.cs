using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class mapGenerator : MonoBehaviour
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

    private int count = 0;
    private int[,] terrainMap; // 0 - not filled, 1 - filled

    int width;
    int height;

    public void startTileGeneration(int repetitions, int widthInput, int heightInput)
    {
        height = heightInput;
        width = widthInput;

        //clearMap(false);
        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPositions(); // random initialization
        }

        // generating tiles
        for (int i = 0; i < numberOfRepetitions; i++)
        {
            terrainMap = generateTilePositions(terrainMap);
        }

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
}
