using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeObject : MonoBehaviour
{
    public float rotate_speed = 10.0f;
    public Transform model_object;
    public AudioClip sound;
    public bool destroyed = false;


    private AudioSource audio_source;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        model_object.Rotate(0, rotate_speed * Time.deltaTime, 0, Space.World);
    }

    public void Interact()
    {
        if (destroyed) return;

        Debug.LogFormat("{0} collected", name);

        //audio_source.PlayOneShot(sound);
        //audio_source.time = 0f;
        audio_source.PlayOneShot(sound);
        destroyed = true;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<Collider>().enabled = false;

        Invoke("remove_obj", 10);
    }

    void remove_obj()
    {
        Destroy(gameObject);
    }
}
