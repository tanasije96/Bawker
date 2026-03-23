using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Enemy Collision")]
public class EnemyCollisionEvent : CustomEvent
{
    public static event Action<GameObject> OnEnemyCollision;

    public override void Trigger(GameObject gameObject)
    {
        OnEnemyCollision?.Invoke(gameObject);
    }
}
