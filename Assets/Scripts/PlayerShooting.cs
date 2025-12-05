using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint;
    public int ammoCount;
    // Start is called before the first frame update
    void Shoot()
    {
        if (ammoCount > 0 && Time.timeScale > 0)
        {
            Instantiate(prefab, shootPoint.transform.position, shootPoint.transform.rotation);
            ammoCount--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }
}
