using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int healthPoints = 3;

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public void TakeDamage()
    {
        healthPoints -= 1;
    }
}
