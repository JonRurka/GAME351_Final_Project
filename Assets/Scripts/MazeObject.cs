using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeObject : MonoBehaviour
{
    public float rotate_speed = 10.0f;
    public Transform model_object;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        model_object.Rotate(0, rotate_speed * Time.deltaTime, 0, Space.World);
    }
}
