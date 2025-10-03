using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField] private GameState currentGameState = GameState.PreGame;

    [Header("References")]
    [SerializeField] private Bowl basinDetector;
    [SerializeField] private BallSpawnerController ballSpawner;
    [SerializeField] private WallGroupManager wallGroupManager;
    [SerializeField] private InputActionReference startGameAction;

    [Header("UI References")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI ballsCountText;
    [SerializeField] private TextMeshProUGUI timerText;

    private float gameTimer = 0f;
    private bool timerRunning = false;

    public enum GameState
    {
        PreGame,
        Playing,
        Completed
    }

    private void OnEnable()
    {
        startGameAction.action.Enable();
        startGameAction.action.performed += OnStartGame;
    }

    private void OnDisable()
    {
        startGameAction.action.performed -= OnStartGame;
        startGameAction.action.Disable();
    }

    private void Start()
    {
        SetGameState(GameState.PreGame);

        // Subscribe to events
        if (basinDetector != null)
        {
            basinDetector.OnBallEnteredBasin += OnBallCountUpdated;
            basinDetector.OnLevelCompleted += OnLevelCompleted;
        }

        if (ballSpawner != null)
        {
            ballSpawner.OnSpawningCompleted += OnBallSpawningCompleted;
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            gameTimer += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        if (currentGameState == GameState.PreGame)
        {
            StartGame();
        }
        else if (currentGameState == GameState.Completed)
        {
            RestartGame();
        }
    }

    private void StartGame()
    {
        Debug.Log("Starting game...");
        SetGameState(GameState.Playing);

        // Start ball spawning
        if (ballSpawner != null)
        {
            ballSpawner.StartSpawn();
        }
        else
        {
            StartTimer();
        }
    }

    private void OnBallSpawningCompleted()
    {
        Debug.Log("All balls spawned, starting timer!");
        StartTimer();
    }

    private void StartTimer()
    {
        timerRunning = true;
        gameTimer = 0f;
    }

    private void OnBallCountUpdated(int currentBalls, int requiredBalls)
    {
        if (ballsCountText != null)
        {
            ballsCountText.text = $"Balls: {currentBalls}/{requiredBalls}";
        }
    }

    private void OnLevelCompleted()
    {
        Debug.Log("Level completed!");
        SetGameState(GameState.Completed);
        timerRunning = false;
    }

    private void SetGameState(GameState newState)
    {
        currentGameState = newState;

        // Update UI based on game state
        if (startScreen != null)
            startScreen.SetActive(currentGameState == GameState.PreGame);

        if (gameUI != null)
            gameUI.SetActive(currentGameState == GameState.Playing);

        if (winScreen != null)
            winScreen.SetActive(currentGameState == GameState.Completed);

        // Enable/disable wall controls
        if (wallGroupManager != null)
        {
            wallGroupManager.SetWallControlsEnabled(currentGameState == GameState.Playing);
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer / 60f);
            int seconds = Mathf.FloorToInt(gameTimer % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    private void RestartGame()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Returning to first level.");
            SceneManager.LoadScene(0);
        }
    }

    // Public methods for UI buttons
    public void StartGameButton()
    {
        StartGame();
    }

    public void RestartGameButton()
    {
        RestartGame();
    }

    public void NextLevelButton()
    {
        LoadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
