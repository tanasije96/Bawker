using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Coroutine musicCoroutine;
    [SerializeField] private AudioClip loseLifeClip;

    public static SoundManager Instance;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLoseLife()
    {
        audioSource.PlayOneShot(loseLifeClip);
    }

    public float GetLoseLifeClipLength()
    {
        return loseLifeClip.length;
    }
    
}
