using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeTimer : MonoBehaviour
{
    public float TTL;
    public int Bee_ID;

    private float cur_time;

    // Start is called before the first frame update
    void Start()
    {
        cur_time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cur_time += Time.deltaTime;

        if (IsExpired())
        {
            BeeController.Instance.DestroyBee(Bee_ID);
        }
    }

    public bool IsExpired()
    {
        return cur_time >= TTL;
    }

    public float Timer()
    {
        return cur_time;
    }
}
