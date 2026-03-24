using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Water Collision")]
public class WaterCollisionEvent : CustomEvent
{
    public static event Action<GameObject> OnWaterCollision;

    public override void Trigger(GameObject gameObject)
    {
        OnWaterCollision?.Invoke(gameObject);
    }
}
