using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float maxTime = 30f;

    private float currentTime;
    private bool isRunning = false;

    public bool IsRunning => isRunning;

    public float CurrentTime => currentTime;
    public float NormalizedTime => currentTime / maxTime * 100f;
    public static event Action<GameObject> OnTimerEnd;

    public static Timer Instance;

    void Start()
    {
        ResetTimer();
    }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;

            OnTimerEnd?.Invoke(null);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = maxTime;
    }
}
