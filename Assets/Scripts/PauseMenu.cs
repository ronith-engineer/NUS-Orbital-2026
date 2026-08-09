using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, ICloseableUI
{
    public bool isPaused = false;

    [SerializeField] private GameObject instructionsPanel;

    public void OpenUI()
    {
        if (!MenuManager.Instance.RegisterOpenUI(this))
            return;
        isPaused = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseUI()
    {
        MenuManager.Instance.UnregisterOpenUI(this);
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        MenuManager.Instance.UnregisterOpenUI(this);
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }


}
