using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericLife : MonoBehaviour
{
    public float amount;
    public float add_life = 10;
    public float remove_life = 1;
    public bool dead = false;

    // Update is called once per frame
    void Update()
    {
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Health")
        {

            amount += add_life;
            //Destroy(other.gameObject);
            
        }

        if (other.gameObject.tag == "Plasma")
        {
            Debug.Log("hit by plasma!");
        }
    }

    public void Hit()
    {
        amount -= remove_life;
        if (amount <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        if (dead)
        {
            return;
        }

        GameController.Instance.PlayerDied();

        dead = true;
    }

}
