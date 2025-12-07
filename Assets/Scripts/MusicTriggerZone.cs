using UnityEngine;

public class MusicTriggerZone : MonoBehaviour
{
    public bool enterCombatZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enterCombatZone)
                MusicManager.Instance.EnterCombat();
            else
                MusicManager.Instance.ExitCombat();
        }
    }
}
