using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WheatMaze;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public GameObject player;
    public int needed_coins = 20;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Attempted to spawn more than one VerminController!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupMap()
    {
        WheatMaze.Instance.GenerateMaze();
        VerminController.Instance.SpawnInitialVermin();
        set_player();
    }

    void set_player()
    {
        MazeGridTile spawnTile = WheatMaze.Instance.GetRandomOpenTile();
        Vector3 spawn_pos = new Vector3(spawnTile.GlobalPosition.x + 0.5f, player.transform.position.y, spawnTile.GlobalPosition.z);
        player.transform.position = spawn_pos;
    }
}
