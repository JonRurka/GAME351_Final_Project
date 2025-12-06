using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericLife : MonoBehaviour
{
    public float amount;
    public float add_life = 10;

    // Update is called once per frame
    void Update()
    {
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Health")
        {

            amount += add_life;
            Destroy(other.gameObject);
            
        }
    }
}
