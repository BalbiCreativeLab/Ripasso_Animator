using TMPro;
using UnityEngine;

public class InGameHUD : MonoBehaviour
{
    [SerializeField] TMP_Text staticCollectibleText;
    [SerializeField] TMP_Text singletonCollectibleText;
    [SerializeField] TMP_Text eventSysCollectibleText;

    int eventSysCollectibles = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collegamento all'action dentro GameState
        GameState.OnCollectiblePick += UpdateCollectibleText;

        GameStateSingleton.Current.OnScoreAdd += AddScore;

        GameEventSystem.Subscribe("OnCollectiblepPickUp", UpdateEventSysText);
    }

    void UpdateCollectibleText(int score)
    {
        staticCollectibleText.text = score.ToString();
    }

    void AddScore(int score)
    {
        singletonCollectibleText.text = score.ToString();
    }

    void UpdateEventSysText(GameEventData data)
    {
        eventSysCollectibles += (data as GEVDATA_CollectiblePickedUp).collectibleValue;
        eventSysCollectibleText.text = eventSysCollectibles.ToString();
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
