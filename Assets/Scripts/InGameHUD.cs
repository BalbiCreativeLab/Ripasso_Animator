using TMPro;
using UnityEngine;

public class InGameHUD : MonoBehaviour
{
    [SerializeField] TMP_Text staticCollectibleText;
    [SerializeField] TMP_Text singletonCollectibleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collegamento all'action dentro GameState
        GameState.OnCollectiblePick += UpdateCollectibleText;

        GameStateSingleton.Current.OnScoreAdd += AddScore;
    }

    void UpdateCollectibleText(int score)
    {
        staticCollectibleText.text = score.ToString();
    }

    void AddScore(int score)
    {
        singletonCollectibleText.text = score.ToString();
    }

    private void OnDestroy()
    {
        GameState.OnCollectiblePick -= UpdateCollectibleText;
        GameStateSingleton.Current.OnScoreAdd -= AddScore;
    }

    private void OnDisable()
    {
        
    }
}
