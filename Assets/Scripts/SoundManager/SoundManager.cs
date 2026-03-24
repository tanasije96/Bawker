using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Coroutine musicCoroutine;
    [SerializeField] private AudioClip failureClip;
    [SerializeField] private AudioClip successClip;

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

    public void PlayFailure()
    {
        audioSource.PlayOneShot(failureClip);
    }

    public float GetFailureClipLength()
    {
        return failureClip.length;
    }

    public void PlaySuccess()
    {
        audioSource.PlayOneShot(successClip);
    }
    
}
