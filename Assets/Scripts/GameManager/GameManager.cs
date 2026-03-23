using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject MainMenuUIDocument;
    [SerializeField] private GameObject InGameUIDocument;
    [SerializeField] private GameObject GameOverUIDocument;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        PauseGame();
    }

    void OnEnable()
    {
        StartGameButton.OnStartButtonClicked += HandleStartGame;
        EnemyCollisionEvent.OnEnemyCollision += HandleEnemyCollision;
    }

    void OnDisable()
    {
        StartGameButton.OnStartButtonClicked -= HandleStartGame;
        EnemyCollisionEvent.OnEnemyCollision -= HandleEnemyCollision;
    }

    private void HandleStartGame()
    {
        MainMenuUIDocument.SetActive(false);
        InGameUIDocument.SetActive(true);
        UnPauseGame();
    }

    private void HandleEnemyCollision(GameObject player)
    {
        //disable player movement
        //play sound
        //play death anim
        //lower player health
        //update score ui
        //wait for anim to finish
        //respawn player
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
