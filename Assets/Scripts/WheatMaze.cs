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
    public GameObject mazeWallPrefab;

    public int tex_size_x;
    public int tex_size_y;

    bool ready = false;
    Texture mazTexture;
    MazGen maze;

    public class MazeGridTile : System.IEquatable<MazeGridTile>
    {
        public Vector2Int GridPosition { get; private set; }
        public Vector3 LocalPosition { get; private set; }
        public Vector3 GlobalPosition { get; private set; }
        public bool filled { get; private set; }

        public GameObject WallObject { get; set; }

        public MazeGridTile(int x, int y, bool is_filled)
        {
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
            return GridPosition.Equals(other.GridPosition);
        }

        public override int GetHashCode()
        {
            return GridPosition.GetHashCode();
        }
    }

    List<MazeGridTile> WallTiles = new List<MazeGridTile>();
    List<MazeGridTile> OpenTiles = new List<MazeGridTile>();

    private void Awake()
    {
        Instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
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
            tile.Init(mazeWallPrefab, transform);
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

        for (int x = 0; x < maze.Width(); x++)
        {
            for (int y = 0; y < maze.Height(); y++)
            {
                double tile_val = maze.GetValue(x, y);
                if (tile_val < 0.5)
                {
                    WallTiles.Add(new MazeGridTile(x, y, true));
                }
                else
                {
                    OpenTiles.Add(new MazeGridTile(x, y, true));
                }
            }
        }

        CreateMazeWalls();

        ready = true;

        genWatch.Stop();
        Debug.LogFormat("Maze generation time: {0} ms", genWatch.Elapsed.TotalMilliseconds);
        Debug.LogFormat("Wall Tiles: {0}, Open Tiles: {1}", WallTiles.Count, OpenTiles.Count);
    }
}
