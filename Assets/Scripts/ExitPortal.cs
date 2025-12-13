using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    public GameObject exitPortal;
    public PlayerControll player;
    public WheatMaze maze;

    // Start is called before the first frame update
    void Awake()
    {
        exitPortal = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.crypto_count >= 20)
        {
            print("Portal may now appear.");
            //Instantiate(exitPortal, maze.exitTile.transform.position, maze.exitTile.transform.rotation);
        }
        else
        {
            print("Portal won't appear yet. ");
            print(player.crypto_count);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("ExitScene");
    }
}
