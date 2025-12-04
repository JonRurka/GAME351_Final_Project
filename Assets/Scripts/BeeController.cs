using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    public static BeeController Instance { get; private set; }

    public GameObject bee_prefab;

    private int _maxBees;
    private GameObject[] Bees;
    private BeeTimer[] Bee_script;
    private int curr_bee_idx;

    float wait_amount;
    float curr_wait_time;
    bool is_waiting;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Try to target the amount of bees to just over the frame rate so one bee spawns every frame to minimize waiting.
        _maxBees = (int)(1.0f / (Time.deltaTime * 1.1)) + 1;
        Bees = new GameObject[_maxBees];
        Bee_script = new BeeTimer[_maxBees];
    }

    // Update is called once per frame
    void Update()
    {
        // If all the bees have a bit of time left before despawning,
        if (is_waiting)
        {
            curr_wait_time += Time.deltaTime;
            if (curr_wait_time >= wait_amount)
            {
                is_waiting = false;
            }
            else
            {
                return;
            }
        }

        // Probably a better way to do all this is have a Queue for the DestroyBee function
        // to add expired bees to, and pull from that queue every frame. If the queue is empty,
        // find the next free slot in the buffer. I, however, thought of this after finishing
        // my script.


        if (Bees[curr_bee_idx] == null || Bee_script[curr_bee_idx].IsExpired())
        {
            // If the current spot is open, great! Spawn a bee.
            SpawnNewBee(curr_bee_idx);
            Iterate_Idx();
        }
        else
        {
            float min_wait_time = float.MaxValue;
            int nearest_idx = 0;
            bool did_find = false;
            int max_iter = _maxBees;

            // Iterate through through the bees until we find an empty or expired bee.
            // If we iterate through all the spaces and don't find anything, set a timer
            // to wait for the bee quickest to despawn and set the idx to that one.
            while (max_iter >= 0)
            {
                if (Bees[curr_bee_idx] == null ||
                    Bee_script[curr_bee_idx].IsExpired())
                {
                    SpawnNewBee(curr_bee_idx);
                    Iterate_Idx();
                    did_find = true;
                    break;
                }
                if (Bee_script[curr_bee_idx].Timer() < min_wait_time)
                {
                    min_wait_time = Bee_script[curr_bee_idx].Timer();
                    nearest_idx = curr_bee_idx;
                }
                Iterate_Idx();
                max_iter--;
            }

            if (!did_find)
            {
                is_waiting = true;
                curr_bee_idx = nearest_idx;
                // try to target one or two frames after.
                wait_amount = min_wait_time + 2 * Time.deltaTime;
                curr_wait_time = 0;
            }
        }
        
    }

    void Iterate_Idx()
    {
        curr_bee_idx++;
        if (curr_bee_idx >= _maxBees)
        {
            curr_bee_idx = 0;
        }
    }

    void SpawnNewBee(int bee_idx)
    {
        DestroyBee(bee_idx);
        Bees[bee_idx] = Instantiate(bee_prefab);
        Bee_script[bee_idx] = Bees[bee_idx].GetComponent<BeeTimer>();
    }

    public void DestroyBee(int bee_idx)
    {
        if (Bees[bee_idx] != null)
        {
            Destroy(Bees[bee_idx]);
            Bees[bee_idx] = null;
            Bee_script[bee_idx] = null;
        }
    }
}
