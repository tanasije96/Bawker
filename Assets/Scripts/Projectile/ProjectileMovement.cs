using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f; 

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
