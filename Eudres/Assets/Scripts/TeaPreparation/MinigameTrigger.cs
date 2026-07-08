using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour
{
    [SerializeField] private BreathingMinigame minigame;
    private bool minigameStarted = false;

    void Start()
    {
        minigame.cupController.OnMinigameComplete += OnMinigameWon;
        minigame.gameObject.SetActive(false);  
    }

    public void CheckTea()
    {
        if (KeywordsTracker.Instance.AreAllBubblesPopped() && !minigameStarted)
        {
            StartMinigame();
        }
        else
        {
            IndicationText.Instance.DisplayText("I think I can do something better than this!");
        }
    }

    public void StartMinigame()
    {
        minigame.gameObject.SetActive(true); 
        minigameStarted = true;
    }

    void OnMinigameWon()
    {
        minigame.gameObject.SetActive(false);
        minigameStarted = false;
        TeaSession.IsReturningFromPreparation = true;
        SceneManager.LoadScene("ShopCounter");
    }
}
