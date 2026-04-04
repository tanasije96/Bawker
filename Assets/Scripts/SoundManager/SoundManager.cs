using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip failureClip;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip backgroundMusicClip;

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

    public float GetSuccessClipLength()
    {
        return successClip.length;
    }

    public void PlaySuccess()
    {
        audioSource.PlayOneShot(successClip, 1.5f);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClip);
    }
    
}
