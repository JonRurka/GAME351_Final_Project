using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Sources")]
    public AudioSource calmSource;
    public AudioSource combatSource;

    [Header("Fade Settings")]
    public float fadeSpeed = 1f;

    private string calmParam = "Music_CalmVolume";
    private string combatParam = "Music_CombatVolume";

    private bool inCombat = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (calmSource != null)
            calmSource.Play();
        if (combatSource != null)
            combatSource.Play();
    }

    private void Update()
    {
        if (mixer == null) return;

        float targetCalm = inCombat ? -80f : 0f;
        float targetCombat = inCombat ? 0f : -80f;

        FadeMixer(calmParam, targetCalm);
        FadeMixer(combatParam, targetCombat);
    }

    private void FadeMixer(string param, float target)
    {
        if (!mixer.GetFloat(param, out float currentValue))
            currentValue = target;

        float newValue = Mathf.Lerp(currentValue, target, Time.deltaTime * fadeSpeed);
        mixer.SetFloat(param, newValue);
    }

    public void EnterCombat()
    {
        inCombat = true;
    }

    public void ExitCombat()
    {
        inCombat = false;
    }
}
