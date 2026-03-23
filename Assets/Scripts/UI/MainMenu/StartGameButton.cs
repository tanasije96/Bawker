using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StartGameButton : MonoBehaviour
{
    public static event Action OnStartButtonClicked;

    private Button startButton;
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("StartButton");
        
        startButton.clicked += StartGameButtonClicked;
    }

    void OnDisable()
    {
        startButton.clicked -= StartGameButtonClicked;
    }

    private void StartGameButtonClicked()
    {
        OnStartButtonClicked?.Invoke();
    }
}
