using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Image lifeBar;
    public GenericLife life;

    // Start is called before the first frame update
    void Awake()
    {
        lifeBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeBar.fillAmount = life.amount / 100;
    }
}
