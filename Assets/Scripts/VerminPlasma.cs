using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerminPlasma : MonoBehaviour
{
    ParticleSystem ps;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        for (int i = 0; i < numEnter; i++)
        {
            PlayerControll.Instance.gameObject.SendMessageUpwards("Hit", SendMessageOptions.DontRequireReceiver);
        }

        //Debug.Log("hit by plasma!");
    }
}
