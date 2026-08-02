using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References (assign in Inspector)")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel; // same prefab/instance used in Main Menu

    [Header("Scene to load when Main Menu is pressed")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OnResumeButtonPressed()
    {
        Resume();
    }

    public void OnSettingsButtonPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings panel not assigned on PauseMenuManager.");
        }
    }

    public void OnMainMenuButtonPressed()
    {
        // Reset timescale before leaving, otherwise MainMenu would load paused.
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
