using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene to load when Start is pressed")]
    [SerializeField] private string gameplaySceneName = "ShopCounter";

    [Header("Scene to load when Tutorial is pressed")]
    [SerializeField] private string tutorialSceneName = "TutorialShopCounter";

    [Header("Settings Panel (assign in Inspector)")]
    [SerializeField] private GameObject settingsPanel;

    public void OnStartPressed()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnTutorialPressed()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void OnSettingsPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings panel not assigned on MainMenuManager.");
        }
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
