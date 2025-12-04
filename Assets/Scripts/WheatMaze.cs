using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StopWatch = System.Diagnostics.Stopwatch;

public class WheatMaze : MonoBehaviour
{
    public static WheatMaze Instance { get; private set; }

    public int maze_size_x;
    public int maze_size_y;
    public int maze_smooth;
    public GameObject[] mazeWallPrefab;
    public GameObject player;

    public int tex_size_x;
    public int tex_size_y;

    bool ready = false;
    Texture mazTexture;
    MazGen maze;

    MazeObjectManager mazeObjects;

    public class MazeGridTile : System.IEquatable<MazeGridTile>
    {
        public Vector2Int GridPosition { get; private set; }
        public Vector3 LocalPosition { get; private set; }
        public Vector3 GlobalPosition { get; private set; }
        public bool filled { get; private set; }

        public int ID { get; private set; }

        public GameObject WallObject { get; set; }

        public MazeGridTile(int x, int y, int id, bool is_filled)
        {
            ID = id;
            GridPosition = new Vector2Int(x, y);
            filled = is_filled;

            LocalPosition = new Vector3(x, 0, y);
            GlobalPosition = Instance.transform.position + LocalPosition;
        }

        public void Init(GameObject prefab, Transform parent)
        {
            WallObject = (GameObject)Instantiate(prefab, parent);
            WallObject.transform.localPosition = LocalPosition;
            GlobalPosition = WallObject.transform.position;
        }

        public bool Equals(MazeGridTile other)
        {
            return ID == other.ID;
            //return GridPosition.Equals(other.GridPosition);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }

    List<MazeGridTile> WallTiles = new List<MazeGridTile>();
    List<MazeGridTile> OpenTiles = new List<MazeGridTile>();
    MazeGridTile exitTile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate Maze script attempted!");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        mazeObjects = GetComponent<MazeObjectManager>();
        GenerateMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (!ready)
            return;
        GUI.DrawTexture(
            new Rect(
                20,
                20,
                tex_size_x, tex_size_y),
            mazTexture);
    }

    void CreateMazeWalls()
    {
        foreach (var tile in WallTiles)
        {
            tile.Init(mazeWallPrefab[Random.Range(0, 100) % 3], transform);
        }
    }

    void GenerateMaze()
    {
        StopWatch genWatch = new StopWatch();
        genWatch.Start();
        int seed = (int)(System.DateTime.Now.TimeOfDay.TotalMilliseconds / 10);
        maze_smooth = Random.Range(400, 600);
        maze = new MazGen(maze_size_x, maze_size_y, seed, maze_smooth);
        mazTexture = maze.GetTexture();

        int width = maze.Width();
        int height = maze.Height();

        int exit_start_offset = Random.Range((int)(height * (1 / 4.0f)), (int)(height * (3 / 4.0f)));

        int id = 0;
        bool found_exit_tile = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                double tile_val = maze.GetValue(x, y);

                bool is_exit_tile = false;
                if (!found_exit_tile && x == 0 && y >= exit_start_offset && maze.GetValue(1, y) >= 0.5)
                {
                    found_exit_tile = true;
                    is_exit_tile = true;
                }

                bool is_wall_tile = tile_val < 0.5 && !is_exit_tile;
                MazeGridTile tile = new MazeGridTile(x, y, id, is_wall_tile);

                if (is_wall_tile)
                {
                    WallTiles.Add(tile);
                }
                else
                {
                    OpenTiles.Add(tile);
                }

                if (is_exit_tile)
                {
                    exitTile = tile;
                }

                id++;

                /*if (tile_val < 0.5 && !is_exit_tile)
                {
                    WallTiles.Add(new MazeGridTile(x, y, id, true));
                }
                else
                {
                    MazeGridTile tile = new MazeGridTile(x, y, id, false);

                    OpenTiles.Add(tile);
                }*/

            }
        }

        

        CreateMazeWalls();
        mazeObjects.PopulateObjects();

        ready = true;

        genWatch.Stop();
        Debug.LogFormat("Maze generation time: {0} ms", genWatch.Elapsed.TotalMilliseconds);
        Debug.LogFormat("Wall Tiles: {0}, Open Tiles: {1}", WallTiles.Count, OpenTiles.Count);

        MazeGridTile spawnTile = OpenTiles[Random.Range(0, OpenTiles.Count - 1)];
        Vector3 spawn_pos = new Vector3(spawnTile.GlobalPosition.x + 0.5f, player.transform.position.y, spawnTile.GlobalPosition.z);
        player.transform.position = spawn_pos;
    }

    public List<MazeGridTile> GetWallTiles()
    {
        return WallTiles;
    }

    public List<MazeGridTile> GetOpenTiles()
    {
        return OpenTiles;
    }

    public MazeGridTile GetRandomOpenTile()
    {
        int id = Random.Range(0, OpenTiles.Count - 1);
        return OpenTiles[id];
    }
}
