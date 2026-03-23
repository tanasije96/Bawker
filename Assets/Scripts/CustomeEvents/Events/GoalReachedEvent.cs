using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Goal Reached")]
public class GoalReachedEvent : CustomEvent
{
    public static event Action<GameObject> OnGoalReached;

    public override void Trigger(GameObject gameObject)
    {
        OnGoalReached?.Invoke(gameObject);
    }
}
