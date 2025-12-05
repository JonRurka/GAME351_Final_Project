using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    TMP_Text count;
    public PlayerShooting shooting;
    void Awake()
    {
        count = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //currently, there is no script for ammo total, so the ## is there
        count.text = "Ammo: " + shooting.ammoCount;
    }
}
