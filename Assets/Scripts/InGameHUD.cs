using TMPro;
using UnityEngine;

public class InGameHUD : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
    [SerializeField] TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collegamento all'action dentro GameState
        GameState.OnCollectiblePick += UpdateCollectibleText;
        GameStateSingleton.Current.OnScoreAdd += AddScore;
    }

    void UpdateCollectibleText(int score)
    {
        uiText.text = score.ToString();
    }

    void AddScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
