using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class CryptoCount : MonoBehaviour
{
    public PlayerControll player;

    TMP_Text count;
    void Awake()
    {
        count = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        count.text = string.Format("Crypto Needed: {0}/{1}", player.crypto_count, GameController.Instance.needed_coins);
    }
}
