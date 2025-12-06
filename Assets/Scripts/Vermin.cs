using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vermin : MonoBehaviour
{
    private enum Direction
    {
        Forward,
        Backward,
        Left,
        Right
    }

    public WheatMaze.MazeGridTile Current_Tile { get; private set; }
    public WheatMaze.MazeGridTile Next_Tile { get; private set; }

    public float turn_speed = 5.0f;
    public float move_speed = 10.0f;
    public float max_player_dist = 5.0f;
    public int max_ping_pong = 4;

    private Vector3 start_pos;
    private Vector3 target_pos;
    private float move_dt = 0;
    private bool inited = false;

    private int ping_pong_count = 0;
    private Direction current_direction = Direction.Forward;
    private Direction last_direction = Direction.Forward;

    private struct TileTest
    {
        bool Valid;
        WheatMaze.MazeGridTile Tile;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inited)
            return;

        bool stop_cause_player = false;

        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * max_player_dist, Color.green);
        if (Physics.Raycast(ray, out hit, max_player_dist))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                stop_cause_player = true;
                //Debug.Log("STOP found player.");
            }
        }


        if (stop_cause_player)
            return;

        Vector3 target_dir = (target_pos - transform.position).normalized;

        if (Vector3.Dot(transform.forward, target_dir) > 0.8f)
        {
            // moving towards next tile.
            move_dt += move_speed * Time.deltaTime;
            transform.position = Vector3.Lerp(start_pos, target_pos, move_dt);

            if (move_dt >= 1.0f)
            {
                Current_Tile = Next_Tile;
                set_next_tile();
            }
        }

        Quaternion orig_quat = transform.rotation;
        transform.LookAt(target_pos);
        Quaternion target_quat = transform.rotation;
        transform.rotation = Quaternion.Lerp(orig_quat, target_quat, turn_speed * Time.deltaTime);

        Debug.DrawRay(transform.position, Vector3.up * 10, Color.red);

    }

    public void Init(WheatMaze.MazeGridTile start_tile)
    {
        Current_Tile = start_tile;
        Debug.DrawRay(Current_Tile.GlobalPosition + new Vector3(0.5f, 0, 0.5f), Vector3.up * 5, Color.blue, 10000);
        set_next_tile();
        inited = true;
    }

    void set_next_tile()
    {
        
        Vector3[] directions =
        {
            Vector3.forward,
            -Vector3.forward,
            Vector3.right,
            -Vector3.right
        };

        Vector3 fwd_dir = Vector3.zero;
        Vector3 back_dir = Vector3.zero;
        Vector3 right_dir = Vector3.zero;
        Vector3 left_dir = Vector3.zero;

        float highest_fwd_dp = float.MinValue;
        float highest_right_dp = float.MinValue;
        for (int i = 0; i < directions.Length; ++i)
        {
            float fwd_dp = Vector3.Dot(transform.forward, directions[i]);
            float right_dp = Vector3.Dot(transform.right, directions[i]);

            if (fwd_dp > highest_fwd_dp)
            {
                highest_fwd_dp = fwd_dp;
                fwd_dir = directions[i];
                back_dir = -fwd_dir;
            }

            if (right_dp > highest_right_dp)
            {
                highest_right_dp = right_dp;
                right_dir = directions[i];
                left_dir = -right_dir;
            }
        }


        //Debug.LogFormat("fwd_dir: {0}, {1}", fwd_dir, highest_fwd_dp);

        Vector2Int gpos = Current_Tile.GridPosition;
        Vector2Int fwd_tile = add_tile(gpos, fwd_dir);
        Vector2Int back_tile = add_tile(gpos, back_dir);
        Vector2Int right_tile = add_tile(gpos, right_dir);
        Vector2Int left_tile = add_tile(gpos, left_dir);

        //Debug.LogFormat("{0} + {1} = {2}", gpos, fwd_dir, fwd_tile);
        WheatMaze.MazeGridTile tst_tile;
        List<WheatMaze.MazeGridTile> valid_tile_choices = new List<WheatMaze.MazeGridTile>();
        WheatMaze.MazeGridTile backwards_tile = null;

        // First, go for tile ahead of vermin.
        if (test_set_next(fwd_tile, out tst_tile))
        {
            valid_tile_choices.Add(tst_tile);
            last_direction = current_direction;
            current_direction = Direction.Forward;
            //if (last_direction != current_direction)
            //{
                //ping_pong_count++;
            //}
            //Debug.Log("Next tile set to forward tile.");
            //return;
        }

        // If tile in front of vermin is a wall, try right and left tiles.
        if (test_set_next(right_tile, out tst_tile))
        {
            valid_tile_choices.Add(tst_tile);
            //ping_pong_count = 0;
            last_direction = current_direction;
            current_direction = Direction.Right;
            //Debug.Log("Next tile set to right tile.");
            //return;
        }

        if (test_set_next(left_tile, out tst_tile))
        {
            valid_tile_choices.Add(tst_tile);
            //ping_pong_count = 0;
            last_direction = current_direction;
            current_direction = Direction.Left;
            //Debug.Log("Next tile set to left tile.");
            //return;
        }

        // If tile in front, right, or left of vermin is a wall, go back.
        if (test_set_next(back_tile, out tst_tile))
        {
            backwards_tile = tst_tile;
            //ping_pong_count++;
            last_direction = current_direction;
            current_direction = Direction.Backward;
            //Debug.Log("Next tile set to back tile.");
            //return;
        }

        WheatMaze.MazeGridTile selected_tile = null;
        if (valid_tile_choices.Count > 0)
        {
            selected_tile = valid_tile_choices[Random.Range(0, 100) % valid_tile_choices.Count];
        }
        else
        {
            if (backwards_tile == null)
            {
                Debug.LogError("Cound not find valid tile!!!");
                return;
            }

            selected_tile = backwards_tile;
        }

        if (Current_Tile.ID == selected_tile.ID)
        {
            Debug.LogError("Same tile selected!!");
            return;
        }

        Next_Tile = selected_tile;
        start_pos = transform.position;
        target_pos = new Vector3(Next_Tile.GlobalPosition.x + 0.5f, transform.position.y, Next_Tile.GlobalPosition.z + 0.5f);
        move_dt = 0;


        //Next_Tile = tile;
        //start_pos = transform.position;
        //target_pos = new Vector3(Next_Tile.GlobalPosition.x + 0.5f, transform.position.y, Next_Tile.GlobalPosition.z + 0.5f);
        //move_dt = 0;
        //Debug.LogError("Cound not find valid tile!!!");
    }

    Vector2Int add_tile(Vector2Int gpos, Vector3 dir)
    {
        Vector2Int tile = new Vector2Int(
            gpos.x + Mathf.RoundToInt(dir.x),
            gpos.y + Mathf.RoundToInt(dir.z)
        );
        return tile;
    }

    bool test_set_next(Vector2Int tile_gpos, out WheatMaze.MazeGridTile res_tile)
    {
        if(!WheatMaze.Instance.HasTile(tile_gpos))
        {
            res_tile = null;
            return false;
        }

        WheatMaze.MazeGridTile tile = WheatMaze.Instance.GetTile(tile_gpos);
        if (tile == null)
        {
            Debug.LogError("Tile null! This shouldn't happen!");
            res_tile = null;
            return false;
        }

        if (tile.filled)
        {
            res_tile = null;
            return false;
        }

        if (tile.ID == WheatMaze.Instance.ExitTile().ID)
        {
            res_tile = null;
            return false;
        }

        //Debug.LogFormat("{0} -> {1}", Current_Tile.GridPosition, tile.GridPosition);
        //Debug.DrawRay(tile.GlobalPosition + new Vector3(0.55f, 0, 0.55f), Vector3.up * 5, Color.red, 10000);

        //Next_Tile = tile;
        //start_pos = transform.position;
        //target_pos = new Vector3(Next_Tile.GlobalPosition.x + 0.5f, transform.position.y, Next_Tile.GlobalPosition.z + 0.5f);
        //move_dt = 0;

        res_tile = tile;
        return true;
    }
}
