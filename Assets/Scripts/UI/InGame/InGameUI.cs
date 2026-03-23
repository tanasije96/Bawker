using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    Label healthLabel;
    Label scoreLabel;
    Label timerLabel;
    
    private void OnEnable()
    {
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        healthLabel = root.Q<Label>("HealthLabel");
        scoreLabel = root.Q<Label>("ScoreLabel");
        timerLabel = root.Q<Label>("TimerLabel");

    }

    public void UpdateUI(int currentHealth, int currentScore)
    {
        healthLabel.text = "Health: " + currentHealth;
        scoreLabel.text = "Score: " + currentScore;
    }

    public void UpdateTimer(float currentTime)
    {
        timerLabel.text = "Time Left: " + currentTime;
    }
}
