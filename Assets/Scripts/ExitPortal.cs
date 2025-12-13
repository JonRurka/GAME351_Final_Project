using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    public GameObject exitPortal;

    // Start is called before the first frame update
    void Awake()
    {
        exitPortal = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("ExitScene");
    }
}
