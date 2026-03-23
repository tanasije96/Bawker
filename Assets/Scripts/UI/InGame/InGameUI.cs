using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    Label healthLabel;
    Label scoreLabel;
    
    private void OnEnable()
    {
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        healthLabel = root.Q<Label>("HealthLabel");
        scoreLabel = root.Q<Label>("ScoreLabel");

    }
    private void OnDisable()
    {

    }

    public void UpdateUI(int currentHealth, int currentScore)
    {
        healthLabel.text = "Health: " + currentHealth;
        scoreLabel.text = "Score: " + currentScore;
    }
}
