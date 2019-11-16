using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform levelParent;

    [Range(0,100)]
    [SerializeField] private int initChance;
    [Range(1, 8)]
    [SerializeField] private int birthLimit;
    [Range(1, 8)]
    [SerializeField] private int deathLimit;
    [Range(1, 10)]
    [SerializeField] private int numR;

    private int count = 0;

    private int[,] terrainMap;
    public Vector3Int tileMapSize;

    public Tilemap TopMap;
    public Tilemap bottomMap;

    public Tile topTile;
    public Tile bottomTile;

    private int width;
    private int height;

    public Transform LevelParent { get => levelParent; set => levelParent = value; }
    public int InitChance { get => initChance; set => initChance = value; }
    public int DeathLimit { get => deathLimit; set => deathLimit = value; }
    public int BirthLimit { get => birthLimit; set => birthLimit = value; }
    public int NumR { get => numR; set => numR = value; }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DoSim(NumR);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ClearMaps(true);
        }
    }
    public void DoSim(int numR) {

        ClearMaps(false);
        width = tileMapSize.x;
        height = tileMapSize.y;

        if (terrainMap == null) {

            terrainMap = new int[width, height];
            InitPos();
        }

        for (int i = 0; i < numR; i++)
        {
            terrainMap = GetTilePos(terrainMap);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 1) {
                    if(TopMap != null)
                    TopMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                    if(bottomMap != null)
                    bottomMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), bottomTile);
                }
            }
        }
    }

    private int[,] GetTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];

        int neighbour;

        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighbour = 0;
                foreach (var b in myB.allPositionsWithin) {

                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                    {
                        neighbour += oldMap[x + b.x, y + b.y];
                    }
                    else {
                        neighbour++;
                    }
                }
                if (oldMap[x, y] == 1)
                {
                    if (neighbour < deathLimit)
                    {
                        newMap[x, y] = 0;
                    }
                    else newMap[x, y] = 1;
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighbour < BirthLimit)
                    {
                        newMap[x, y] = 1;
                    }
                    else newMap[x, y] = 0;
                }
            }
        }

        return newMap;
    }

    private void InitPos()
    {
         for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = UnityEngine.Random.Range(1, 101) < InitChance ? 1 : 0; 
            }
        }
         
    }

    private void ClearMaps(bool complete)
    {
        if(TopMap != null)
        TopMap.ClearAllTiles();
        if(bottomMap != null)
        bottomMap.ClearAllTiles();
        if (complete)
        {
            terrainMap = null;

        }
    }
}
