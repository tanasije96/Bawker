using UnityEngine;

public abstract class CustomEvent : ScriptableObject
{
    public abstract void Trigger(GameObject gameObject);
}
