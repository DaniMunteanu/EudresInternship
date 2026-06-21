using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    [SerializeField] private BreathingMinigame minigame;

    void Start()
    {
        minigame.cupController.OnMinigameComplete += OnMinigameWon;
        minigame.gameObject.SetActive(false);  
    }

    public void StartMinigame()
    {
        minigame.gameObject.SetActive(true); 
    }

    void OnMinigameWon()
    {
        minigame.gameObject.SetActive(false);
    }
}
