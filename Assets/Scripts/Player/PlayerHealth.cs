using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int healthPoints = 3;

    private int healthMax;

    void Start()
    {
        healthMax = healthPoints;
    }

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public void ResetHealth()
    {
        healthPoints = healthMax;
    }

    public void TakeDamage()
    {
        healthPoints -= 1;
    }
}
