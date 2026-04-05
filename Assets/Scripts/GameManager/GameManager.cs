using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject MainMenuUIDocument;
    [SerializeField] private GameObject InGameUIDocument;
    [SerializeField] private GameObject GameOverUIDocument;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject successEffectPrefab;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
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
        WaterCollisionEvent.OnWaterCollision += HandleLoseLife;

        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAnimator = player.GetComponent<Animator>();

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
        WaterCollisionEvent.OnWaterCollision -= HandleLoseLife;
    }

    // Event Handlers

    private void HandleStartGame()
    {
        soundManager.PlayButtonClick();
        ResetGame();
        MusicManager.Instance.StopBackgroundMusic();
        MusicManager.Instance.PlayBackgroundMusic();
        InGameUIDocument.SetActive(true);
        MainMenuUIDocument.SetActive(false);
        GameOverUIDocument.SetActive(false);
        UnPauseGame();
    }

    private void HandleMainMenu()
    {
        soundManager.PlayButtonClick();
        MusicManager.Instance.PlayMainMenuMusic();
        MainMenuUIDocument.SetActive(true);
        GameOverUIDocument.SetActive(false);
    }

    private void HandleLoseLife(GameObject enemy)
    {
        StartCoroutine(HandleLoseLifeCoroutine());
    }

    private void HandleGoalReached(GameObject goal)
    {
        StartCoroutine(HandleGoalReachedCoroutine(goal));
    }

    private void HandleGameOver()
    {
        MusicManager.Instance.StopBackgroundMusic();
        PauseGame();
        GameOverUIDocument.SetActive(true);
        InGameUIDocument.SetActive(false);
        GameOverUIDocument.GetComponent<FinalScoreLabel>().UpdateScore(scoreManager.GetScore());
    }

    // Coroutines
    private IEnumerator HandleLoseLifeCoroutine()
    {
        MusicManager.Instance.StopBackgroundMusic();
        soundManager.PlayFailure();
        timer.StopTimer();
        playerAnimator.SetTrigger("Damage Taken");
        GameObject effect = Instantiate(hitEffectPrefab, new Vector3(player.transform.position.x,0,player.transform.position.z), Quaternion.identity);
        Destroy(effect, soundManager.GetFailureClipLength());
        Debug.Log("Sparkle");
        playerHealth.TakeDamage();
        InGameUIDocument.GetComponent<InGameUI>().UpdateHealth(playerHealth.GetHealthPoints());
        playerMovement.FreezePlayer();

        yield return new WaitForSeconds(soundManager.GetFailureClipLength());  
        
        if (playerHealth.GetHealthPoints() == 0)
        {
            HandleGameOver();   
        }
        RespawnPlayer();
        playerAnimator.SetTrigger("Player Respawned");
        MusicManager.Instance.PlayBackgroundMusic();
        playerMovement.UnFreezePlayer();
        timer.ResetTimer();
        timer.StartTimer();
    }

    private IEnumerator HandleGoalReachedCoroutine(GameObject goal)
    {
        MusicManager.Instance.StopBackgroundMusic();
        soundManager.PlaySuccess();
        timer.StopTimer();
        playerMovement.FreezePlayer();
        player.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().ForEach(r => r.enabled = false);
        UpdateInGameScore();
        SpawnStaticPlayer(goal);
        Transform top = goal.transform.Find("Top");
        GameObject effect = Instantiate(successEffectPrefab, new Vector3(top.transform.position.x,0,top.transform.position.z), Quaternion.identity);
        Destroy(effect, soundManager.GetSuccessClipLength());

        yield return new WaitForSeconds(soundManager.GetSuccessClipLength()); 

        RespawnPlayer();
        player.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().ForEach(r => r.enabled = true);
        MusicManager.Instance.PlayBackgroundMusic();
        playerMovement.UnFreezePlayer();
        timer.ResetTimer();
        timer.StartTimer();
        DeactivateGoal(goal);
        currentGoalCount++;
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
        playerAnimator.Rebind();
        playerAnimator.Update(0f);
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
        Transform top = goal.transform.Find("Top");
        GameObject staticPlayer = Instantiate(playerPrefab, top.transform.position, top.transform.rotation);
        staticPlayer.GetComponent<PlayerMovement>().enabled = false;
        staticPlayer.GetComponent<Collider>().enabled = false;
        staticPlayer.GetComponent<Rigidbody>().isKinematic = true;
        staticPlayer.tag = "StaticPlayer";
    }

    private void DeactivateGoal(GameObject goal)
    {
        goal.GetComponent<Collider>().isTrigger = false;
        goal.GetComponent<Collidable>().enabled = false;
    }
}
