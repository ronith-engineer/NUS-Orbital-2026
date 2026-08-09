using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance {  get; private set; }
    public bool IsAnyUIOpen => activeUI != null;

    [Header("Panels")]
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject gameOverLosePanel;
    [SerializeField] private GameObject gameOverWinPanel;


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

        if (!pauseMenu.isPaused)
            pauseMenu.OpenUI();
    }


    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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


    public bool RegisterOpenUI(ICloseableUI ui)
    {
        if (activeUI != null && activeUI != ui)
            return false; //denies ui registration if another UI is already active

        activeUI = ui;
        WeaponManager.Instance.SetShootingEnabled(false);

        return true;
    }

    public void UnregisterOpenUI(ICloseableUI ui)
    {
        if (activeUI == ui)
        {
            activeUI = null;
            WeaponManager.Instance.SetShootingEnabled(true);
        }
    }
}