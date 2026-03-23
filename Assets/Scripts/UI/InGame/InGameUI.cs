using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    private Label healthLabel;
    private Label scoreLabel;
    private ProgressBar timerBar;
    
    private void OnEnable()
    {
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        healthLabel = root.Q<Label>("HealthLabel");
        scoreLabel = root.Q<Label>("ScoreLabel");
        timerBar = root.Q<ProgressBar>("TimerBar");
    }

    public void UpdateHealth(int currentHealth)
    {
        healthLabel.text = "Health: " + currentHealth;
    }

    public void UpdateScore(int currentScore)
    {
        scoreLabel.text = "Score: " + currentScore;
    }

    public void UpdateTimer(float currentTimeLeft)
    {
        timerBar.value = currentTimeLeft;
    }
}
