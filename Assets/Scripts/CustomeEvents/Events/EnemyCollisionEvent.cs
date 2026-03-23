using System;
using UnityEngine;

public class EnemyCollisionEvent : CustomEvent
{
    public static event Action<GameObject> OnEnemyCollision;

    public override void Trigger(GameObject gameObject)
    {
        OnEnemyCollision?.Invoke(gameObject);
    }
}
