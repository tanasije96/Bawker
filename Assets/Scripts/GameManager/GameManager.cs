using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject MainMenuUIDocument;
    [SerializeField] private GameObject InGameUIDocument;
    [SerializeField] private GameObject GameOverUIDocument;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private ScoreManager scoreManager;
    private Timer timer;
    private SoundManager soundManager;

    private int totalGoalCount;
    private int currentGoalCount = 0;

    private GameObject[] goals;

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
        goals = GameObject.FindGameObjectsWithTag("Goal");
        totalGoalCount = goals.Length;
    }

    void Update()
    {
        if (timer.IsRunning)
        {
            InGameUIDocument.GetComponent<InGameUI>().UpdateTimer(timer.NormalizedTime);   
        }
    }

    void OnEnable()
    {
        StartGameButton.OnStartButtonClicked += HandleStartGame;
        MainMenuButton.OnMainMenuButtonClicked += HandleMainMenu;
        EnemyCollisionEvent.OnEnemyCollision += HandleLoseLife;
        Timer.OnTimerEnd += HandleLoseLife;
        GoalReachedEvent.OnGoalReached += HandleGoalReached;

        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();

        scoreManager = ScoreManager.Instance;
        timer = Timer.Instance;
        soundManager = SoundManager.Instance;
    }

    void OnDisable()
    {
        StartGameButton.OnStartButtonClicked -= HandleStartGame;
        MainMenuButton.OnMainMenuButtonClicked -= HandleMainMenu;
        EnemyCollisionEvent.OnEnemyCollision -= HandleLoseLife;
        Timer.OnTimerEnd -= HandleLoseLife;
        GoalReachedEvent.OnGoalReached -= HandleGoalReached;
    }

    // Event Handlers

    private void HandleStartGame()
    {
        ResetGame();
        InGameUIDocument.SetActive(true);
        MainMenuUIDocument.SetActive(false);
        GameOverUIDocument.SetActive(false);
        UnPauseGame();
    }

    private void HandleMainMenu()
    {
        MainMenuUIDocument.SetActive(true);
        GameOverUIDocument.SetActive(false);
    }

    private void HandleLoseLife(GameObject enemy)
    {
        soundManager.PlayLoseLife();
        timer.StopTimer();
        playerMovement.FreezePlayer();
        //play death anim
        playerHealth.TakeDamage();
        InGameUIDocument.GetComponent<InGameUI>().UpdateHealth(playerHealth.GetHealthPoints());
        //wait for sound to finish
        if (playerHealth.GetHealthPoints() == 0)
        {
            PauseGame();
            GameOverUIDocument.SetActive(true);
            InGameUIDocument.SetActive(false);
            GameOverUIDocument.GetComponent<FinalScoreLabel>().UpdateScore(scoreManager.GetScore());
        }
        RespawnPlayer();
        playerMovement.UnFreezePlayer();
        timer.ResetTimer();
        timer.StartTimer();
    }

    private void HandleGoalReached(GameObject goal)
    {
        timer.StopTimer();
        playerMovement.FreezePlayer();
        UpdateInGameScore();
        RespawnPlayer();
        playerMovement.UnFreezePlayer();
        timer.ResetTimer();
        timer.StartTimer();
        DeactivateGoal(goal);
        currentGoalCount++;
        SpawnStaticPlayer(goal);
        if (currentGoalCount == totalGoalCount)
        {
            DestroyStaticPlayers();
            ResetGoals();
        }
    }

    // Private Helper Functions

    private void UnPauseGame()
    {
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void RespawnPlayer()
    {
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }

    private void DestroyStaticPlayers()
    {
        GameObject[] staticPlayers = GameObject.FindGameObjectsWithTag("StaticPlayer");

        foreach (GameObject staticPlayer in staticPlayers)
        {
            Destroy(staticPlayer);
        }
    }

    private void ResetGoals()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");

        foreach (GameObject goal in goals)
        {
            goal.GetComponent<Collider>().isTrigger = true;
            goal.GetComponent<Collidable>().enabled = true;
        }

        currentGoalCount = 0;
    }

    private void ResetGame()
    {
        playerHealth.ResetHealth();
        scoreManager.ResetScore();
        DestroyStaticPlayers();
        ResetGoals();
        timer.ResetTimer();
        timer.StartTimer();
    }

    private void UpdateInGameScore()
    {
        scoreManager.UpdateScoreByAmt(10);
        scoreManager.UpdateScoreByAmt(Mathf.CeilToInt(timer.CurrentTime));
        InGameUIDocument.GetComponent<InGameUI>().UpdateScore(scoreManager.GetScore());
    }

    private void SpawnStaticPlayer(GameObject goal)
    {
        GameObject staticPlayer = Instantiate(playerPrefab, goal.transform.position, goal.transform.rotation);
        staticPlayer.GetComponent<PlayerMovement>().enabled = false;
        staticPlayer.GetComponent<Collider>().enabled = false;
        staticPlayer.GetComponent<Rigidbody>().isKinematic = false;
        staticPlayer.tag = "StaticPlayer";
    }

    private void DeactivateGoal(GameObject goal)
    {
        goal.GetComponent<Collider>().isTrigger = false;
        goal.GetComponent<Collidable>().enabled = false;
    }
}
