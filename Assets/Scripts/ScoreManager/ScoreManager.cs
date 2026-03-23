using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    public static ScoreManager Instance;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public int GetScore()
    {
        return score;
    }

    public void UpdateScoreByAmt(int amt)
    {
        score += amt;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
