using UnityEngine;

public class Collidable : MonoBehaviour
{
    [SerializeField] private string tagName;
    [SerializeField] private CustomEvent triggerEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            triggerEvent.Trigger(gameObject);
        }
    }
}
