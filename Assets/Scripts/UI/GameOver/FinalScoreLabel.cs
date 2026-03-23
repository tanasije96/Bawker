using UnityEngine;
using UnityEngine.UIElements;

public class FinalScoreLabel : MonoBehaviour
{
    private Label scoreLabel;

    private void OnEnable()
    {
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        scoreLabel = root.Q<Label>("ScoreLabel");
    }

    public void UpdateScore(int currentScore)
    {
        scoreLabel.text = "Final Score: " + currentScore;
    }
}
