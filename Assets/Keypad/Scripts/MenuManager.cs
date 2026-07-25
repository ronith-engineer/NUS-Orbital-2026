using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance {  get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject gameOverLosePanel;
    [SerializeField] private GameObject gameOverWinPanel;

    public bool canPause = true;

    private bool isPaused = false;

    private bool isGameOver = false;


    private ICloseableUI activeUI;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }   
        Instance = this;
    }
    private void Update()
    {
        if (isGameOver) return; // Escape does nothing on the game over screen

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    private void HandleEscape()
    {
        if (activeUI != null)
        {
            activeUI.CloseUI();
            activeUI = null;
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else if (canPause)
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    public void ShowGameOverLose()
    {
        isGameOver = true;
        gameOverLosePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowGameOverWin()
    {
        isGameOver = true;
        gameOverWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void RegisterOpenUI(ICloseableUI ui)
    {
        activeUI = ui;
    }

    public void UnregisterOpenUI(ICloseableUI ui)
    {
        if (activeUI == ui)
            activeUI = null;
    }
}