using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CryptoCount : MonoBehaviour
{
    TMP_Text count;
    void Awake()
    {
        count = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // no script for tracking amount of crypto coins
        // on field or picked up, currently.
        count.text = "##/##" + " CryptoCoins";
    }
}
