using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    Label healthLabel;
    Label scoreLabel;
    ProgressBar timerBar;
    
    private void OnEnable()
    {
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        healthLabel = root.Q<Label>("HealthLabel");
        scoreLabel = root.Q<Label>("ScoreLabel");
        timerBar = root.Q<ProgressBar>("TimerBar");
    }

    public void UpdateUI(int currentHealth, int currentScore)
    {
        healthLabel.text = "Health: " + currentHealth;
        scoreLabel.text = "Score: " + currentScore;
    }

    public void UpdateTimer(float currentTimeLeft)
    {
        timerBar.value = currentTimeLeft;
    }
}
