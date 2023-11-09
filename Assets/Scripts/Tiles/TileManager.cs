﻿using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<GameObject> activeTiles;
    public GameObject[] tilePrefabs;

    public float tileLength = 30;
    public int numberOfTiles = 3;
    public int totalNumOfTiles = 8;

    public float zSpawn = 0;

    private Transform playerTransform;

    private int previousIndex;

    void Start()
    {
        activeTiles = new List<GameObject>();
        for (var i = 0; i < numberOfTiles; i++)
        {
            if(i==0)
                SpawnTile();
            else
                SpawnTile(Random.Range(0, totalNumOfTiles));
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void Update()
    {
        if (!(playerTransform.position.z - 30 >= zSpawn - (numberOfTiles * tileLength))) return;
        
        var index = Random.Range(0, totalNumOfTiles);
        while(index == previousIndex) index = Random.Range(0, totalNumOfTiles);

        DeleteTile();
        SpawnTile(index);

    }

    public void SpawnTile(int index = 0)
    {
        // ToDo: Clean this up into a loop
        var tile = tilePrefabs[index];
        if (tile.activeInHierarchy)
            tile = tilePrefabs[index + 8];

        if(tile.activeInHierarchy)
            tile = tilePrefabs[index + 16];
        
        if(tile.activeInHierarchy)
            tile = tilePrefabs[index + 24];
        
        if(tile.activeInHierarchy)
            tile = tilePrefabs[index + 32];
        
        if(tile.activeInHierarchy)
            tile = tilePrefabs[index + 40];
        
        if(tile.activeInHierarchy)
            tile = tilePrefabs[index + 48];
        
        if(tile.activeInHierarchy)
            tile = tilePrefabs[index + 56];

        tile.transform.position = Vector3.forward * zSpawn;
        tile.transform.rotation = Quaternion.identity;
        tile.SetActive(true);

        activeTiles.Add(tile);
        zSpawn += tileLength;
        previousIndex = index;
    }

    private void DeleteTile()
    {
        activeTiles[0].SetActive(false);
        activeTiles.RemoveAt(0);
        // PlayerManager.score += 3;
    }
}
