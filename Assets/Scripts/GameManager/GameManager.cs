using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject MainMenuUIDocument;
    [SerializeField] private GameObject InGameUIDocument;
    [SerializeField] private GameObject GameOverUIDocument;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject player;

    private ScoreManager scoreManager;

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

        scoreManager = ScoreManager.Instance;
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

    private void HandleEnemyCollision(GameObject enemy)
    {
        //disable player movement
        player.GetComponent<PlayerMovement>().enabled = false;
        //disable collider and rigid body
        player.GetComponent<Collider>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        //play sound
        //play death anim
        //lower player health
        player.GetComponent<PlayerHealth>().TakeDamage();
        //update ui
        InGameUIDocument.GetComponent<InGameUI>().UpdateUI(player.GetComponent<PlayerHealth>().GetHealthPoints(),scoreManager.GetScore());
        //wait for anim to finish
        //respawn player
        Respawn(player);
        //re-enable player movement
        player.GetComponent<PlayerMovement>().enabled = true;
        //e-enable collider and rigid body
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void Respawn(GameObject player)
    {
        player.transform.position = spawnPoint.position;
    }
}
