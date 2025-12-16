using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MazeGridTile = WheatMaze.MazeGridTile;

public class MazeObjectManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject ammoPrefab;
    public GameObject healthPrefab;

    public int maxCoins = 10;
    public int maxAmmo = 10;
    public int maxHealth = 10;

    public Dictionary<int, MazeGridTile> object_tiles = new Dictionary<int, MazeGridTile>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateObjects()
    {
        //maxCoins = Mathf.Min(GameController.Instance.needed_coins, maxCoins);

        if (maxCoins + maxAmmo + maxHealth >= WheatMaze.Instance.GetOpenTiles().Count)
        {
            Debug.LogError("ERROR: Combined obect counts cannot exceed number of open maze tiles!");
            return;
        }

        PopulateObjectType(maxCoins, SpawnCoin);
        PopulateObjectType(maxAmmo, SpawnAmmo);
        PopulateObjectType(maxHealth, SpawnHealth);
    }

    private void PopulateObjectType(int max, Action<MazeGridTile> func)
    {
        int num = 0;

        while (num < max)
        {
            MazeGridTile perspective_tile = WheatMaze.Instance.GetRandomOpenTile();
            if (object_tiles.ContainsKey(perspective_tile.ID))
            {
                continue;
            }
            object_tiles[perspective_tile.ID] = perspective_tile;
            func(perspective_tile);
            num++;
        }
    }

    private void SpawnCoin(MazeGridTile new_tile)
    {
        Instantiate(coinPrefab, new_tile.GlobalPosition, Quaternion.identity, WheatMaze.Instance.transform);
    }

    private void SpawnAmmo(MazeGridTile new_tile)
    {
        Instantiate(ammoPrefab, new_tile.GlobalPosition, Quaternion.identity, WheatMaze.Instance.transform);
    }

    private void SpawnHealth(MazeGridTile new_tile)
    {
        Instantiate(healthPrefab, new_tile.GlobalPosition, Quaternion.identity, WheatMaze.Instance.transform);
    }
}
