using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MazGen;
using static WheatMaze;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Game,
        Win,
        Loose
    }

    public static GameController Instance { get; private set; }
    public GameObject player;
    public GameObject exitPortal_prefab;
    public int needed_coins = 20;
    public GameState current_state = GameState.Game;

    private bool portal_spawned = false;

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

        //SpawnExitPortal();
    }

    public void SpawnExitPortal()
    {
        if (portal_spawned)
            return;

        Instantiate(exitPortal_prefab, WheatMaze.Instance.ExitTile().GlobalPosition, exitPortal_prefab.transform.rotation);
    }

    public void VerminDied()
    {
        
    }

    public void PlayerEnterPortal()
    {
        current_state = GameState.Win;
        SceneManager.LoadScene("WinScene");
    }

    public void PlayerDied()
    {
        current_state = GameState.Loose;
        SceneManager.LoadScene("LoseScene");
    }

    void set_player()
    {
        MazeGridTile spawnTile = WheatMaze.Instance.GetRandomOpenTile();
        Vector3 spawn_pos = new Vector3(spawnTile.GlobalPosition.x + 0.5f, player.transform.position.y, spawnTile.GlobalPosition.z);
        player.transform.position = spawn_pos;
    }
}
