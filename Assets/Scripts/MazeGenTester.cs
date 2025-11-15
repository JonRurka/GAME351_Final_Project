using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StopWatch = System.Diagnostics.Stopwatch;

public class MazeGenTester : MonoBehaviour
{
    public int maze_size_x;
    public int maze_size_y;
    public int maze_smooth;

    public int tex_size_x;
    public int tex_size_y;

    bool ready = false;
    Texture mazTexture;
    MazGen maze;

    // Start is called before the first frame update
    void Start()
    {
        StopWatch genWatch = new StopWatch();
        genWatch.Start();
        maze = new MazGen(maze_size_x, maze_size_y, 0, maze_smooth);
        mazTexture = maze.GetTexture();
        ready = true;
        genWatch.Stop();
        Debug.LogFormat("Maze generation time: {0} ms", genWatch.Elapsed.TotalMilliseconds);
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
                (Screen.width / 2) - (tex_size_x / 2), 
                (Screen.height / 2) - (tex_size_y / 2),
                tex_size_x, tex_size_y),
            mazTexture);
    }
}
