using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuButton : MonoBehaviour
{
    public static event Action OnMainMenuButtonClicked;

    private Button mainMenuButton;
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        mainMenuButton = root.Q<Button>("MainMenuButton");
        
        mainMenuButton.clicked += MainMenuButtonClicked;
    }

    void OnDisable()
    {
        mainMenuButton.clicked -= MainMenuButtonClicked;
    }

    private void MainMenuButtonClicked()
    {
        OnMainMenuButtonClicked?.Invoke();
    }
}
