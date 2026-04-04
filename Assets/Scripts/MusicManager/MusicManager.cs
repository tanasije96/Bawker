using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusicClip;

    public static MusicManager Instance;

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

    public void PlayBackgroundMusic()
    {
        StartCoroutine(PlayMusicCoroutine(0.5f));
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }

    private IEnumerator PlayMusicCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);  

        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }
}
