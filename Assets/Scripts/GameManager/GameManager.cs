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

    private ScoreManager scoreManager;
    private Timer timer;

    private int totalGoalCount;
    private int currentGoalCount = 0;

    GameObject[] goals;


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
        EnemyCollisionEvent.OnEnemyCollision += HandleEnemyCollision;
        Timer.OnTimerEnd += HandleEnemyCollision;
        GoalReachedEvent.OnGoalReached += HandleGoalReached;

        scoreManager = ScoreManager.Instance;
        timer = Timer.Instance;
    }

    void OnDisable()
    {
        StartGameButton.OnStartButtonClicked -= HandleStartGame;
        MainMenuButton.OnMainMenuButtonClicked -= HandleMainMenu;
        EnemyCollisionEvent.OnEnemyCollision -= HandleEnemyCollision;
        Timer.OnTimerEnd -= HandleEnemyCollision;
        GoalReachedEvent.OnGoalReached -= HandleGoalReached;
    }

    private void HandleStartGame()
    {
        MainMenuUIDocument.SetActive(false);
        GameOverUIDocument.SetActive(false);
        player.GetComponent<PlayerHealth>().ResetHealth();
        scoreManager.ResetScore();
        DestroyStaticPlayers();
        ResetGoals();
        currentGoalCount = 0;
        InGameUIDocument.SetActive(true);
        timer.ResetTimer();
        timer.StartTimer();
        UnPauseGame();
    }

    private void HandleMainMenu()
    {
        MainMenuUIDocument.SetActive(true);
        GameOverUIDocument.SetActive(false);
    }

    private void HandleEnemyCollision(GameObject enemy)
    {
        timer.StopTimer();
        player.GetComponent<PlayerMovement>().FreezePlayer();
        //play sound
        //play death anim
        player.GetComponent<PlayerHealth>().TakeDamage();
        InGameUIDocument.GetComponent<InGameUI>().UpdateHealth(player.GetComponent<PlayerHealth>().GetHealthPoints());
        //wait for anim to finish
        if (player.GetComponent<PlayerHealth>().GetHealthPoints() == 0)
        {
            PauseGame();
            InGameUIDocument.SetActive(false);
            GameOverUIDocument.SetActive(true);
            GameOverUIDocument.GetComponent<FinalScoreLabel>().UpdateScore(scoreManager.GetScore());
        }
        RespawnPlayer();
        player.GetComponent<PlayerMovement>().UnFreezePlayer();
        timer.ResetTimer();
        timer.StartTimer();
    }

    private void HandleGoalReached(GameObject goal)
    {
        timer.StopTimer();
        player.GetComponent<PlayerMovement>().FreezePlayer();
        scoreManager.UpdateScoreByAmt(10);
        scoreManager.UpdateScoreByAmt(Mathf.CeilToInt(timer.CurrentTime));
        InGameUIDocument.GetComponent<InGameUI>().UpdateScore(scoreManager.GetScore());
        RespawnPlayer();
        player.GetComponent<PlayerMovement>().UnFreezePlayer();
        timer.ResetTimer();
        timer.StartTimer();
        goal.GetComponent<Collider>().isTrigger = false;
        goal.GetComponent<Collidable>().enabled = false;
        GameObject staticPlayer = Instantiate(playerPrefab, goal.transform.position, goal.transform.rotation);
        staticPlayer.GetComponent<PlayerMovement>().enabled = false;
        staticPlayer.GetComponent<Collider>().enabled = false;
        staticPlayer.GetComponent<Rigidbody>().isKinematic = false;
        staticPlayer.tag = "StaticPlayer";
        currentGoalCount++;
        if (currentGoalCount == totalGoalCount)
        {
            DestroyStaticPlayers();
            ResetGoals();
            currentGoalCount = 0;
        }
    }

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
    }
}
