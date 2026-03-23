using System;
using UnityEngine;

public class StartButtonClickedEvent : CustomEvent
{
    public static event Action<GameObject> OnStartButtonClicked;

    public override void Trigger(GameObject gameObject)
    {
        OnStartButtonClicked?.Invoke(gameObject);
    }
}
