using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TileManager : MonoBehaviour
{
    public QuestionManager questionManager;
    private List<GameObject> activeTiles;
    public GameObject[] tilePrefabs;
    public GameObject portalTilePrefab;

    public float tileLength = 30;
    public int numberOfTiles = 3;
    public int totalNumOfTiles = 8;

    public float zSpawn = 0;

    public bool isForPlayer1;

    private Transform playerTransform;

    private int previousIndex;

    void Start()
    {
        activeTiles = new List<GameObject>();
        for (var i = 0; i < numberOfTiles; i++)
        {
            if (i == 0) SpawnTile();
            else SpawnTile(Random.Range(0, totalNumOfTiles));
        }
        SpawnTile(8);

        playerTransform = GameObject.Find(isForPlayer1 ? "Player1" : "Player2").transform;

    }
    void Update()
    {
        if (!(playerTransform.position.z - 30 >= zSpawn - numberOfTiles * tileLength)) return;

        var index = Random.Range(0, totalNumOfTiles);
        while (index == previousIndex) index = Random.Range(0, totalNumOfTiles);
        if (activeTiles.IndexOf(portalTilePrefab) == 0) index = 8;

        DeleteTile();
        SpawnTile(index);

    }

    public void SpawnTile(int index = 0)
    {
        if (index == 8)
        {
            portalTilePrefab.transform.position = Vector3.forward * zSpawn + (isForPlayer1 ? new Vector3(25, 0, 0) : new Vector3(-25, 0, 0));
            portalTilePrefab.transform.rotation = Quaternion.identity;
            portalTilePrefab.SetActive(true);

            activeTiles.Add(portalTilePrefab);
            zSpawn += tileLength;
            previousIndex = index;
            return;
        }
        
        var tile = tilePrefabs[index];
        
        for (var i = 8; i <= 56 && tile.activeInHierarchy; i += 8) tile = tilePrefabs[index + i];

        tile.transform.position = Vector3.forward * zSpawn + (isForPlayer1 ? new Vector3(-25, 0, 0) : new Vector3(25, 0, 0));
        tile.transform.rotation = Quaternion.identity;
        tile.SetActive(true);

        activeTiles.Add(tile);
        zSpawn += tileLength;
        previousIndex = index;
    }

    private void DeleteTile()
    {
        if (activeTiles[2] == portalTilePrefab) SubmitAnswer();
        activeTiles[0].SetActive(false);
        activeTiles.RemoveAt(0);
    }

    private void SubmitAnswer()
    {
        var lane = playerTransform.GetComponent<PlayerController>().desiredLane;
        if (isForPlayer1) questionManager.AnswerQuestionPlayer1(lane);
        else questionManager.AnswerQuestionPlayer2(lane);
    }
}
